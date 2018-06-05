DROP DATABASE CinemaBookingDB;
CREATE DATABASE CinemaBookingDB;

use [CinemaBookingDB];


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
	filmStatus int
);

CREATE TABLE DigitalType (
    digTypeId int IDENTITY(1,1) PRIMARY KEY,
    name nvarchar(255),
);

CREATE TABLE GroupCinema (
    GroupId int IDENTITY(1,1) PRIMARY KEY,
	logoImg nvarchar(50),
    name nvarchar(255),
);

CREATE TABLE PartnerAccount (
    partnerId nvarchar(255) PRIMARY KEY,
	partnerPassword nvarchar(255),
	phone nvarchar(255),
	email nvarchar(255),
	groupOfCinemaId int,
);

CREATE TABLE Cinema (
    cinemaId int IDENTITY(1,1) PRIMARY KEY,
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
	phone nvarchar(255),
	email nvarchar(255),
	cinemaId int,
);

CREATE TABLE ShowTime (
    timeId int IDENTITY(1,1) PRIMARY KEY,
    startTime datetime,
    endTime datetime,
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
	locationY nvarchar(2),
);

CREATE TABLE TypeOfSeat (
    typeSeatId int IDENTITY(1,1) PRIMARY KEY,
    typeName int,
	capacity int,
	groupId int,
	price float
);

CREATE TABLE MovieSchedule (
    scheduleId int IDENTITY(1,1) PRIMARY KEY,
	filmId int,
    timeId int,
    roomId int,
);

CREATE TABLE Ticket (
    ticketId int IDENTITY(1,1) PRIMARY KEY,
	scheduleId int,
	seatId int,
	paymentCode nvarchar(50),
	qrCode int,
	ticketStatus nvarchar(20),
	price float,
);

CREATE TABLE BookingTicket (
    bookingId int IDENTITY(1,1) PRIMARY KEY,
	customerId int,
	paymentMethodId int,
	quantity int,
	bookingDate datetime,
);

CREATE TABLE BookingDetail (
    bookingDetailId int IDENTITY(1,1) PRIMARY KEY,
	bookingId int,
	ticketId int,
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

CREATE TABLE BankAccount (
    cardNumber nvarchar(50) PRIMARY KEY,
	ownerName nvarchar(255),
	expDate date,
	email nvarchar(255),
	BankId int,
);

CREATE TABLE BankList (
    bankId int IDENTITY(1,1) PRIMARY KEY,
	bankName nvarchar(255),
	imgLogo nvarchar(255),
);

CREATE TABLE Promotion (
    promotionId int IDENTITY(1,1) PRIMARY KEY,
	cinemaId int,
	urlDocument nvarchar(255),
);

CREATE TABLE News (
    newsId int IDENTITY(1,1) PRIMARY KEY,
	filmId int,
	urlDocument nvarchar(255),
);

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

ALTER TABLE BookingDetail ADD CONSTRAINT FKBookingDetailTicket001 FOREIGN KEY (ticketId) REFERENCES Ticket (ticketId);
ALTER TABLE BookingDetail ADD CONSTRAINT FKBookingDetailBookingTicket001 FOREIGN KEY (bookingId) REFERENCES BookingTicket (bookingId);
ALTER TABLE BookingTicket ADD CONSTRAINT FKBookingTicketCustomer001 FOREIGN KEY (customerId) REFERENCES Customer (customerId);

ALTER TABLE News ADD CONSTRAINT FKNewsFilm001 FOREIGN KEY (filmId) REFERENCES Film (filmId);
ALTER TABLE Promotion ADD CONSTRAINT FKPromotionCinema001 FOREIGN KEY (cinemaId) REFERENCES Cinema (cinemaId);
ALTER TABLE PartnerAccount ADD CONSTRAINT FKPartnerAccountGroupCinema001 FOREIGN KEY (groupOfCinemaId) REFERENCES GroupCinema (groupId);
ALTER TABLE CinemaManager ADD CONSTRAINT FKCinemaManagerCinema001 FOREIGN KEY (cinemaId) REFERENCES Cinema (cinemaId);

ALTER TABLE Film
ADD filmStatus int;

ALTER TABLE Film
DROP COLUMN isAvailable;











