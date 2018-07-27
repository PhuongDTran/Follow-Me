package com.followme.groupid;

import static com.followme.util.Release.release;

import java.lang.invoke.MethodHandles;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.followme.util.ConnectionManager;

public class GroupIdDao {

	private Connection conn = null;
	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());
	
	/**
	 * The constructor makes a connection to database
	 * @throws SQLException
	 */
	public GroupIdDao() throws SQLException{
		if(conn == null){
			conn = ConnectionManager.getInstance().getConnection();
			if (conn == null){
				throw new SQLException("Could not make a connection to database");
			}
		}
	}
	
	/**
	 * Find the specified group id in GroupInfo table in database 
	 * @param groupId id looked up
	 * @return <i>true</i> if found</br>
	 * <i>false</i> if not. 
	 */
	public boolean doesExist(String groupId){
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		try {
			String sql = "SELECT group_id FROM GroupInfo WHERE group_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString( 1, groupId);
			rs = pstmt.executeQuery();
			return rs.next();
		}catch (SQLException ex) {
			logger.error("doesExist() failed. " + ex.getMessage());
		}finally {
			release(pstmt, rs);
		}
		return false;
	}
	
	/**
	 * add group id to corresponding table in mysql
	 * @param groupId
	 */
	public void addGroupId(String groupId){
		PreparedStatement pstmt = null;
		try {
			String sql = "INSERT INTO GroupInfo( group_id) VALUES (?)";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, groupId);
			pstmt.executeUpdate();
		}catch (SQLException ex) {
			logger.error("getInvestment() failed. " + ex.getMessage());
		}finally {
			release(pstmt);
		}
	}
	
	/**
	 * close connection to database
	 */
	public void releaseConnection(){
		ConnectionManager.getInstance().releaseConnection(conn);
	}
}
