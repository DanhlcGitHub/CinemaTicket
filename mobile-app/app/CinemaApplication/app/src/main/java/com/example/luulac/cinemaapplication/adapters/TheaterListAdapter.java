package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.graphics.Typeface;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;
import android.widget.ExpandableListAdapter;
import android.widget.TextView;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.CinemaModel;
import com.example.luulac.cinemaapplication.data.models.Cinemas;
import com.example.luulac.cinemaapplication.data.models.Theater;

import java.util.HashMap;
import java.util.List;

public class TheaterListAdapter extends BaseExpandableListAdapter {

    private Context context;
    private List<CinemaModel> cinemas;
    private HashMap<CinemaModel, List<Theater>> mapCinema;

    public TheaterListAdapter(Context context, List<CinemaModel> cinemas, HashMap<CinemaModel, List<Theater>> expandableListDetail) {
        this.context = context;
        this.cinemas = cinemas;
        this.mapCinema = expandableListDetail;
    }

    @Override
    public Object getChild(int listPosition, int expandedListPosition) {
        return this.mapCinema.get(this.cinemas.get(listPosition))
                .get(expandedListPosition);
    }

    @Override
    public long getChildId(int listPosition, int expandedListPosition) {
        return expandedListPosition;
    }

    @Override
    public View getChildView(int listPosition, final int expandedListPosition, boolean isLastChild, View convertView, ViewGroup parent) {
        final Theater theater = (Theater) getChild(listPosition, expandedListPosition);
        if (convertView == null) {
            LayoutInflater layoutInflater = (LayoutInflater) this.context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView = layoutInflater.inflate(R.layout.theater_item, null);
        }
        TextView expandedListTextView = (TextView) convertView.findViewById(R.id.tv_theater_item_theater_name);
        expandedListTextView.setText(theater.getTheaterName());
        return convertView;
    }

    @Override
    public int getChildrenCount(int listPosition) {
        return this.mapCinema.get(this.cinemas.get(listPosition)).size();
    }

    @Override
    public Object getGroup(int listPosition) {
        return this.cinemas.get(listPosition);
    }

    @Override
    public int getGroupCount() {
        return this.cinemas.size();
    }

    @Override
    public long getGroupId(int listPosition) {
        return listPosition;
    }

    @Override
    public View getGroupView(int listPosition, boolean isExpanded, View convertView, ViewGroup parent) {
        CinemaModel cinema = (CinemaModel) getGroup(listPosition);
        if (convertView == null) {
            LayoutInflater layoutInflater = (LayoutInflater) this.context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView = layoutInflater.inflate(R.layout.theater_list, null);
        }
        TextView listTitleTextView = (TextView) convertView.findViewById(R.id.tv_theater_list_cinemas_name);
        listTitleTextView.setText(cinema.getCinemaName() +" (" + this.mapCinema.get(cinema).size() + ")");
        return convertView;
    }

    @Override
    public boolean hasStableIds() {
        return false;
    }

    @Override
    public boolean isChildSelectable(int listPosition, int expandedListPosition) {
        return true;
    }

}
