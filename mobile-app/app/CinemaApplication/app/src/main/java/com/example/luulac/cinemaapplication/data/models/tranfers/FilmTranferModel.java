package com.example.luulac.cinemaapplication.data.models.tranfers;

import java.io.Serializable;

public class FilmTranferModel implements Serializable{

    private int filmId;
    private int roomId;
    private int groupId;
    private int scheduleId;
    private int col;
    private int row;
    private String datetime;

    public FilmTranferModel() {
    }

    public FilmTranferModel(int filmId, int roomId, int groupId, int scheduleId, int col, int row, String datetime) {
        this.filmId = filmId;
        this.roomId = roomId;
        this.groupId = groupId;
        this.scheduleId = scheduleId;
        this.col = col;
        this.row = row;
        this.datetime = datetime;
    }

    public int getFilmId() {
        return filmId;
    }

    public void setFilmId(int filmId) {
        this.filmId = filmId;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public int getGroupId() {
        return groupId;
    }

    public void setGroupId(int groupId) {
        this.groupId = groupId;
    }

    public int getScheduleId() {
        return scheduleId;
    }

    public void setScheduleId(int scheduleId) {
        this.scheduleId = scheduleId;
    }

    public int getCol() {
        return col;
    }

    public void setCol(int col) {
        this.col = col;
    }

    public int getRow() {
        return row;
    }

    public void setRow(int row) {
        this.row = row;
    }

    public String getDatetime() {
        return datetime;
    }

    public void setDatetime(String datetime) {
        this.datetime = datetime;
    }
}
