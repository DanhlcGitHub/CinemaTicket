package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.BookingTicketModel;
import com.example.luulac.cinemaapplication.data.models.FinishChoiceSeatModel;
import com.example.luulac.cinemaapplication.data.models.FinishOrderTicket;
import com.example.luulac.cinemaapplication.data.models.OrderChoiceTicketModel;
import com.example.luulac.cinemaapplication.data.models.OrderIncludeFimlModel;
import com.example.luulac.cinemaapplication.data.models.SeatCollectionModel;
import com.example.luulac.cinemaapplication.data.models.SeatModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Query;

public interface OrderService {

    @GET("orders/orderStepOne")
    Call<OrderChoiceTicketModel> getOrderChoiceTicket(@Query("filmId") int filmId, @Query("roomId") int roomId, @Query("groupId") int groupId, @Query("scheduleId") int scheduleId);

    @GET("orders/seats")
    Call<List<SeatModel>> getOrderChoiceSeats(@Query("roomId") int roomId,@Query("scheduleId") int scheduleId);

    @POST("orders/bookingTicket")
    Call<SeatCollectionModel> orderTicket(@Body SeatCollectionModel seatCollectionModel);

    @PUT("orders/changeStatusTicket")
    Call<List<TicketModel>> changeStatusTicket(@Body List<TicketModel> tickets);

    @POST("orders/finishPaypalPayment")
        Call<BookingTicketModel> finishPaypalPayment(@Body BookingTicketModel bookingTicketModel);

    @PUT("orders/resellTicket")
    Call<List<TicketModel>> resellTicket(@Body List<TicketModel> tickets);

    @GET("orders/ordersByAccountId")
    Call<List<OrderIncludeFimlModel>> ordersByAccountId(@Query("accountId") int accountId);
}
