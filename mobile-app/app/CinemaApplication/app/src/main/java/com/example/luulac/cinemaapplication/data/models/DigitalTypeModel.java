package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class DigitalTypeModel {

    private int digTypeId;
    private String name;
    private List<RoomModel> room;

    public DigitalTypeModel() {
    }

    public int getDigTypeId() {
        return digTypeId;
    }

    public void setDigTypeId(int digTypeId) {
        this.digTypeId = digTypeId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public List<RoomModel> getRoom() {
        return room;
    }

    public void setRoom(List<RoomModel> room) {
        this.room = room;
    }
}
