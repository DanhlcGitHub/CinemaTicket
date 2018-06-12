package com.example.luulac.cinemaapplication.fragments.theaters;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ExpandableListView;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.TheaterListAdapter;
import com.example.luulac.cinemaapplication.data.models.CinemaModel;
import com.example.luulac.cinemaapplication.data.models.Cinemas;
import com.example.luulac.cinemaapplication.data.models.GroupCinemaModel;
import com.example.luulac.cinemaapplication.data.models.Theater;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.GroupCinemaService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import iammert.com.expandablelib.ExpandCollapseListener;
import iammert.com.expandablelib.ExpandableLayout;
import iammert.com.expandablelib.Section;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class TheaterFragment extends Fragment {

    private ExpandableListView expandableListView;
    private TheaterListAdapter adapter;
    private List<Theater> theaters;
    private List<Cinemas> cinemas;
    private HashMap<Cinemas, List<Theater>> mapCinema;

    private Context context;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_theater, null);
        context = view.getContext();


        final ExpandableLayout expandableLayout = (ExpandableLayout) view.findViewById(R.id.el);

        GroupCinemaService service = ServiceBuilder.buildService(GroupCinemaService.class);
        Call<List<GroupCinemaModel>> call = service.getCinemaGroup();

        expandableLayout.setRenderer(new ExpandableLayout.Renderer<GroupCinemaModel, CinemaModel>() {
            @Override
            public void renderParent(View view, GroupCinemaModel model, boolean isExpanded, int parentPosition) {
                ImageView iconCinema = (ImageView) view.findViewById(R.id.cinema_icon);
                Glide.with(context).load(BaseService.BASE_URL + model.getLogoImg()).into(iconCinema);

                ((TextView) view.findViewById(R.id.tv_theater_list_cinemas_name)).setText(model.getName());
                view.findViewById(R.id.icon_arrow).setBackgroundResource(isExpanded ? R.drawable.arrow_down : R.drawable.arrow_right);
            }

            @Override
            public void renderChild(View view, CinemaModel model, int parentPosition, int childPosition) {
                ((TextView) view.findViewById(R.id.tv_theater_item_theater_name)).setText(model.getCinemaName());
                ((TextView) view.findViewById(R.id.tv_theater_item_theater_location)).setText(model.getCinemaAddress());

            }
        });

        call.enqueue(new Callback<List<GroupCinemaModel>>() {
            @Override
            public void onResponse(Call<List<GroupCinemaModel>> call, Response<List<GroupCinemaModel>> response) {
                List<GroupCinemaModel> groupCinema = response.body();

                List<Section<GroupCinemaModel, CinemaModel>> sections = setSection(groupCinema);

                for (Section<GroupCinemaModel, CinemaModel> section : sections) {
                    expandableLayout.addSection(section);
                }
            }

            @Override
            public void onFailure(Call<List<GroupCinemaModel>> call, Throwable t) {
                Log.d("HttpFail", t.getMessage());
            }
        });

        expandableLayout.setExpandListener(new ExpandCollapseListener.ExpandListener<GroupCinemaModel>() {
            @Override
            public void onExpanded(int parentIndex, GroupCinemaModel parent, View view) {
                view.findViewById(R.id.icon_arrow).setBackgroundResource(R.drawable.arrow_down);
            }
        });

        expandableLayout.setCollapseListener(new ExpandCollapseListener.CollapseListener<GroupCinemaModel>() {
            @Override
            public void onCollapsed(int parentIndex, GroupCinemaModel parent, View view) {
                view.findViewById(R.id.icon_arrow).setBackgroundResource(R.drawable.arrow_right);
            }
        });
        return view;
    }

    public List<Section<GroupCinemaModel, CinemaModel>> setSection(List<GroupCinemaModel> models) {
        List<Section<GroupCinemaModel, CinemaModel>> result = new ArrayList<>();

        for (int i = 0; i < models.size(); i++) {
            Section<GroupCinemaModel, CinemaModel> section = new Section<>();

            section.parent = models.get(i);
            section.children = models.get(i).getCinemas();

            result.add(section);
        }

        return result;
    }

}
