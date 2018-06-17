package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.view.WindowManager;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.BookingDetailModel;
import com.example.luulac.cinemaapplication.data.models.BookingTicketModel;
import com.example.luulac.cinemaapplication.data.models.CustomerModel;
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

public class PaypalPaymentActivity extends AppCompatActivity {

    private BookingTicketModel result;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.activity_paypal_payment);

        final Intent intent = this.getIntent();

        final FilmTranferModel filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");
        final ScheduleTranferModel scheduleTranfer = (ScheduleTranferModel) intent.getSerializableExtra("scheduleTranfer");
        final SeatCollectionModel seatCollectionModel = (SeatCollectionModel) intent.getExtras().getSerializable("seatCollectionModel");
        final String email = intent.getStringExtra("email");
        final String phone = intent.getStringExtra("phone");

        final String stringSeats = intent.getStringExtra("stringSeats");

        TextView tvTotalPayment = (TextView) findViewById(R.id.tv_paypal_payment_total_price);
        tvTotalPayment.setText("ĐẶT VÉ (" + scheduleTranfer.getTotalPrice() + ") đ");

        //set event for click paypal payment
        RelativeLayout relativeLayout = (RelativeLayout) findViewById(R.id.rl_paypal);
        relativeLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                List<BookingDetailModel> bookingDetails = new ArrayList<>();

                for (int i = 0; i < scheduleTranfer.getQuantityTicket(); i++) {

                    int ticketId = seatCollectionModel.getTicketModels().get(i).getTicketId();

                    //new booking detail object
                    BookingDetailModel bookingTicketModel = new BookingDetailModel(ticketId);

                    //add booking detail object to list booking detail
                    bookingDetails.add(bookingTicketModel);
                }

                //new Customer
                CustomerModel customer = new CustomerModel(email, phone);

                //new Booking ticket model for send data to server
                BookingTicketModel bookingTicket = new BookingTicketModel(scheduleTranfer.getQuantityTicket(),customer,bookingDetails);

                //call service from server to make booking ticket and booking detail
                OrderService orderService = ServiceBuilder.buildService(OrderService.class);
                Call<BookingTicketModel> request = orderService.finishPaypalPayment(bookingTicket);

                //receiving and process data from the server
                request.enqueue(new Callback<BookingTicketModel>() {
                    @Override
                    public void onResponse(Call<BookingTicketModel> request, Response<BookingTicketModel> response) {
                        result = response.body();

                        //if(result.isSuccesBookingTicket()){
                        Intent intentPaymentFinished = new Intent(getApplicationContext(), FinishPaymentActivity.class);

                        intentPaymentFinished.putExtra("scheduleTranfer", scheduleTranfer);
                        intentPaymentFinished.putExtra("filmTranfer", filmTranfer);
                        intentPaymentFinished.putExtra("stringSeats", stringSeats);
                        intentPaymentFinished.putExtra("seatCollectionModel", seatCollectionModel);

                        startActivity(intentPaymentFinished);
                       /* }
                        else{
                            Toast.makeText(getApplicationContext(), "Có lỗi trong quá trình giao dịch. Mời bạn thử lại.", Toast.LENGTH_LONG).show();;
                        }*/
                    }

                    @Override
                    public void onFailure(Call<BookingTicketModel> request, Throwable t) {
                        Toast.makeText(getApplicationContext(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                    }
                });

            }
        });
    }
}
