package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class RoomModel {
    private int roomId;
    private int cinemaId;
    private int capacity;
    private String name;
    private int digTypeId;
    private CinemaModel cinema;
    private DigitalTypeModel digType;
    private List<MovieSchedule> movieSchedule;
    private List<SeatModel> seat;

    public RoomModel() {
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public int getCinemaId() {
        return cinemaId;
    }

    public void setCinemaId(int cinemaId) {
        this.cinemaId = cinemaId;
    }

    public int getCapacity() {
        return capacity;
    }

    public void setCapacity(int capacity) {
        this.capacity = capacity;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getDigTypeId() {
        return digTypeId;
    }

    public void setDigTypeId(int digTypeId) {
        this.digTypeId = digTypeId;
    }

    public CinemaModel getCinema() {
        return cinema;
    }

    public void setCinema(CinemaModel cinema) {
        this.cinema = cinema;
    }

    public DigitalTypeModel getDigType() {
        return digType;
    }

    public void setDigType(DigitalTypeModel digType) {
        this.digType = digType;
    }

    public List<MovieSchedule> getMovieSchedule() {
        return movieSchedule;
    }

    public void setMovieSchedule(List<MovieSchedule> movieSchedule) {
        this.movieSchedule = movieSchedule;
    }

    public List<SeatModel> getSeat() {
        return seat;
    }

    public void setSeat(List<SeatModel> seat) {
        this.seat = seat;
    }
}
