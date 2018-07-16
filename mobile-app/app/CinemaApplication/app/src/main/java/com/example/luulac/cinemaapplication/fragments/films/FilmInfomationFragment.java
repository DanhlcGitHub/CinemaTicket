package com.example.luulac.cinemaapplication.fragments.films;

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
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.FilmService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FilmInfomationFragment extends Fragment {

    private int filmdId;
    private FilmModel data;
    private ImageView imgFilm;
    private TextView tvFilmName, tvFilmSubInfo, tvFilmContent;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_film_infomation, null);

        //The value of movieId is derived from FilmFragment
        filmdId = this.getArguments().getInt("filmId");

        FilmService filmService = ServiceBuilder.buildService(FilmService.class);
        Call<FilmModel> request = filmService.getFilm(filmdId);

        //img_film_information
        imgFilm = view.findViewById(R.id.img_film_information);

        //tv_film_name_information
        tvFilmName = view.findViewById(R.id.tv_film_name_information);

        //tv_film_sub_info
        tvFilmSubInfo = view.findViewById(R.id.tv_film_sub_info);

        //tv_film_content
        tvFilmContent = view.findViewById(R.id.tv_film_content);

        request.enqueue(new Callback<FilmModel>() {
            @Override
            public void onResponse(Call<FilmModel> request, Response<FilmModel> response) {
                data = response.body();

                Glide.with(getContext())
                        .load(BaseService.BASE_URL + data.getAdditionPicture())
                        .into(imgFilm);

                tvFilmName.setText(data.getName());

                DateFormat inputFormat = new SimpleDateFormat("yyyy-MM-dd");
                DateFormat outputFormat = new SimpleDateFormat("dd/MM ");
                Date date = null;
                try {
                    date = inputFormat.parse(data.getDateRelease());
                } catch (ParseException e) {
                    e.printStackTrace();
                }
                String outputDateStr = outputFormat.format(date);
                tvFilmSubInfo.setText(outputDateStr + " - " + data.getFilmLength() + " phút - IMDb " + data.getImdb());

                tvFilmContent.setText(data.getFilmContent());
            }

            @Override
            public void onFailure(Call<FilmModel> request, Throwable t) {
                Toast.makeText(getContext(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
            }
        });
        return view;
    }
}
