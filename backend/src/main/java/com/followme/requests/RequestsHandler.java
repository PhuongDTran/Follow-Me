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
		String groupId = request.queryParams("groupid");
		Gson gson = new GsonBuilder().create();
		JsonObject json = gson.fromJson(request.body(), JsonObject.class);
		User user = new User(json);
		
		addOrUpdateUser(groupId, user);
		return GroupController.getLeaderId(groupId);
	};
	
	public static Route HandleUpdatingLocation = (Request request, Response response) -> {
		String groupId = request.queryParams("groupid");
		Gson gson = new GsonBuilder().create();
		JsonObject json = gson.fromJson(request.body(), JsonObject.class);
		User user = new User(json);
		TripController.addOrUpdateMember(groupId, user.getId(), user.getLatitude(), user.getLongitude(), user.getHeading(), user.getSpeed(), true);
		return null; 
	};
	
	public static Route HandleGettingLocation = (Request request, Response response) -> {
		String groupId = request.queryParams("groupid");
		String memberId = request.queryParams("memberid");
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
