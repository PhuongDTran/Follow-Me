package com.followme;
import static spark.Spark.get;
import static spark.Spark.post;

import com.followme.group.GroupController;
import com.followme.util.DatabaseConnection;
import com.followme.util.Path;

public class FollowMeMain {

	public static void main(String[] args) {
		
		//initialize database connection
		DatabaseConnection.initialize();
		
		get("/hello/", (req,res) -> "hello");
		post(Path.Web.GROUPID,   GroupController.HandleGroupIdRequest);

	}

}
