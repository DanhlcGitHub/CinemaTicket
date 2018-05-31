package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class Cinemas {
    private String cinemaName;
    private String cinemaImage;

    public Cinemas() {
    }

    public Cinemas(String cinemaName, String cinemaImage) {
        this.cinemaName = cinemaName;
        this.cinemaImage = cinemaImage;
    }

    public String getCinemaName() {
        return cinemaName;
    }

    public void setCinemaName(String cinemaName) {
        this.cinemaName = cinemaName;
    }

    public String getCinemaImage() {
        return cinemaImage;
    }

    public void setCinemaImage(String cinemaImage) {
        this.cinemaImage = cinemaImage;
    }
}
