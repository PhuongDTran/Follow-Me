package com.followme.requests;

import com.followme.group.GroupController;
import com.followme.member.MemberController;
import com.followme.trip.Location;
import com.followme.trip.TripController;
import com.followme.util.JsonUtil;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonObject;

import spark.Request;
import spark.Response;
import spark.Route;

public class RequestsHandler {

	public static Route HandleGroupIdRequest = (Request request, Response response) -> {
		if( !request.body().isEmpty()){
			Gson gson = new GsonBuilder().create();
			JsonObject json = gson.fromJson(request.body(), JsonObject.class);
			User user = new User(json);
			
			String groupId = GroupController.generateGroupId();
			
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
		Gson gson = new GsonBuilder().create();
		JsonObject json = gson.fromJson(request.body(), JsonObject.class);
		User user = new User(json);
		TripController.addOrUpdateMember(groupId, user.getId(), user.getLatitude(), user.getLongitude(), user.getHeading(), user.getSpeed(), true);
		return null; 
	};
	
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
	
	
	private static Location getLocation( String groupId, String memberId){
		if (groupId != null && memberId != null) {
			return TripController.getLocation(groupId, memberId);
		}
		return null;
	}
	
	private static void addOrUpdateUser(String groupId, User user){
			MemberController.addOrUpdateUser(user.getId(), user.getName(), user.getPlatform());
			TripController.addOrUpdateMember(groupId, user.getId(), user.getLatitude(), user.getLongitude(), user.getHeading(), user.getSpeed(), false);
	}
}
