package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.v7.widget.CardView;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.activities.FilmInfomationActivity;
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.services.BaseService;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

public class FilmComingSoonAdapter extends RecyclerView.Adapter<FilmComingSoonAdapter.FilmCommingSoonHolder> {

    private List<FilmModel> films;
    private Context context;

    public FilmComingSoonAdapter(List<FilmModel> films) {
        this.films = films;
    }

    @NonNull
    @Override
    public FilmComingSoonAdapter.FilmCommingSoonHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.card_comming_soon, parent, false);

        return new FilmCommingSoonHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull FilmComingSoonAdapter.FilmCommingSoonHolder holder, final int position) {
        Glide.with(context).load(BaseService.BASE_URL + films.get(position).getAdditionPicture())
                .into(holder.imgCommingSoon);

        DateFormat inputFormat = new SimpleDateFormat("yyyy-MM-dd");
        DateFormat outputFormat = new SimpleDateFormat("dd/MM ");
        Date date = null;
        try {
            date = inputFormat.parse(films.get(position).getDateRelease());
        } catch (ParseException e) {
            e.printStackTrace();
        }
        String outputDateStr = outputFormat.format(date);

        holder.tvDate.setText(outputDateStr);

        holder.cardView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (onItemClickedListener != null) {
                    onItemClickedListener.onItemClick(films.get(position).getFilmId());
                }
            }
        });

    }

    @Override
    public int getItemCount() {
        return films.size();
    }

    public class FilmCommingSoonHolder extends RecyclerView.ViewHolder {

        ImageView imgCommingSoon;
        TextView tvDate;
        CardView cardView;


        public FilmCommingSoonHolder(View itemView) {
            super(itemView);

            imgCommingSoon = (ImageView) itemView.findViewById(R.id.img_film_coming_soon);
            tvDate = (TextView) itemView.findViewById(R.id.tv_date_coming);
            cardView = (CardView) itemView.findViewById(R.id.cv_film_coming_soon_item);
        }
    }

    public interface OnItemClickedListener {
        void onItemClick(int filmId);
    }

    private FilmComingSoonAdapter.OnItemClickedListener onItemClickedListener;

    public void setOnItemClickedListener(FilmComingSoonAdapter.OnItemClickedListener onItemClickedListener) {
        this.onItemClickedListener = onItemClickedListener;
    }
}
