package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
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
import com.example.luulac.cinemaapplication.data.models.FilmModel;
import com.example.luulac.cinemaapplication.services.BaseService;

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
        Glide.with(context).load(BaseService.BASE_URL + films.get(position).getPosterPicture())
                .thumbnail(0.5f)
                .into(holder.imgCommingSoon);
        holder.tvDate.setText("01/06");

        holder.cardView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (onItemClickListener != null) {
                    onItemClickListener.onItemClick(films.get(position).getName());
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

    public interface OnItemClickListener {
        void onItemClick(String filmName);
    }

    private OnItemClickListener onItemClickListener;

    public void setOnItemClickListener(OnItemClickListener itemClickListener) {
        this.onItemClickListener = itemClickListener;
    }
}
