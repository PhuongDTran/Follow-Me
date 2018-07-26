package com.followme.groupid;

import spark.Request;
import spark.Response;
import spark.Route;
import org.apache.commons.lang3.RandomStringUtils;

public class GroupIdController {

	public static Route HandleGroupIdRequest = (Request request, Response response) -> {
		//a 20-length string includes letters and numbers 
		String randomString = RandomStringUtils.random(20, true, true);
		return randomString;
	};
}
