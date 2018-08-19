package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class MovieScheduleModel {

    private int scheduleId;
    private int filmId;
    private int timeId;
    private int roomId;
    private FilmModel film;
    private RoomModel roomModel;
    private ShowTimeModel shoTime;
    private List<TicketModel> tickets;

    public MovieScheduleModel() {
    }

    public int getScheduleId() {
        return scheduleId;
    }

    public void setScheduleId(int scheduleId) {
        this.scheduleId = scheduleId;
    }

    public int getFilmId() {
        return filmId;
    }

    public void setFilmId(int filmId) {
        this.filmId = filmId;
    }

    public int getTimeId() {
        return timeId;
    }

    public void setTimeId(int timeId) {
        this.timeId = timeId;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public FilmModel getFilm() {
        return film;
    }

    public void setFilm(FilmModel film) {
        this.film = film;
    }

    public RoomModel getRoomModel() {
        return roomModel;
    }

    public void setRoomModel(RoomModel roomModel) {
        this.roomModel = roomModel;
    }

    public ShowTimeModel getShoTime() {
        return shoTime;
    }

    public void setShoTime(ShowTimeModel shoTime) {
        this.shoTime = shoTime;
    }

    public List<TicketModel> getTickets() {
        return tickets;
    }

    public void setTickets(List<TicketModel> tickets) {
        this.tickets = tickets;
    }
}
