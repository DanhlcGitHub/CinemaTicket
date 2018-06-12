package com.example.luulac.cinemaapplication.fragments.dashboards;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.adapters.FilmComingSoonAdapter;
import com.example.luulac.cinemaapplication.adapters.FilmShowingAdapter;
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.services.FilmService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


public class CommingSoonFragment extends Fragment {

    private RecyclerView recyclerView;
    private List<FilmModel> films;
    private FilmComingSoonAdapter adapter;
    private Context context;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_comming_soon, null);
        context = view.getContext();

        recyclerView = (RecyclerView) view.findViewById(R.id.recycler_view_film_comming_soon);

        FilmService filmService = ServiceBuilder.buildService(FilmService.class);
        Call<List<FilmModel>> request = filmService.getFilms(false);


        request.enqueue(new Callback<List<FilmModel>>() {
            @Override
            public void onResponse(Call<List<FilmModel>> request, Response<List<FilmModel>> response) {
                films = response.body();
                adapter = new FilmComingSoonAdapter(films);

                GridLayoutManager layoutManager = new GridLayoutManager(getActivity(), 2);

                recyclerView.setLayoutManager(layoutManager);
                recyclerView.setAdapter(adapter);

                adapter.setOnItemClickListener(new FilmComingSoonAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(String filmName) {
                        Toast.makeText(getContext(), "Comming Soon Card View " + filmName +" clicked!", Toast.LENGTH_SHORT).show();
                    }
                });
            }

            @Override
            public void onFailure(Call<List<FilmModel>> request, Throwable t) {
                Toast.makeText(context, "Failed to retrieve item.", Toast.LENGTH_SHORT).show();;
            }
        });


        return view;
    }
}
