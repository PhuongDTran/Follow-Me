package com.followme.groupid;

import spark.Request;
import spark.Response;
import spark.Route;
import org.apache.commons.lang3.RandomStringUtils;
import com.google.gson.*;

public class GroupIdController {

	public static Route HandleGroupIdRequest = (Request request, Response response) -> {
		Gson gson = new GsonBuilder().create();
		JsonObject json = gson.fromJson(request.body(), JsonObject.class);
		System.out.println(json.toString());
		String groupId = "";
		GroupIdDao groupIdDao = new GroupIdDao(); 
		
		do{
			//a 20-length string includes letters and numbers 
			groupId = RandomStringUtils.random(20, true, true);
		}while(	groupIdDao.doesExist(groupId));
		
		groupIdDao.addGroupId(groupId);
		response.body(groupId);
		return groupId;
	};
}
