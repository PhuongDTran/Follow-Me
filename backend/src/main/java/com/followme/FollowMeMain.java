package com.followme;
import static spark.Spark.get;
import static spark.Spark.port;
import static spark.Spark.post;
import static spark.Spark.staticFiles;
import static spark.debug.DebugScreen.enableDebugScreen;

import com.followme.requests.RequestsHandler;
import com.followme.util.DatabaseConnection;
import com.followme.util.Path;

public class FollowMeMain {

	public static void main(String[] args) {
		
		//initialize database connection
		DatabaseConnection.initialize();
		
		//configure Spark
		port(4567);
		enableDebugScreen();
		staticFiles.location("/public");
		
		get("/hello/", (req,res) -> "hello");
		post(Path.Web.NEWGROUP,			RequestsHandler.HandleGroupIdRequest);
		post(Path.Web.TRIP,				RequestsHandler.HandleAddingMember);
		get(Path.Web.TRIP, "application/json" ,RequestsHandler.HandleGettingLocation);
	}

}
