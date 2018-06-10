use [CinemaBookingDB];


create proc spGetMovieScheduleOfCinema
@cinemaId int
as
begin
	select ms.filmId as filmId,ms.timeId as timeId FROM MovieSchedule ms JOIN Room r ON ms.roomId = r.roomId
	where r.cinemaId = @cinemaId   
	group by ms.filmId , ms.timeId
end

spGetMovieScheduleOfCinema 1

select ms.scheduleId as scheduleId,ms.filmId as filmId,ms.timeId as timeId FROM MovieSchedule ms JOIN Room r ON ms.roomId = r.roomId
	where r.cinemaId = 1 and ms.scheduleDate = '2018-06-08'
	group by ms.filmId , ms.timeId, ms.scheduleId

DROP PROCEDURE spGetMovieScheduleOfCinema;  
GO 