package com.followme.group;

import spark.Request;
import spark.Response;
import spark.Route;
import org.apache.commons.lang3.RandomStringUtils;

import com.followme.member.MemberController;
import com.google.gson.*;

public class GroupController {

	public static Route HandleGroupIdRequest = (Request request, Response response) -> {

		if( !request.body().isEmpty()){
			GroupDao groupDao = new GroupDao(); 
			String groupId = "";
			Gson gson = new GsonBuilder().create();
			JsonObject json = gson.fromJson(request.body(), JsonObject.class);
			String memberId = json.get("id").getAsString();
			String memberName = json.get("name").getAsString();
			String platform = json.get("platform").getAsString();
			System.out.println(json.toString());
			do{
				//a 20-length string includes letters and numbers 
				groupId = RandomStringUtils.random(20, true, true);
			}while(	groupDao.doesExist(groupId));
			groupDao.addGroup(groupId, memberId);
			MemberController.addMember(memberId, memberName, platform);
			return groupId;
		}else {
			response.status(301);
			return null;
		}
		
	};
}
