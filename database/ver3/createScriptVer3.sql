DROP DATABASE CinemaBookingDB;
CREATE DATABASE CinemaBookingDB;

use [CinemaBookingDB];

/*use [Test]*/


CREATE TABLE Film (
    filmId int IDENTITY(1,1) PRIMARY KEY,
    name nvarchar(255),
	dateRelease datetime not null,
    restricted int, 
    filmLength int,
    imdb float,
    digTypeId nvarchar(50),
	author nvarchar(255),
	movieGenre nvarchar(50),
	filmContent nvarchar(1000),
	actorList nvarchar(255),
	countries nvarchar(255),
	trailerLink nvarchar(500),
	posterPicture nvarchar(255),
	additionPicture nvarchar(1000),
	filmStatus int,
	ticketSold int,
);

CREATE TABLE DigitalType (
    digTypeId int IDENTITY(1,1) PRIMARY KEY,
    name nvarchar(255),
);

CREATE TABLE GroupCinema (
    GroupId int IDENTITY(1,1) PRIMARY KEY,
	logoImg nvarchar(50),
    name nvarchar(255),
	address nvarchar(255),
	email nvarchar(255),
	phone nvarchar(40),
);

CREATE TABLE PartnerAccount (
    partnerId nvarchar(255) PRIMARY KEY,
	partnerPassword nvarchar(255),
	partnerName nvarchar(255),
	phone nvarchar(255),
	email nvarchar(255),
	groupOfCinemaId int,
	isAvailable bit,
);


CREATE TABLE Cinema (
    cinemaId int IDENTITY(1,1) PRIMARY KEY,
	cinemaName nvarchar(255),
	groupId int,
    profilePicture nvarchar(255),
    cinemaAddress nvarchar(255),
	phone nvarchar(200),
	email nvarchar(200),
	openTime nvarchar(200),
	introduction nvarchar(1000)
);

CREATE TABLE CinemaManager (
    managerId nvarchar(255) PRIMARY KEY,
	managerPassword nvarchar(255),
	managerName nvarchar(255),
	phone nvarchar(255),
	email nvarchar(255),
	cinemaId int,
	isAvailable bit,
);



CREATE TABLE ShowTime (
    timeId int IDENTITY(1,1) PRIMARY KEY,
    startTime nvarchar(10),
    endTime nvarchar(10),
	startTimeDouble float,
);


CREATE TABLE Room (
    roomId int IDENTITY(1,1) PRIMARY KEY,
	cinemaId int,
    capacity int,
    name nvarchar(15),
	digTypeId int,
	matrixSizeX int,
	matrixSizeY int,
);

CREATE TABLE Seat (
    seatId int IDENTITY(1,1) PRIMARY KEY,
	typeSeatId int,
    roomId int,
    px int,
	py int,
	locationX int,
	locationY int,
);

CREATE TABLE TypeOfSeat (
    typeSeatId int IDENTITY(1,1) PRIMARY KEY,
    typeName nvarchar(20),
	capacity int,
	groupId int,
	isPrimary bit,
	price float
);

CREATE TABLE MovieSchedule (
    scheduleId int IDENTITY(1,1) PRIMARY KEY,
	filmId int not null,
    timeId int not null,
    roomId int not null,
	scheduleDate datetime not null,
);


CREATE TABLE Ticket (
    ticketId int IDENTITY(1,1) PRIMARY KEY,
	bookingId int,
	scheduleId int,
	seatId int,
	paymentCode nvarchar(50),
	qrCode nvarchar(255),
	ticketStatus nvarchar(20),
	resellDescription nvarchar(50),
	/*ticketTimeout date,*/
	price float,
);

CREATE TABLE BookingTicket (
    bookingId int IDENTITY(1,1) PRIMARY KEY,
	customerId int,
	paymentMethodId int,
	paymentCode nvarchar(255),
	qrCode nvarchar(255),
	quantity int,
	bookingDate datetime,
);

CREATE TABLE Customer (
    customerId int IDENTITY(1,1) PRIMARY KEY,
	userId nvarchar(255) null,
	phone nvarchar(255) null,
	email nvarchar(255) null,
);

CREATE TABLE UserAccount (
    userId nvarchar(255) PRIMARY KEY,
	userPassword nvarchar(255),
	phone nvarchar(255),
	email nvarchar(255),
);

CREATE TABLE AdminAccount (
    adminId nvarchar(255) PRIMARY KEY,
	adminPassword nvarchar(255),
	phone nvarchar(255),
	email nvarchar(255),
);

/*CREATE TABLE Promotion (
    promotionId int IDENTITY(1,1) PRIMARY KEY,
	cinemaId int,
	urlDocument nvarchar(255),
);*/


/* ------------------------------------------------ */
ALTER TABLE MovieSchedule ADD CONSTRAINT FKMovieScheduleFilm001 FOREIGN KEY (filmId) REFERENCES Film (filmId);
ALTER TABLE MovieSchedule ADD CONSTRAINT FKMovieScheduleRoom001 FOREIGN KEY (roomId) REFERENCES Room (roomId);
ALTER TABLE MovieSchedule ADD CONSTRAINT FKMovieScheduleShowTime001 FOREIGN KEY (timeId) REFERENCES ShowTime (timeId);

ALTER TABLE Room ADD CONSTRAINT FKRoomCinema001 FOREIGN KEY (cinemaId) REFERENCES Cinema (cinemaId);
ALTER TABLE Cinema ADD CONSTRAINT FKCinemaGroupCinema001 FOREIGN KEY (groupId) REFERENCES GroupCinema (groupId);
ALTER TABLE Room ADD CONSTRAINT FKRoomDigitalType001 FOREIGN KEY (digTypeId) REFERENCES DigitalType (digTypeId);
ALTER TABLE Seat ADD CONSTRAINT FKSeatRoom001 FOREIGN KEY (roomId) REFERENCES Room (roomId);
ALTER TABLE Seat ADD CONSTRAINT FKSeatTypeOfSeat001 FOREIGN KEY (typeSeatId) REFERENCES TypeOfSeat (typeSeatId);
ALTER TABLE TypeOfSeat ADD CONSTRAINT FKTypeOfSeatGroupCinema001 FOREIGN KEY (groupId) REFERENCES GroupCinema (groupId);

ALTER TABLE Ticket ADD CONSTRAINT FKTicketMovieSchedule001 FOREIGN KEY (scheduleId) REFERENCES MovieSchedule (scheduleId);
ALTER TABLE Ticket ADD CONSTRAINT FKTicketSeat001 FOREIGN KEY (seatId) REFERENCES Seat (seatId);

ALTER TABLE BookingTicket ADD CONSTRAINT FKBookingTicketCustomer001 FOREIGN KEY (customerId) REFERENCES Customer (customerId);
ALTER TABLE Ticket ADD CONSTRAINT FKTicketBookingTicket001 FOREIGN KEY (bookingId) REFERENCES BookingTicket (bookingId);

ALTER TABLE PartnerAccount ADD CONSTRAINT FKPartnerAccountGroupCinema001 FOREIGN KEY (groupOfCinemaId) REFERENCES GroupCinema (groupId);
ALTER TABLE CinemaManager ADD CONSTRAINT FKCinemaManagerCinema001 FOREIGN KEY (cinemaId) REFERENCES Cinema (cinemaId);

/*ALTER TABLE BookingTicket
ADD qrCode nvarchar(255);

ALTER TABLE Film
DROP COLUMN isAvailable;

DROP TABLE Promotion;*/











