package com.example.luulac.cinemaapplication.data.models;

public class BookingDetailModel {

    private int bookingDetailId;
    private int bookingId;
    private int ticketId;
    private BookingTicketModel booking;
    private TicketModel ticket;

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

    public BookingTicketModel getBooking() {
        return booking;
    }

    public void setBooking(BookingTicketModel booking) {
        this.booking = booking;
    }

    public TicketModel getTicket() {
        return ticket;
    }

    public void setTicket(TicketModel ticket) {
        this.ticket = ticket;
    }
}
