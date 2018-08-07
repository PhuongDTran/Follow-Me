package com.followme.group;

import spark.Request;
import spark.Response;
import spark.Route;
import org.apache.commons.lang3.RandomStringUtils;
import com.google.gson.*;

public class GroupController {

	public static Route HandleGroupIdRequest = (Request request, Response response) -> {

		if( !request.body().isEmpty()){
			GroupDao groupIdDao = new GroupDao(); 
			String groupId = "";
			Gson gson = new GsonBuilder().create();
			JsonObject json = gson.fromJson(request.body(), JsonObject.class);
			String memberId = json.get("id").getAsString();
			System.out.println(json.toString());
			do{
				//a 20-length string includes letters and numbers 
				groupId = RandomStringUtils.random(20, true, true);
			}while(	groupIdDao.doesExist(groupId));
			groupIdDao.addToGroupInfo(groupId, memberId);
			return groupId;
		}else {
			response.status(301);
			return null;
		}
		
	};
}
