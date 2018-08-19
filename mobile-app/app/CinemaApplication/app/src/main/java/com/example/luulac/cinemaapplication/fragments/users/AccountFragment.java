package com.example.luulac.cinemaapplication.fragments.users;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.activities.LoginActivity;
import com.example.luulac.cinemaapplication.activities.MainActivity;
import com.example.luulac.cinemaapplication.data.models.UserAccountModel;
import com.example.luulac.cinemaapplication.dialogs.LoginDialog;
import com.example.luulac.cinemaapplication.services.AccountService;
import com.example.luulac.cinemaapplication.services.ServiceBuilder;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static android.app.Activity.RESULT_OK;

public class AccountFragment extends Fragment {

    private ImageView imvAccountUserAvatar;
    private TextView txtNote, txtUsername, txtEmail, txtPhone, txtLogout;
    private Boolean isLogin = false;
    private UserAccountModel user;


    public static final int REQUEST_CODE_LOGIN = 139;

    private LoginDialog loginDialog;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_account, null);
        loginDialog = new LoginDialog(getContext());

        txtNote = (TextView) view.findViewById(R.id.tv_account_note);
        txtUsername = (TextView) view.findViewById(R.id.tv_account_username);
        txtEmail = (TextView) view.findViewById(R.id.tv_account_email);
        txtPhone = (TextView) view.findViewById(R.id.tv_account_phone);
        txtLogout = (TextView) view.findViewById(R.id.tv_account_logout);

        SharedPreferences sharedPreferences = getActivity().getSharedPreferences("loginInformation", Context.MODE_PRIVATE);

        isLogin = sharedPreferences.getBoolean("isLogin", false);

        imvAccountUserAvatar = (ImageView) view.findViewById(R.id.imv_account_user_avatar);

        if (!isLogin) {
            imvAccountUserAvatar.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Intent intentLogin = new Intent(getContext(), LoginActivity.class);

                    startActivityForResult(intentLogin, REQUEST_CODE_LOGIN);
                }
            });
            txtLogout.setVisibility(TextView.INVISIBLE);

        } else {
            txtNote.setVisibility(TextView.INVISIBLE);

            txtUsername.setText(sharedPreferences.getString("username", ""));

            txtEmail.setText(sharedPreferences.getString("email", ""));

            txtPhone.setText(sharedPreferences.getString("phoneNumber", ""));

            txtLogout.setVisibility(TextView.VISIBLE);

            txtLogout.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    SharedPreferences sharedPreferences = getActivity().getSharedPreferences("loginInformation", Context.MODE_PRIVATE);
                    SharedPreferences.Editor editor = sharedPreferences.edit();
                    editor.remove("username");
                    editor.remove("email");
                    editor.remove("phoneNumber");
                    editor.remove("isLogin");
                    editor.commit();
                    renderView();

                    txtNote.setVisibility(TextView.VISIBLE);

                    txtUsername.setText(sharedPreferences.getString("username", ""));

                    txtEmail.setText(sharedPreferences.getString("email", ""));

                    txtPhone.setText(sharedPreferences.getString("phoneNumber", ""));

                    txtLogout.setVisibility(TextView.INVISIBLE);

                    imvAccountUserAvatar.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Intent intentLogin = new Intent(getContext(), LoginActivity.class);

                            startActivityForResult(intentLogin, REQUEST_CODE_LOGIN);
                        }
                    });

                }
            });

        }


        return view;
    }

    public void renderView() {
        final MainActivity activity = (MainActivity) this.getActivity();
        activity.recreate();



    }


    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REQUEST_CODE_LOGIN) {
            if (resultCode == RESULT_OK) {
                renderView();
                user = (UserAccountModel) data.getSerializableExtra("user");

                txtNote.setVisibility(TextView.INVISIBLE);

                txtUsername.setVisibility(TextView.VISIBLE);
                txtUsername.setText(user.getUserId());

                txtEmail.setVisibility(TextView.VISIBLE);
                txtEmail.setText(user.getEmail());

                txtPhone.setVisibility(TextView.VISIBLE);
                txtPhone.setText(user.getPhone());

                txtLogout.setVisibility(TextView.VISIBLE);

                txtLogout.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        SharedPreferences sharedPreferences = getActivity().getSharedPreferences("loginInformation", Context.MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferences.edit();
                        editor.remove("username");
                        editor.remove("email");
                        editor.remove("phoneNumber");
                        editor.remove("isLogin");
                        editor.commit();

                        txtNote.setVisibility(TextView.VISIBLE);

                        txtUsername.setText(sharedPreferences.getString("username", ""));

                        txtEmail.setText(sharedPreferences.getString("email", ""));

                        txtPhone.setText(sharedPreferences.getString("phoneNumber", ""));

                        txtLogout.setVisibility(TextView.INVISIBLE);

                        imvAccountUserAvatar.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View v) {
                                Intent intentLogin = new Intent(getContext(), LoginActivity.class);

                                startActivityForResult(intentLogin, REQUEST_CODE_LOGIN);
                            }
                        });
                    }
                });

                SharedPreferences sharedPreferences = getActivity().getSharedPreferences("loginInformation", Context.MODE_PRIVATE);

                SharedPreferences.Editor editor = sharedPreferences.edit();

                editor.putString("username", txtUsername.getText().toString());
                editor.putString("phoneNumber", txtPhone.getText().toString());
                editor.putString("email", txtEmail.getText().toString());
                editor.putBoolean("isLogin", true);

                editor.commit();
            }
        }
    }

}
