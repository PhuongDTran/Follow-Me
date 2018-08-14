package com.followme.trip;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class TripController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	public static void addOrUpdateMember(String groupId,String memberId, double latitude, double longitude, int heading, int speed, boolean updateLocationOnly){
		try{
			TripDao tripDao = new TripDao();
			if (!updateLocationOnly){
				tripDao.addTrip(groupId, memberId, latitude, longitude, heading, speed);
			} else{
				tripDao.update(groupId, memberId, latitude, longitude, heading, speed);
			}
		}catch(SQLException ex){
			logger.error("error when creating TripDao()." + ex.getMessage());
		}
	}
	
	public static Location getLocation( String groupId, String memberId) {
		Location location = null;
		try {
			TripDao tripDao = new TripDao();
			location = tripDao.getLocation(groupId, memberId);
		} catch(SQLException ex){
			logger.error("error when creating getLocation()." + ex.getMessage());
		}
		return location;
	}

}
