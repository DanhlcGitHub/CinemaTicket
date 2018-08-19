package com.example.luulac.cinemaapplication.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.text.InputFilter;
import android.text.Spanned;
import android.util.Patterns;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.UserAccountModel;
import com.example.luulac.cinemaapplication.data.models.validations.UserAccountValidationModel;
import com.example.luulac.cinemaapplication.services.AccountService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegisterAccountActivity extends AppCompatActivity {
    private EditText edtUserId, edtPassword, edtEmail, edtPhone;
    private Button btnRegister;
    private TextView tvError;
    private UserAccountModel user;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.layout_register_account);

        InputFilter filter = new InputFilter() {
            public CharSequence filter(CharSequence source, int start, int end,
                                       Spanned dest, int dstart, int dend) {
                for (int i = start; i < end; i++) {
                    if (Character.isWhitespace(source.charAt(i))) {
                        return "";
                    }
                }
                return null;
            }

        };
        edtUserId = findViewById(R.id.edt_register_account_userid);
        edtUserId.setFilters(new InputFilter[]{filter});

        edtPassword = findViewById(R.id.edt_register_account_password);
        edtPassword.setFilters(new InputFilter[]{filter});

        edtEmail = findViewById(R.id.edt_register_account_email);
        edtEmail.setFilters(new InputFilter[]{filter});

        edtPhone = findViewById(R.id.edt_register_account_phone);
        edtPhone.setFilters(new InputFilter[]{filter});

        btnRegister = findViewById(R.id.btnRegister);

        tvError = findViewById(R.id.tv_register_account_error);

        btnRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String userId = edtUserId.getText().toString();
                String password = edtPassword.getText().toString();
                String email = edtEmail.getText().toString();
                String phone = edtPhone.getText().toString();
                tvError.setText("");
                UserAccountValidationModel model = checkValidationForm(userId, password, email, phone);
                if (model.isValidation()) {
                    //xu ly them tai khoan

                    AccountService accountService = ServiceBuilder.buildService(AccountService.class);
                    Call<UserAccountModel> request = accountService.register(userId, password, email, phone);

                    request.enqueue(new Callback<UserAccountModel>() {
                        @Override
                        public void onResponse(Call<UserAccountModel> request, Response<UserAccountModel> response) {
                            user = response.body();

                            if (user.isExited()) {//user.isExited() == true la da ton tai roi
                                //neu ton tai roi thi thong bao loi
                                tvError.setText("Tài khoản đã tồn tại. Bạn vui lòng nhập một tài khoản mới nha");

                            } else {//user.isExited() == false la chua ton tai roi
                                //thong bao tao thanh cong va huy activity
                                Toast.makeText(RegisterAccountActivity.this, "Đăng ký thành công!", Toast.LENGTH_SHORT).show();

                                finish();

                            }
                        }

                        @Override
                        public void onFailure(Call<UserAccountModel> request, Throwable t) {
                            Toast.makeText(getApplication(), "Xin hãy kiểm tra lại kết nối mạng!", Toast.LENGTH_SHORT).show();
                        }
                    });
                } else {
                    //bao loi tren man hinh
                    tvError.setText(model.getError());
                }
            }
        });


    }

    public UserAccountValidationModel checkValidationForm(String userId, String password, String email, String phone) {

        UserAccountValidationModel model = new UserAccountValidationModel();
        model.setValidation(true);

        if (userId.length() < 6) {
            model.setValidation(false);
            model.setError("Tên đăng nhập phải lớn hơn 6 ký tự!");

            return model;
        }

        if (password.length() < 6) {
            model.setValidation(false);
            model.setError("Mật khẩu phải lớn hơn 8 ký tự!");

            return model;
        }

        if (!Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            model.setValidation(false);
            model.setError("Địa chỉ email chưa đúng!");

            return model;
        }

        if (!Patterns.PHONE.matcher(phone).matches()) {
            model.setValidation(false);
            model.setError("Số điện thoại chưa đúng!");

            return model;
        }

        return model;
    }
}
