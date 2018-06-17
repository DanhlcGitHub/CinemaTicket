package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.media.Image;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.SeatCollectionModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;
import com.example.luulac.cinemaapplication.services.OrderService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PaymentActivity extends AppCompatActivity {

    private SeatCollectionModel result;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.activity_payment);

        final Intent intent = this.getIntent();

        final FilmTranferModel filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");
        final ScheduleTranferModel scheduleTranfer = (ScheduleTranferModel) intent.getSerializableExtra("scheduleTranfer");
        final List<TicketModel> seats = (ArrayList<TicketModel>) (intent.getBundleExtra("listTicket").getSerializable("list"));
        final String stringSeats = intent.getStringExtra("stringSeats");

        final SeatCollectionModel seatCollectionModel = new SeatCollectionModel(filmTranfer.getScheduleId(), seats);

        //tv_order_ticket_short_key
        TextView tvGroupCinemaName = (TextView) findViewById(R.id.tv_order_ticket_short_key);
        tvGroupCinemaName.setText(scheduleTranfer.getGroupCinemaName());

        //tv_order_ticket_cinema_name
        TextView tvCinemaName = (TextView) findViewById(R.id.tv_order_ticket_cinema_name);
        tvCinemaName.setText(scheduleTranfer.getCinemaName());

        //tv_payment_ticket_full
        TextView tvTicketFull = (TextView) findViewById(R.id.tv_payment_ticket_full);
        tvTicketFull.setText(scheduleTranfer.getShowTime() + " - " + scheduleTranfer.getCinemaName() + " - " + scheduleTranfer.getRoomName());
        //tv_payment_filmd_name
        TextView tvFilmName = (TextView) findViewById(R.id.tv_payment_filmd_name);
        tvFilmName.setText(scheduleTranfer.getFilmName());

        //tv_payment_restricted
        TextView tvRestricted = (TextView) findViewById(R.id.tv_payment_restricted);
        tvRestricted.setText(scheduleTranfer.getRestricted());

        //tv_payment_film_sub_info
        TextView tvSubInfo = (TextView) findViewById(R.id.tv_payment_film_sub_info);
        tvSubInfo.setText(scheduleTranfer.getFilmLength() + " - " + scheduleTranfer.getDigType());

        //tv_payment_film_list_seat
        TextView tvStringSeats = (TextView) findViewById(R.id.tv_payment_film_list_seat);
        tvStringSeats.setText(stringSeats);

        //tv_order_ticker_price
        TextView tvTotalPrice = (TextView) findViewById(R.id.tv_order_ticker_price);
        tvTotalPrice.setText(scheduleTranfer.getTotalPrice().toString());

        ImageView image = (ImageView) findViewById(R.id.img_payment_film_picture);
        Glide.with(getApplicationContext()).load(scheduleTranfer.getFilmImage()).into(image);

        final EditText edtEmail = (EditText) findViewById(R.id.edt_payment_email);
        final EditText edtPhone = (EditText) findViewById(R.id.edt_payment_phone);

        //rl_finish_payment_next
        RelativeLayout relativeLayout = (RelativeLayout) findViewById(R.id.rl_finish_payment_next);
        relativeLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                //call service to save list ticket
                OrderService orderService = ServiceBuilder.buildService(OrderService.class);
                Call<SeatCollectionModel> request = orderService.orderTicket(seatCollectionModel);

                //get email and phone from edit
                final String email = edtEmail.getText().toString();
                final String phone = edtPhone.getText().toString();

                //receiving and process data from the server
                request.enqueue(new Callback<SeatCollectionModel>() {
                    @Override
                    public void onResponse(Call<SeatCollectionModel> request, Response<SeatCollectionModel> response) {
                        result = response.body();

                        if (result.isSuccesBookingTicket()) {
                            Intent intentPaypalPayment = new Intent(getApplicationContext(), PaypalPaymentActivity.class);

                            intentPaypalPayment.putExtra("stringSeats", stringSeats);
                            intentPaypalPayment.putExtra("email", email);
                            intentPaypalPayment.putExtra("phone", phone);
                            intentPaypalPayment.putExtra("scheduleTranfer", scheduleTranfer);
                            intentPaypalPayment.putExtra("filmTranfer", filmTranfer);
                            intentPaypalPayment.putExtra("stringSeats", stringSeats);
                            intentPaypalPayment.putExtra("seatCollectionModel", result);

                            startActivity(intentPaypalPayment);

                        } else {
                            Toast.makeText(getApplicationContext(), "Ghế tạm thời đang trong thời gian giao dịch", Toast.LENGTH_LONG).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<SeatCollectionModel> request, Throwable t) {
                        Toast.makeText(getApplicationContext(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                    }
                });
            }
        });
    }
}
