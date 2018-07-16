package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.SeatModel;

import java.util.ArrayList;
import java.util.List;

public class ChoiceSeatAdapter extends RecyclerView.Adapter<ChoiceSeatAdapter.ChoiceSeatHolder> {
    private Context context;
    private List<SeatModel> seatModels;
    private List<Character> resultAbc;
    private int row;

    public ChoiceSeatAdapter(List<SeatModel> seatModels, List<Character> resultAbc, int row) {
        this.seatModels = seatModels;
        this.resultAbc = resultAbc;
        this.row = row;
    }

    @NonNull
    @Override
    public ChoiceSeatHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.layout_seat_item, null);

        return new ChoiceSeatHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull final ChoiceSeatHolder holder, final int position) {
        if (seatModels.get(position).getSeatId() == 0) {
            holder.seatItem.setBackgroundResource(R.drawable.text_choice_seat_null);
        } else {
            holder.seatItem.setBackgroundResource(R.drawable.text_view_seat);
            holder.seatItem.setText(resultAbc.get(seatModels.get(position).getPy()).toString() + ((position % row) + 1));
            String ticketStatus = seatModels.get(position).getTicketStatus();
            switch (ticketStatus) {
                case "buying":
                    holder.seatItem.setBackgroundResource(R.drawable.text_choice_seat_booked);
                    holder.seatItem.setText(resultAbc.get(seatModels.get(position).getPy()).toString() + ((position % row) + 1));
                    break;
                case "buyed":
                    holder.seatItem.setBackgroundResource(R.drawable.text_choice_seat_booked);
                    holder.seatItem.setText(resultAbc.get(seatModels.get(position).getPy()).toString() + ((position % row) + 1));
                    break;
                case "resell":
                    holder.seatItem.setBackgroundResource(R.drawable.text_view_seat_resell);
                    holder.seatItem.setText(resultAbc.get(seatModels.get(position).getPy()).toString() + ((position % row) + 1));
                    break;
            }
        }
    }

    @Override
    public int getItemCount() {
        return seatModels.size();
    }

    public class ChoiceSeatHolder extends RecyclerView.ViewHolder {

        TextView seatItem;

        public ChoiceSeatHolder(View itemView) {
            super(itemView);

            seatItem = (TextView) itemView.findViewById(R.id.tv_seat_item);
        }
    }

}
