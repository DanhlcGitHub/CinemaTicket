package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class GroupCinemaModel {

    private int groupId;
    private String logoImg;
    private String name ;
    private List<CinemaModel> cinemas;

    public GroupCinemaModel() {
    }

    public int getGroupId() {
        return groupId;
    }

    public void setGroupId(int groupId) {
        this.groupId = groupId;
    }

    public String getLogoImg() {
        return logoImg;
    }

    public void setLogoImg(String logoImg) {
        this.logoImg = logoImg;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public List<CinemaModel> getCinemas() {
        return cinemas;
    }

    public void setCinemas(List<CinemaModel> cinema) {
        this.cinemas = cinema;
    }
}
