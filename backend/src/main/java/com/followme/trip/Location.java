package com.followme.trip;

import lombok.*;

public class Location {

	@Getter private double latitude;
	@Getter private double longitude;
	@Getter private int speed;
	@Getter private int heading;
	
	public Location(double latitude, double longitude, int speed, int heading) {
		this.latitude = latitude;
		this.longitude = longitude;
		this.speed = speed;
		this.heading = heading;
	}
}
