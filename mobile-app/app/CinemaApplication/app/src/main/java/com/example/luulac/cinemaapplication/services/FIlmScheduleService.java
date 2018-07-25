package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.showtimes.FilmScheduleModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeChildModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeListModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface FIlmScheduleService {
    @GET("filmschedule")
    Call<List<FilmScheduleModel>> getScheduleByDayAndFilmId(@Query("filmId") int filmId, @Query("indexDate") int indexDate);

    @GET("filmschedule/GetSchdeduleByCinemaId")
    Call<List<FilmScheduleModel>> getSchdeduleByCinemaId(@Query("cinemaId") int cinemaId, @Query("indexDate") int indexDate);

    @GET("filmschedule/GetSchdeduleForChangeTicket")
    Call<ShowTimeListModel> getScheduleForChangeTicket(@Query("cinemaId") int cinemaId, @Query("indexDate") int indexDate, @Query("filmId") int filmId);
}
