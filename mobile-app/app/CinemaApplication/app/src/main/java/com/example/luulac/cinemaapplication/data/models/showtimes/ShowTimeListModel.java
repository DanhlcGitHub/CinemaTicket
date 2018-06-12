package com.example.luulac.cinemaapplication.data.models.showtimes;

import java.util.List;

public class ShowTimeListModel {

    private String cinemaName;
    private String cinemaGroupName;
    private String groupCinemaLogo;
    private List<ShowTimeChildModel> showTimeChildModels;

    public ShowTimeListModel() {
    }

    public ShowTimeListModel(String cinemaName, List<ShowTimeChildModel> showTimeChildModels, String cinemaGroupName) {
        this.cinemaName = cinemaName;
        this.showTimeChildModels = showTimeChildModels;
        this.cinemaGroupName = cinemaGroupName;
    }

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
}
