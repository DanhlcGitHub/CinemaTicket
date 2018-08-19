package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.AccountPurchasedModel;
import com.example.luulac.cinemaapplication.data.models.UserAccountModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface AccountService {

    @GET("account/login")
    Call<UserAccountModel> login(@Query("username") String username, @Query("password") String password);

    @GET("account/register")
    Call<UserAccountModel> register(@Query("userId") String userId, @Query("password") String password, @Query("email") String email, @Query("phone") String phone);

    @GET("account/getAllOrderByAccountId")
    Call<List<AccountPurchasedModel>> getAllOrderByAccountId(@Query("accountId") String accountId);
}
