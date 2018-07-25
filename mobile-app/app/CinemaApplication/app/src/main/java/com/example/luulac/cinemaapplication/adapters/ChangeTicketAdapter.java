package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeChildModel;
import com.example.luulac.cinemaapplication.data.models.showtimes.ShowTimeListModel;


import java.util.List;

public class ChangeTicketAdapter extends RecyclerView.Adapter<ChangeTicketAdapter.ChangeTicketHolder> {
    private Context context;
    private List<ShowTimeChildModel> data;

    public ChangeTicketAdapter(List<ShowTimeChildModel> data) {
        this.data = data;
    }

    @NonNull
    @Override
    public ChangeTicketHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(parent.getContext());
        View view = inflater.inflate(R.layout.showtime_child, parent, false);
        return new ChangeTicketHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ChangeTicketHolder holder, int position) {
        holder.tvStartTime.setText(data.get(position).getTimeStart());
        holder.tvEndTime.setText(data.get(position).getTimeEnd());
        holder.tvDigType.setText(data.get(position).getType());
        holder.tvPrice.setText(data.get(position).getPrice());
        final int tmpPosition = position;
        holder.linearLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (onItemClickedListener != null) {
                    onItemClickedListener.onItemClick(tmpPosition);
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return data.size();
    }

    public class ChangeTicketHolder extends RecyclerView.ViewHolder{

        private TextView tvStartTime, tvEndTime, tvDigType, tvPrice;
        private LinearLayout linearLayout;


        public ChangeTicketHolder(View itemView) {
            super(itemView);

            tvStartTime = itemView.findViewById(R.id.tv_showtime_child_time_start);
            tvEndTime = itemView.findViewById(R.id.tv_showtime_child_time_end);
            tvDigType = itemView.findViewById(R.id.tv_showtime_child_type);
            tvPrice = itemView.findViewById(R.id.tv_showtime_child_price);
            linearLayout = itemView.findViewById(R.id.liner_order_ticket_two_button);

        }
    }

    public interface OnItemClickedListener {
        void onItemClick(int postion);
    }

    private ChangeTicketAdapter.OnItemClickedListener onItemClickedListener;

    public void setOnItemClickedListener(ChangeTicketAdapter.OnItemClickedListener onItemClickedListener) {
        this.onItemClickedListener = onItemClickedListener;
    }
}
