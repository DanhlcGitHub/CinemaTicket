package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.WindowManager;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.SeatCollectionModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;

import java.util.ArrayList;
import java.util.List;

public class FinishPaymentActivity extends AppCompatActivity{
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.layout_finish_payment);

        final Intent intent = this.getIntent();

        final String stringSeats = intent.getStringExtra("stringSeats");
        final FilmTranferModel filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");
        final ScheduleTranferModel scheduleTranfer = (ScheduleTranferModel) intent.getSerializableExtra("scheduleTranfer");
        final String email = intent.getStringExtra("email");
        final String phone = intent.getStringExtra("phone");



        TextView tvFilmName = findViewById(R.id.tv_finish_payment_film_name);
        tvFilmName.setText(scheduleTranfer.getFilmName());

        TextView tvRestricted= findViewById(R.id.tv_finish_payment_restricted);
        tvRestricted.setText(scheduleTranfer.getRestricted());

        TextView tvDigType = findViewById(R.id.tv_finish_payment_digType);
        tvDigType.setText(scheduleTranfer.getDigType());

        TextView tvGroupCinemaName = findViewById(R.id.tv_finish_payment_cinema_group_name);
        tvGroupCinemaName.setText(scheduleTranfer.getGroupCinemaName());

        TextView tvCinemaName = findViewById(R.id.tv_finish_payment_cinema_location);
        tvCinemaName.setText(scheduleTranfer.getCinemaName());

        TextView tvStringSeats = findViewById(R.id.tv_finish_payment_type_ticket);
        tvStringSeats.setText(stringSeats);

        TextView tvTotalPrice = findViewById(R.id.tv_finish_payment_totel);
        tvTotalPrice.setText(scheduleTranfer.getTotalPrice().toString());

        TextView tvGroupNameMiddle = findViewById(R.id.tv_fp_group_cinema_name_middle);
        tvGroupNameMiddle.setText(scheduleTranfer.getGroupCinemaName());

        TextView tvCinemaNameMiddle = findViewById(R.id.tv_fp_cinema_name_middle);
        tvCinemaNameMiddle.setText(scheduleTranfer.getCinemaName());

        ImageView filmImage = (ImageView) findViewById(R.id.img_film_fp);
        Glide.with(getApplicationContext()).load(scheduleTranfer.getFilmImage()).into(filmImage);

        //tv_finish_payment_time_show
        TextView tvTime = (TextView) findViewById(R.id.tv_finish_payment_time_show);
        tvTime.setText(scheduleTranfer.getShowTime());

        //tv_finish_payment_date_ticket
        TextView tvDate = (TextView) findViewById(R.id.tv_finish_payment_date_ticket);
        tvDate.setText(filmTranfer.getDatetime());

        //tv_finish_payment_phone
        TextView tvPhoneNumber = (TextView) findViewById(R.id.tv_finish_payment_phone);
        tvPhoneNumber.setText(phone);

        //tv_finish_payment_email
        TextView tvEmail = (TextView) findViewById(R.id.tv_finish_payment_email);
        tvEmail.setText(email);

        //tv_payment_code
        TextView tvPaymentCode = (TextView) findViewById(R.id.tv_payment_code);
        tvPaymentCode.setText("84567322");

    }
}
