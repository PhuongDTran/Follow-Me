package com.followme.trip;

import java.lang.invoke.MethodHandles;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.followme.util.ConnectionManager;

class TripDao {

	private Connection conn = null;
	private DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());
	
	/**
	 * The constructor makes a connection to database
	 * @throws SQLException
	 */
	protected TripDao() throws SQLException{
		if(conn == null){
			conn = ConnectionManager.getInstance().getConnection();
			if (conn == null){
				throw new SQLException("Could not make a connection to database");
			}
		}
	}
	
	protected boolean doesExist( String groupId, String memberId){
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		try {
			
			String sql = "SELECT * FROM TripInfo WHERE group_id =? AND member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, groupId);
			pstmt.setString(2, memberId);
			rs = pstmt.executeQuery();
			return rs.next() ? true : false;
		}catch (SQLException ex){
			logger.error("addTrip() failed." + ex.getMessage());
		}
		return false;
	}
	
	protected void addMemberToTrip(String groupId, String memberId, double latitude, double longitude, int heading, int speed){
		PreparedStatement pstmt = null;
		try {
			String sql = "INSERT INTO TripInfo VALUES(null, ?, ?, ?, ?, ?, ?,?)";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, groupId);
			pstmt.setString(2, memberId);
			pstmt.setDouble(3, latitude);
			pstmt.setDouble(4, longitude);
			pstmt.setInt(5, heading);
			pstmt.setInt(6, speed);
			pstmt.setString(7, LocalDateTime.now().format(formatter));
			pstmt.executeUpdate();
		}catch (SQLException ex){
			logger.error("addMemberToTrip() failed." + ex.getMessage());
		}
	}
	
	protected void update(String groupId, String memberId, double latitude, double longitude, int heading, int speed){
		PreparedStatement pstmt = null;
		try {
			String sql = "UPDATE TripInfo"
					+ " SET latitude=?, longitude=?, heading=?, speed=?, location_updated_at=?"
					+ " WHERE group_id=? AND member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setDouble(1, latitude);
			pstmt.setDouble(2, longitude);
			pstmt.setInt(3, heading);
			pstmt.setInt(4, speed);
			pstmt.setString(5, LocalDateTime.now().format(formatter));
			pstmt.setString(6, groupId);
			pstmt.setString(7, memberId);
			pstmt.executeUpdate();
		}catch (SQLException ex){
			logger.error("update() failed." + ex.getMessage());
		}
	}
	
	protected Location getLocation( String groupId, String memberId) {
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		Location location = null;
		try {
			String sql = "SELECT latitude,longitude,speed,heading FROM TripInfo "
					+ "WHERE group_id=? AND member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, groupId);
			pstmt.setString(2, memberId);
			rs = pstmt.executeQuery();
			if(rs.next()) {
				double lat = rs.getDouble("latitude");
				double lon = rs.getDouble("longitude");
				int speed = rs.getInt("speed");
				int heading = rs.getInt("heading");
				location = new Location(memberId, lat, lon, speed, heading);
			}
		}catch (SQLException ex){
			logger.error("getLocation() failed." + ex.getMessage());
		}
		return location;
	}
}
