package com.example.luulac.cinemaapplication.fragments.users;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.activities.OrderDetailActivity;
import com.example.luulac.cinemaapplication.adapters.PurchasedOrderAdapter;
import com.example.luulac.cinemaapplication.adapters.RecyclerItemClickListener;
import com.example.luulac.cinemaapplication.data.models.AccountPurchasedModel;
import com.example.luulac.cinemaapplication.services.AccountService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PurchaseOrderFragment extends Fragment {
    private List<AccountPurchasedModel> data;
    private Context context;
    private TextView tvPurchasedOffline, tvPurchasedNoOrder;
    private RecyclerView recyclerView;
    private View view;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        context = container.getContext();
        view = inflater.inflate(R.layout.fragment_purchase_order, null);

        renderView();
        return view;
    }

    public void renderView(){

        recyclerView = view.findViewById(R.id.rcl_purchase_order);
        tvPurchasedOffline = view.findViewById(R.id.tv_purchased_offline);
        tvPurchasedNoOrder = view.findViewById(R.id.tv_purchased_no_order);

        AccountService filmService = ServiceBuilder.buildService(AccountService.class);

        SharedPreferences sharedPreferences = getActivity().getSharedPreferences("loginInformation", Context.MODE_PRIVATE);

        String accountId = sharedPreferences.getString("username", "");

        if(accountId == ""){
            tvPurchasedOffline.setText("Bạn đang offline. Vui lòng đăng nhập!");
        }else{
            tvPurchasedOffline.setText("");
            Call<List<AccountPurchasedModel>> request = filmService.getAllOrderByAccountId(accountId);

            request.enqueue(new Callback<List<AccountPurchasedModel>>() {
                @Override
                public void onResponse(Call<List<AccountPurchasedModel>> request, Response<List<AccountPurchasedModel>> response) {
                    data = response.body();
                    if(data.size() != 0){
                        tvPurchasedNoOrder.setText("");
                        PurchasedOrderAdapter adapter = new PurchasedOrderAdapter(data);

                        LinearLayoutManager layoutManager = new LinearLayoutManager(getContext());
                        layoutManager.setOrientation(LinearLayoutManager.VERTICAL);

                        recyclerView.setLayoutManager(layoutManager);
                        recyclerView.setAdapter(adapter);


                        recyclerView.addOnItemTouchListener(
                                new RecyclerItemClickListener(context, recyclerView, new RecyclerItemClickListener.OnItemClickListener() {
                                    @Override
                                    public void onItemClick(View view, int position) {
                                        Intent intent = new Intent(context, OrderDetailActivity.class);
                                        intent.putExtra("accountPurchasedModel", data.get(position));
                                        startActivity(intent);
                                    }
                                    @Override
                                    public void onLongItemClick(View view, int position) {

                                    }
                                }));

                    }else{
                        tvPurchasedNoOrder.setText("Bạn chưa có giao dịch nào. Đặt vé ngay nhé");
                    }
                }

                @Override
                public void onFailure(Call<List<AccountPurchasedModel>> request, Throwable t) {
                    Toast.makeText(context, "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();;
                }
            });
        }
    }
}
