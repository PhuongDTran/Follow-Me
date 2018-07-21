# create database
create database follow_me;
use follow_me;

#create tables
create table DeviceInfo( device_id varchar(50) not null,
							device_name varchar(50),
                            latitude double,
                            longitude double,
                            heading int,
                            speed int,
                            primary key(device_id));
