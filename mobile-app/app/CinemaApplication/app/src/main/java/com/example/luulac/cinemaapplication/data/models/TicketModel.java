package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class TicketModel {
    private int ticketId;
    private int scheduleId;
    private int seatId;
    private String paymentCode;
    private int qrCode;
    private String ticketStatus;
    private double price;
    private MovieSchedule schedule;
    private SeatModel seatModel;
    private List<BookingDetailModel> bookingDetail;

    public TicketModel() {
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

    public String getPaymentCode() {
        return paymentCode;
    }

    public void setPaymentCode(String paymentCode) {
        this.paymentCode = paymentCode;
    }

    public int getQrCode() {
        return qrCode;
    }

    public void setQrCode(int qrCode) {
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

    public MovieSchedule getSchedule() {
        return schedule;
    }

    public void setSchedule(MovieSchedule schedule) {
        this.schedule = schedule;
    }

    public SeatModel getSeatModel() {
        return seatModel;
    }

    public void setSeatModel(SeatModel seatModel) {
        this.seatModel = seatModel;
    }

    public List<BookingDetailModel> getBookingDetail() {
        return bookingDetail;
    }

    public void setBookingDetail(List<BookingDetailModel> bookingDetail) {
        this.bookingDetail = bookingDetail;
    }
}
