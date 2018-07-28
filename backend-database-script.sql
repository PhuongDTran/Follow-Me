# create database
create database follow_me;
use follow_me;

#create tables
create table DeviceInfo( device_id varchar(50) not null,
							device_name varchar(50),
                            latitude double,
                            longitude double,
                            heading int(1),
                            speed int(1),
                            primary key(device_id));
create table GroupInfo( group_id varchar(50) not null,
							leader_id varchar(50),
                            number_of_members int(1),
                            primary key(group_id));