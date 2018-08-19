package com.example.luulac.cinemaapplication.data.models;

import java.io.Serializable;

public class MovieSchedule implements Serializable{

    private int scheduleId;
    private FilmModel film;
    private ShowTimeModel showTime;

    public MovieSchedule() {
    }

    public int getScheduleId() {
        return scheduleId;
    }

    public void setScheduleId(int scheduleId) {
        this.scheduleId = scheduleId;
    }

    public FilmModel getFilm() {
        return film;
    }

    public void setFilm(FilmModel film) {
        this.film = film;
    }

    public ShowTimeModel getShowTime() {
        return showTime;
    }

    public void setShowTime(ShowTimeModel showTime) {
        this.showTime = showTime;
    }
}
