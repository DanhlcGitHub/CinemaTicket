package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.example.luulac.cinemaapplication.R;

import java.util.ArrayList;
import java.util.List;

public class ChoiceSeatAbcAdapter extends RecyclerView.Adapter<ChoiceSeatAbcAdapter.ChoiceSeatAbcHolder> {
    private List<Character> items= new ArrayList<>();
    private Context context;

    public ChoiceSeatAbcAdapter(List<Character> items) {
        this.items = items;
    }

    @NonNull
    @Override
    public ChoiceSeatAbcHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.layout_choice_seat_abc, null);

        return new ChoiceSeatAbcHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ChoiceSeatAbcHolder holder, int position) {
        holder.textViewAbc.setText(items.get(position)+"");
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    public class ChoiceSeatAbcHolder  extends RecyclerView.ViewHolder{
        private TextView textViewAbc;

        public ChoiceSeatAbcHolder(View itemView) {
            super(itemView);

            textViewAbc = (TextView) itemView.findViewById(R.id.tv_choice_seat_abc_name);
        }
    }
}
