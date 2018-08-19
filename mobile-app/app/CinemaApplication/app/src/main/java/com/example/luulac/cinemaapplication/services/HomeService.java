package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.HomeModel;

import retrofit2.Call;
import retrofit2.http.GET;

public interface HomeService {

    @GET("home")
    Call<HomeModel> getDataForHomeScreen();
}
