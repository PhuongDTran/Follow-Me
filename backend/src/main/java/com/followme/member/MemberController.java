package com.followme.member;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class MemberController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	public static void addOrUpdateUser(String id, String name, String platform){
		try{
			MemberDao memberDao = new MemberDao();
			if(!memberDao.doesExist(id)){

				memberDao.addNewMember(id, name, platform);
			}else{
				memberDao.updateName(id, name);
			}
		}catch(SQLException ex){
			logger.error("error when creating MemberDao()." + ex.getMessage());
		}
	}
	
	public static void updateToken(String id, String token){
		try{
			MemberDao memberDao = new MemberDao();
			if(!memberDao.doesExist(id)){
				memberDao.updateToken(id, token);
			}
		}catch(SQLException ex){
			logger.error("error when creating MemberDao()." + ex.getMessage());
		}
	}
}
