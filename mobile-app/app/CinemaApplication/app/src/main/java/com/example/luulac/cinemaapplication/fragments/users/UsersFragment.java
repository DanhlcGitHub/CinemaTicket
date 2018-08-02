package com.example.luulac.cinemaapplication.fragments.users;

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


public class UsersFragment extends Fragment {
    private static List<Fragment> fragments;
    private static List<String> titles;
    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_users, null);
        return view;
    }
    public static List<Fragment> getFragments() {
        fragments = new ArrayList<>();
        fragments.add(new AccountFragment());
        fragments.add(new PurchaseOrderFragment());

        return fragments;
    }

    public static List<String> getTitles() {
        titles = new ArrayList<>();
        titles.add("Tài Khoản");
        titles.add("Vé Đã Mua");

        return titles;
    }
}
