package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.widget.TextView;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.TabLayoutMainAdapter;
import com.example.luulac.cinemaapplication.fragments.films.FilmFragment;

import java.util.List;


public class FilmActivity extends AppCompatActivity {

    private Fragment fragment;
    private TabLayout tabLayout;
    private ViewPager viewPager;
    private TabLayoutMainAdapter adapter;
    private List<Fragment> fragmentList;
    private List<String> titleList;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_film);

        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar_film_activity);

        setSupportActionBar(toolbar);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        tabLayout = (TabLayout) findViewById(R.id.tabs_film_activity);

        viewPager = (ViewPager) findViewById(R.id.viewpager_film_activity);

        Intent intent = this.getIntent();

        toolbar.setTitle(intent.getStringExtra("filmName"));

        adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

        FilmFragment filmFragment = new FilmFragment();

        fragmentList = filmFragment.getFragments();
        titleList = filmFragment.getTitles();

        setTabLayoutMain(fragmentList, titleList);

        fragment = filmFragment;
        loadFragment(fragment);



    }

    @Override
    public boolean onSupportNavigateUp() {
        finish();
        return true;
    }

    public void setTabLayoutMain(List<Fragment> fragments, List<String> titles) {

        for (int i = 0; i < fragments.size(); i++) {
            Fragment f = fragments.get(i);
            String t = titles.get(i);
            adapter.addFragment(f, t);
        }

        viewPager.setAdapter(adapter);
        viewPager.addOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
        tabLayout.setupWithViewPager(viewPager);
        tabLayout.setTabGravity(TabLayout.GRAVITY_FILL);
        tabLayout.setTabMode(TabLayout.MODE_FIXED);
        tabLayout.setOnTabSelectedListener(new TabLayout.OnTabSelectedListener() {

            @Override
            public void onTabSelected(TabLayout.Tab tab) {
                viewPager.setCurrentItem(tab.getPosition());
            }

            @Override
            public void onTabUnselected(TabLayout.Tab tab) {
            }

            @Override
            public void onTabReselected(TabLayout.Tab tab) {
            }
        });
    }


    private void loadFragment(Fragment fragment) {
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.frame_container_film_activity, fragment);
        transaction.addToBackStack(null);
        transaction.commit();
    }
}
