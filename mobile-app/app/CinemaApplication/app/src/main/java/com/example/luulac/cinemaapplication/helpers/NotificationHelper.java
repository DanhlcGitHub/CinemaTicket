package com.example.luulac.cinemaapplication.helpers;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Context;
import android.content.ContextWrapper;
import android.graphics.Bitmap;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Build;
import android.support.annotation.RequiresApi;
import android.support.v4.app.NotificationCompat;

import com.example.luulac.cinemaapplication.R;
import com.example.luulac.cinemaapplication.data.models.notifications.MyNotificationModel;

public class NotificationHelper extends ContextWrapper {
    private static final String CINEMA_ID = "com.example.luulac.cinemaapplication.Cinema";
    private static final String CINEMA_NAME = "Cinema";
    private NotificationManager manager;

    public NotificationHelper(Context base) {
        super(base);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            createChanel();
        }
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    private void createChanel() {
        NotificationChannel channel = new NotificationChannel(CINEMA_ID, CINEMA_NAME, NotificationManager.IMPORTANCE_DEFAULT);
        channel.enableVibration(true);
        channel.enableLights(true);
        channel.setLockscreenVisibility(Notification.VISIBILITY_PRIVATE);

        getManager().createNotificationChannel(channel);
    }

    public NotificationManager getManager() {
        if (manager == null) {
            manager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        }
        return manager;
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    public Notification.Builder getChannel(String title, String body, Bitmap bitmap) {

        Notification.Style style = new Notification.BigPictureStyle().bigPicture(bitmap);


        Uri defaultSound = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);

        return new Notification.Builder(getApplicationContext(), CINEMA_ID)
                .setSmallIcon(R.mipmap.ic_launcher)
                .setContentTitle(MyNotificationModel.title)
                .setAutoCancel(true)
                .setSound(defaultSound)
                .setStyle(style);
    }
}
