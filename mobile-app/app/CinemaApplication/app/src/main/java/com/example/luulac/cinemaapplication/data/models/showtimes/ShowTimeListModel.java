package com.example.luulac.cinemaapplication.data.models.showtimes;

import java.util.List;

public class ShowTimeListModel {

    private String cinemaName;
    private String filmName;
    private String filmImg;
    private String cinemaGroupName;
    private String groupCinemaLogo;
    private List<ShowTimeChildModel> showTimeChildModels;

    public String getCinemaGroupName() {
        return cinemaGroupName;
    }

    public void setCinemaGroupName(String cinemaGroupName) {
        this.cinemaGroupName = cinemaGroupName;
    }

    public String getCinemaName() {
        return cinemaName;
    }

    public void setCinemaName(String cinemaName) {
        this.cinemaName = cinemaName;
    }

    public List<ShowTimeChildModel> getShowTimeChildModels() {
        return showTimeChildModels;
    }

    public void setShowTimeChildModels(List<ShowTimeChildModel> showTimeChildModels) {
        this.showTimeChildModels = showTimeChildModels;
    }

    public String getGroupCinemaLogo() {
        return groupCinemaLogo;
    }

    public void setGroupCinemaLogo(String groupCinemaLogo) {
        this.groupCinemaLogo = groupCinemaLogo;
    }

    public String getFilmName() {
        return filmName;
    }

    public void setFilmName(String filmName) {
        this.filmName = filmName;
    }

    public String getFilmImg() {
        return filmImg;
    }

    public void setFilmImg(String filmImg) {
        this.filmImg = filmImg;
    }
}
