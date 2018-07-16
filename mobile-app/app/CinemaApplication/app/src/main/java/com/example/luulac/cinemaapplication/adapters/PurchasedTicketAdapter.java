package com.example.luulac.cinemaapplication.adapters;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.support.annotation.NonNull;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.activities.OrderDetailActivity;
import com.example.luulac.cinemaapplication.data.models.TicketModel;
import com.example.luulac.cinemaapplication.services.BaseService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;
import com.example.luulac.cinemaapplication.services.TicketService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PurchasedTicketAdapter extends RecyclerView.Adapter<PurchasedTicketAdapter.PurchasedTicketHolder> {
    private Context context;
    private List<TicketModel> data;
    private View view;
    private OrderDetailActivity orderDetailActivity;

    public PurchasedTicketAdapter(List<TicketModel> data, OrderDetailActivity orderDetailActivity) {
        this.data = data;
        this.orderDetailActivity = orderDetailActivity;
    }

    @NonNull
    @Override
    public PurchasedTicketHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);

        view = inflater.inflate(R.layout.layout_order_ticket_item, null);

        return new PurchasedTicketHolder(view);
    }

    @Override
    public void onBindViewHolder(final @NonNull PurchasedTicketHolder holder, final int position) {

        holder.tvTicketPrice.setText(data.get(position).getPrice() + "");
        holder.tvTicketSeatPosition.setText(data.get(position).getSeatPosition());
        holder.tvTicketStatus.setText(data.get(position).getTicketStatus());

        Glide.with(context).load(BaseService.BASE_URL + data.get(position).getQrCode()).into(holder.imgTicketQrCode);


        switch (data.get(position).getTicketStatus()) {
            case "buyed":
                holder.btnResellTicket.setText("Bán lại vé");
                holder.btnResellTicket.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        confirmResellTicket(data.get(position).getTicketId(), holder.btnResellTicket, holder.btnChangeTicket, position, holder.tvTicketStatus);

                    }
                });

                holder.btnChangeTicket.setText("Đổi vé");
                holder.btnChangeTicket.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Toast.makeText(context, "Click to button Change ticket", Toast.LENGTH_SHORT).show();
                    }
                });

                break;
            case "resell":

                holder.btnResellTicket.setText("Hủy bán lại vé");
                holder.btnResellTicket.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        confirmCancelResellTicket(data.get(position).getTicketId(), holder.btnResellTicket, holder.btnChangeTicket, position, holder.tvTicketStatus);

                    }
                });

                holder.btnChangeTicket.setText("Xác nhận bán");
                holder.btnChangeTicket.setBackgroundColor(Color.GREEN);
                holder.btnChangeTicket.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        confirmEmail(data.get(position).getTicketId());
                    }
                });

                break;
        }

    }

    @Override
    public int getItemCount() {
        return data.size();
    }

    public class PurchasedTicketHolder extends RecyclerView.ViewHolder {

        TextView tvTicketSeatPosition, tvTicketStatus, tvTicketPrice;
        ImageView imgTicketQrCode;
        Button btnChangeTicket, btnResellTicket;

        public PurchasedTicketHolder(View itemView) {
            super(itemView);

            tvTicketSeatPosition = itemView.findViewById(R.id.tv_ticket_seat_position);
            tvTicketStatus = itemView.findViewById(R.id.tv_ticket_status);
            tvTicketPrice = itemView.findViewById(R.id.tv_ticket_price);
            imgTicketQrCode = itemView.findViewById(R.id.img_ticket_qr_code);
            btnChangeTicket = itemView.findViewById(R.id.btn_purchased_ticket_change_ticket);
            btnResellTicket = itemView.findViewById(R.id.btn_purchased_ticket_resell_ticket);
        }
    }

    private TicketModel ticketModel;

    public void confirmResellTicket(final int ticketId, final Button btnResellTicket, final Button btnChangeTicket, final int position, final TextView tvStatus) {
        new AlertDialog.Builder(context)
                .setIcon(android.R.drawable.ic_dialog_alert)
                .setTitle("Xác nhận bán lại vé")
                .setMessage("Bạn muốn bán lại vé này?")
                .setPositiveButton("Đồng ý", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        //xy ly ban lai ve o day
                        TicketService ticketService = ServiceBuilder.buildService(TicketService.class);
                        ticketModel = new TicketModel();
                        Call<TicketModel> request = ticketService.resellTicket(ticketId);

                        //receiving and process data from the server
                        request.enqueue(new Callback<TicketModel>() {
                            @Override
                            public void onResponse(Call<TicketModel> request, Response<TicketModel> response) {
                                ticketModel = response.body();

                                orderDetailActivity.recreate();

                            }

                            @Override
                            public void onFailure(Call<TicketModel> request, Throwable t) {
                                Toast.makeText(context, "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                            }
                        });
                    }
                })
                .setNegativeButton("Hủy", null)
                .show();
    }


    public void confirmCancelResellTicket(final int ticketId, final Button btnResellTicket, final Button btnChangeTicket, final int position, final TextView tvStatus) {
        new AlertDialog.Builder(context)
                .setIcon(android.R.drawable.ic_dialog_alert)
                .setTitle("Xác nhận hủy bán lại vé")
                .setMessage("Bạn muốn hủy bán lại vé này?")
                .setPositiveButton("Đồng ý", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        //xy ly ban lai ve o day
                        TicketService ticketService = ServiceBuilder.buildService(TicketService.class);
                        ticketModel = new TicketModel();
                        Call<TicketModel> request = ticketService.cancelResellTicket(ticketId);

                        //receiving and process data from the server
                        request.enqueue(new Callback<TicketModel>() {
                            @Override
                            public void onResponse(Call<TicketModel> request, Response<TicketModel> response) {
                                ticketModel = response.body();

                                orderDetailActivity.recreate();
                            }

                            @Override
                            public void onFailure(Call<TicketModel> request, Throwable t) {
                                Toast.makeText(context, "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                            }
                        });
                    }
                })
                .setNegativeButton("Hủy", null)
                .show();
    }

    public void confirmEmail(final int ticketId) {
        final EditText txtEmail = new EditText(context);

// Set the default text to a link of the Queen
        txtEmail.setHint("you_email@gmail.com");

        new AlertDialog.Builder(context)
                .setTitle("Nhập email ")
                .setMessage("Email người mà bạn muốn bán lại vé")
                .setView(txtEmail)
                .setPositiveButton("Đồng ý", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int whichButton) {
                        String email = txtEmail.getText().toString();

                        TicketService ticketService = ServiceBuilder.buildService(TicketService.class);
                        ticketModel = new TicketModel();
                        Call<TicketModel> request = ticketService.confirmResellTicket(ticketId, email);

                        //receiving and process data from the server
                        request.enqueue(new Callback<TicketModel>() {
                            @Override
                            public void onResponse(Call<TicketModel> request, Response<TicketModel> response) {
                                ticketModel = response.body();

                                orderDetailActivity.recreate();
                            }

                            @Override
                            public void onFailure(Call<TicketModel> request, Throwable t) {
                                Toast.makeText(context, "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                            }
                        });
                    }
                })
                .setNegativeButton("Hủy", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int whichButton) {
                    }
                })
                .show();
    }
}
