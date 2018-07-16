package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.UserAccountModel;
import com.example.luulac.cinemaapplication.services.AccountService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {

    private EditText edtUsername, edtPassword;
    private Button btnLogin;
    private int resultCode = RESULT_CANCELED;

    public UserAccountModel user;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.layout_dialog_login);

        edtUsername = (EditText) findViewById(R.id.edt_dialog_username);
        edtPassword = (EditText) findViewById(R.id.edt_dialog_password);

        btnLogin = (Button) findViewById(R.id.btn_dialog_login);
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

                            Intent resultIntent = new Intent();

                            resultIntent.putExtra("user", user);
                            resultCode = RESULT_OK;
                            setResult(resultCode, resultIntent);

                            finish();

                        } else {
                            Toast.makeText(getApplication(), "Invalid username or passwrod!", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<UserAccountModel> request, Throwable t) {
                        Toast.makeText(getApplication(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                    }
                });

            }
        });
    }
}
