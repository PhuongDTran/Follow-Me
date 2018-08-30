package com.followme.messaging;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class MessagingController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	public static String getRegistrationToken(String memberId){
		try{
			MessagingDao messagingDao = new MessagingDao();
			return messagingDao.getRegistrationToken(memberId);
		}catch(SQLException ex){
			logger.error("error when creating GroupDao()." + ex.getMessage());
		}
		return null;
	}

}
