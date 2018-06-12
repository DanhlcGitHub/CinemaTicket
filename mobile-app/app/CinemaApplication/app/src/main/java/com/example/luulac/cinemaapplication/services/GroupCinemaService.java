package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.GroupCinemaModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;

public interface GroupCinemaService {

    @GET("groupcinema")
    Call<List<GroupCinemaModel>> getCinemaGroup();
}
