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
import static com.followme.util.Release.release;

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
		}finally {
			release(pstmt, rs);
		}
		return false;
	}
	
	protected void addNewLocation(String groupId, String memberId, double latitude, double longitude, int heading, int speed){
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
			logger.error("addNewLocation() failed." + ex.getMessage());
		}finally {
			release(pstmt);
		}
	}

	protected Location getLocation( String groupId, String memberId) {
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		Location location = null;
		try {
			String sql = "SELECT latitude,longitude,speed,heading FROM TripInfo "
					+ "WHERE group_id=? AND member_id=? ORDER BY location_updated_at DESC LIMIT 1";
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
		}finally {
			release(pstmt, rs);
		}
		return location;
	}
	
	protected boolean removeLocations(String groupId, String memberId){
		PreparedStatement pstmt = null;
		try {
			String sql = "DELETE FROM TripInfo WHERE group_id=? AND member_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, groupId);
			pstmt.setString(2, memberId);
			return pstmt.executeUpdate()>0 ? true : false;
		}catch (SQLException ex){
			logger.error("removeLocations() failed." + ex.getMessage());
		}finally {
			release(pstmt);
		}
		return false;
	}
}
