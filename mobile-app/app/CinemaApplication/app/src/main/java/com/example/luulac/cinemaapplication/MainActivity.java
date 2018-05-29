package com.example.luulac.cinemaapplication;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.MenuItem;

import com.example.luulac.cinemaapplication.adapters.TabLayoutMainAdapter;
import com.example.luulac.cinemaapplication.fragments.dashboards.DashboardFragment;
import com.example.luulac.cinemaapplication.fragments.news.NewsFragment;
import com.example.luulac.cinemaapplication.fragments.theaters.TheatersFragment;
import com.example.luulac.cinemaapplication.fragments.users.UsersFragment;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {
    private Fragment fragment;
    private Toolbar toolbar;
    private TabLayout tabLayout;
    private ViewPager viewPager;
    private TabLayoutMainAdapter adapter;
    private List<Fragment> fragmentList;
    private List<String> titleList;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        tabLayout = (TabLayout) findViewById(R.id.tabs);

        viewPager = (ViewPager) findViewById(R.id.viewpager);

        BottomNavigationView navigation = (BottomNavigationView) findViewById(R.id.navigation);
        navigation.setOnNavigationItemSelectedListener(mOnNavigationItemSelectedListener);

        toolbar.setTitle(R.string.title_dashboard);

        adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

        DashboardFragment dashboardFragment = new DashboardFragment();
        fragmentList = new ArrayList<>();
        titleList = new ArrayList<>();
        fragmentList = dashboardFragment.getFragments();
        titleList = dashboardFragment.getTitles();

        setTabLayoutMain(fragmentList, titleList);

        fragment = dashboardFragment;
        loadFragment(fragment);
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

    private BottomNavigationView.OnNavigationItemSelectedListener mOnNavigationItemSelectedListener
            = new BottomNavigationView.OnNavigationItemSelectedListener() {

        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem item) {
            switch (item.getItemId()) {
                case R.id.navigation_shop:
                    toolbar.setTitle(R.string.title_dashboard);

                    adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

                    DashboardFragment dashboardFragment = new DashboardFragment();

                    fragmentList = dashboardFragment.getFragments();
                    titleList = dashboardFragment.getTitles();

                    setTabLayoutMain(fragmentList, titleList);

                    fragment = dashboardFragment;
                    loadFragment(fragment);

                    return true;

                case R.id.navigation_gifts:
                    toolbar.setTitle(R.string.title_theater);

                    adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

                    TheatersFragment theatersFragment = new TheatersFragment();

                    fragmentList = theatersFragment.getFragments();
                    titleList = theatersFragment.getTitles();

                    setTabLayoutMain(fragmentList, titleList);

                    fragment = theatersFragment;
                    loadFragment(fragment);

                    return true;

                case R.id.navigation_cart:
                    toolbar.setTitle(R.string.title_news);

                    adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

                    NewsFragment newsFragment = new NewsFragment();

                    fragmentList = newsFragment.getFragments();
                    titleList = newsFragment.getTitles();

                    setTabLayoutMain(fragmentList, titleList);

                    fragment = newsFragment;
                    loadFragment(fragment);

                    return true;

                case R.id.navigation_profile:
                    toolbar.setTitle(R.string.title_user);

                    adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

                    UsersFragment usersFragment = new UsersFragment();

                    fragmentList = usersFragment.getFragments();
                    titleList = usersFragment.getTitles();

                    setTabLayoutMain(fragmentList, titleList);

                    fragment = usersFragment;
                    loadFragment(fragment);

                    return true;
            }

            return false;
        }
    };


    private void loadFragment(Fragment fragment) {
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.frame_container, fragment);
        transaction.addToBackStack(null);
        transaction.commit();
    }
}