package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;

public class AccountPurchasedModel implements Serializable{

    private int bookingTicketId;
    private String filmImage;
    private String filmName;
    private String groupCinemaName;
    private String cinemaName;
    private String showTime;
    private String date;
    private String roomName;
    private int scheduleId;
    private int roomId;
    private Double totalPrice;
    private String digType;
    private int restricted;
    private String stringSeats;
    private String email;
    private String phone;
    private boolean isCanChange;

    public int getBookingTicketId() {
        return bookingTicketId;
    }

    public void setBookingTicketId(int bookingTicketId) {
        this.bookingTicketId = bookingTicketId;
    }

    public String getFilmImage() {
        return filmImage;
    }

    public void setFilmImage(String filmImage) {
        this.filmImage = filmImage;
    }

    public String getFilmName() {
        return filmName;
    }

    public void setFilmName(String filmName) {
        this.filmName = filmName;
    }

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

    public String getShowTime() {
        return showTime;
    }

    public void setShowTime(String showTime) {
        this.showTime = showTime;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public String getRoomName() {
        return roomName;
    }

    public void setRoomName(String roomName) {
        this.roomName = roomName;
    }

    public int getScheduleId() {
        return scheduleId;
    }

    public void setScheduleId(int scheduleId) {
        this.scheduleId = scheduleId;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public Double getTotalPrice() {
        return totalPrice;
    }

    public void setTotalPrice(Double totalPrice) {
        this.totalPrice = totalPrice;
    }

    public String getDigType() {
        return digType;
    }

    public void setDigType(String digType) {
        this.digType = digType;
    }

    public int getRestricted() {
        return restricted;
    }

    public void setRestricted(int restricted) {
        this.restricted = restricted;
    }

    public String getStringSeats() {
        return stringSeats;
    }

    public void setStringSeats(String stringSeats) {
        this.stringSeats = stringSeats;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public boolean isCanChange() {
        return isCanChange;
    }

    public void setCanChange(boolean canChange) {
        isCanChange = canChange;
    }
}
