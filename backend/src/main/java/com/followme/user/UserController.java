package com.followme.user;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class UserController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	public static void addMember(String memberId, String memberName,String platform){
		try{
			UserDao memberDao = new UserDao();
			if(!memberDao.doesExist(memberId)){

				memberDao.addNewUser(memberId, memberName, platform);
			}else{
				memberDao.updateMemberName(memberId, memberName);
			}
		}catch(SQLException ex){
			logger.error("error when creating MemberDao()." + ex.getMessage());
		}
	}
}
