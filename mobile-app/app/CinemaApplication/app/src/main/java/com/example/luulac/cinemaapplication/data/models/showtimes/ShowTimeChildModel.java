package com.example.luulac.cinemaapplication.data.models.showtimes;

public class ShowTimeChildModel {

    private  int id;
    private String timeStart;
    private String timeEnd;
    private String type;
    private String price;

    public ShowTimeChildModel(int id, String timeStart, String timeEnd, String type, String price) {
        this.id = id;
        this.timeStart = timeStart;
        this.timeEnd = timeEnd;
        this.type = type;
        this.price = price;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getTimeStart() {
        return timeStart;
    }

    public void setTimeStart(String timeStart) {
        this.timeStart = timeStart;
    }

    public String getTimeEnd() {
        return timeEnd;
    }

    public void setTimeEnd(String timeEnd) {
        this.timeEnd = timeEnd;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getPrice() {
        return price;
    }

    public void setPrice(String price) {
        this.price = price;
    }
}
