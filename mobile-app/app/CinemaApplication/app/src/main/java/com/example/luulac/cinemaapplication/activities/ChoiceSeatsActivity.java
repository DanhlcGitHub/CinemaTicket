package com.example.luulac.cinemaapplication.activities;

import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.view.WindowManager;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.ChoiceSeatAbcAdapter;
import com.example.luulac.cinemaapplication.adapters.ChoiceSeatAdapter;
import com.example.luulac.cinemaapplication.adapters.RecyclerItemClickListener;
import com.example.luulac.cinemaapplication.data.models.SeatCollectionModel;
import com.example.luulac.cinemaapplication.data.models.SeatModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;
import com.example.luulac.cinemaapplication.services.OrderService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ChoiceSeatsActivity extends AppCompatActivity {
    private ChoiceSeatAdapter adapter;
    private Context context;
    private TextView listSelectedSeat;
    private List<String> litsStringSeatSelected = new ArrayList<>();
    private List<TicketModel> tickets = new ArrayList<>();
    private int count = 0;
    private List<SeatModel> seats = new ArrayList<>();
    private List<SeatModel> data;
    final SeatCollectionModel seatCollectionModel = new SeatCollectionModel();


    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.layout_choice_seats);

        context = this.getApplicationContext();

        final Intent intent = this.getIntent();
        final FilmTranferModel filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");
        final ScheduleTranferModel scheduleTranfer = (ScheduleTranferModel) intent.getSerializableExtra("scheduleTranfer");

        OrderService orderService = ServiceBuilder.buildService(OrderService.class);
        Call<List<SeatModel>> request = orderService.getOrderChoiceSeats(filmTranfer.getRoomId());

        request.enqueue(new Callback<List<SeatModel>>() {
            @Override
            public void onResponse(Call<List<SeatModel>> request, Response<List<SeatModel>> response) {
                data = response.body();

                for (int i = 0; i < filmTranfer.getRow() * filmTranfer.getCol(); i++) {
                    seats.add(new SeatModel());
                }

                for (int i = 0; i < data.size(); i++) {

                    int tmpX = data.get(i).getPx();
                    int tmpY = data.get(i).getPy();

                    seats.set(tmpY * filmTranfer.getRow() + tmpX, data.get(i));
                }

                listSelectedSeat = (TextView) findViewById(R.id.tv_cs_please_choice_seat);

                char character = 'A';
                final List<Character> resultAbc = new ArrayList<>();
                int ascii = (int) character;

                for (int i = 0; i < filmTranfer.getRow(); i++) {
                    char tmp = (char) (ascii + i);
                    resultAbc.add(tmp);
                }

                //new adapter for recycler view seats attachment date: List<Character>
                ChoiceSeatAbcAdapter abcAdapter = new ChoiceSeatAbcAdapter(resultAbc);

                //Get recycler view abc from layout
                RecyclerView rcvAbc = findViewById(R.id.rcv_cs_abc);

                //new liner layout for recycler view abc and set orientation is VERTICAL
                LinearLayoutManager linearLayout = new LinearLayoutManager(getApplicationContext());
                linearLayout.setOrientation(LinearLayout.VERTICAL);

                //set layout for recycler view abc is liner layout
                rcvAbc.setLayoutManager(linearLayout);

                //set adapter for recycler view abc
                rcvAbc.setAdapter(abcAdapter);

                //new adapter for recycler view seats attachment date: List<SeatModel>
                adapter = new ChoiceSeatAdapter(seats);

                //Get recycler view seat from layout
                RecyclerView recyclerView = findViewById(R.id.rcv_cs_seat);

                //New grid layout with col
                GridLayoutManager layoutManager = new GridLayoutManager(getApplicationContext(), filmTranfer.getCol());

                //set recycler view of seat is Grid Layout Manager
                recyclerView.setLayoutManager(layoutManager);

                //set adapter for recycler view of seat
                recyclerView.setAdapter(adapter);

                recyclerView.addOnItemTouchListener(
                        new RecyclerItemClickListener(context, recyclerView, new RecyclerItemClickListener.OnItemClickListener() {

                            @Override
                            public void onItemClick(View view, int position) {

                                if (count < scheduleTranfer.getQuantityTicket()) {

                                    //get character of Px (A, B, C,...)
                                    String tmpPx = resultAbc.get(seats.get(position).getPy()).toString();

                                    //get index of position (1, 2, 3,...)
                                    //because index begin 0 then + 1 to index
                                    String tmpPy = (seats.get(position).getPx() + 1) + "";

                                    //seat String (A1, B2, C3,...)
                                    String seatString = tmpPx + tmpPy;

                                    //
                                    String stringSeats = "";

                                    if (seats.get(position).getSeatId() != 0) {

                                        TextView tvSeatItem = view.findViewById(R.id.tv_seat_item);
                                        if (!seats.get(position).isSelected()) {

                                            //add ticket name String to list String ticket
                                            litsStringSeatSelected.add(seatString);

                                            //set background: green (selected)
                                            tvSeatItem.setBackgroundResource(R.drawable.text_choice_seat_selected);

                                            //set seat status: selected
                                            seats.get(position).setSelected(true);

                                            //new ticketModel
                                            TicketModel ticket = new TicketModel(filmTranfer.getScheduleId(), seats.get(position).getSeatId(), seats.get(position).getTypeSeat().getPrice());

                                            //add ticket to list tickets
                                            tickets.add(ticket);

                                            //increase count one unit
                                            count++;
                                        } else {

                                            //remove ticket name String from list String ticket
                                            litsStringSeatSelected.remove(seatString);

                                            //set background normal (not selected)
                                            tvSeatItem.setBackgroundResource(R.drawable.text_view_seat);

                                            //set seat status: normal (can select)
                                            seats.get(position).setSelected(false);

                                            //find and remove ticket from list tickets
                                            for (int i = 0; i < tickets.size(); i++) {
                                                TicketModel ticketModel = tickets.get(i);

                                                if (ticketModel.getSeatId() == seats.get(position).getSeatId()) {
                                                    tickets.remove(ticketModel);
                                                }
                                            }

                                            //reduce one unit of count
                                            count--;
                                        }
                                    }

                                    //get all item int listStringSeatSelected and append to stringSeat
                                    //result ( A1 B2 C3 ...)
                                    for (String item : litsStringSeatSelected) {
                                        stringSeats += item + " ";
                                    }

                                    final String tmpStringSeats = stringSeats;

                                    listSelectedSeat.setText(stringSeats);

                                    //active click when customer choice enough seat
                                    if (count == scheduleTranfer.getQuantityTicket()) {

                                        RelativeLayout relativeLayout = findViewById(R.id.liner_cs_bottom);
                                        relativeLayout.setBackgroundColor(Color.GREEN);

                                        //set event when click
                                        relativeLayout.setOnClickListener(new View.OnClickListener() {

                                            @Override
                                            public void onClick(View v) {
                                                Bundle bundle = new Bundle();
                                                bundle.putSerializable("list", (Serializable) tickets);

                                                Intent intentPayment = new Intent(getApplicationContext(), PaymentActivity.class);

                                                intentPayment.putExtra("scheduleTranfer", scheduleTranfer);
                                                intentPayment.putExtra("filmTranfer", filmTranfer);
                                                intentPayment.putExtra("stringSeats", tmpStringSeats);
                                                intentPayment.putExtra("listTicket", bundle);

                                                startActivity(intentPayment);
                                            }
                                        });
                                    }
                                }
                            }

                            @Override
                            public void onLongItemClick(View view, int position) {
                            }
                        })
                );
            }

            @Override
            public void onFailure(Call<List<SeatModel>> request, Throwable t) {
            }
        });
    }
}