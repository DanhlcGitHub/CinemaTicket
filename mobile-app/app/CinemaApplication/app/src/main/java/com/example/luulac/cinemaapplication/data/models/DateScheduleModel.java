package com.example.luulac.cinemaapplication.data.models;

public class DateScheduleModel {
    private String dateText;
    private String dateNumber;

    public DateScheduleModel(String dateText, String dateNumber) {
        this.dateText = dateText;
        this.dateNumber = dateNumber;
    }

    public String getDateText() {
        return dateText;
    }

    public void setDateText(String dateText) {
        this.dateText = dateText;
    }

    public String getDateNumber() {
        return dateNumber;
    }

    public void setDateNumber(String dateNumber) {
        this.dateNumber = dateNumber;
    }
}
