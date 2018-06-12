package com.example.luulac.cinemaapplication.data.models;

import java.util.List;

public class HomeModel {
    private List<FilmModel> filmTopSix;

    private List<NewsModel> newTopEight;

    public HomeModel() {
    }

    public List<FilmModel> getFilms() {
        return filmTopSix;
    }

    public void setFilms(List<FilmModel> films) {
        this.filmTopSix = films;
    }

    public List<NewsModel> getNewsList() {
        return newTopEight;
    }

    public void setNewsList(List<NewsModel> newsList) {
        this.newTopEight = newsList;
    }
}
