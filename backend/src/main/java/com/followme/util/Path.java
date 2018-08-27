package com.followme.util;

import lombok.Getter;

public class Path {
	public static class Web{
		@Getter public static final String GROUP = "/group/";
		@Getter public static final String TRIP = "/trip/";
		@Getter public static final String LEADER = "/leader/";
		@Getter public static final String TOKEN = "/token/";
		@Getter public static final String MEMBER = "/member/";
	}
}
