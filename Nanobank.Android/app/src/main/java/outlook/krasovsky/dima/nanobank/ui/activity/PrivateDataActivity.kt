package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.ProgressDialog
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_private_data.*
import org.jetbrains.anko.alert
import org.jetbrains.anko.indeterminateProgressDialog
import org.jetbrains.anko.yesButton

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.PrivateDataPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.PrivateDataViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter

class PrivateDataActivity : MvpAppCompatActivity(), PrivateDataViewInterface {

    var dialogWaiting: ProgressDialog? = null

    @InjectPresenter
    lateinit var mPresenter: PrivateDataPresenter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_private_data)

        if (savedInstanceState == null) {
            mPresenter.getPrivateData()
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
        cardNumber.text = data.cardNumber
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
