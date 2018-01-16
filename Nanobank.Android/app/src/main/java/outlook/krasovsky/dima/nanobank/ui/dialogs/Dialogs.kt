package outlook.krasovsky.dima.nanobank.ui.dialogs

import android.content.Context
import android.view.View
import org.jetbrains.anko.alert
import org.jetbrains.anko.layoutInflater
import org.jetbrains.anko.yesButton
import outlook.krasovsky.dima.nanobank.R

class Dialogs {
    companion object {
        fun startAlertInfo(context: Context, isShow: Boolean) {
            context.alert({
                val view = context.layoutInflater.inflate(R.layout.layout_help, null)
                view.findViewById<View>(R.id.root_wait_info).visibility = if (!isShow) View.GONE else View.VISIBLE
                customView = view
                yesButton { it.dismiss() }
            }).show()
        }
    }
}