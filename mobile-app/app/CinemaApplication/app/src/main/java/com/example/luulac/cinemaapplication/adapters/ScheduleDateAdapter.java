package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.showtimes.DateScheduleModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.FilmScheduleModel;

import java.util.List;

public class ScheduleDateAdapter extends RecyclerView.Adapter<ScheduleDateAdapter.ScheduleHolder> {

    private Context context;
    private List<FilmScheduleModel> filmScheduleModels;

    public List<FilmScheduleModel> getFilmScheduleModels() {
        return filmScheduleModels;
    }

    public void setFilmScheduleModels(List<FilmScheduleModel> filmScheduleModels) {
        this.filmScheduleModels = filmScheduleModels;
    }

    public ScheduleDateAdapter(Context context, List<FilmScheduleModel> filmScheduleModels) {
        this.context = context;
        this.filmScheduleModels = filmScheduleModels;
    }

    @NonNull
    @Override
    public ScheduleDateAdapter.ScheduleHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();

        LayoutInflater inflater = LayoutInflater.from(context);

        View view = inflater.inflate(R.layout.layout_schedule_date_item, parent, false);

        return new ScheduleHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ScheduleHolder holder, int position) {
        if(position != 0){
            holder.dateText.setWidth(90);
        }
        holder.dateText.setText(filmScheduleModels.get(position).getDateOfWeek());
        holder.dateNumber.setText(filmScheduleModels.get(position).getDay());

    }

    @Override
    public int getItemCount() {
        return filmScheduleModels.size();
    }

    public class ScheduleHolder extends RecyclerView.ViewHolder{

        TextView dateText;
        TextView dateNumber;

        public ScheduleHolder(View itemView) {
            super(itemView);
            dateText = (TextView) itemView.findViewById(R.id.tv_schedule_date_item_text);
            dateNumber = (TextView) itemView.findViewById(R.id.tv_schedule_date_item_number);
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
