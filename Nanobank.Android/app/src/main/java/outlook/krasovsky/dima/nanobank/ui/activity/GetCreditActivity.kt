package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.ProgressDialog
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.design.widget.TextInputLayout
import android.util.Log
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_get_credit.*
import org.jetbrains.anko.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.data.cash.realm.model.UserInfoModel
import outlook.krasovsky.dima.nanobank.mvp.presenter.GetCreditPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.GetCreditViewInterface
import outlook.krasovsky.dima.nanobank.utils.CheckInput
import outlook.krasovsky.dima.nanobank.utils.CheckInput.Companion.checkPercentDeal
import outlook.krasovsky.dima.nanobank.utils.CheckInput.Companion.checkPeriod
import outlook.krasovsky.dima.nanobank.utils.CheckInput.Companion.checkPutMoney
import outlook.krasovsky.dima.nanobank.utils.TransformData.Companion.transformAboutCreditInput

class GetCreditActivity : MvpAppCompatActivity(), GetCreditViewInterface {

    @InjectPresenter
    lateinit var mPresenter: GetCreditPresenter

    var dialogWaiting: ProgressDialog? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_get_credit)

        if (savedInstanceState == null) {
            mPresenter.getData()
        }

        initBtn()
    }

    private fun initBtn() {
        btn_send_credit.setOnClickListener { onClickGetCredit() }
    }

    private fun onClickGetCredit() {
        alert("Вы действительно хотите получить кредит с такими условиями?", "Предупреждение") {
            yesButton {
                dialog -> dialog.dismiss()
                validateData()
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    private fun validateData() {
        val about = et_about_credit.text.toString()
        val amount = et_amount.text.toString()
        val distance = et_distance.text.toString()
        val percent = et_percent.text.toString()

        val isValidate = checkEmpty(about, about_credit_layout)
                .and(checkPrice(amount, amount_layout))
                .and(checkDistance(distance, distance_layout))
                .and(checkPercent(percent, percent_layout))
        if (isValidate) {
            mPresenter.getCredit(transformAboutCreditInput(about),
                    amount.toDouble(), distance.toInt(), percent.toDouble())
        }
    }

    private fun checkEmpty(values: String, layout: TextInputLayout): Boolean {
        val empty = CheckInput().isEmpty(values)
        if (empty) {
            layout.error = "пусто"
            return false
        }
        layout.error = null
        return true
    }

    private fun checkPrice(values: String, layout: TextInputLayout): Boolean {
        try {
            val empty = CheckInput().isEmpty(values)
            if (empty) {
                layout.error = "пусто"
                return false
            }
            if (!checkPutMoney(values)) {
                layout.error = "неккоректный формат"
                return false
            }
            layout.error = null
            return true
        } catch (e: Exception) {
            layout.error = e.message
            return false
        }
    }

    private fun checkDistance(values: String, layout: TextInputLayout): Boolean {
        try {
            val empty = CheckInput().isEmpty(values)
            if (empty) {
                layout.error = "пусто"
                return false
            }
            if (!checkPeriod(values)) {
                layout.error = "неккоректный формат.([1-24])"
                return false
            }
            layout.error = null
            return true
        } catch (e: Exception) {
            layout.error = "неккоректный формат.([1-24])"
            return false
        }
    }

    private fun checkPercent(values: String, layout: TextInputLayout): Boolean {
        try {
            val empty = CheckInput().isEmpty(values)
            if (empty) {
                layout.error = "пусто"
                return false
            }
            if (!checkPercentDeal(values)) {
                layout.error = "неккоректный формат"
                return false
            }
            layout.error = null
            return true
        } catch (e: Exception) {
            layout.error = e.message
            return false
        }
    }

    override fun setUserInfo(model: UserInfoModel) {
        dialogWaiting?.dismiss()

        firstName.text = model.firstName
        lastName.text = model.lastName
        patronymic.text = model.patronymic
        email.text = model.email
        phoneNumber.text = model.phoneNumber
    }

    override fun requestDone() {
        dialogWaiting?.dismiss()
        toast("заявка подана")
        this.finish()
    }

    override fun errorInfo(message: String) {
        Log.e("MYLOG", "message = ${message}")
        dialogWaiting?.dismiss()
        this.finish()
        toast("проверьте соединение с интернетом")
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

    override fun onDestroy() {
        super.onDestroy()
        dialogWaiting?.dismiss()
    }

}
