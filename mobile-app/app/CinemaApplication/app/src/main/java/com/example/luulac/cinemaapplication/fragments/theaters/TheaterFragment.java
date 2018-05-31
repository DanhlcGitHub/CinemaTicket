package com.example.luulac.cinemaapplication.fragments.theaters;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ExpandableListView;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.TheaterListAdapter;
import com.example.luulac.cinemaapplication.data.models.Cinemas;
import com.example.luulac.cinemaapplication.data.models.Theater;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class TheaterFragment extends Fragment {

    private ExpandableListView expandableListView;
    private TheaterListAdapter adapter;
    private List<Theater> theaters;
    private List<Cinemas> cinemas;
    private HashMap<Cinemas, List<Theater>> mapCinema;


    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_theater, null);

        expandableListView = (ExpandableListView) view.findViewById(R.id.expand_list_theater);

        mapCinema = setupCinema();
        cinemas = new ArrayList<Cinemas>(mapCinema.keySet());

        adapter = new TheaterListAdapter(getContext(),cinemas, mapCinema);

        expandableListView.setAdapter(adapter);

        return view;
    }

    public HashMap<Cinemas, List<Theater>> setupCinema(){

        cinemas = new ArrayList<>();
        HashMap<Cinemas, List<Theater>> result = new HashMap<Cinemas, List<Theater>>();

        theaters = new ArrayList<Theater>();
        theaters.add(new Theater("CNS - Hai Bà Trưng","135 Hai Bà Trưng, Bến Nghé, Q.1", "13.5 km"));
        theaters.add(new Theater("CNS - Quốc Thanh","271 Nguyễn Trãi, Q.1", "14.6 km"));
        result.put(new Cinemas("CineStar", ""), theaters);

        cinemas = new ArrayList<>();
        theaters = new ArrayList<>();
        theaters.add(new Theater("Lotte - Gò Vấp","L3-Lotte Mart, 242 Nguyễn Văn Lượng, Gò Vấp", "5.6 km"));
        theaters.add(new Theater("Lotte - Cộng Hòa","L4-Pico Plaza, 20 Cộng hòa, Tân Bình", "7.8 km"));
        theaters.add(new Theater("Lotte - Phú Thọ","L4-Lotte Mart Phú Thọ, Q.11", "13.1 km"));
        result.put(new Cinemas("Lotte", ""), theaters);

        return result;
    }

}
