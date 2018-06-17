package com.example.luulac.cinemaapplication.fragments.dashboards;

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

public class DashboardFragment extends Fragment {

    private static List<Fragment> fragments;
    private static List<String> titles;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRetainInstance(true);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_dashboard, null);

        return view;
    }

    public static List<Fragment> getFragments() {
        fragments = new ArrayList<>();
        fragments.add(new HomeFragment());
        fragments.add(new ShowingFragment());
        fragments.add(new CommingSoonFragment());

        return fragments;
    }

    public static List<String> getTitles() {
        titles = new ArrayList<>();
        titles.add("Home");
        titles.add("Đang Chiếu");
        titles.add("Sắp Chiếu");

        return titles;
    }
}
