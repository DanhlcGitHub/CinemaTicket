package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;
import java.util.List;

public class SeatModel implements Serializable {
    private int seatId;
    private int typeSeatId;
    private int roomId;
    private int px;
    private int py;
    private RoomModel roomModel;
    private TypeOfSeatModel typeOfSeat;
    private List<TicketModel> ticket;
    private boolean isSelected;

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

    public RoomModel getRoomModel() {
        return roomModel;
    }

    public void setRoomModel(RoomModel roomModel) {
        this.roomModel = roomModel;
    }

    public TypeOfSeatModel getTypeSeat() {
        return typeOfSeat;
    }

    public void setTypeSeat(TypeOfSeatModel typeOfSeat) {
        this.typeOfSeat = typeOfSeat;
    }

    public List<TicketModel> getTicket() {
        return ticket;
    }

    public void setTicket(List<TicketModel> ticket) {
        this.ticket = ticket;
    }

    public boolean isSelected() {
        return isSelected;
    }

    public void setSelected(boolean selected) {
        isSelected = selected;
    }
}
