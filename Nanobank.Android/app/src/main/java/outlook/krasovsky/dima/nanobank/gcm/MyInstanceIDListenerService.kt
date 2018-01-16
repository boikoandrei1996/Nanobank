package outlook.krasovsky.dima.nanobank.gcm

import android.content.Context
import android.content.Intent
import android.util.Log
import com.google.firebase.iid.FirebaseInstanceId
import com.google.firebase.iid.FirebaseInstanceIdService
import outlook.krasovsky.dima.nanobank.R


class MyInstanceIDListenerService : FirebaseInstanceIdService() {

    companion object {
        val TOKEN_GCM = "TOKEN_GCM"
    }

    override fun onTokenRefresh() {
        val refreshedToken = FirebaseInstanceId.getInstance().token
        Log.e("MYLOG", "refreshedToken = ${refreshedToken}")
        saveToken(refreshedToken!!)
    }

    private fun saveToken(token: String) {
        val editor = getSharedPreferences(getString(R.string.app_name), Context.MODE_PRIVATE).edit()
        editor.putString(TOKEN_GCM, token)
        editor.apply()
    }
}