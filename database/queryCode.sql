use [CinemaBookingDB];


create proc spGetMovieScheduleOfCinema
@cinemaId int,
@currentDate date
as
begin
	select ms.filmId as filmId,ms.timeId as timeId FROM MovieSchedule ms JOIN Room r ON ms.roomId = r.roomId
	where r.cinemaId = @cinemaId and ms.scheduleDate = @currentDate
	group by ms.filmId , ms.timeId
end

spGetMovieScheduleOfCinema 1
spGetMovieScheduleForDetailFilm 11, 1, 1, '2018-06-08'  


create proc spGetMovieScheduleForDetailFilm
@cinemaId int,
@filmId int,
@digTypeId int,
@currentDate date
as
begin
	select ms.timeId FROM MovieSchedule ms JOIN Room r ON ms.roomId = r.roomId
								JOIN Film f On f.filmId = ms.filmId
	                             JOIN DigitalType dt ON dt.digTypeId = r.digTypeId 					     
	where r.cinemaId = @cinemaId and ms.scheduleDate = @currentDate and f.filmId = @filmId and r.digTypeId = @digTypeId
	group by ms.timeId
end


create proc spGetCinemaHasScheduleInCurrentDate
@currentDate date,
@filmId int
as
begin
	select * 
	FROM Cinema 
	where cinemaId in (SELECT c.cinemaId FROM Cinema c JOIN Room r ON c.cinemaId = r.cinemaId
														JOIN MovieSchedule ms ON ms.roomId = r.roomId 
															JOIN Film f On f.filmId = ms.filmId
														where ms.scheduleDate = @currentDate and f.filmId = @filmId
														group by c.cinemaId)
end

DROP PROCEDURE spGetCinemaHasScheduleInCurrentDate;  
GO 

select * from MovieSchedule ms JOIN Room r ON ms.roomId = r.roomId
where ms.filmId = 3 and ms.timeId = 1 and ms.scheduleDate = '2018-06-08' and r.cinemaId = 1 

create proc spGetTicketByEmail
@email nvarchar(255)
as
begin
	select * from Ticket t JOIN BookingTicket b ON t.bookingId = b.bookingId
		where b.bookingId in (select customerId from Customer where email = @email)
end

DROP PROCEDURE spGetTicketByEmail;  
GO 



