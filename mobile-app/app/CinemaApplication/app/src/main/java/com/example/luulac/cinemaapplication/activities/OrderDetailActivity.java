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
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.FilmShowingAdapter;
import com.example.luulac.cinemaapplication.adapters.PurchasedTicketAdapter;
import com.example.luulac.cinemaapplication.adapters.RecyclerItemClickListener;
import com.example.luulac.cinemaapplication.data.models.AccountPurchasedModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;
import com.example.luulac.cinemaapplication.services.TicketService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class OrderDetailActivity extends AppCompatActivity {


    private Context context;
    private List<TicketModel> data;

    private final int REQUEST_CODE_CHANGE_TICKET = 102;

    private RecyclerView recyclerView;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {

        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        super.onCreate(savedInstanceState);

        setContentView(R.layout.layout_order_detail);
        context = getApplicationContext();

        Intent intent = this.getIntent();

        final AccountPurchasedModel accountPurchasedModel = (AccountPurchasedModel) intent.getSerializableExtra("accountPurchasedModel");
        int bookingTicketId = accountPurchasedModel.getBookingTicketId();

        TextView tvFilmName = findViewById(R.id.tv_finish_payment_film_name);
        tvFilmName.setText(accountPurchasedModel.getFilmName());

        TextView tvDigType = findViewById(R.id.tv_finish_payment_digType);
        tvDigType.setText(accountPurchasedModel.getDigType() + " Cả tuần");

        TextView tvCinemaName = findViewById(R.id.tv_order_detail_groupCinemaName);
        tvCinemaName.setText(accountPurchasedModel.getGroupCinemaName() + " - " + accountPurchasedModel.getCinemaName());

        TextView tvTotalPrice = findViewById(R.id.tv_finish_payment_totel);
        tvTotalPrice.setText(accountPurchasedModel.getTotalPrice().toString());

        TextView tvCinemaNameMiddle = findViewById(R.id.tv_fp_cinema_name_middle);
        tvCinemaNameMiddle.setText(accountPurchasedModel.getCinemaName());

        ImageView filmImage = (ImageView) findViewById(R.id.img_film_fp);
        Glide.with(getApplicationContext()).load(accountPurchasedModel.getFilmImage()).into(filmImage);

        //icon_back_order
        ImageView icon_back_order = (ImageView) findViewById(R.id.icon_back_order);
        icon_back_order.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                finish();
            }
        });

        //tv_finish_payment_time_show
        TextView tvTime = (TextView) findViewById(R.id.tv_finish_payment_time_show);
        tvTime.setText(accountPurchasedModel.getShowTime());

        //tv_finish_payment_date_ticket
        TextView tvDate = (TextView) findViewById(R.id.tv_finish_payment_date_ticket);
        tvDate.setText(accountPurchasedModel.getDate());

        //tv_finish_payment_phone
        TextView tvPhoneNumber = (TextView) findViewById(R.id.tv_finish_payment_phone);
        tvPhoneNumber.setText(accountPurchasedModel.getPhone());

        //tv_finish_payment_email
        TextView tvEmail = (TextView) findViewById(R.id.tv_finish_payment_email);
        tvEmail.setText(accountPurchasedModel.getEmail());

        //danh sach cac ve
        recyclerView = findViewById(R.id.rcl_ticket_list);

        TicketService ticketService = ServiceBuilder.buildService(TicketService.class);
        Call<List<TicketModel>> request = ticketService.getAllTicketByBookingTicketId(bookingTicketId);

        request.enqueue(new Callback<List<TicketModel>>() {
            @Override
            public void onResponse(Call<List<TicketModel>> request, Response<List<TicketModel>> response) {
                data = response.body();

                boolean isCanChange = accountPurchasedModel.isCanChange();

                for (TicketModel ticketModel : data) {
                    ticketModel.setCanChange(isCanChange);
                }

                PurchasedTicketAdapter adapter = new PurchasedTicketAdapter(data, OrderDetailActivity.this);

                LinearLayoutManager layoutManager = new LinearLayoutManager(context);
                layoutManager.setOrientation(LinearLayoutManager.VERTICAL);

                recyclerView.setLayoutManager(layoutManager);
                recyclerView.setAdapter(adapter);

                adapter.setOnItemClickedListener(new PurchasedTicketAdapter.OnItemClickedListener() {
                    @Override
                    public void onItemClick(int position) {
                        Intent intentChangeTicket = new Intent(getApplicationContext(), ChangeTicketActivity.class);

                        //pass filmId, cinemaId, indexDate to ChangeTicketActivity
                        intentChangeTicket.putExtra("cinemaId", data.get(position).getCinemaId());
                        intentChangeTicket.putExtra("cinemaName", data.get(position).getCinemaName());
                        intentChangeTicket.putExtra("indexDate", data.get(position).getIndexDate());
                        intentChangeTicket.putExtra("filmId", data.get(position).getFilmId());
                        intentChangeTicket.putExtra("filmName", data.get(position).getFilmName());
                        intentChangeTicket.putExtra("price", data.get(position).getPrice());
                        intentChangeTicket.putExtra("ticketId", data.get(position).getTicketId());
                        intentChangeTicket.putExtra("scheduleId", data.get(position).getScheduleId());
                        intentChangeTicket.putExtra("groupCinemaName", data.get(position).getGroupCinemaName());
                        //cinemaName

                        startActivityForResult(intentChangeTicket, REQUEST_CODE_CHANGE_TICKET);
                    }
                });
            }

            @Override
            public void onFailure(Call<List<TicketModel>> request, Throwable t) {
                Toast.makeText(context, "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REQUEST_CODE_CHANGE_TICKET) {
            recreateView();
        }
    }

    public void recreateView() {
        this.recreate();
    }


}
