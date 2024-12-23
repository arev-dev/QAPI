create database QADB;
go
use QADB
go
create table Users(
	Id int primary key Identity(1,1) not null,
	Username varchar(50) not null UNIQUE,
	Password varchar(255) not null,
	CreatedAt datetime not null default GETDATE()
)
go
create table Posts(
	Id int primary key Identity(1,1) not null,
	UserId int foreign key references Users(Id),
	Title varchar(50) not null,
	Content varchar(500) not null,
	IsClosed bit not null default 0,
	CreatedAt datetime not null default GETDATE()
)
go
create table Comments(
	Id int primary key Identity(1,1) not null,
	UserId int foreign key references Users(Id),
	PostId int foreign key references Posts(Id),
	Content varchar(500) not null,
	CreatedAt datetime not null default GETDATE(),
);
