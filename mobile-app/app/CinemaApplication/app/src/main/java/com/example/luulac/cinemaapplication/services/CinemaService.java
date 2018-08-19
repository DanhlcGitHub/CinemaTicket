package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.CinemaModel;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

public interface CinemaService {
    @GET("cinema/{cinemaId}")
    Call<CinemaModel> getCinemaById(@Path("cinemaId")int cinemaId);

}
