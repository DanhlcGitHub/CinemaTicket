package com.example.luulac.cinemaapplication.activities;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;

import com.example.luulac.cinemaapplication.R;

public class OrderTicketActivity extends AppCompatActivity {

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_order_ticket);

    }

    @Override
    public boolean onSupportNavigateUp() {
        finish();
        return true;
    }
}
