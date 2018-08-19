package com.example.luulac.cinemaapplication.services;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.util.Log;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.data.models.SeatCollectionModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ChangeStatusService extends Service {

    private Intent intentStatus;
    private SeatCollectionModel seatCollectionModel;
    private List<TicketModel> resultChangeTickets;

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d("ChangeStatusService", "Service Started");
        intentStatus = intent;
        return START_NOT_STICKY;
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        Log.d("ChangeStatusService", "Service Destroy");


        Log.d("ChangeStatusService", "Service Destroyed");
    }

    @Override
    public void onTaskRemoved(Intent rootIntent) {
        Log.e("ChangeStatusService", "END");
        //Code here
        seatCollectionModel = (SeatCollectionModel) intentStatus.getSerializableExtra("seatCollectionModel");
        List<TicketModel> ticketModels = seatCollectionModel.getTicketModels();
        resultChangeTickets = new ArrayList<>();
        OrderService orderService = ServiceBuilder.buildService(OrderService.class);
        Call<List<TicketModel>> request = orderService.changeStatusTicket(ticketModels);

        //receiving and process data from the server
        request.enqueue(new Callback<List<TicketModel>>() {
            @Override
            public void onResponse(Call<List<TicketModel>> request, Response<List<TicketModel>> response) {
                resultChangeTickets = response.body();
            }

            @Override
            public void onFailure(Call<List<TicketModel>> request, Throwable t) {
                Toast.makeText(getApplicationContext(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
            }
        });

        stopSelf();
    }
}
