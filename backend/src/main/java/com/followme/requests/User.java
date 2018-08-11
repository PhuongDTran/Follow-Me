package com.followme.requests;

import lombok.*;
import com.google.gson.*;

public class User {

	@Getter private String id;
	@Getter private String name;
	@Getter private double latitude;
	@Getter private double longitude;
	@Getter private int heading;
	@Getter private int speed;
	@Getter private String platform;
	
	public User(JsonObject jObject){
		id = jObject.get("id").getAsString();
		name = jObject.get("name").getAsString();
		latitude = jObject.get("lat").getAsDouble();
		longitude = jObject.get("lon").getAsDouble();
		heading = jObject.get("heading").getAsInt();
		speed = jObject.get("speed").getAsInt();
		platform = jObject.get("platform").getAsString();
	}
	
}
