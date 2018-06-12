package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

class ShowTimeModel {

    private int timeId;
    private String startTime;
    private String endTime;
    private List<MovieSchedule> movieSchedule;

    public ShowTimeModel() {
    }

    public int getTimeId() {
        return timeId;
    }

    public void setTimeId(int timeId) {
        this.timeId = timeId;
    }

    public String getStartTime() {
        return startTime;
    }

    public void setStartTime(String startTime) {
        this.startTime = startTime;
    }

    public String getEndTime() {
        return endTime;
    }

    public void setEndTime(String endTime) {
        this.endTime = endTime;
    }

    public List<MovieSchedule> getMovieSchedule() {
        return movieSchedule;
    }

    public void setMovieSchedule(List<MovieSchedule> movieSchedule) {
        this.movieSchedule = movieSchedule;
    }
}
