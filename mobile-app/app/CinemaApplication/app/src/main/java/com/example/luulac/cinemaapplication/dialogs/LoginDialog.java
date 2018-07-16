package com.example.luulac.cinemaapplication.dialogs;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.UserAccountModel;
import com.example.luulac.cinemaapplication.services.AccountService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static java.security.AccessController.getContext;

public class LoginDialog {

    private Context context;
    private EditText edtUsername, edtPassword;
    private Button btnLogin;
    private Boolean isLoginDialog = false;

    public UserAccountModel user;

    public LoginDialog(Context context) {
        this.context = context;
    }

    public UserAccountModel showDialog(Activity activity) {
        final Dialog dialog = new Dialog(activity);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.setCancelable(false);
        dialog.setContentView(R.layout.layout_dialog_login);

        edtUsername = (EditText) dialog.findViewById(R.id.edt_dialog_username);
        edtPassword = (EditText) dialog.findViewById(R.id.edt_dialog_password);

        btnLogin = (Button) dialog.findViewById(R.id.btn_dialog_login);
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                AccountService accountService = ServiceBuilder.buildService(AccountService.class);
                Call<UserAccountModel> request = accountService.login(edtUsername.getText().toString(), edtPassword.getText().toString());


                request.enqueue(new Callback<UserAccountModel>() {
                    @Override
                    public void onResponse(Call<UserAccountModel> request, Response<UserAccountModel> response) {
                        user = response.body();
                        if (user.getUserId() != "") {
                            dialog.dismiss();
                        } else {
                            Toast.makeText(context, "Invalid username or passwrod!", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<UserAccountModel> request, Throwable t) {
                        Toast.makeText(context, "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                        ;
                    }
                });

            }
        });

        dialog.show();
        return user;

    }

}
