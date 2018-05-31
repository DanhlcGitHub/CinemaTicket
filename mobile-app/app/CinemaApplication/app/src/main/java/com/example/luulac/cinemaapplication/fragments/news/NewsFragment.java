package com.example.luulac.cinemaapplication.fragments.news;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.fragments.dashboards.CommingSoonFragment;
import com.example.luulac.cinemaapplication.fragments.dashboards.HomeFragment;
import com.example.luulac.cinemaapplication.fragments.dashboards.ShowingFragment;

import java.util.ArrayList;
import java.util.List;

public class NewsFragment extends Fragment {
    private static List<Fragment> fragments;
    private static List<String> titles;
    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_news, null);
        return view;
    }
    public static List<Fragment> getFragments() {
        fragments = new ArrayList<>();
        fragments.add(new FilmNewsFragment());
        fragments.add(new PromotionFragment());

        return fragments;
    }

    public static List<String> getTitles() {
        titles = new ArrayList<>();
        titles.add("Điện Ảnh 24h");
        titles.add("Khuyến Mãi");

        return titles;
    }
}

