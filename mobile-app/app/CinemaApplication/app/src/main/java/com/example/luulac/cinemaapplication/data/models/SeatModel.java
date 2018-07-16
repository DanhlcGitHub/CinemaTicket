package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;
import java.util.List;

public class SeatModel implements Serializable {
    private int seatId;
    private int typeSeatId;
    private int roomId;
    private int px;
    private int py;
    private Double price;
    private boolean isBooked;
    private boolean isSelected;
    private String ticketStatus;
    private String email;
    private String phone;

    public SeatModel() {
        isSelected = false;
    }

    public int getSeatId() {
        return seatId;
    }

    public void setSeatId(int seatId) {
        this.seatId = seatId;
    }

    public int getTypeSeatId() {
        return typeSeatId;
    }

    public void setTypeSeatId(int typeSeatId) {
        this.typeSeatId = typeSeatId;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public int getPx() {
        return px;
    }

    public void setPx(int px) {
        this.px = px;
    }

    public int getPy() {
        return py;
    }

    public void setPy(int py) {
        this.py = py;
    }

    public boolean isSelected() {
        return isSelected;
    }

    public void setSelected(boolean selected) {
        isSelected = selected;
    }

    public boolean isBooked() {
        return isBooked;
    }

    public void setBooked(boolean booked) {
        isBooked = booked;
    }

    public Double getPrice() {
        return price;
    }

    public void setPrice(Double price) {
        this.price = price;
    }

    public String getTicketStatus() {
        return ticketStatus;
    }

    public void setTicketStatus(String ticketStatus) {
        this.ticketStatus = ticketStatus;
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
}
