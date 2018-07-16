package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.WindowManager;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.TabLayoutMainAdapter;
import com.example.luulac.cinemaapplication.fragments.films.FilmFragment;
import com.example.luulac.cinemaapplication.fragments.theaters.CinemaFragment;

import java.util.List;

public class CinemaActivity extends AppCompatActivity {

    private Fragment fragment;
    private TabLayout tabLayout;
    private ViewPager viewPager;
    private TabLayoutMainAdapter adapter;
    private List<Fragment> fragmentList;
    private List<String> titleList;
    private int cinemaId;
    private String cinemaName;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.activity_cinema);

        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar_cinema_activity);

        setSupportActionBar(toolbar);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        tabLayout = (TabLayout) findViewById(R.id.tabs_cinema_activity);
        viewPager = (ViewPager) findViewById(R.id.viewpager_cinema_activity);

        Intent intent = this.getIntent();

        cinemaName = intent.getStringExtra("cinemaName");
        cinemaId = intent.getIntExtra("cinemaId", 0);

        toolbar.setTitle(cinemaName);

        adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

        CinemaFragment cinemaFragment = new CinemaFragment();

        fragmentList = cinemaFragment.getFragments(cinemaId);
        titleList = cinemaFragment.getTitles();

        setTabLayoutMain(fragmentList, titleList);

        fragment = cinemaFragment;
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
        Bundle bundle = new Bundle();
        bundle.putInt("cinemaId", cinemaId);
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        fragment.setArguments(bundle);
        transaction.replace(R.id.frame_container_cinema_activity, fragment);
        transaction.addToBackStack(null);
        transaction.commit();
    }
}
