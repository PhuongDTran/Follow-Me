package com.followme.group;

import java.lang.invoke.MethodHandles;
import java.sql.SQLException;

import org.apache.commons.lang3.RandomStringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class GroupController {

	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	public static void addGroup(String groupId, String leaderId){
		try{
			GroupDao groupDao = new GroupDao();
			groupDao.addGroup(groupId, leaderId);
		}catch(SQLException ex){
			logger.error("error when creating GroupDao()." + ex.getMessage());
		}
	}

	public static String generateGroupId(){
		String groupId = "";
		try{
			GroupDao groupDao = new GroupDao();
			do{
				//a 20-length string includes letters and numbers 
				groupId = RandomStringUtils.random(20, true, true);
			}while(	groupDao.doesExist(groupId));
			
		}catch(SQLException ex){
			logger.error("error when creating GroupDao()." + ex.getMessage());
		}
		
		return groupId;
	}
	
	public static String getLeaderId(String groupId){
		String leaderId = "";
		try{
			GroupDao groupDao = new GroupDao();
			leaderId = groupDao.getLeaderId(groupId);
		}catch(SQLException ex){
			logger.error("getLeaderId() error." + ex.getMessage());
		}
		return leaderId;
	}
	
	public static boolean remove(String groupId){
		try{
			GroupDao groupDao = new GroupDao();
			return groupDao.remove(groupId);
		}catch(SQLException ex){
			logger.error("remove() error." + ex.getMessage());
		}
		return false;
	}
}
