package com.example.luulac.cinemaapplication.activities;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.FilmShowingAdapter;
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.data.models.OrderChoiceTicketModel;
import com.example.luulac.cinemaapplication.data.models.TypeOfSeatModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.FilmService;
import com.example.luulac.cinemaapplication.services.OrderService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class OrderTicketActivity extends AppCompatActivity {

    public static final int REQUEST_CODE_ORDER = 256;
    private OrderChoiceTicketModel model;
    private int numberTicketOrder = 0;
    private Double totalPrice = 0.0;

    private RelativeLayout rv;

    public static final int NUMBER_LAYOUT_CHOICE_ITEM_ORDER = 2;
    public static final int NUMBER_MAX_TICKET = 10;
    public static final int NUMBER_MIN_TICKET = 0;

    ImageView iconCancelOrder;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);

        setContentView(R.layout.activity_order_ticket);

        iconCancelOrder = (ImageView) findViewById(R.id.icon_cancel_order);

        iconCancelOrder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                comfirmCancelOrder();
            }
        });

        final Intent intent = this.getIntent();
        final FilmTranferModel filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");

        //call service from server to get data to show in order ticket activity
        OrderService orderService = ServiceBuilder.buildService(OrderService.class);
        Call<OrderChoiceTicketModel> request = orderService.getOrderChoiceTicket(filmTranfer.getScheduleId());

        //receiving and process data from the server
        request.enqueue(new Callback<OrderChoiceTicketModel>() {
            @Override
            public void onResponse(Call<OrderChoiceTicketModel> request, Response<OrderChoiceTicketModel> response) {
                model = response.body();

                TextView tvGroupCinemaName = (TextView) findViewById(R.id.tv_order_ticket_short_key);
                TextView tvCinemaName = (TextView) findViewById(R.id.tv_order_ticket_cinema_name);
                TextView tvFilmSubInfo = (TextView) findViewById(R.id.tv_order_ticket_full);
                TextView tvFilmName = (TextView) findViewById(R.id.tv_order_ticket_film_name);
                TextView tvRestricted = (TextView) findViewById(R.id.tv_order_ticket_restricted);
                TextView tvSubInfo = (TextView) findViewById(R.id.tv_order_ticket_sub_info);
                final TextView tvTotalPrice = (TextView) findViewById(R.id.tv_order_ticker_price);

                final String groupCinemaName = model.getGroupCinemaName();
                final String cinemaName = model.getCinemaName();
                final String showTime = model.getTimeShow();
                final String roomName = model.getRoomName();
                final String filmName = model.getFilmName();
                final String restricted = model.getRestricted();
                final String fimlLength = model.getFilmLength();
                final String digType = model.getDigType();
                final String stringImageFilm = model.getFilmImage();

                tvGroupCinemaName.setText(groupCinemaName);
                tvCinemaName.setText(cinemaName);
                tvFilmSubInfo.setText(showTime + " - " + cinemaName + " - " + roomName);
                tvFilmName.setText(filmName);
                if (restricted.equalsIgnoreCase("0")) {
                    tvRestricted.setVisibility(View.INVISIBLE);
                } else {
                    tvRestricted.setText(restricted);
                }
                tvSubInfo.setText(fimlLength + " phút - " + digType + " cả tuần");

                ImageView filmImage = (ImageView) findViewById(R.id.img_order_ticket_film);
                Glide.with(getApplicationContext()).load(stringImageFilm).into(filmImage);

                List<TypeOfSeatModel> typeOfSeats = model.getTypeOfSeats();

                RelativeLayout rlTypeOfSeatOne;


                rlTypeOfSeatOne = findViewById(R.id.layout_choice_item_order_one);

                final TextView quantityTicket = (TextView) rlTypeOfSeatOne.findViewById(R.id.edit_text_choice_item_order_number);
                final Double tmpPrice = typeOfSeats.get(0).getPrice();

                TextView typeTicket = (TextView) rlTypeOfSeatOne.findViewById(R.id.tv_choice_item_order_type_ticket);
                TextView price = (TextView) rlTypeOfSeatOne.findViewById(R.id.tv_choice_item_order_price_ticket);

                typeTicket.setText(typeOfSeats.get(0).getTypeName());
                price.setText(tmpPrice.toString());

                // btn remove one number ticket
                Button btnMius = (Button) rlTypeOfSeatOne.findViewById(R.id.btn_choice_item_order_remove);
                btnMius.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Integer quantityString = Integer.valueOf(quantityTicket.getText().toString()) - 1;
                        if (quantityString >= NUMBER_MIN_TICKET) {

                            //update quantityTicket
                            quantityTicket.setText(quantityString.toString());

                            //reduce numberTicketOrder one unit
                            numberTicketOrder--;

                            //reduce totalPrice one unit per price of ticket
                            totalPrice -= tmpPrice;

                            //update tvTotalPrice to show total price
                            tvTotalPrice.setText(totalPrice.toString());
                        }

                        if (numberTicketOrder != 0) {
                            rv.setBackgroundColor(Color.parseColor("#27b400"));

                        } else {
                            rv.setBackgroundColor(Color.GRAY);
                        }
                    }
                });

                // btn add one number ticket
                Button btnAdd = (Button) rlTypeOfSeatOne.findViewById(R.id.btn_choice_item_order_add);
                btnAdd.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Integer quantityString = Integer.valueOf(quantityTicket.getText().toString()) + 1;

                        if (quantityString <= NUMBER_MAX_TICKET) {

                            //update quantityTicket
                            quantityTicket.setText(quantityString.toString());

                            //add numberTicketOrder one unit
                            numberTicketOrder++;

                            //add totalPrice one unit per price of ticket
                            totalPrice += tmpPrice;

                            //update tvTotalPrice to show total price
                            tvTotalPrice.setText(totalPrice.toString());
                        }

                        if (numberTicketOrder != 0) {
                            rv.setBackgroundColor(Color.parseColor("#27b400"));
                        } else {
                            rv.setBackgroundColor(Color.GRAY);
                        }
                    }
                });

                //click to continus order ticket
                rv = (RelativeLayout) findViewById(R.id.rv_order_ticket_continues);

                rv.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (numberTicketOrder != 0) {
                            Intent intentChoiceSeat = new Intent(getApplicationContext(), ChoiceSeatsActivity.class);

                            ScheduleTranferModel scheduleTranfer = new ScheduleTranferModel(numberTicketOrder, totalPrice, groupCinemaName, showTime, cinemaName, roomName, filmName, restricted, fimlLength, digType, stringImageFilm);

                            intentChoiceSeat.putExtra("scheduleTranfer", scheduleTranfer);
                            intentChoiceSeat.putExtra("filmTranfer", filmTranfer);

                            startActivityForResult(intentChoiceSeat, REQUEST_CODE_ORDER);
                        } else {
                            Toast.makeText(OrderTicketActivity.this, "Please pick more than one seat to continues!", Toast.LENGTH_SHORT).show();
                        }
                    }

                });
            }

            @Override
            public void onFailure(Call<OrderChoiceTicketModel> request, Throwable t) {
            }
        });
    }

    @Override
    public void onBackPressed() {
        comfirmCancelOrder();
    }

    public void comfirmCancelOrder() {
        new AlertDialog.Builder(this)
                .setIcon(android.R.drawable.ic_dialog_alert)
                .setTitle("Hủy đặt vé")
                .setMessage("Bạn muốn hủy đơn hàng này?")
                .setPositiveButton("Đồng ý", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        finish();
                    }

                })
                .setNegativeButton("Không", null)
                .show();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REQUEST_CODE_ORDER) {
            finish();
        }
    }

    @Override
    public boolean onSupportNavigateUp() {
        return true;
    }
}
