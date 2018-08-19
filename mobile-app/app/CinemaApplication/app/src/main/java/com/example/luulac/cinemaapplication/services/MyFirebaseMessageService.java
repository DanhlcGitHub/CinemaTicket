package com.example.luulac.cinemaapplication.services;

import android.app.Notification;
import android.app.NotificationManager;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.drawable.Drawable;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Build;
import android.os.Handler;
import android.os.Looper;
import android.support.annotation.RequiresApi;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.notifications.MyNotificationModel;
import com.example.luulac.cinemaapplication.helpers.NotificationHelper;
import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;
import com.squareup.picasso.Picasso;
import com.squareup.picasso.Target;

public class MyFirebaseMessageService extends FirebaseMessagingService {

    private String TAG = "MessageService";
    Target target = new Target() {
        @Override
        public void onBitmapLoaded(Bitmap bitmap, Picasso.LoadedFrom from) {
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                showNotificationImageLevel26(bitmap);

            } else {
                showNotificationImage(bitmap);

            }
        }

        @Override
        public void onBitmapFailed(Drawable errorDrawable) {

        }

        @Override
        public void onPrepareLoad(Drawable placeHolderDrawable) {

        }
    };


    @RequiresApi(api = Build.VERSION_CODES.O)
    private void showNotificationImageLevel26(Bitmap bitmap) {
        NotificationHelper helper = new NotificationHelper(getBaseContext());

        Notification.Builder builder = helper.getChannel(MyNotificationModel.title, MyNotificationModel.message, bitmap);
        helper.getManager().notify(0, builder.build());
    }


    @Override
    public void onMessageReceived(RemoteMessage remoteMessage) {

        // TODO(developer): Handle FCM messages here.
        // Not getting messages here? See why this may be: https://goo.gl/39bRNJ
        Log.d(TAG, "From: " + remoteMessage.getFrom());


        // Check if message contains a data payload.
        if (remoteMessage.getData().size() > 0) {
            Log.d(TAG, "Message data payload: " + remoteMessage.getData());

            getImage(remoteMessage);

        }

        // Check if message contains a notification payload.
        if (remoteMessage.getNotification() != null) {
            Log.d(TAG, "Message Notification Body: " + remoteMessage.getNotification().getBody());
        }

    }

    private void showNotificationImage(Bitmap bitmap) {

        SharedPreferences sharedPreferences = getSharedPreferences("loginInformation", Context.MODE_PRIVATE);

        String accountId = sharedPreferences.getString("username", "");
        if (MyNotificationModel.accountId.equalsIgnoreCase(accountId)) {
            NotificationCompat.BigPictureStyle style = new NotificationCompat.BigPictureStyle();
            style.setSummaryText(MyNotificationModel.message);
            style.bigPicture(bitmap);

            Uri defaultSound = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);

            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(getApplicationContext())
                    .setSmallIcon(R.mipmap.ic_launcher)
                    .setContentTitle(MyNotificationModel.title)
                    .setAutoCancel(true)
                    .setSound(defaultSound)
                    .setStyle(style);
            NotificationManager manager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
            manager.notify(0, notificationBuilder.build());

        }
    }

    private void showNotificationImage(RemoteMessage remoteMessage) {
    }

    private void getImage(final RemoteMessage remoteMessage) {
        MyNotificationModel.message = remoteMessage.getNotification().getBody();
        MyNotificationModel.title = remoteMessage.getNotification().getTitle();
        MyNotificationModel.accountId = remoteMessage.getData().get("accountId");

        if (remoteMessage.getData() != null) {

            Handler handler = new Handler(Looper.getMainLooper());

            handler.post(new Runnable() {
                @Override
                public void run() {
                    Picasso.with(getApplicationContext()).load(remoteMessage.getData().get("image")).into(target);
                }
            });
        }


    }
}
