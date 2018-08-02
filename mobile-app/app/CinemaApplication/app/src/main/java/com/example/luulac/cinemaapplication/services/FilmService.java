package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.FilmModel;

import java.util.HashMap;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
import retrofit2.http.Query;
import retrofit2.http.QueryMap;

public interface FilmService {

    @GET("films/getFilmNowShowing")
    Call<List<FilmModel>> getFilmNowShowing();

    @GET("films/getFilmComingSoon")
    Call<List<FilmModel>> getFilmComingSoon();

    @GET("films/{filmId}")
    Call<FilmModel> getFilm(@Path("filmId")int filmId);
}
