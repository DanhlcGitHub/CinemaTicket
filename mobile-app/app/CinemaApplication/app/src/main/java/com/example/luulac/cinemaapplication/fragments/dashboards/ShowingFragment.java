package com.example.luulac.cinemaapplication.fragments.dashboards;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.MovieShowingAdapter;
import com.example.luulac.cinemaapplication.data.models.Movie;

import java.util.ArrayList;
import java.util.List;

public class ShowingFragment extends Fragment {

    private RecyclerView recyclerView;
    private MovieShowingAdapter adapter;
    private List<Movie> movies;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_showing, null);

        recyclerView = (RecyclerView) view.findViewById(R.id.recycler_view_movie_showing);

        movies = new ArrayList<>();
        movies.add(new Movie("1", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "8.8"));
        movies.add(new Movie("1", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "8.8"));
        movies.add(new Movie("1", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "8.8"));
        movies.add(new Movie("1", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "8.8"));

        adapter = new MovieShowingAdapter(movies);

        LinearLayoutManager layoutManager = new LinearLayoutManager(getContext());
        layoutManager.setOrientation(LinearLayoutManager.VERTICAL);

        recyclerView.setLayoutManager(layoutManager);
        recyclerView.setAdapter(adapter);

        adapter.setOnItemClickedListener(new MovieShowingAdapter.OnItemClickedListener() {
            @Override
            public void onItemClick(String movieName) {
                Toast.makeText(getContext(), "Now Showing Card view " + movieName + " clicked.", Toast.LENGTH_SHORT).show();
            }
        });

        return view;
    }
}
