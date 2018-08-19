package com.example.luulac.cinemaapplication.data.models;

class NewsModel{
    private int newsId;
    private int filmId;
    private String urlDocument;
    private FilmModel film;

    public NewsModel() {
    }

    public int getNewsId() {
        return newsId;
    }

    public void setNewsId(int newsId) {
        this.newsId = newsId;
    }

    public int getFilmId() {
        return filmId;
    }

    public void setFilmId(int filmId) {
        this.filmId = filmId;
    }

    public String getUrlDocument() {
        return urlDocument;
    }

    public void setUrlDocument(String urlDocument) {
        this.urlDocument = urlDocument;
    }

    public FilmModel getFilm() {
        return film;
    }

    public void setFilm(FilmModel film) {
        this.film = film;
    }
}
