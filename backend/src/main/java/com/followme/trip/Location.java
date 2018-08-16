package com.followme.trip;

import lombok.*;

public class Location {

	@Getter private String id;
	@Getter private double latitude;
	@Getter private double longitude;
	@Getter private int speed;
	@Getter private int heading;
	
	public Location(String id, double latitude, double longitude, int speed, int heading) {
		this.id = id;
		this.latitude = latitude;
		this.longitude = longitude;
		this.speed = speed;
		this.heading = heading;
	}
}
