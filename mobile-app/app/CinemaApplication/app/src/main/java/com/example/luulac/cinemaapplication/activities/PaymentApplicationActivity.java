package com.example.luulac.cinemaapplication.activities;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.support.annotation.Nullable;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
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
import com.example.luulac.cinemaapplication.data.models.BookingTicketModel;
import com.example.luulac.cinemaapplication.data.models.CustomerModel;
import com.example.luulac.cinemaapplication.data.models.SeatCollectionModel;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;
import com.example.luulac.cinemaapplication.services.ChangeStatusService;
import com.example.luulac.cinemaapplication.services.OrderService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;
import com.google.firebase.iid.FirebaseInstanceId;
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

    private final int PAYPAL_REQUEST_CODE = 7;
    private final int REQUEST_CODE_ORDER = 256;

    private CountDownTimer countDownTimer;
    private long timeLeftMilliseconds = 300000;
    private long timeReduceMilliseconds = 1000;


    private static PayPalConfiguration config = new PayPalConfiguration().environment(PayPalConfiguration.ENVIRONMENT_SANDBOX).clientId(Config.PAYPAL_CLIENT_ID);

    private String amount = "";
    private String stringSeats;
    private FilmTranferModel filmTranfer;
    private ScheduleTranferModel scheduleTranfer;
    private SeatCollectionModel seatCollectionModel;
    private SeatCollectionModel newSeatCollectionModel;
    private BookingTicketModel result;

    private EditText edtEmail;
    private EditText edtPhone;
    private ImageView iconCancelOrder;
    private TextView timer;

    List<TicketModel> resultChangeTickets;

    public void updateTimer() {
        int minutes = (int) timeLeftMilliseconds / 60000;
        int seconds = (int) timeLeftMilliseconds % 60000 / 1000;

        String timeText;

        timeText = "" + minutes;
        timeText += ":";
        if (seconds < 10)
            timeText += "0";
        timeText += seconds;

        timer.setText(timeText);
        Log.d("updateTimer", timeText);
    }

    @SuppressLint("SetTextI18n")
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.activity_payment);

        Intent intentPaypalService = new Intent(this, PayPalService.class);

        intentPaypalService.putExtra(PayPalService.EXTRA_PAYPAL_CONFIGURATION, config);

        startService(intentPaypalService);

        final Intent intent = this.getIntent();

        filmTranfer = (FilmTranferModel) intent.getSerializableExtra("filmTranfer");
        scheduleTranfer = (ScheduleTranferModel) intent.getSerializableExtra("scheduleTranfer");
        List<TicketModel> seats = (ArrayList<TicketModel>) (intent.getBundleExtra("listTicket").getSerializable("list"));
        stringSeats = intent.getStringExtra("stringSeats");

        iconCancelOrder = findViewById(R.id.icon_cancel_order);

        iconCancelOrder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                comfirmCancelOrder();
            }
        });

        seatCollectionModel = new SeatCollectionModel(filmTranfer.getScheduleId(), seats);

        //tv_order_ticket_short_key
        TextView tvGroupCinemaName = findViewById(R.id.tv_order_ticket_short_key);
        tvGroupCinemaName.setText(scheduleTranfer.getGroupCinemaName());

        //tv_order_ticket_cinema_name
        TextView tvCinemaName = findViewById(R.id.tv_order_ticket_cinema_name);
        tvCinemaName.setText(scheduleTranfer.getCinemaName());

        //tv_payment_ticket_full
        TextView tvTicketFull = findViewById(R.id.tv_payment_ticket_full);
        tvTicketFull.setText(scheduleTranfer.getShowTime() + " - " + scheduleTranfer.getCinemaName() + " - " + scheduleTranfer.getRoomName());

        //tv_payment_filmd_name
        TextView tvFilmName = findViewById(R.id.tv_payment_filmd_name);
        tvFilmName.setText(scheduleTranfer.getFilmName());

        //tv_payment_restricted
        TextView tvRestricted = findViewById(R.id.tv_payment_restricted);
        tvRestricted.setText(scheduleTranfer.getRestricted());

        //tv_payment_film_sub_info
        TextView tvSubInfo = findViewById(R.id.tv_payment_film_sub_info);
        tvSubInfo.setText(scheduleTranfer.getFilmLength() + " - " + scheduleTranfer.getDigType());

        //tv_payment_film_list_seat
        TextView tvStringSeats = findViewById(R.id.tv_payment_film_list_seat);
        tvStringSeats.setText(stringSeats);

        //tv_order_ticker_price
        TextView tvTotalPrice = findViewById(R.id.tv_order_ticker_price);
        tvTotalPrice.setText(scheduleTranfer.getTotalPrice().toString());
        amount = (scheduleTranfer.getTotalPrice() / 23000) + "";

        ImageView image = findViewById(R.id.img_payment_film_picture);
        Glide.with(getApplicationContext()).load(scheduleTranfer.getFilmImage()).into(image);

        edtEmail = findViewById(R.id.edt_payment_email);
        edtPhone = findViewById(R.id.edt_payment_phone);


        SharedPreferences sharedPreferences = getSharedPreferences("loginInformation", Context.MODE_PRIVATE);

        boolean isLogin = sharedPreferences.getBoolean("isLogin", false);

        if (isLogin) {
            edtEmail.setText(sharedPreferences.getString("email", ""));
            edtPhone.setText(sharedPreferences.getString("phoneNumber", ""));
        }

        //rl_finish_payment_next
        RelativeLayout relativeLayout = findViewById(R.id.rl_finish_payment_next);
        relativeLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                OrderService orderService = ServiceBuilder.buildService(OrderService.class);
                Call<SeatCollectionModel> request = orderService.orderTicket(seatCollectionModel);

                //receiving and process data from the server
                request.enqueue(new Callback<SeatCollectionModel>() {
                    @Override
                    public void onResponse(Call<SeatCollectionModel> request, Response<SeatCollectionModel> response) {
                        newSeatCollectionModel = response.body();

                        if (newSeatCollectionModel.isSuccesBookingTicket()) {
                            Intent intentChangeStatus = new Intent(getBaseContext(), ChangeStatusService.class);
                            intentChangeStatus.putExtra("seatCollectionModel", newSeatCollectionModel);
                            startService(intentChangeStatus);

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

        timer = findViewById(R.id.tv_time_count_down_payment_activity);

        countDownTimer = new CountDownTimer(timeLeftMilliseconds, timeReduceMilliseconds) {
            @Override
            public void onTick(long millisUntilFinished) {
                timeLeftMilliseconds = millisUntilFinished;
                updateTimer();
            }

            @Override
            public void onFinish() {
                if (newSeatCollectionModel != null) {
                    List<TicketModel> ticketModels = newSeatCollectionModel.getTicketModels();
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
                }

                Toast.makeText(PaymentApplicationActivity.this, "Time out", Toast.LENGTH_SHORT).show();

                finish();
            }
        };
        countDownTimer.start();
    }

    private void processPayment() {
        Log.d("processPayment", "processPayment");
        PayPalPayment payPalPayment = new PayPalPayment(new BigDecimal(String.valueOf(amount)), "USD", "Payment for ticket", PayPalPayment.PAYMENT_INTENT_SALE);

        Intent intentPaypal = new Intent(this, PaymentActivity.class);
        intentPaypal.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
        intentPaypal.putExtra(PayPalService.EXTRA_PAYPAL_CONFIGURATION, config);
        intentPaypal.putExtra(PaymentActivity.EXTRA_PAYMENT, payPalPayment);
        startActivityForResult(intentPaypal, PAYPAL_REQUEST_CODE);


    }

    @Override
    protected void onDestroy() {
        countDownTimer.cancel();
        finishActivity(PAYPAL_REQUEST_CODE);
        //stopService(new Intent(this, PayPalService.class));
        OrderService orderService = ServiceBuilder.buildService(OrderService.class);

        if (result != null) {
            List<TicketModel> ticketModels = result.getTickets();
            resultChangeTickets = new ArrayList<>();
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
        }
        super.onDestroy();
        finish();
    }

    //process paypal payment after login paypal
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == PAYPAL_REQUEST_CODE) {
            if(timeLeftMilliseconds > 2000){
                if (resultCode == RESULT_OK) {
                    PaymentConfirmation confirmation = data.getParcelableExtra(PaymentActivity.EXTRA_RESULT_CONFIRMATION);
                    if (confirmation != null) {
                        try {
                            String paymentDetail = confirmation.toJSONObject().toString(4);

                            List<TicketModel> ticketModels = new ArrayList<>();

                            for (int i = 0; i < scheduleTranfer.getQuantityTicket(); i++) {

                                ticketModels.add(newSeatCollectionModel.getTicketModels().get(i));
                            }

                            SharedPreferences sharedPreferences = getSharedPreferences("loginInformation", Context.MODE_PRIVATE);

                            String userId = sharedPreferences.getString("username", "");
                            //new Customer
                            CustomerModel customer = new CustomerModel(edtEmail.getText().toString(), edtPhone.getText().toString(), userId);

                            //new Booking ticket model for send data to server
                            BookingTicketModel bookingTicket = new BookingTicketModel(scheduleTranfer.getQuantityTicket(), customer,
                                                                                         ticketModels);
                            bookingTicket.setCinemaName(scheduleTranfer.getCinemaName());
                            bookingTicket.setFilmName(scheduleTranfer.getFilmName());
                            bookingTicket.setRoomName(scheduleTranfer.getRoomName());
                            bookingTicket.setStartTime(scheduleTranfer.getShowTime());

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
                                    intentPaymentFinished.putExtra("seatCollectionModel", newSeatCollectionModel);
                                    intentPaymentFinished.putExtra("email", edtEmail.getText().toString());
                                    intentPaymentFinished.putExtra("phone", edtPhone.getText().toString());

                                    startActivityForResult(intentPaymentFinished, REQUEST_CODE_ORDER);
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
                } else if (resultCode == Activity.RESULT_CANCELED) {

                    //process when user cancel paypal
                    //=> return ticketStatus Buying -> Available
                    OrderService orderService = ServiceBuilder.buildService(OrderService.class);
                    List<TicketModel> ticketModels = newSeatCollectionModel.getTicketModels();
                    resultChangeTickets = new ArrayList<>();
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
                }
            }
        } else if (requestCode == PaymentActivity.RESULT_EXTRAS_INVALID) {
            finish();
        } else if (requestCode == REQUEST_CODE_ORDER) {
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
}
