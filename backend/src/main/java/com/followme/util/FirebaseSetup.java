package com.followme.util;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.lang.invoke.MethodHandles;
import java.util.Properties;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.google.auth.oauth2.GoogleCredentials;
import com.google.firebase.*;

public class FirebaseSetup {

	private static String serviceAccountFileName;
	final static Logger logger = LoggerFactory.getLogger(MethodHandles.lookup().lookupClass());

	//https://firebase.google.com/docs/admin/setup
	public static void initialize(){
		getServiceAccountFileName();
		try{
			FileInputStream serviceAccount = new FileInputStream(serviceAccountFileName);

			FirebaseOptions options = new FirebaseOptions.Builder()
					.setCredentials(GoogleCredentials.fromStream(serviceAccount))
					//.setDatabaseUrl omitted by Phuong Tran
					.build();

			FirebaseApp.initializeApp(options);
			
		} catch(FileNotFoundException ex){
			logger.debug(ex.getMessage());
		} catch(IOException ex){
			logger.debug(ex.getMessage());
		}
	}

	private static void getServiceAccountFileName(){
		Properties props = new Properties();
		InputStream input = null;
		try{
			input = FirebaseSetup.class.getResourceAsStream("/config/firebase.properties");
			props.load(input);
			serviceAccountFileName = props.getProperty("credentialsfilename");
			if (input != null) {
				input.close();
			}
		}catch(IOException ex){
			logger.error("loading config settings failed. " + ex.getMessage());
		}
	}
}

