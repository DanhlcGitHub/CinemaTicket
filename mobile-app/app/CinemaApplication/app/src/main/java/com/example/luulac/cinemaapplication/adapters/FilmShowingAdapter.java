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

public class FilmShowingAdapter extends RecyclerView.Adapter<FilmShowingAdapter.FilmShowingHolder> {

    private List<FilmModel> films;
    private Context context;

    public FilmShowingAdapter(List<FilmModel> film) {
        this.films = film;
    }

    @NonNull
    @Override
    public FilmShowingHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(parent.getContext());
        View view = inflater.inflate(R.layout.film_showing_item, parent, false);
        return new FilmShowingHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull final FilmShowingHolder holder, final int position) {

        Glide.with(context)
                .load(BaseService.BASE_URL + films.get(position).getPosterPicture())
                .into(holder.imageView);
        holder.index.setText((position + 1) + "");
        holder.imdb.setText(films.get(position).getImdb() + "");

        holder.cardView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (onItemClickedListener != null) {
                    onItemClickedListener.onItemClick(films.get(position).getName(), films.get(position).getFilmId());
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return films.size();
    }

    public class FilmShowingHolder extends RecyclerView.ViewHolder {

        ImageView imageView;
        TextView index;
        TextView imdb;

        CardView cardView;

        public FilmShowingHolder(View itemView) {
            super(itemView);

            imageView = (ImageView) itemView.findViewById(R.id.img_film_showing_item);
            index = (TextView) itemView.findViewById(R.id.tv_film_showing_item_index);
            imdb = (TextView) itemView.findViewById(R.id.tv_film_showing_item_imdb);

            cardView = (CardView) itemView.findViewById(R.id.cv_film_showing_item);
        }
    }

    public interface OnItemClickedListener {
        void onItemClick(String filmName, int filmId);
    }

    private OnItemClickedListener onItemClickedListener;

    public void setOnItemClickedListener(OnItemClickedListener onItemClickedListener) {
        this.onItemClickedListener = onItemClickedListener;
    }
}
