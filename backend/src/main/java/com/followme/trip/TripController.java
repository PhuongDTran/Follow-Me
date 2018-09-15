package com.followme.trip;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class TripController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	/*
	 * update location.<br> 
	 * If the member for a specific group does not exist, that member will be added along with location info.<br>
	 * If the member already exists, only location info updated 
	 */
	public static void updateLocation(String groupId,String memberId, double latitude, double longitude, int heading, int speed){
		groupId = groupId.trim();
		memberId = memberId.trim();
		try{
			TripDao tripDao = new TripDao();
			tripDao.addNewLocation(groupId, memberId, latitude, longitude, heading, speed);
		}catch(SQLException ex){
			logger.error("error when creating TripDao()." + ex.getMessage());
		}
	}

	public static Location getLocation( String groupId, String memberId) {
		Location location = null;
		groupId = groupId.trim();
		memberId = memberId.trim();
		try {
			TripDao tripDao = new TripDao();
			location = tripDao.getLocation(groupId, memberId);
		} catch(SQLException ex){
			logger.error("error when creating getLocation()." + ex.getMessage());
		}
		return location;
	}

}
