package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.ProgressDialog
import android.content.Context
import android.content.Intent
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_info_user.*
import org.jetbrains.anko.alert
import org.jetbrains.anko.indeterminateProgressDialog
import org.jetbrains.anko.yesButton

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.InfoUserPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.InfoUserViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse

class InfoUserActivity : MvpAppCompatActivity(), InfoUserViewInterface {

    var dialogWaiting: ProgressDialog? = null

    @InjectPresenter
    lateinit var mPresenter: InfoUserPresenter

    companion object {
        val LOGIN_INFO = "LOGIN_INFO"

        fun getIntent(context: Context, login: String): Intent {
            val intent = Intent(context, InfoUserActivity::class.java)
            intent.putExtra(LOGIN_INFO, login)

            return intent
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_info_user)

        if (savedInstanceState == null) {
            mPresenter.getPrivateData(intent.getStringExtra(LOGIN_INFO))
        }
    }

    override fun setView(data: PrivateDataResponse) {
        dialogWaiting?.dismiss()
        userName.text = data.userName
        firstName.text = data.firstName
        lastName.text = data.lastName
        patronymic.text = data.patronymic
        email.text = data.email
        phoneNumber.text = data.phoneNumber
        value_positive_star.text = ": ${data.ratingPositive?.toString()?: "___"}"
        value_negative_star.text = ": ${data.ratingNegative?.toString()?: "___"}"
    }

    override fun error(message: String) {
        dialogWaiting?.dismiss()
        alert(message, "Ошибка") {
            yesButton { dialog -> dialog.dismiss() }
        }.show()
    }

    override fun weiting() {
        showDialogWaiting()
    }

    private fun showDialogWaiting() {
        dialogWaiting = indeterminateProgressDialog("подождите, соединение с сервером")
        dialogWaiting?.setCancelable(false)
        dialogWaiting?.show()
    }
}
