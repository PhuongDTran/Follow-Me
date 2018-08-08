package com.followme.requests;

import com.followme.group.GroupController;
import com.followme.member.MemberController;
import com.followme.trip.TripController;
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
			Member member = new Member(json);
			
			System.out.println(json.toString());
			String groupId = GroupController.generateGroupId();
			
			//add info to database
			GroupController.addGroup(groupId, member.getId());
			MemberController.addMember(member.getId(), member.getName(), member.getPlatform());
			TripController.addNewOrUpdate(groupId, member.getId(), member.getLatitude(), member.getLongitude(), member.getHeading(), member.getSpeed(), true);
			
			return groupId;
			
		}else {
			//TODO:handle empty request body, have not tested yet.
			response.status(301);
			return null;
		}
		
	};
}
