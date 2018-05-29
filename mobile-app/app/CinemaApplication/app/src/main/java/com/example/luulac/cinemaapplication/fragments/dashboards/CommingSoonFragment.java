package com.example.luulac.cinemaapplication.fragments.dashboards;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.MovieCommingSoonAdapter;
import com.example.luulac.cinemaapplication.adapters.MovieShowingAdapter;
import com.example.luulac.cinemaapplication.data.models.Movie;

import java.util.ArrayList;
import java.util.List;


public class CommingSoonFragment extends Fragment {

    private RecyclerView recyclerView;
    private List<Movie> movies;
    private MovieCommingSoonAdapter adapter;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_comming_soon, null);

        recyclerView = (RecyclerView) view.findViewById(R.id.recycler_view_movie_comming_soon);

        movies = new ArrayList<>();

        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));
        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));
        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));
        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));
        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));
        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));
        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));
        movies.add(new Movie("Dead Pool 2", "http://api.androidhive.info/images/glide/medium/deadpool.jpg", "123"));


        adapter = new MovieCommingSoonAdapter(movies);

        GridLayoutManager layoutManager = new GridLayoutManager(getActivity(), 2);

        recyclerView.setLayoutManager(layoutManager);
        recyclerView.setAdapter(adapter);
        
        adapter.onItemClickListener(new MovieCommingSoonAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(String movieName) {
                Toast.makeText(getContext(), "Comming Soon Card View " + movieName +" clicked!", Toast.LENGTH_SHORT).show();
            }
        });

        return view;
    }
}
