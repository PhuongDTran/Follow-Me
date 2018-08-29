package com.followme;
import static spark.Spark.get;
import static spark.Spark.port;
import static spark.Spark.post;
import static spark.Spark.staticFiles;
import static spark.debug.DebugScreen.enableDebugScreen;

import com.followme.requests.RequestsHandler;
import com.followme.util.DatabaseConnection;
import com.followme.util.FirebaseSetup;
import com.followme.util.Path;

public class FollowMeMain {

	public static void main(String[] args) {
		
		//initialize database connection
		DatabaseConnection.initialize();
		
		//initialize Firebase SDK
		FirebaseSetup.initialize();
		
		//configure Spark
		port(4567);
		enableDebugScreen();
		staticFiles.location("/public");
		
		get("/hello/", (req,res) -> "hello");
		post(Path.Web.GROUP,					RequestsHandler.HandleGroupIdRequest);
		get(Path.Web.TRIP, 	"application/json" ,RequestsHandler.HandleGettingLocation);
		post(Path.Web.TRIP,						RequestsHandler.HandleUpdatingLocation);
		post(Path.Web.MEMBER, 					RequestsHandler.HandleAddingMember);
		get(Path.Web.LEADER, 					RequestsHandler.HandleGettingLeaderId);
		post(Path.Web.TOKEN, 					RequestsHandler.HandleUpdatingToken);
		get(Path.Web.TOKEN, RequestsHandler.Test);
	}

}
