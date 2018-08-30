package com.followme.messaging;

import static com.followme.util.Release.release;

import java.lang.invoke.MethodHandles;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.followme.util.ConnectionManager;

class MessagingDao {

	private Connection conn = null;
	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());
	
	/**
	 * The constructor makes a connection to database
	 * @throws SQLException
	 */
	protected MessagingDao() throws SQLException{
		if(conn == null){
			conn = ConnectionManager.getInstance().getConnection();
			if (conn == null){
				throw new SQLException("Could not make a connection to database");
			}
		}
	}
	
	protected String getRegistrationToken(String memberId){
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		try {
			String sql = "SELECT token FROM MemberInfo WHERE member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString( 1, memberId);
			rs = pstmt.executeQuery();
			if(rs.next()){
				return rs.getString("token");
			}
		}catch (SQLException ex) {
			logger.error("getRegistrationToken() failed. " + ex.getMessage());
		}finally {
			release(pstmt, rs);
		}
		return null;
	}
	
}
