package com.followme.groupid;

import spark.Request;
import spark.Response;
import spark.Route;
import org.apache.commons.lang3.RandomStringUtils;
import com.google.gson.*;

public class GroupIdController {

	public static Route HandleGroupIdRequest = (Request request, Response response) -> {

		if( !request.body().isEmpty()){
			GroupIdDao groupIdDao = new GroupIdDao(); 
			String groupId = "";
			Gson gson = new GsonBuilder().create();
			JsonObject json = gson.fromJson(request.body(), JsonObject.class);
			System.out.println(json.toString());
			do{
				//a 20-length string includes letters and numbers 
				groupId = RandomStringUtils.random(20, true, true);
			}while(	groupIdDao.doesExist(groupId));
			groupIdDao.addGroupId(groupId);
			return groupId;
		}else {
			response.status(301);
			return null;
		}
		
	};
}
