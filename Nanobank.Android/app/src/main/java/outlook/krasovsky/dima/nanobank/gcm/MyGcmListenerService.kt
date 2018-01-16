package outlook.krasovsky.dima.nanobank.gcm

import android.app.PendingIntent
import android.content.Intent
import android.support.v4.app.NotificationCompat
import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.SignInActivity
import android.app.Notification
import android.support.v4.app.NotificationManagerCompat
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage
import org.jetbrains.anko.custom.async


class MyGcmListenerService : FirebaseMessagingService() {

    override fun onMessageReceived(message: RemoteMessage) {
        async {
            val data = message.data
            if (data != null) {
                sendNotification(title = data["title"], body = data["body"])
            }
        }
    }

    private fun sendNotification(title: String?, body: String?) {
        val intent = Intent(this, SignInActivity::class.java)
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP)
        val pendingIntent = PendingIntent.getActivity(this, 0, intent,
                PendingIntent.FLAG_ONE_SHOT)

        val notificationBuilder = NotificationCompat.Builder(this)
                .setSmallIcon(R.drawable.logo_app)
                .setContentTitle(title)
                .setContentText(body)
                .setAutoCancel(true)
                .setDefaults(Notification.DEFAULT_ALL)
                .setContentIntent(pendingIntent)

        val notificationManager = NotificationManagerCompat.from(this)

        notificationManager.notify(0, notificationBuilder.build())
    }
}