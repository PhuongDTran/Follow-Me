package com.followme.member;

import static com.followme.util.Release.release;

import java.lang.invoke.MethodHandles;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.followme.util.ConnectionManager;

class MemberDao {

	private Connection conn = null;
	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());
	
	/**
	 * The constructor makes a connection to database
	 * @throws SQLException
	 */
	protected MemberDao() throws SQLException{
		if(conn == null){
			conn = ConnectionManager.getInstance().getConnection();
			if (conn == null){
				throw new SQLException("Could not make a connection to database");
			}
		}
	}
	
	public boolean doesExist(String id){
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		try {
			String sql  = "SELECT member_id FROM MemberInfo WHERE member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString( 1, id);
			rs = pstmt.executeQuery();
			return rs.next();
		}catch (SQLException ex) {
			logger.error("doesExist() failed. " + ex.getMessage());
		}finally {
			release(pstmt, rs);
		}
		return false;
	}
	
	protected void addNewMember(String id, String memberName, String platform) {
		PreparedStatement pstmt = null;
		try {
			String sql = "INSERT INTO MemberInfo(member_id,member_name,platform) VALUES (?,?,?)";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, id);
			pstmt.setString(2, memberName);
			pstmt.setString(3, platform);
			pstmt.executeUpdate();
		}catch (SQLException ex) {
			logger.error("addNewMember() failed. " + ex.getMessage());
		}finally {
			release(pstmt);
		}
	}
	
	protected void updateToken(String memberId, String token){
		PreparedStatement pstmt = null;
		try {
			String sql = "UPDATE MemberInfo SET token=? WHERE member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, token);
			pstmt.setString(2, memberId);
			pstmt.executeUpdate();
		}catch (SQLException ex){
			logger.error("updateToken() failed." + ex.getMessage());
		}finally {
			release(pstmt);
		}
	}

	protected String getToken( String memberId){
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		try {
			String sql = "SELECT token FROM MemberInfo WHERE member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, memberId);
			rs = pstmt.executeQuery();
			if(rs.next()){
				return rs.getString("token");
			}
		}catch (SQLException ex){
			logger.error("getToken() failed." + ex.getMessage());
		}finally {
			release(pstmt,rs);
		}
		return null;
	}
	
	protected void updateName(String memberId,  String memberName){
		PreparedStatement pstmt = null;
		try {
			String sql = "UPDATE MemberInfo SET member_name=? WHERE member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, memberName);
			pstmt.setString(2, memberId);
			pstmt.executeUpdate();
		}catch (SQLException ex){
			logger.error("updateMemberName() failed." + ex.getMessage());
		}finally {
			release(pstmt);
		}
	}
	
}
