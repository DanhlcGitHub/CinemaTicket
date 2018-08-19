package com.example.luulac.cinemaapplication.activities;

import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.ChoiceSeatAdapter;
import com.example.luulac.cinemaapplication.adapters.FilmComingSoonAdapter;
import com.example.luulac.cinemaapplication.adapters.RecyclerItemClickListener;
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.data.models.SeatModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;
import com.example.luulac.cinemaapplication.services.FilmService;
import com.example.luulac.cinemaapplication.services.OrderService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;
import com.example.luulac.cinemaapplication.services.TicketService;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ChoiceSeatsActivity extends AppCompatActivity {
    private Context context;
    private TextView listSelectedSeat;
    private List<String> listStringSeatSelected = new ArrayList<>();
    private List<TicketModel> tickets = new ArrayList<>();
    private int count = 0;
    private List<SeatModel> seats = new ArrayList<>();
    private List<SeatModel> data;
    private RelativeLayout relativeLayout;

    private ImageView iconCancelOrder;


    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.layout_choice_seats);

        context = this.getApplicationContext();

        iconCancelOrder = (ImageView) findViewById(R.id.icon_cancel_order);

        iconCancelOrder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                comfirmCancelOrder();
            }
        });

        final Intent intent = this.getIntent();

        final FilmTranferModel filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");

        final ScheduleTranferModel scheduleTranfer = (ScheduleTranferModel) intent.getSerializableExtra("scheduleTranfer");

        final boolean isChangeTicket = intent.getBooleanExtra("isChangeTicket", false);
        final int ticketId = intent.getIntExtra("ticketId", 0);

        TextView tvGroupCinemaName = (TextView) findViewById(R.id.tv_choice_seat_short_key);
        TextView tvCinemaName = (TextView) findViewById(R.id.tv_choice_seat_cinema_name);
        TextView tvFilmSubInfo = (TextView) findViewById(R.id.tv_choice_seat_full);
        TextView tvFilmName = (TextView) findViewById(R.id.tv_cs_film_name);

        tvGroupCinemaName.setText(scheduleTranfer.getGroupCinemaName());
        tvCinemaName.setText(scheduleTranfer.getCinemaName());
        tvFilmSubInfo.setText(scheduleTranfer.getShowTime() + " - " + scheduleTranfer.getCinemaName() + " - " + scheduleTranfer.getRoomName());
        tvFilmName.setText(scheduleTranfer.getFilmName());

        OrderService orderService = ServiceBuilder.buildService(OrderService.class);
        Call<List<SeatModel>> request = orderService.getOrderChoiceSeats(filmTranfer.getRoomId(), filmTranfer.getScheduleId());

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

                    int tmpPosition = tmpY * filmTranfer.getRow() + tmpX;
                    SeatModel seatModel = data.get(i);
                    seats.set(tmpPosition, seatModel);
                }

                listSelectedSeat = (TextView) findViewById(R.id.tv_cs_please_choice_seat);

                char character = 'A';
                final List<Character> resultAbc = new ArrayList<>();
                int ascii = (int) character;

                for (int i = 0; i < filmTranfer.getCol(); i++) {
                    char tmp = (char) (ascii + i);
                    resultAbc.add(tmp);
                }

                //new adapter for recycler view seats attachment date: List<SeatModel>
                ChoiceSeatAdapter adapter = new ChoiceSeatAdapter(seats, resultAbc, filmTranfer.getRow());

                //Get recycler view seat from layout
                RecyclerView recyclerView = findViewById(R.id.rcv_cs_seat);

                //New grid layout with col
                GridLayoutManager layoutManager = new GridLayoutManager(getApplicationContext(), filmTranfer.getRow());

                //set recycler view of seat is Grid Layout Manager
                recyclerView.setLayoutManager(layoutManager);

                //set adapter for recycler view of seat
                recyclerView.setAdapter(adapter);

                final int quantity = scheduleTranfer.getQuantityTicket();

                relativeLayout = findViewById(R.id.liner_cs_bottom);

                relativeLayout.setBackgroundColor(Color.GRAY);

                relativeLayout.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Toast.makeText(getApplicationContext(), "Please choose " + (quantity - count) + " seats to continues!", Toast.LENGTH_SHORT).show();
                    }
                });

                recyclerView.addOnItemTouchListener(
                        new RecyclerItemClickListener(context, recyclerView, new RecyclerItemClickListener.OnItemClickListener() {
                            @Override
                            public void onItemClick(View view, int position) {

                                if (seats.get(position).getSeatId() != 0) {
                                    String ticketStatus = seats.get(position).getTicketStatus();
                                    switch (ticketStatus) {
                                        case "available":
                                            if (count <= quantity) {

                                                //get character of Px (A, B, C,...)
                                                String tmpPx = (seats.get(position).getLocationX()) + "";

                                                //get index of position (1, 2, 3,...)
                                                //because index begin 0 then + 1 to index
                                                String tmpPy = resultAbc.get(seats.get(position).getLocationY() - 1).toString();

                                                //seat String (A1, B2, C3,...)
                                                String seatString = tmpPy + tmpPx;

                                                String stringSeats = "";

                                                TextView tvSeatItem = view.findViewById(R.id.tv_seat_item);

                                                if (!seats.get(position).isSelected()) {

                                                    if (count != quantity) {
                                                        //add ticket name String to list String ticket
                                                        listStringSeatSelected.add(seatString);

                                                        //set background: green (selected)
                                                        tvSeatItem.setBackgroundResource(R.drawable.text_choice_seat_selected);

                                                        //set seat status: selected
                                                        seats.get(position).setSelected(true);

                                                        //new ticketModel
                                                        TicketModel ticket = new TicketModel(seats.get(position).getTicketId(), filmTranfer.getScheduleId(),
                                                                seats.get(position).getSeatId(), seats.get(position).getPrice());

                                                        //add ticket to list tickets
                                                        tickets.add(ticket);

                                                        //increase count one unit
                                                        count++;
                                                    }
                                                } else {

                                                    //remove ticket name String from list String ticket
                                                    listStringSeatSelected.remove(seatString);

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

                                                //get all item int listStringSeatSelected and append to stringSeat
                                                //result ( A1 B2 C3 ...)
                                                if (count == 0) {
                                                    listSelectedSeat.setText("Vui lòng chọn ghế");
                                                } else {
                                                    for (String item : listStringSeatSelected) {
                                                        stringSeats += item + " ";
                                                    }

                                                    listSelectedSeat.setText(stringSeats);
                                                }

                                                final String tmpStringSeats = stringSeats;

                                                //active click when customer choice enough seat
                                                if (count == scheduleTranfer.getQuantityTicket()) {

                                                    relativeLayout.setBackgroundColor(Color.parseColor("#27b400"));

                                                    //set event when click
                                                    relativeLayout.setOnClickListener(new View.OnClickListener() {

                                                        @Override
                                                        public void onClick(View v) {
                                                            if (isChangeTicket) {

                                                                //xy ly doi lai ve o day
                                                                TicketService ticketService = ServiceBuilder.buildService(TicketService.class);
                                                                Call<TicketModel> request = ticketService.confirmChangeTicket(ticketId, filmTranfer.getScheduleId(), tickets.get(0).getSeatId());

                                                                request.enqueue(new Callback<TicketModel>() {
                                                                    @Override
                                                                    public void onResponse(Call<TicketModel> request, Response<TicketModel> response) {
                                                                        TicketModel result = response.body();
                                                                        finish();
                                                                    }

                                                                    @Override
                                                                    public void onFailure(Call<TicketModel> request, Throwable t) {
                                                                        Toast.makeText(context, "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                                                                    }
                                                                });


                                                            } else {
                                                                Bundle bundle = new Bundle();
                                                                bundle.putSerializable("list", (Serializable) tickets);

                                                                Intent intentPayment = new Intent(getApplicationContext(), PaymentApplicationActivity.class);

                                                                intentPayment.putExtra("scheduleTranfer", scheduleTranfer);
                                                                intentPayment.putExtra("filmTranfer", filmTranfer);
                                                                intentPayment.putExtra("stringSeats", tmpStringSeats);
                                                                intentPayment.putExtra("listTicket", bundle);

                                                                startActivityForResult(intentPayment, REQUEST_CODE_ORDER);
                                                            }

                                                        }
                                                    });
                                                } else {
                                                    relativeLayout.setBackgroundColor(Color.GRAY);
                                                    relativeLayout.setOnClickListener(new View.OnClickListener() {
                                                        @Override
                                                        public void onClick(View v) {
                                                            Toast.makeText(getApplicationContext(), "Please choose " + (quantity - count) + " seats to continues!", Toast.LENGTH_SHORT).show();
                                                        }
                                                    });
                                                }
                                            }
                                            break;
                                        case "reselling":
                                            //click hien len dialog hien thi thong tin
                                            int px = seats.get(position).getPx();
                                            int py = seats.get(position).getPy();
                                            int postionOfSeat = py * (filmTranfer.getRow()) + px;
                                            String email = seats.get(postionOfSeat).getEmail();
                                            String phone = seats.get(postionOfSeat).getPhone();
                                            String description = seats.get(postionOfSeat).getResellDescription();
                                            showDialog(email, phone, description);
                                            break;
                                        case "buyed":
                                            break;
                                        case "buying":
                                            break;
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

    public static final int REQUEST_CODE_ORDER = 256;

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REQUEST_CODE_ORDER) {
            finish();
        }
    }

    @Override
    public void onBackPressed() {
        comfirmCancelOrder();
    }

    public void comfirmCancelOrder() {
        new AlertDialog.Builder(this)
                .setIcon(android.R.drawable.ic_dialog_alert)
                .setTitle("Hủy đặt vé")
                .setMessage("Bạn có muốn hủy đơn hàng này?")
                .setPositiveButton("Có", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        finish();
                    }

                })
                .setNegativeButton("Không", null)
                .show();
    }

    public void showDialog(String email, String phone, String descriptiont) {
        final Dialog dialog = new Dialog(this);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.setCancelable(false);
        dialog.setContentView(R.layout.dialog_info_resell_person);

        TextView tvEmail = (TextView) dialog.findViewById(R.id.tv_dialog_resell_person_email);
        tvEmail.setText(email);

        TextView tvPhone = (TextView) dialog.findViewById(R.id.tv_dialog_resell_person_phone);
        tvPhone.setText(phone);

        TextView tvDescription = (TextView) dialog.findViewById(R.id.tv_dialog_resell_description_content);
        tvDescription.setText(descriptiont);

        Button dialogButton = (Button) dialog.findViewById(R.id.btn_dialog_resell_person_ok);
        dialogButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialog.dismiss();
            }
        });

        dialog.show();

    }
}
