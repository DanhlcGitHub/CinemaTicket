package com.example.luulac.cinemaapplication.data.models;

public class Theater {
    private String theaterName;
    private String theaterLocation;
    private String theaterGap;

    public Theater(String theaterName, String theaterLocation, String theaterGap) {
        this.theaterName = theaterName;
        this.theaterLocation = theaterLocation;
        this.theaterGap = theaterGap;
    }

    public String getTheaterName() {
        return theaterName;
    }

    public void setTheaterName(String theaterName) {
        this.theaterName = theaterName;
    }

    public String getTheaterLocation() {
        return theaterLocation;
    }

    public void setTheaterLocation(String theaterLocation) {
        this.theaterLocation = theaterLocation;
    }

    public String getTheaterGap() {
        return theaterGap;
    }

    public void setTheaterGap(String theaterGap) {
        this.theaterGap = theaterGap;
    }
}
