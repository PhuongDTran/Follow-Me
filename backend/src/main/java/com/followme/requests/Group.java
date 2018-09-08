package com.followme.requests;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import lombok.*;
public class Group {

	@Getter @Setter private String leaderId;
	private Map<String,String> members;
	
	public Group(){
		members = new HashMap<String,String>();
	}
	
	public void addRegistrationToken(String memberId, String token){
			members.put(memberId, token);
	}
	
	public Set<String> getMembers(){
		return members.keySet();
	}
	
	public List<String> getRegistrationToken(){
		List<String> tokens = new ArrayList<String>();
		tokens.addAll(members.values());
		return tokens;
	}
	
	public String getRegistrationToken(String memberId){
		if(members.containsKey(memberId)){
			return members.get(memberId);
		}
		return null;
	}
}
