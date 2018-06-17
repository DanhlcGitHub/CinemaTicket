package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.showtimes.FilmScheduleModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface FIlmScheduleService {
    @GET("filmschedule")
    Call<List<FilmScheduleModel>> getScheduleByDayAndFilmId(@Query("filmId") int filmId);
}
