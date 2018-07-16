package com.example.luulac.cinemaapplication.fragments.theaters;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.fragments.films.ScheduleShowFragment;

import java.util.ArrayList;
import java.util.List;

public class CinemaFragment extends Fragment{
    private static List<Fragment> fragments;
    private static List<String> titles;
    private static int cinemaId;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        cinemaId = this.getArguments().getInt("cinemaId");

    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_cinema_about, null);

        //The value of movieId is derived from FilmActivity

        return view;
    }

    public static List<Fragment> getFragments(int cinemaId) {
        fragments = new ArrayList<>();
        Bundle bundle = new Bundle();
        //Pass values of filmId to fragment (ScheduleShowFragment and FilmInfomationFragment)
        bundle.putInt("cinemaId", cinemaId);

        ScheduleCinemaFragment scheduleCinemaFragment = new ScheduleCinemaFragment();
        scheduleCinemaFragment.setArguments(bundle);

        CinemaInfomationFragment cinemaInfomationFragment = new CinemaInfomationFragment();
        cinemaInfomationFragment.setArguments(bundle);

        fragments.add(scheduleCinemaFragment);
        fragments.add(cinemaInfomationFragment);

        return fragments;
    }

    public static List<String> getTitles() {
        titles = new ArrayList<>();
        titles.add("Lịch Chiếu");
        titles.add("Thông Tin");

        return titles;
    }
}
