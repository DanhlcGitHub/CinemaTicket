package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.AccountPurchasedModel;
import com.example.luulac.cinemaapplication.services.BaseService;

import java.util.List;

public class PurchasedOrderAdapter extends RecyclerView.Adapter<PurchasedOrderAdapter.PurchasedOrderHolder> {
    private Context context;

    private List<AccountPurchasedModel> data;

    public PurchasedOrderAdapter(List<AccountPurchasedModel> data) {
        this.data = data;
    }

    @NonNull
    @Override
    public PurchasedOrderHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.layout_user_purchased_order_item, null);

        return new PurchasedOrderHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull final PurchasedOrderHolder holder, final int position) {
        holder.tvCinemaName.setText(data.get(position).getCinemaName());
        holder.tvDate.setText(data.get(position).getDate());
        holder.tvFilmName.setText(data.get(position).getFilmName());
        //holder.tvGroupCinemaName.setText(data.get(position).getGroupCinemaName());
        holder.tvRoomName.setText(data.get(position).getRoomName());
        holder.tvShowTime.setText(data.get(position).getShowTime());

        Glide.with(context)
                .load(data.get(position).getFilmImage())
                .into(holder.imvFilmImage);

    }

    @Override
    public int getItemCount() {
        return data.size();
    }

    public class PurchasedOrderHolder extends RecyclerView.ViewHolder {

        private TextView tvFilmName, tvCinemaName, tvShowTime, tvDate, tvRoomName;
        private ImageView imvFilmImage;

        public PurchasedOrderHolder(View itemView) {
            super(itemView);
            tvFilmName = itemView.findViewById(R.id.tv_purchased_item_film_name);
            //tvGroupCinemaName = itemView.findViewById(R.id.tv_purchased_item_group_cinema_name);
            tvCinemaName = itemView.findViewById(R.id.tv_purchased_item_cinema_name);
            tvShowTime = itemView.findViewById(R.id.tv_purchased_item_show_time);
            tvDate = itemView.findViewById(R.id.tv_purchased_item_date);
            tvRoomName = itemView.findViewById(R.id.tv_purchased_item_room_name);
            imvFilmImage = itemView.findViewById(R.id.imv_purchased_item_film_image);
        }
    }
}
