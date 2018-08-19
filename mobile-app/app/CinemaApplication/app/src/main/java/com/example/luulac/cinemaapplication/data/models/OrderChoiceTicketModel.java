package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class OrderChoiceTicketModel {
    private String groupCinemaName;
    private String cinemaName;
    private String timeShow;
    private String roomName;
    private String filmName;
    private String restricted;
    private String filmLength;
    private String digType;
    private String filmImage;
    private List<TypeOfSeatModel> typeOfSeats;

    public String getGroupCinemaName() {
        return groupCinemaName;
    }

    public void setGroupCinemaName(String groupCinemaName) {
        this.groupCinemaName = groupCinemaName;
    }

    public String getCinemaName() {
        return cinemaName;
    }

    public void setCinemaName(String cinemaName) {
        this.cinemaName = cinemaName;
    }

    public String getTimeShow() {
        return timeShow;
    }

    public void setTimeShow(String timeShow) {
        this.timeShow = timeShow;
    }

    public String getRoomName() {
        return roomName;
    }

    public void setRoomName(String roomName) {
        this.roomName = roomName;
    }

    public String getFilmName() {
        return filmName;
    }

    public void setFilmName(String filmName) {
        this.filmName = filmName;
    }

    public String getRestricted() {
        return restricted;
    }

    public void setRestricted(String restricted) {
        this.restricted = restricted;
    }

    public String getFilmLength() {
        return filmLength;
    }

    public void setFilmLength(String filmLength) {
        this.filmLength = filmLength;
    }

    public String getDigType() {
        return digType;
    }

    public void setDigType(String digType) {
        this.digType = digType;
    }

    public String getFilmImage() {
        return filmImage;
    }

    public void setFilmImage(String filmImage) {
        this.filmImage = filmImage;
    }
    public List<TypeOfSeatModel> getTypeOfSeats() {
        return typeOfSeats;
    }

    public void setTypeOfSeats(List<TypeOfSeatModel> typeOfSeats) {
        this.typeOfSeats = typeOfSeats;
    }
}
