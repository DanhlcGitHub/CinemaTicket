package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.ChangeTicketAdapter;
import com.example.luulac.cinemaapplication.data.models.OrderChoiceTicketModel;
import com.example.luulac.cinemaapplication.data.models.UserAccountModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeChildModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeListModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.FilmTranferModel;
import com.example.luulac.cinemaapplication.data.models.tranfers.ScheduleTranferModel;
import com.example.luulac.cinemaapplication.services.AccountService;
import com.example.luulac.cinemaapplication.services.FIlmScheduleService;
import com.example.luulac.cinemaapplication.services.OrderService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ChangeTicketActivity extends AppCompatActivity {

    private RecyclerView recyclerView;
    private ShowTimeListModel data;
    private OrderChoiceTicketModel modelOrder;
    private final int REQUEST_CODE_CHANGE_TICKET = 102;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.layout_change_ticket);

        recyclerView = findViewById(R.id.rcl_list_schedule_change);

        //dung intent lay du lieu pass qua tu OrderDetailActivity

        Intent intent = getIntent();

        final int cinemaId = intent.getIntExtra("cinemaId", 0);
        int indexDate = intent.getIntExtra("indexDate", 0);
        final int filmId = intent.getIntExtra("filmId", 0);
        final double price = intent.getDoubleExtra("price", 0);
        final int ticketId = intent.getIntExtra("ticketId", 0);

        //request getScheduleForChangeTicket
        FIlmScheduleService service = ServiceBuilder.buildService(FIlmScheduleService.class);
        Call<ShowTimeListModel> request = service.getScheduleForChangeTicket(cinemaId, indexDate, filmId);

        request.enqueue(new Callback<ShowTimeListModel>() {
            @Override
            public void onResponse(Call<ShowTimeListModel> request, Response<ShowTimeListModel> response) {
                data = response.body();
                data.getShowTimeChildModels();

                List<ShowTimeChildModel> childs = data.getShowTimeChildModels();
                ChangeTicketAdapter adapter = new ChangeTicketAdapter(childs);

                LinearLayoutManager layoutManager = new LinearLayoutManager(getApplicationContext(), LinearLayoutManager.VERTICAL, false);

                recyclerView.setLayoutManager(layoutManager);
                recyclerView.setAdapter(adapter);
                adapter.setOnItemClickedListener(new ChangeTicketAdapter.OnItemClickedListener() {
                    @Override
                    public void onItemClick(int position) {

                        final ShowTimeChildModel model = data.getShowTimeChildModels().get(position);

                        //request getOrderChoiceTicket
                        OrderService orderService = ServiceBuilder.buildService(OrderService.class);
                        Call<OrderChoiceTicketModel> requestOrder = orderService.getOrderChoiceTicket(model.getScheduleId());

                        //receiving and process data from the server
                        requestOrder.enqueue(new Callback<OrderChoiceTicketModel>() {
                            @Override
                            public void onResponse(Call<OrderChoiceTicketModel> request, Response<OrderChoiceTicketModel> response) {
                                modelOrder = response.body();

                                Intent intent = new Intent(getApplicationContext(), ChoiceSeatsActivity.class);

                                ScheduleTranferModel scheduleTranfer = new ScheduleTranferModel(1, price, modelOrder.getGroupCinemaName(), modelOrder.getTimeShow(),
                                        modelOrder.getCinemaName(), modelOrder.getRoomName(), modelOrder.getFilmName(), modelOrder.getRestricted(), modelOrder.getFilmLength(),
                                        modelOrder.getDigType(), modelOrder.getFilmImage());

                                FilmTranferModel filmTranfer = new FilmTranferModel(model.getFilmId(), model.getRoomId(), model.getGroupId(), model.getScheduleId(),
                                        model.getCol(), model.getRow(), model.getDatetime());

                                intent.putExtra("scheduleTranfer", scheduleTranfer);
                                intent.putExtra("filmTranfer", filmTranfer);
                                intent.putExtra("isChangeTicket", true);
                                intent.putExtra("ticketId", ticketId);

                                startActivityForResult(intent, REQUEST_CODE_CHANGE_TICKET);
                            }

                            @Override
                            public void onFailure(Call<OrderChoiceTicketModel> request, Throwable t) {
                                Toast.makeText(getApplication(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();

                            }
                        });

                    }
                });
            }

            @Override
            public void onFailure(Call<ShowTimeListModel> request, Throwable t) {
                Toast.makeText(getApplication(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REQUEST_CODE_CHANGE_TICKET) {
            finish();
        }
    }
}
