package com.example.luulac.cinemaapplication.activities;

import android.app.Activity;
import android.content.Intent;
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
import com.example.luulac.cinemaapplication.config.Config;
import com.example.luulac.cinemaapplication.data.models.BookingDetailModel;
import com.example.luulac.cinemaapplication.data.models.BookingTicketModel;
import com.example.luulac.cinemaapplication.data.models.CustomerModel;
import com.example.luulac.cinemaapplication.data.models.SeatCollectionModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;
import com.example.luulac.cinemaapplication.services.OrderService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;
import com.paypal.android.sdk.payments.PayPalConfiguration;
import com.paypal.android.sdk.payments.PayPalPayment;
import com.paypal.android.sdk.payments.PayPalService;
import com.paypal.android.sdk.payments.PaymentActivity;
import com.paypal.android.sdk.payments.PaymentConfirmation;

import org.json.JSONException;

import java.util.ArrayList;
import java.util.List;
import java.math.BigDecimal;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PaymentApplicationActivity extends AppCompatActivity {

    private SeatCollectionModel resultSaveSeat;
    private static final int PAYPAL_REQUEST_CODE = 7;
    private static PayPalConfiguration config = new PayPalConfiguration().environment(PayPalConfiguration.ENVIRONMENT_SANDBOX).clientId(Config.PAYPAL_CLIENT_ID);

    private String amount = "";
    private String stringSeats;
    private FilmTranferModel filmTranfer;
    private ScheduleTranferModel scheduleTranfer;
    private SeatCollectionModel seatCollectionModel;

    private EditText edtEmail;
    private EditText edtPhone;

    private BookingTicketModel result;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.activity_payment);

        Intent intent = this.getIntent();

        filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");
        scheduleTranfer = (ScheduleTranferModel) intent.getSerializableExtra("scheduleTranfer");
        List<TicketModel> seats = (ArrayList<TicketModel>) (intent.getBundleExtra("listTicket").getSerializable("list"));
        stringSeats = intent.getStringExtra("stringSeats");

        seatCollectionModel = new SeatCollectionModel(filmTranfer.getScheduleId(), seats);

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
        amount = (scheduleTranfer.getTotalPrice()/22000) + "";

        ImageView image = (ImageView) findViewById(R.id.img_payment_film_picture);
        Glide.with(getApplicationContext()).load(scheduleTranfer.getFilmImage()).into(image);

        edtEmail = (EditText) findViewById(R.id.edt_payment_email);
        edtPhone = (EditText) findViewById(R.id.edt_payment_phone);

        //rl_finish_payment_next
        RelativeLayout relativeLayout = (RelativeLayout) findViewById(R.id.rl_finish_payment_next);
        relativeLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                OrderService orderService = ServiceBuilder.buildService(OrderService.class);
                Call<SeatCollectionModel> request = orderService.orderTicket(seatCollectionModel);

                //receiving and process data from the server
                request.enqueue(new Callback<SeatCollectionModel>() {
                    @Override
                    public void onResponse(Call<SeatCollectionModel> request, Response<SeatCollectionModel> response) {
                        seatCollectionModel = response.body();

                        if (seatCollectionModel.isSuccesBookingTicket()) {
                            processPayment();
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

    private void processPayment() {
        PayPalPayment payPalPayment = new PayPalPayment(new BigDecimal(String.valueOf(amount)), "USD", "Payment for ticket", PayPalPayment.PAYMENT_INTENT_SALE);

        Intent intentPaypal = new Intent(this, PaymentActivity.class);
        intentPaypal.putExtra(PayPalService.EXTRA_PAYPAL_CONFIGURATION, config);
        intentPaypal.putExtra(PaymentActivity.EXTRA_PAYMENT, payPalPayment);

        startActivityForResult(intentPaypal, PAYPAL_REQUEST_CODE);
    }

    @Override
    protected void onDestroy() {
        stopService(new Intent(this, PayPalService.class));
        super.onDestroy();
    }

    //process paypal payment after login paypal
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if(requestCode == PAYPAL_REQUEST_CODE){
            if(resultCode == RESULT_OK){
                PaymentConfirmation confirmation = data.getParcelableExtra(PaymentActivity.EXTRA_RESULT_CONFIRMATION);
                if(confirmation != null){
                    try {
                        String paymentDetail = confirmation.toJSONObject().toString(4);

                        List<BookingDetailModel> bookingDetails = new ArrayList<>();

                        for (int i = 0; i < scheduleTranfer.getQuantityTicket(); i++) {

                            int ticketId = seatCollectionModel.getTicketModels().get(i).getTicketId();

                            //new booking detail object
                            BookingDetailModel bookingTicketModel = new BookingDetailModel(ticketId);

                            //add booking detail object to list booking detail
                            bookingDetails.add(bookingTicketModel);
                        }

                        //new Customer
                        CustomerModel customer = new CustomerModel(edtEmail.getText().toString(), edtPhone.getText().toString());

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

                                Intent intentPaymentFinished = new Intent(getApplicationContext(), FinishPaymentActivity.class);

                                intentPaymentFinished.putExtra("scheduleTranfer", scheduleTranfer);
                                intentPaymentFinished.putExtra("filmTranfer", filmTranfer);
                                intentPaymentFinished.putExtra("stringSeats", stringSeats);
                                intentPaymentFinished.putExtra("seatCollectionModel", seatCollectionModel);
                                intentPaymentFinished.putExtra("email", edtEmail.getText().toString());
                                intentPaymentFinished.putExtra("phone", edtPhone.getText().toString());

                                startActivity(intentPaymentFinished);
                            }

                            @Override
                            public void onFailure(Call<BookingTicketModel> request, Throwable t) {
                                Toast.makeText(getApplicationContext(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                            }
                        });

                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                }
            }else if(resultCode == Activity.RESULT_CANCELED){
                Toast.makeText(this, "Cancel", Toast.LENGTH_SHORT).show();
            }
        }else if(requestCode == PaymentActivity.RESULT_EXTRAS_INVALID){
            Toast.makeText(this, "Invalid", Toast.LENGTH_SHORT).show();
        }
    }
}
