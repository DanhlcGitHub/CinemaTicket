package com.example.luulac.cinemaapplication.services;

import com.example.luulac.cinemaapplication.data.models.TicketModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.PUT;
import retrofit2.http.Query;

public interface TicketService {

    @PUT("ticket/resellTicket")
    Call<TicketModel> resellTicket(@Query("ticketId") int ticketId,@Query("resellDescription") String resellDescription);

    @PUT("ticket/cancelResellTicket")
    Call<TicketModel> cancelResellTicket(@Query("ticketId") int ticketId);

    @PUT("ticket/confirmResellTicket")
    Call<TicketModel> confirmResellTicket(@Query("ticketId") int ticketId,@Query("email") String email);

    @GET("ticket/getAllTicketByBookingTicketId")
    Call<List<TicketModel>> getAllTicketByBookingTicketId(@Query("bookingTicketId") int bookingTicketId);

    @GET("ticket/confirmChangeTicket")
    Call<TicketModel> confirmChangeTicket(@Query("ticketId") int ticketId, @Query("scheduleId") int scheduleId, @Query("seatId") int seatId);
}
