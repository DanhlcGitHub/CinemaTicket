package com.example.luulac.cinemaapplication.fragments.dashboards;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.CardView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.activities.FilmActivity;
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.data.models.HomeModel;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.FilmService;
import com.example.luulac.cinemaapplication.services.HomeService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


public class HomeFragment extends Fragment {

    private static final String TAG = "HomeFragment";

    private Context context;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        final View view = inflater.inflate(R.layout.fragment_home, null);

        context = view.getContext();

        HomeService ideaService = ServiceBuilder.buildService(HomeService.class);
        Call<HomeModel> request = ideaService.getDataForHomeScreen();

        request.enqueue(new Callback<HomeModel>() {
            @Override
            public void onResponse(Call<HomeModel> request, Response<HomeModel> response) {
                HomeModel data = response.body();
                List<FilmModel> films = data.getFilms();
                CardView cardViewLager = null;
                CardView cardShowing = view.findViewById(R.id.card_into_showing);
                LinearLayout linearLayout = null;


                FilmModel film = null;

                for (int i = 0; i < 3; i++) {
                    switch (i) {
                        case 0:
                            cardViewLager = (CardView) view.findViewById(R.id.card_1);
                            break;
                        case 1:
                            cardViewLager = (CardView) view.findViewById(R.id.card_2);
                            break;
                        case 2:
                            cardViewLager = (CardView) view.findViewById(R.id.card_3);
                            break;
                    }

                    film = films.get(i);

                    ImageView imgFilm = (ImageView) cardViewLager.findViewById(R.id.img_film_card_lager);
                    Glide.with(context).load(BaseService.BASE_URL + film.getPosterPicture()).into(imgFilm);

                    TextView filmTitle = (TextView) cardViewLager.findViewById(R.id.tv_film_card_lager_title);
                    filmTitle.setText(film.getName());

                    TextView filmStatus = (TextView) cardViewLager.findViewById(R.id.tv_film_card_lager_film_status);
                    filmStatus.setText("Đang chiếu");

                    TextView filmImdb = (TextView) cardViewLager.findViewById(R.id.tv_card_langer_imdb);
                    filmImdb.setText(film.getImdb() + "");

                    TextView filmBookingTicket = (TextView) cardViewLager.findViewById(R.id.tv_film_card_lager_booking_ticket);

                    final int filmId = film.getFilmId();
                    final String filmName = film.getName();

                    cardViewLager.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Intent intent = new Intent(context, FilmActivity.class);
                            intent.putExtra("filmId", filmId);
                            intent.putExtra("filmName", filmName);
                            startActivity(intent);
                        }
                    });
                }

                for (int i = 3; i < 6; i++) {
                    switch (i) {
                        case 3:
                            linearLayout = cardShowing.findViewById(R.id.card_show_1);
                            break;
                        case 4:
                            linearLayout = cardShowing.findViewById(R.id.card_show_2);
                            break;
                        case 5:
                            linearLayout = cardShowing.findViewById(R.id.card_show_3);
                            break;
                    }

                    film = films.get(i);

                    ImageView imgFilm = (ImageView) linearLayout.findViewById(R.id.img_show);
                    Glide.with(context).load(BaseService.BASE_URL + film.getPosterPicture()).into(imgFilm);

                    TextView filmTitle = (TextView) linearLayout.findViewById(R.id.tv_show_movie_title);
                    filmTitle.setText(film.getName());

                    TextView filmShowTime = (TextView) linearLayout.findViewById(R.id.tv_show_time);
                    filmShowTime.setText(film.getFilmLength() + "p - IMDb ");

                    TextView filmImdb = (TextView) linearLayout.findViewById(R.id.tv_show_imdb);
                    filmImdb.setText(film.getImdb() + "");

                    TextView filmBookingTicket = (TextView) linearLayout.findViewById(R.id.tv_film_card_lager_booking_ticket);
                }
            }

            @Override
            public void onFailure(Call<HomeModel> request, Throwable t) {
                Toast.makeText(context, "Failed to retrieve item.", Toast.LENGTH_SHORT).show();
            }
        });
        return view;
    }
}
