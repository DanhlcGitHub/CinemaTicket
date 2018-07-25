package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;
import java.util.List;

public class BookingTicketModel implements Serializable {

    private int bookingId;
    private int customerId;
    private int paymentMethodId;
    private int quantity;
    private String bookingDate;
    private String tokenId;

    private CustomerModel customer;
    private List<TicketModel> tickets;

    public BookingTicketModel() {
    }

    public BookingTicketModel(int quantity, CustomerModel customer, List<TicketModel> tickets, String tokenId) {
        this.quantity = quantity;
        this.customer = customer;
        this.tickets = tickets;
        this.tokenId = tokenId;
    }

    public int getBookingId() {
        return bookingId;
    }

    public void setBookingId(int bookingId) {
        this.bookingId = bookingId;
    }

    public int getCustomerId() {
        return customerId;
    }

    public void setCustomerId(int customerId) {
        this.customerId = customerId;
    }

    public int getPaymentMethodId() {
        return paymentMethodId;
    }

    public void setPaymentMethodId(int paymentMethodId) {
        this.paymentMethodId = paymentMethodId;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public String getBookingDate() {
        return bookingDate;
    }

    public void setBookingDate(String bookingDate) {
        this.bookingDate = bookingDate;
    }

    public CustomerModel getCustomer() {
        return customer;
    }

    public void setCustomer(CustomerModel customer) {
        this.customer = customer;
    }

    public List<TicketModel> getTickets() {
        return tickets;
    }

    public void setTickets(List<TicketModel> tickets) {
        this.tickets = tickets;
    }

    public String getTokenId() {
        return tokenId;
    }

    public void setTokenId(String tokenId) {
        this.tokenId = tokenId;
    }
}
