package com.followme;
import static spark.Spark.get;
import static spark.Spark.port;
import static spark.Spark.post;
import static spark.Spark.staticFiles;
import static spark.debug.DebugScreen.enableDebugScreen;

import com.followme.requests.RequestHandlers;
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
		post(Path.Web.GROUP,					RequestHandlers.groupIdRequestHandler);
		get(Path.Web.TRIP, 	"application/json" ,RequestHandlers.getLocationHandler);
		post(Path.Web.TRIP,						RequestHandlers.updateLocationHandler);
		post(Path.Web.MEMBER, 					RequestHandlers.addMemberHandler);
		get(Path.Web.LEADER, 					RequestHandlers.getLeaderIdHandler);
		post(Path.Web.TOKEN, 					RequestHandlers.updateTokenHandler);
		get(Path.Web.ENDTRIP,						RequestHandlers.removeLocationsHandler);
	}

}
