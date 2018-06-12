package com.example.luulac.cinemaapplication.fragments.films;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.activities.OrderTicketActivity;
import com.example.luulac.cinemaapplication.adapters.ScheduleDateAdapter;
import com.example.luulac.cinemaapplication.data.models.showtimes.DateScheduleModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.FilmScheduleModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeChildModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeListModel;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.FIlmScheduleService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.ArrayList;
import java.util.List;

import iammert.com.expandablelib.ExpandCollapseListener;
import iammert.com.expandablelib.ExpandableLayout;
import iammert.com.expandablelib.Section;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ScheduleShowFragment extends Fragment {

    private Context context;
    private RecyclerView recyclerView;
    private ScheduleDateAdapter adapter;
    private List<ShowTimeListModel> showTimes = new ArrayList<>();
    private List<Section<ShowTimeListModel, ShowTimeChildModel>> sections;
    private  View view;

    private String filmName;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragmen_schedule_show, null);
        context = view.getContext();


        recyclerView = (RecyclerView) view.findViewById(R.id.rv_schedule_show);


        FIlmScheduleService service = ServiceBuilder.buildService(FIlmScheduleService.class);
        Call<List<FilmScheduleModel>> request = service.getScheduleByDayAndFilmId();

        request.enqueue(new Callback<List<FilmScheduleModel>>() {
            @Override
            public void onResponse(Call<List<FilmScheduleModel>> request, Response<List<FilmScheduleModel>> response) {
                List<FilmScheduleModel> models = response.body();
                adapter = new ScheduleDateAdapter(context, models);

                LinearLayoutManager layoutManager
                        = new LinearLayoutManager(context, LinearLayoutManager.HORIZONTAL, false);

                recyclerView.setLayoutManager(layoutManager);
                recyclerView.setAdapter(adapter);

                //dang bi loi click k dc
                adapter.setOnItemClickListener(new ScheduleDateAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(String filmName) {
                        Toast.makeText(getContext(), "Comming Soon Card View " + filmName +" clicked!", Toast.LENGTH_SHORT).show();
                    }
                });

                ExpandableLayout expandableLayout = (ExpandableLayout) view.findViewById(R.id.el_schedule_show);

                expandableLayout.setRenderer(new ExpandableLayout.Renderer<ShowTimeListModel, ShowTimeChildModel>() {
                    @Override
                    public void renderParent(View view, ShowTimeListModel model, boolean isExpanded, int parentPosition) {
                        ImageView imgLogo = (ImageView) view.findViewById(R.id.img_showtime_group_cinema_logo);

                        Glide.with(context)
                                .load(BaseService.BASE_URL + model.getGroupCinemaLogo())
                                .into(imgLogo);
                        ((TextView) view.findViewById(R.id.tv_showtime_cinema_name)).setText(model.getCinemaName());
                        view.findViewById(R.id.icon_arrow_showtime_list).setBackgroundResource(isExpanded ? R.drawable.arrow_down : R.drawable.arrow_right);
                    }

                    @Override
                    public void renderChild(View view, ShowTimeChildModel model, int parentPosition, int childPosition) {
                        ((TextView) view.findViewById(R.id.tv_showtime_child_time_start)).setText(model.getTimeStart());
                        ((TextView) view.findViewById(R.id.tv_showtime_child_time_end)).setText(model.getTimeEnd());
                        ((TextView) view.findViewById(R.id.tv_showtime_child_type)).setText(model.getType());
                        ((TextView) view.findViewById(R.id.tv_showtime_child_price)).setText("~ " + model.getPrice());

                        view.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View v) {
                                Intent intent = new Intent(context, OrderTicketActivity.class);
                                startActivity(intent);
                            }
                        });

                    }
                });

                List<DateScheduleModel> dateScheduleModels = models.get(0).getDateScheduleModels();
                for (int i = 0; i < dateScheduleModels.size(); i++) {
                    showTimes.add(dateScheduleModels.get(i).getShowTimeListModel());
                };

                sections = setSection(showTimes);

                for (Section<ShowTimeListModel, ShowTimeChildModel> section : sections) {
                    expandableLayout.addSection(section);
                }

                expandableLayout.setExpandListener(new ExpandCollapseListener.ExpandListener<ShowTimeListModel>() {
                    @Override
                    public void onExpanded(int parentIndex, ShowTimeListModel parent, View view) {
                        view.findViewById(R.id.icon_arrow_showtime_list).setBackgroundResource(R.drawable.arrow_down);
                    }
                });

                expandableLayout.setCollapseListener(new ExpandCollapseListener.CollapseListener<ShowTimeListModel>() {
                    @Override
                    public void onCollapsed(int parentIndex, ShowTimeListModel parent, View view) {
                        view.findViewById(R.id.icon_arrow_showtime_list).setBackgroundResource(R.drawable.arrow_right);
                    }
                });
            }

            @Override
            public void onFailure(Call<List<FilmScheduleModel>> request, Throwable t) {
                Toast.makeText(context, "Failed to retrieve item.", Toast.LENGTH_SHORT).show();;
            }
        });

        return view;
    }

    public List<Section<ShowTimeListModel, ShowTimeChildModel>> setSection(List<ShowTimeListModel> models) {
        List<Section<ShowTimeListModel, ShowTimeChildModel>> result = new ArrayList<>();

        for (int i = 0; i < models.size(); i++) {
            Section<ShowTimeListModel, ShowTimeChildModel> section = new Section<>();

            section.parent = models.get(i);

            section.children = models.get(i).getShowTimeChildModels();

            result.add(section);
        }

        return result;
    }



}
