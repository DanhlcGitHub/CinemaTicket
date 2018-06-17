package com.example.luulac.cinemaapplication.data.models.tranfers;

import java.io.Serializable;

public class ScheduleTranferModel implements Serializable{

    private int quantityTicket;
    private Double totalPrice;
    private String groupCinemaName;
    private String showTime;
    private String cinemaName;
    private String roomName;
    private String filmName;
    private String restricted;
    private String filmLength;
    private String digType;
    private String filmImage;

    public ScheduleTranferModel() {
    }

    public ScheduleTranferModel(int quantityTicket, Double totalPrice, String groupCinemaName, String showTime, String cinemaName, String roomName, String filmName, String restricted, String filmLength, String digType, String filmImage) {
        this.quantityTicket = quantityTicket;
        this.totalPrice = totalPrice;
        this.groupCinemaName = groupCinemaName;
        this.showTime = showTime;
        this.cinemaName = cinemaName;
        this.roomName = roomName;
        this.filmName = filmName;
        this.restricted = restricted;
        this.filmLength = filmLength;
        this.digType = digType;
        this.filmImage = filmImage;
    }

    public int getQuantityTicket() {
        return quantityTicket;
    }

    public void setQuantityTicket(int quantityTicket) {
        this.quantityTicket = quantityTicket;
    }

    public Double getTotalPrice() {
        return totalPrice;
    }

    public void setTotalPrice(Double totalPrice) {
        this.totalPrice = totalPrice;
    }

    public String getGroupCinemaName() {
        return groupCinemaName;
    }

    public void setGroupCinemaName(String groupCinemaName) {
        this.groupCinemaName = groupCinemaName;
    }

    public String getShowTime() {
        return showTime;
    }

    public void setShowTime(String showTime) {
        this.showTime = showTime;
    }

    public String getCinemaName() {
        return cinemaName;
    }

    public void setCinemaName(String cinemaName) {
        this.cinemaName = cinemaName;
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
}
