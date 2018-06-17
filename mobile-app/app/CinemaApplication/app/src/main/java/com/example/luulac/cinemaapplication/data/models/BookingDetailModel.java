package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;

public class BookingDetailModel implements Serializable{

    private int bookingDetailId;
    private int bookingId;
    private int ticketId;

    public BookingDetailModel() {
    }

    public BookingDetailModel(int ticketId) {
        this.ticketId = ticketId;
    }

    public int getBookingDetailId() {
        return bookingDetailId;
    }

    public void setBookingDetailId(int bookingDetailId) {
        this.bookingDetailId = bookingDetailId;
    }

    public int getBookingId() {
        return bookingId;
    }

    public void setBookingId(int bookingId) {
        this.bookingId = bookingId;
    }

    public int getTicketId() {
        return ticketId;
    }

    public void setTicketId(int ticketId) {
        this.ticketId = ticketId;
    }
}
