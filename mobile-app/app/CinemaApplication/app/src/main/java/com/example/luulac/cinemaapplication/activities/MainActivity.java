package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.os.Bundle;
import android.os.PersistableBundle;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.TextUtils;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.WindowManager;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.TabLayoutMainAdapter;
import com.example.luulac.cinemaapplication.fragments.dashboards.DashboardFragment;
import com.example.luulac.cinemaapplication.fragments.news.NewsFragment;
import com.example.luulac.cinemaapplication.fragments.theaters.TheatersFragment;
import com.example.luulac.cinemaapplication.fragments.users.UsersFragment;
import com.example.luulac.cinemaapplication.navigations.BottomNavigationViewHelper;
import com.example.luulac.cinemaapplication.services.MyFirebaseIdService;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.messaging.FirebaseMessaging;

import java.util.List;

public class MainActivity extends AppCompatActivity {
    private Fragment fragment;
    public Toolbar toolbar;
    private TabLayout tabLayout;
    private ViewPager viewPager;
    private TabLayoutMainAdapter adapter;
    private List<Fragment> fragmentList;
    private List<String> titleList;
    private boolean isFirstLoad = true;
    private String TAG = "MainActivity";
    private String msg = "msg";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        FirebaseMessaging.getInstance().subscribeToTopic("schedule-coming-show-soon");

        Log.d("TokenID", FirebaseInstanceId.getInstance().getToken() + "");
        // Check whether we're recreating a previously destroyed instance
        if (savedInstanceState != null) {
            // Restore value of members from saved state
            isFirstLoad = savedInstanceState.getBoolean(RECREATE);
        }
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);

        setContentView(R.layout.activity_main);

        toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        tabLayout = (TabLayout) findViewById(R.id.tabs);

        viewPager = (ViewPager) findViewById(R.id.viewpager);

        BottomNavigationView navigation = (BottomNavigationView) findViewById(R.id.navigation);
        BottomNavigationViewHelper.removeShiftMode(navigation);
        navigation.setOnNavigationItemSelectedListener(mOnNavigationItemSelectedListener);

        adapter = new TabLayoutMainAdapter(getSupportFragmentManager());

        if (isFirstLoad) {
            toolbar.setTitle(R.string.title_dashboard);
            DashboardFragment dashboardFragment = new DashboardFragment(tabLayout);

            fragmentList = dashboardFragment.getFragments();
            titleList = dashboardFragment.getTitles();

            fragment = dashboardFragment;
        } else {
            toolbar.setTitle(R.string.title_user);

            UsersFragment usersFragment = new UsersFragment();

            fragmentList = usersFragment.getFragments();
            titleList = usersFragment.getTitles();
            fragment = usersFragment;
        }

        loadFragment(fragment);
        setTabLayoutMain(fragmentList, titleList);

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

                    DashboardFragment dashboardFragment = new DashboardFragment(tabLayout);

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

    private final String RECREATE = "recreate";

    public void loadFragment(Fragment fragment) {
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.frame_container, fragment);
        transaction.addToBackStack(null);

        transaction.detach(fragment);
        transaction.attach(fragment);
        transaction.commit();
    }

    @Override
    public void onSaveInstanceState(Bundle outState, PersistableBundle outPersistentState) {
        outPersistentState.putBoolean(RECREATE, isFirstLoad);
    }
}