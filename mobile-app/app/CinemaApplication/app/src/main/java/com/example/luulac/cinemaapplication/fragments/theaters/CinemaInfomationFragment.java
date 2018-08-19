package com.example.luulac.cinemaapplication.fragments.theaters;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.CinemaModel;
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.CinemaService;
import com.example.luulac.cinemaapplication.services.FilmService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CinemaInfomationFragment extends Fragment{
        private int cinemaId;
        private CinemaModel data;
        private ImageView imgCinema;
        private TextView tvCinemaName, tvCinemaAddress, tvPhoneNumber;

        @Override
        public void onCreate(@Nullable Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
        }

        @Nullable
        @Override
        public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
            View view = inflater.inflate(R.layout.fragment_cinema_infomation, null);

            //The value of movieId is derived from FilmFragment
            cinemaId = this.getArguments().getInt("cinemaId");

            //img_film_information
            imgCinema = view.findViewById(R.id.img_cinema_information);

            //tv_film_name_information
            tvCinemaName = view.findViewById(R.id.tv_cinema_name_information);

            //tv_film_sub_info
            tvCinemaAddress = view.findViewById(R.id.tv_cinema_address);

            //tv_film_content
            tvPhoneNumber = view.findViewById(R.id.tv_cinema_phone_number);

            CinemaService cinemaService = ServiceBuilder.buildService(CinemaService.class);
            Call<CinemaModel> request = cinemaService.getCinemaById(cinemaId);
            request.enqueue(new Callback<CinemaModel>() {
                @Override
                public void onResponse(Call<CinemaModel> request, Response<CinemaModel> response) {
                    data = response.body();

                    Glide.with(getContext())
                            .load( data.getProfilePicture())
                            .into(imgCinema);

                    tvCinemaName.setText(data.getCinemaName());


                    tvCinemaAddress.setText(data.getCinemaAddress());

                    tvPhoneNumber.setText(data.getPhone());
                }

                @Override
                public void onFailure(Call<CinemaModel> request, Throwable t) {
                    Toast.makeText(getContext(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                }
            });
            return view;
        }
}
