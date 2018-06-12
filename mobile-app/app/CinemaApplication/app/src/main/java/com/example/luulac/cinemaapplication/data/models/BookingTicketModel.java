package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class BookingTicketModel {

    private int BookingId;
    private int CustomerId;
    private int PaymentMethodId;
    private int Quantity;
    private String BookingDate;

    private CustomerModel Customer;
    private List<BookingDetailModel> BookingDetail;

    public int getBookingId() {
        return BookingId;
    }

    public void setBookingId(int bookingId) {
        BookingId = bookingId;
    }

    public int getCustomerId() {
        return CustomerId;
    }

    public void setCustomerId(int customerId) {
        CustomerId = customerId;
    }

    public int getPaymentMethodId() {
        return PaymentMethodId;
    }

    public void setPaymentMethodId(int paymentMethodId) {
        PaymentMethodId = paymentMethodId;
    }

    public int getQuantity() {
        return Quantity;
    }

    public void setQuantity(int quantity) {
        Quantity = quantity;
    }

    public String getBookingDate() {
        return BookingDate;
    }

    public void setBookingDate(String bookingDate) {
        BookingDate = bookingDate;
    }

    public CustomerModel getCustomer() {
        return Customer;
    }

    public void setCustomer(CustomerModel customer) {
        Customer = customer;
    }

    public List<BookingDetailModel> getBookingDetail() {
        return BookingDetail;
    }

    public void setBookingDetail(List<BookingDetailModel> bookingDetail) {
        BookingDetail = bookingDetail;
    }
}
