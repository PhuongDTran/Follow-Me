package com.followme.requests;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import lombok.Getter;
import lombok.Setter;
public class Group {

	@Getter @Setter private String leaderId;
	private Map<String,String> members;

	public Group(){
		members = new HashMap<String,String>();
	}

	public boolean doesContain(String memberId){
		return members.containsKey(memberId);
	}

	public void addToken(String memberId, String token){
		members.put(memberId, token);
	}

	public List<String> getMemberTokens(){
		List<String> tokens = new ArrayList<String>();
		for(String id : members.keySet()) {
			if (!id.equals(leaderId)) {
				tokens.add(members.get(id));
			}
		}
		return tokens;
	}

	public String getLeaderToken(){
		return members.get(leaderId);
	}
	
	public String getToken(String memberId){
		return members.get(memberId);
	}
}
