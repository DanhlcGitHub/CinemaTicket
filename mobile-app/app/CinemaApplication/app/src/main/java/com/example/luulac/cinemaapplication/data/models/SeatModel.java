package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class SeatModel {
    private int seatId;
    private int typeSeatId;
    private int roomId;
    private int px;
    private int py;
    private RoomModel roomModel;
    private TypeOfSeatModel typeSeat;
    private List<TicketModel> ticket;

    public SeatModel() {
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

    public RoomModel getRoomModel() {
        return roomModel;
    }

    public void setRoomModel(RoomModel roomModel) {
        this.roomModel = roomModel;
    }

    public TypeOfSeatModel getTypeSeat() {
        return typeSeat;
    }

    public void setTypeSeat(TypeOfSeatModel typeSeat) {
        this.typeSeat = typeSeat;
    }

    public List<TicketModel> getTicket() {
        return ticket;
    }

    public void setTicket(List<TicketModel> ticket) {
        this.ticket = ticket;
    }
}
