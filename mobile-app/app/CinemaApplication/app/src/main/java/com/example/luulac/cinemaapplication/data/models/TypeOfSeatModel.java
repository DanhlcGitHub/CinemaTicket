package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class TypeOfSeatModel {

    private int typeSeatId;
    private int typeName;
    private int capacity;
    private int groupId;
    private double price;

    private GroupCinemaModel group;
    private List<SeatModel> seat;

    public TypeOfSeatModel() {
    }

    public int getTypeSeatId() {
        return typeSeatId;
    }

    public void setTypeSeatId(int typeSeatId) {
        this.typeSeatId = typeSeatId;
    }

    public int getTypeName() {
        return typeName;
    }

    public void setTypeName(int typeName) {
        this.typeName = typeName;
    }

    public int getCapacity() {
        return capacity;
    }

    public void setCapacity(int capacity) {
        this.capacity = capacity;
    }

    public int getGroupId() {
        return groupId;
    }

    public void setGroupId(int groupId) {
        this.groupId = groupId;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public GroupCinemaModel getGroup() {
        return group;
    }

    public void setGroup(GroupCinemaModel group) {
        this.group = group;
    }

    public List<SeatModel> getSeat() {
        return seat;
    }

    public void setSeat(List<SeatModel> seat) {
        this.seat = seat;
    }
}
