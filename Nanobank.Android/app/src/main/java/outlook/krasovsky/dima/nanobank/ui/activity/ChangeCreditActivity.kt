package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.ProgressDialog
import android.content.Context
import android.content.Intent
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.design.widget.TextInputLayout
import android.text.Editable
import android.util.Log
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_change_credit.*
import org.jetbrains.anko.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.data.cash.realm.model.UserInfoModel
import outlook.krasovsky.dima.nanobank.mvp.presenter.ChangeCreditPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.ChangeCreditViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterDealModel
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login
import outlook.krasovsky.dima.nanobank.utils.CheckInput
import outlook.krasovsky.dima.nanobank.utils.TransformData

class ChangeCreditActivity : MvpAppCompatActivity(), ChangeCreditViewInterface {

    @InjectPresenter
    lateinit var mPresenter: ChangeCreditPresenter

    private lateinit var id: String
    private var deal = RegisterDealModel()
    var dialogWaiting: ProgressDialog? = null

    companion object {
        val ID = "ID"
        val TITLE = "TITLE"
        val START_AMOUNT = "START_AMOUNT"
        val DEAL_DURATION_IN_MONTH = "DEAL_DURATION_IN_MONTH"
        val PERCENT_RATE = "PERCENT_RATE"

        fun getIntent(context: Context, deal: DepositsResponse): Intent {
            val intent = Intent(context, ChangeCreditActivity::class.java)
            intent.putExtra(ID, deal.id)
            intent.putExtra(TITLE, deal.title)
            intent.putExtra(START_AMOUNT, deal.startAmount)
            intent.putExtra(DEAL_DURATION_IN_MONTH, deal.dealDurationInMonth)
            intent.putExtra(PERCENT_RATE, deal.percentRate)
            return intent
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_change_credit)

        id = intent.getStringExtra(ID)
        deal.title = intent.getStringExtra(TITLE)
        deal.startAmount = intent.getDoubleExtra(START_AMOUNT, 0.0)
        deal.dealDurationMonth = intent.getIntExtra(DEAL_DURATION_IN_MONTH, 0)
        deal.percentRate = intent.getDoubleExtra(PERCENT_RATE, 0.0)
        if (savedInstanceState == null) {
            initView()
            mPresenter.getData()
        }

        initBtn()
    }

    private fun initView() {
        et_about_credit_ch.setText(deal.title)
        et_amount_ch.setText(deal.startAmount.toString())
        et_distance_ch.setText(deal.dealDurationMonth.toString())
        et_percent_ch.setText(deal.percentRate.toString())
    }

    private fun initBtn() {
        btn_send_credit_ch.setOnClickListener { onClickGetCredit() }
        btn_close_credit_ch.setOnClickListener { onClickCancelCredit() }
    }

    private fun validateData() {
        val about = et_about_credit_ch.text.toString()
        val amount = et_amount_ch.text.toString()
        val distance = et_distance_ch.text.toString()
        val percent = et_percent_ch.text.toString()

        val isValidate = checkEmpty(about, about_credit_layout_ch)
                .and(checkPrice(amount, amount_layout_ch))
                .and(checkDistance(distance, distance_layout_ch))
                .and(checkPercent(percent, percent_layout_ch))
        if (isValidate) {
            deal.title = TransformData.transformAboutCreditInput(about)
            deal.startAmount = amount.toDouble()
            deal.dealDurationMonth = distance.toInt()
            deal.percentRate = percent.toDouble()
            mPresenter.changeCredit(id, deal)
        }
    }

    private fun onClickGetCredit() {
        alert("Вы действительно хотите изменить условия кредита?", "Предупреждение") {
            yesButton {
                dialog -> dialog.dismiss()
                validateData()
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    private fun onClickCancelCredit() {
        alert("Вы действительно хотите выйти? Все изменения будут утрачены.", "Предупреждение") {
            yesButton {
                dialog -> dialog.dismiss()
                this@ChangeCreditActivity.finish()
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    override fun onBackPressed() {
        onClickCancelCredit()
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
            if (!CheckInput.checkPutMoney(values)) {
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
            if (!CheckInput.checkPeriod(values)) {
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
            if (!CheckInput.checkPercentDeal(values)) {
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

    override fun requestDone() {
        dialogWaiting?.dismiss()
        toast("заявка изменена")
        startActivity<CurrentDealsActivity>()
    }

    override fun error(message: String) {
        dialogWaiting?.dismiss()
        toast(message)
    }

    override fun errorInfo(message: String) {
        Log.e("MYLOG", "message = ${message}")
        dialogWaiting?.dismiss()
        this.finish()
        toast("проверьте соединение с интернетом")
    }

    override fun setUserInfo(model: UserInfoModel) {
        dialogWaiting?.dismiss()

        firstName.text = model.firstName
        lastName.text = model.lastName
        patronymic.text = model.patronymic
        email.text = model.email
        phoneNumber.text = model.phoneNumber
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
