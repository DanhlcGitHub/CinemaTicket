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
import com.example.luulac.cinemaapplication.data.models.Movie;

import java.util.List;

public class MovieCommingSoonAdapter extends RecyclerView.Adapter<MovieCommingSoonAdapter.MovieCommingSoonHolder> {

    private List<Movie> movies;
    private Context context;

    public MovieCommingSoonAdapter(List<Movie> movies) {
        this.movies = movies;
    }

    @NonNull
    @Override
    public MovieCommingSoonAdapter.MovieCommingSoonHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.card_comming_soon, parent, false);

        return new MovieCommingSoonHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull MovieCommingSoonAdapter.MovieCommingSoonHolder holder, final int position) {
        Glide.with(context).load(movies.get(position).getImage())
                .thumbnail(0.5f)
                .into(holder.imgCommingSoon);
        holder.tvDate.setText("01/06");

        holder.cardView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(onItemClickListener != null){
                    onItemClickListener.onItemClick(movies.get(position).getTitle());
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return movies.size();
    }

    public class MovieCommingSoonHolder extends RecyclerView.ViewHolder {

        ImageView imgCommingSoon;
        TextView tvDate;
        CardView cardView;


        public MovieCommingSoonHolder(View itemView) {
            super(itemView);

            imgCommingSoon = (ImageView) itemView.findViewById(R.id.img_movie_comming_soon);
            tvDate =(TextView) itemView.findViewById(R.id.tv_date_comming);
            cardView = (CardView) itemView.findViewById(R.id.cv_movie_comming_soon_item);
        }
    }

    public interface OnItemClickListener {
        void onItemClick(String movieName);
    }

    private OnItemClickListener onItemClickListener;

    public void onItemClickListener(OnItemClickListener itemClickListener){
        this.onItemClickListener = itemClickListener;
    }
}
