package com.followme.user;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class UserController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	public static void addOrUpdateUser(String id, String userName,String platform){
		try{
			UserDao userDao = new UserDao();
			if(!userDao.doesExist(id)){

				userDao.addNewUser(id, userName, platform);
			}else{
				userDao.updateUserName(id, userName);
			}
		}catch(SQLException ex){
			logger.error("error when creating MemberDao()." + ex.getMessage());
		}
	}
}
