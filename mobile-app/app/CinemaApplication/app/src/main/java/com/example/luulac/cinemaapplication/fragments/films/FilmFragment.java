package com.example.luulac.cinemaapplication.fragments.films;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.luulac.cinemaapplication.R;

import java.util.ArrayList;
import java.util.List;

public class FilmFragment extends Fragment {

    private static List<Fragment> fragments;
    private static List<String> titles;
    private static int filmId;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        filmId = this.getArguments().getInt("filmId");

    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_film_about, null);

        return view;
    }

    public static List<Fragment> getFragments(int filmId) {
        fragments = new ArrayList<>();
        Bundle bundle = new Bundle();
        //Pass values of filmId to fragment (ScheduleShowFragment and FilmInfomationFragment)
        bundle.putInt("filmId", filmId);

        ScheduleShowFragment scheduleShowFragment = new ScheduleShowFragment();
        scheduleShowFragment.setArguments(bundle);

        FilmInfomationFragment filmInfomationFragment = new FilmInfomationFragment();
        filmInfomationFragment.setArguments(bundle);

        fragments.add(scheduleShowFragment);
        fragments.add(filmInfomationFragment);

        return fragments;
    }

    public static List<String> getTitles() {
        titles = new ArrayList<>();
        titles.add("Lịch Chiếu");
        titles.add("Thông Tin");

        return titles;
    }
}
