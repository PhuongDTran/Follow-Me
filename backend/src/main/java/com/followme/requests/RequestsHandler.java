package com.followme.requests;

import org.apache.commons.lang3.RandomStringUtils;

import com.followme.group.GroupController;
import com.followme.group.GroupDao;
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
			String memberId = json.get("id").getAsString();
			String memberName = json.get("name").getAsString();
			double latitude = json.get("lat").getAsDouble();
			double longitude = json.get("lon").getAsDouble();
			int heading = json.get("heading").getAsInt();
			int speed = json.get("speed").getAsInt();
			String platform = json.get("platform").getAsString();
			System.out.println(json.toString());
			String groupId = GroupController.generateGroupId();
			//add info to database
			GroupController.addGroup(groupId, memberId);
			MemberController.addMember(memberId, memberName, platform);
			TripController.addNewOrUpdate(groupId, memberId, latitude, longitude, heading, speed, true);
			return groupId;
		}else {
			//TODO:handle empty request body, have not tested yet.
			response.status(301);
			return null;
		}
		
	};
}
