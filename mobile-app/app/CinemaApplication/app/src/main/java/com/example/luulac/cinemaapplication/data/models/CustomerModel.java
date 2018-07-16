package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;
import java.util.List;

public class CustomerModel implements Serializable{

    private int customerId;
    private String userId;
    private String phone;
    private String email;
    private List<BookingTicketModel> bookingTicket;

    public CustomerModel(String email, String phone) {
        this.phone = phone;
        this.email = email;
    }

    public int getCustomerId() {
        return customerId;
    }

    public void setCustomerId(int customerId) {
        this.customerId = customerId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public List<BookingTicketModel> getBookingTicket() {
        return bookingTicket;
    }

    public void setBookingTicket(List<BookingTicketModel> bookingTicket) {
        this.bookingTicket = bookingTicket;
    }
}
