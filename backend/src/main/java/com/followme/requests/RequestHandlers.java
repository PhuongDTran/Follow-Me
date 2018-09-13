package com.followme.requests;

import java.lang.invoke.MethodHandles;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.followme.group.GroupController;
import com.followme.member.MemberController;
import com.followme.trip.Location;
import com.followme.trip.TripController;
import com.followme.util.JsonUtil;
import com.google.firebase.messaging.FirebaseMessaging;
import com.google.firebase.messaging.FirebaseMessagingException;
import com.google.firebase.messaging.Message;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonObject;

import spark.Request;
import spark.Response;
import spark.Route;

public class RequestHandlers {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	// <groupid : leaderid>
	private static Map<String,Group> groups = new HashMap<String,Group>();
	// <leaderid : token>
	//private static Map<String,String> leaders = new HashMap<String,String>();

	public static Route HandleGroupIdRequest = (Request request, Response response) -> {
		if( !request.body().isEmpty()){
			Gson gson = new GsonBuilder().create();
			JsonObject json = gson.fromJson(request.body(), JsonObject.class);
			User user = new User(json);

			Group group = new Group();
			group.setLeaderId(user.getId());
			//String groupId = GroupController.generateGroupId();
			//TODO" static group id to test, change it later
			String groupId = "phuong";
			groups.put(groupId, group);

			//add info to database
			GroupController.addGroup(groupId, user.getId());
			addOrUpdateUser(groupId, user);

			return groupId;

		}else {
			//TODO:handle empty request body, have not tested yet.
			response.status(301);
			return null;
		}

	};

	public static Route Test = (Request request, Response response) -> {
		return "hello";
	};

	public static Route HandleUpdatingToken = (Request request, Response response) -> {
		String memberId = request.queryParams("member");
		Gson gson = new GsonBuilder().create();
		JsonObject json = gson.fromJson(request.body(), JsonObject.class);
		String token = json.get("token").getAsString();
		MemberController.updateToken(memberId, token);
		return "ok";
	};

	public static Route HandleAddingMember = (Request request, Response response) -> {
		Gson gson = new GsonBuilder().create();
		JsonObject json = gson.fromJson(request.body(), JsonObject.class);
		String id = json.get("id").getAsString();
		String name = json.get("name").getAsString();
		String platform = json.get("platform").getAsString();
		MemberController.addOrUpdateUser(id, name, platform);
		return response.status();
	};

	public static Route HandleUpdatingLocation = (Request request, Response response) -> {
		String groupId = request.queryParams("group");
		String sendingMember = request.queryParams("member");

		if ( !groups.get(groupId).doesContain(sendingMember) || groups.get(groupId).getToken(sendingMember) == null) {
			String token = MemberController.getToken(sendingMember);
			groups.get(groupId).addToken(sendingMember, token);
		}

		Gson gson = new GsonBuilder().create();
		JsonObject json = gson.fromJson(request.body(), JsonObject.class);
		double latitude = json.get("lat").getAsDouble();
		double longitude = json.get("lon").getAsDouble();
		int speed = json.get("speed").getAsInt();
		int heading = json.get("heading").getAsInt();

		TripController.updateLocation(groupId, sendingMember, latitude, longitude, heading,speed);
		
		if( sendingMember.equals( groups.get(groupId).getLeaderId())){
			notifyToMembers(groupId, sendingMember);
		}else{
			notifyToLeader(groupId, sendingMember);
		}
		
		return ""; 
	};

	//https://firebase.google.com/docs/cloud-messaging/admin/send-messages
	private static void notifyToLeader(String groupId, String payload ){
		String leaderToken = getLeaderToken(groupId);

		Message message = Message.builder()
				.putData("member", payload)
				.setToken(leaderToken)
				.build();
		try{
			FirebaseMessaging.getInstance().send(message);
		} catch (FirebaseMessagingException e) {
			logger.error("notifyToLeader() failed" + e.getMessage());
		}

	}


	private static void notifyToMembers(String groupId,String payload) {
		List<String> tokens = groups.get(groupId).getMemberTokens();
		for( String token : tokens){
			if(token != null){
				Message message = Message.builder()
						.putData("leader", payload)
						.setToken(token)
						.build();
				try{
					FirebaseMessaging.getInstance().send(message);
				}catch (FirebaseMessagingException e) {
					logger.error("notifyToMembers() failed. " + e.getMessage());
				}
			}
		}
	}

	public static Route HandleGettingLeaderId = (Request request, Response response) -> {
		String groupId = request.queryParams("group");
		String leaderId = GroupController.getLeaderId(groupId);

		return leaderId;
	};

	public static Route HandleGettingLocation = (Request request, Response response) -> {
		String groupId = request.queryParams("group");
		String memberId = request.queryParams("member");
		Location location = getLocation(groupId, memberId);
		return JsonUtil.dataToJson(location);
	};

	private static String getLeaderToken(String groupId){
		Group currentGroup = groups.get(groupId); 
		String leaderToken = currentGroup.getLeaderToken();
		
		if (leaderToken == null){
			String leaderId = currentGroup.getLeaderId();
			leaderToken = MemberController.getToken(leaderId);
			currentGroup.addToken(leaderId, leaderToken);
		}
		
		return leaderToken;
	}
	
	/*private static String getMemberToken(String memberId){
		Group currentGroup = groups.get(groupId); 
		String leaderToken = currentGroup.getLeaderToken();
		
		if (leaderToken == null){
			String leaderId = currentGroup.getLeaderId();
			leaderToken = MemberController.getToken(leaderId);
			currentGroup.addToken(leaderId, leaderToken);
		}
		
		return leaderToken;
	}
	*/
	private static Location getLocation( String groupId, String memberId){
		if (groupId != null && memberId != null) {
			return TripController.getLocation(groupId, memberId);
		}
		return null;
	}

	private static void addOrUpdateUser(String groupId, User user){
		MemberController.addOrUpdateUser(user.getId(), user.getName(), user.getPlatform());
		TripController.updateLocation(groupId, user.getId(), user.getLatitude(), user.getLongitude(), user.getHeading(), user.getSpeed());
	}
}
