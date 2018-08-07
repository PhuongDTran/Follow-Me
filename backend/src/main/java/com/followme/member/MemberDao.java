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

public class MemberDao {

	private Connection conn = null;
	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());
	
	/**
	 * The constructor makes a connection to database
	 * @throws SQLException
	 */
	public MemberDao() throws SQLException{
		if(conn == null){
			conn = ConnectionManager.getInstance().getConnection();
			if (conn == null){
				throw new SQLException("Could not make a connection to database");
			}
		}
	}
	
	public boolean doesExist(String memberId){
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		try {
			String sql = "SELECT member_id FROM MemberInfo WHERE member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString( 1, memberId);
			rs = pstmt.executeQuery();
			return rs.next();
		}catch (SQLException ex) {
			logger.error("doesExist() failed. " + ex.getMessage());
		}finally {
			release(pstmt, rs);
		}
		return false;
	}
	
	public void addMember(String memberId,String memberName, String platform){
		PreparedStatement pstmt = null;
		try {
			String sql = "INSERT INTO MemberInfo VALUES (?,?,?)";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, memberId);
			pstmt.setString(2, memberName);
			pstmt.setString(3, platform);
			pstmt.executeUpdate();
		}catch (SQLException ex) {
			logger.error("addMember() failed. " + ex.getMessage());
		}finally {
			release(pstmt);
		}
	}
	
	public void updateMemberName(String memberId, String memberName){
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
