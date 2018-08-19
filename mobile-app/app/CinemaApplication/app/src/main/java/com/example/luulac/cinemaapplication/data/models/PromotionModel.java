package com.example.luulac.cinemaapplication.data.models;

public class PromotionModel {

    private int promotionId;
    private int cinemaId;
    private String urlDocument;
    private CinemaModel cinema;

    public PromotionModel() {
    }

    public int getPromotionId() {
        return promotionId;
    }

    public void setPromotionId(int promotionId) {
        this.promotionId = promotionId;
    }

    public int getCinemaId() {
        return cinemaId;
    }

    public void setCinemaId(int cinemaId) {
        this.cinemaId = cinemaId;
    }

    public String getUrlDocument() {
        return urlDocument;
    }

    public void setUrlDocument(String urlDocument) {
        this.urlDocument = urlDocument;
    }

    public CinemaModel getCinema() {
        return cinema;
    }

    public void setCinema(CinemaModel cinema) {
        this.cinema = cinema;
    }
}
