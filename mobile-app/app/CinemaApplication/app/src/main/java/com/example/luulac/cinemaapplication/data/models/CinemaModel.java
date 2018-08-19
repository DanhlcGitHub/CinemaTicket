package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class CinemaModel {

    private int cinemaId;
    private String cinemaName;
    private int groupId;
    private String profilePicture;
    private String cinemaAddress;
    private String phone;
    private String email;
    private String openTime;
    private String introduction;
    private GroupCinemaModel group;
    private List<PromotionModel> promotion;
    private List<RoomModel> room;

    public CinemaModel() {
    }

    public int getCinemaId() {
        return cinemaId;
    }

    public void setCinemaId(int cinemaId) {
        this.cinemaId = cinemaId;
    }

    public String getCinemaName() {
        return cinemaName;
    }

    public void setCinemaName(String cinemaName) {
        this.cinemaName = cinemaName;
    }

    public int getGroupId() {
        return groupId;
    }

    public void setGroupId(int groupId) {
        this.groupId = groupId;
    }

    public String getProfilePicture() {
        return profilePicture;
    }

    public void setProfilePicture(String profilePicture) {
        this.profilePicture = profilePicture;
    }

    public String getCinemaAddress() {
        return cinemaAddress;
    }

    public void setCinemaAddress(String cinemaAddress) {
        this.cinemaAddress = cinemaAddress;
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

    public String getOpenTime() {
        return openTime;
    }

    public void setOpenTime(String openTime) {
        this.openTime = openTime;
    }

    public String getIntroduction() {
        return introduction;
    }

    public void setIntroduction(String introduction) {
        this.introduction = introduction;
    }

    public GroupCinemaModel getGroup() {
        return group;
    }

    public void setGroup(GroupCinemaModel group) {
        this.group = group;
    }

    public List<PromotionModel> getPromotion() {
        return promotion;
    }

    public void setPromotion(List<PromotionModel> promotion) {
        this.promotion = promotion;
    }

    public List<RoomModel> getRoom() {
        return room;
    }

    public void setRoom(List<RoomModel> room) {
        this.room = room;
    }
}
