package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;

public class UserAccountModel implements Serializable{
    private String userId;
    private String userPassword;
    private String phone;
    private String email;
    private boolean isExited;

    public UserAccountModel() {
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getUserPassword() {
        return userPassword;
    }

    public void setUserPassword(String userPassword) {
        this.userPassword = userPassword;
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

    public boolean isExited() {
        return isExited;
    }

    public void setExited(boolean exited) {
        isExited = exited;
    }
}
