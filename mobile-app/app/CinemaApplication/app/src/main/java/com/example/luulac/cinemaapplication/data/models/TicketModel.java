package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;
import java.util.Date;
import java.util.List;

public class TicketModel implements Serializable {
    private int bookingId;
    private int ticketId;
    private int scheduleId;
    private int seatId;
    private String paymentCode;
    private String qrCode;
    private String ticketStatus;
    private double price;
    private String seatPosition;
    private int cinemaId;
    private int indexDate;
    private int filmId;
    private boolean isCanChange;
    private String cinemaName;
    private String filmName;
    private String groupCinemaName;

    public TicketModel() {
    }

    public TicketModel(int ticketId) {
        this.ticketId = ticketId;
    }

    public TicketModel(int ticketId, int scheduleId, int seatId, double price) {
        this.ticketId = ticketId;
        this.scheduleId = scheduleId;
        this.seatId = seatId;
        this.price = price;
    }

    public int getTicketId() {
        return ticketId;
    }

    public void setTicketId(int ticketId) {
        this.ticketId = ticketId;
    }

    public int getScheduleId() {
        return scheduleId;
    }

    public void setScheduleId(int scheduleId) {
        this.scheduleId = scheduleId;
    }

    public int getSeatId() {
        return seatId;
    }

    public void setSeatId(int seatId) {
        this.seatId = seatId;
    }

    public String getQrCode() {
        return qrCode;
    }

    public void setQrCode(String qrCode) {
        this.qrCode = qrCode;
    }

    public String getTicketStatus() {
        return ticketStatus;
    }

    public void setTicketStatus(String ticketStatus) {
        this.ticketStatus = ticketStatus;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public String getSeatPosition() {
        return seatPosition;
    }

    public void setSeatPosition(String seatPosition) {
        this.seatPosition = seatPosition;
    }

    public int getBookingId() {
        return bookingId;
    }

    public void setBookingId(int bookingId) {
        this.bookingId = bookingId;
    }

    public int getCinemaId() {
        return cinemaId;
    }

    public void setCinemaId(int cinemaId) {
        this.cinemaId = cinemaId;
    }

    public int getIndexDate() {
        return indexDate;
    }

    public void setIndexDate(int indexDate) {
        this.indexDate = indexDate;
    }

    public int getFilmId() {
        return filmId;
    }

    public void setFilmId(int filmId) {
        this.filmId = filmId;
    }

    public String getPaymentCode() {
        return paymentCode;
    }

    public void setPaymentCode(String paymentCode) {
        this.paymentCode = paymentCode;
    }

    public boolean isCanChange() {
        return isCanChange;
    }

    public void setCanChange(boolean canChange) {
        isCanChange = canChange;
    }

    public String getCinemaName() {
        return cinemaName;
    }

    public void setCinemaName(String cinemaName) {
        this.cinemaName = cinemaName;
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
}
