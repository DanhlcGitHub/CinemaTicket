package com.example.luulac.cinemaapplication.data.models.showtimes;

import java.util.List;

public class FilmScheduleModel {
    public String date;
    public String day;
    public String dateOfWeek;
    public List<DateScheduleModel> dateScheduleModels;

    public FilmScheduleModel() {
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public String getDay() {
        return day;
    }

    public void setDay(String day) {
        this.day = day;
    }

    public String getDateOfWeek() {
        return dateOfWeek;
    }

    public void setDateOfWeek(String dateOfWeek) {
        this.dateOfWeek = dateOfWeek;
    }

    public List<DateScheduleModel> getDateScheduleModels() {
        return dateScheduleModels;
    }

    public void setDateScheduleModels(List<DateScheduleModel> dateScheduleModels) {
        this.dateScheduleModels = dateScheduleModels;
    }
}
