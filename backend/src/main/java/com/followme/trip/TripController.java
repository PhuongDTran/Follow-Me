package com.followme.trip;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class TripController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	public static void addOrUpdateMember(String groupId,String memberId, double latitude, double longitude, int heading, int speed, boolean isNew){
		try{
			TripDao tripDao = new TripDao();
			if (isNew){
				tripDao.addTrip(groupId, memberId, latitude, longitude, heading, speed);
			} else{
				tripDao.update(groupId, memberId, latitude, longitude, heading, speed);
			}
		}catch(SQLException ex){
			logger.error("error when creating TripDao()." + ex.getMessage());
		}
	}

}
