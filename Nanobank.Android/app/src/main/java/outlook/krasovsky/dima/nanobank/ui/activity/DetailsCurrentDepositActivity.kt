package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.Activity
import android.app.ProgressDialog
import android.content.Context
import android.content.Intent
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_details_current_deposit.*
import kotlinx.android.synthetic.main.layout_current_details_creditor.*
import kotlinx.android.synthetic.main.layout_current_details_creditor.view.*
import kotlinx.android.synthetic.main.layout_current_details_owner.*
import kotlinx.android.synthetic.main.layout_current_details_owner.view.*
import kotlinx.android.synthetic.main.layout_current_details_wait.view.*
import org.jetbrains.anko.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.DetailsCurrentDepositsPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.DepositCurrentViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login
import outlook.krasovsky.dima.nanobank.utils.CheckInput
import outlook.krasovsky.dima.nanobank.utils.CheckInput.Companion.checkPutMoney
import java.text.SimpleDateFormat
import java.util.*

class DetailsCurrentDepositActivity : MvpAppCompatActivity(), DepositCurrentViewInterface {

    private lateinit var id: String

    private lateinit var deal: DepositsResponse

    var dialogWaiting: ProgressDialog? = null

    @InjectPresenter
    lateinit var mPresenter: DetailsCurrentDepositsPresenter

    companion object {
        private val ID = "ID_DEAL"
        val LOCAL_ID = "LOCAL_ID"

        fun getIntent(context: Context, id: String): Intent {
            val intent = Intent(context, DetailsCurrentDepositActivity::class.java)
            intent.putExtra(ID, id)
            return intent
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_details_current_deposit)

        id = intent.getStringExtra(ID)
        if (savedInstanceState == null) {
            mPresenter.getDeposit(id)
        }
    }

    override fun error(message: String) {
        dialogWaiting?.dismiss()
        alert(message, "Ошибка") {
            yesButton { dialog -> dialog.dismiss() }
        }.show()
    }

    override fun errorRating(message: String) {
        setDefauldRating()
        dialogWaiting?.dismiss()
        alert(message, "Ошибка") {
            yesButton { dialog -> dialog.dismiss() }
        }.show()
    }

    private fun setDefauldRating() {
        positive_rating_star.rating = mPresenter.positiveRating
        negative_rating_star.rating = mPresenter.negativeRating
    }

    override fun ratingOK() {
        toast("рейтинг установлен успешно")
    }

    override fun weiting() {
        showDialogWaiting()
    }

    private fun showDialogWaiting() {
        dialogWaiting = indeterminateProgressDialog("подождите, соединение с сервером")
        dialogWaiting?.setCancelable(false)
        dialogWaiting?.show()
    }

    override fun onClose() {
        val intent = Intent()
        intent.putExtra(LOCAL_ID, id)
        setResult(Activity.RESULT_OK, intent)
        this.finish()
    }

    override fun onPutMoney() {
        dialogWaiting?.dismiss()
        toast("Вы погасили часть кредита")
        val value = et_edit_money.text.toString().toDouble()
        current_returnd_cash_owner.text = "${deal.returnedAmount!! + value} руб."
        et_edit_money.text.clear()
    }

    override fun setView(response: DepositsResponse) {
        dialogWaiting?.dismiss()
        deal = response
        if (response.creditorUserName == null) {
            val v = viewStub_details_wait.inflate()
            v.current_details_about.text = response.title
            v.current_wait_amount.text = "${response.startAmount} руб."
            v.current_wait_percent.text = "${response.percentRate}"
            v.current_wait_period.text = "${response.dealDurationInMonth} мес."
            v.btn_edit_deal.setOnClickListener { editDeal() }
        } else {
            if (response.ownerUserName == login) {
                val v = viewStub_details_owner.inflate()
                v.current_details_username_owner.text = response.creditorUserName
                v.current_details_username_owner.setOnClickListener { startActivity(InfoUserActivity.getIntent(this, response.creditorUserName!!)) }
                v.current_details_about_owner.text = response.title
                v.current_start_value_details_owner.text = "${response.startAmount} руб."
                v.current_finish_value_details_owner.text = "${response.startAmount * (1 + response.dealDurationInMonth * (response.percentRate / 100))} р."
                v.current_duration_details_owner.text = "${response.dealDurationInMonth} мес."
                v.old_percent_details_owner.text = response.percentRate.toString()
                v.current_deposit_start_data_owner.text = response.dealStartDate.split("T")[0]
                v.current_returnd_cash_owner.text = "${response.returnedAmount} руб."
                v.btn_put_money_deal.setOnClickListener { onClickPutMoney(response.id) }
            } else {
                val v = viewStub_details_creditor.inflate()
                v.current_details_username_creditor.text = response.ownerUserName
                v.current_details_username_creditor.setOnClickListener { startActivity(InfoUserActivity.getIntent(this, response.ownerUserName)) }
                v.current_details_about_creditor.text = response.title
                v.current_start_value_details_creditor.text = "${response.startAmount} руб."
                val endAmount = response.startAmount * (1 + response.dealDurationInMonth * (response.percentRate / 100))
                v.current_finish_value_details_creditor.text = "${endAmount} руб."
                v.current_duration_details_creditor.text = "${response.dealDurationInMonth} мес."
                v.old_percent_details_creditor.text = response.percentRate.toString()
                v.current_deposit_start_data_creditor.text = response.dealStartDate.split("T")[0]
                v.current_returnd_cash_creditor.text = "${response.returnedAmount} руб."
                v.positive_rating_star.rating = response.ratingPositive?.toFloat() ?: 0f
                v.positive_rating_star.setOnRatingBarChangeListener { ratingBar, fl, b ->
                    if (mPresenter.positiveRating != fl) onClickRatingPositive(fl)
                }
                v.negative_rating_star.rating = response.ratingNegative?.toFloat() ?: 0f
                v.negative_rating_star.setOnRatingBarChangeListener { ratingBar, fl, b ->
                    if (mPresenter.negativeRating != fl) onClickRatingNegative(fl)
                }
                v.btn_close_deal.setOnClickListener { onClickCloseDeal(response.id) }
                val enable = calculateAccessComplaint(endAmount, response.returnedAmount?: 0.0,
                        response.dealStartDate, response.dealDurationInMonth)
                if (enable) {
                    v.btn_complaint_deal.isEnabled = true
                    v.btn_complaint_deal.setOnClickListener { startActivity(ComplainActivity.getIntent(this, response.id)) }
                } else {
                    v.btn_complaint_deal.isEnabled = false
                }
            }
        }
    }

    private fun calculateAccessComplaint(endAmount: Double, returnedAmount: Double,
                                         startDate: String, valueMonths: Int): Boolean {
        val formatter = SimpleDateFormat("yyy-MM-dd'T'HH:mm:ss")
        val date = formatter.parse(startDate)

        val calendar = GregorianCalendar()
        calendar.time = date
        calendar.add(Calendar.MONTH, valueMonths)

        val currentDate = GregorianCalendar()
        return (endAmount > returnedAmount) and (currentDate.after(calendar))
    }

    private fun onClickPutMoney(id: String) {
        alert("Вы действительно хотите погасить часть кредита?", "Предупреждение") {
            yesButton { dialog ->
                dialog.dismiss()
                val valid = checkPutMoney(et_edit_money.text.toString())
                if (valid) {
                    mPresenter.putMoney(id, et_edit_money.text.toString())
                    edit_money.error = null
                } else {
                    val empty = CheckInput().isEmpty(et_edit_money.text.toString())
                    if (empty) {
                        edit_money.error = "пусто"
                    } else {
                        edit_money.error = "некорректный формат"
                    }
                }
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    private fun onClickRatingNegative(rating: Float) {
        alert("Вы действительно поставить плохую оценку?", "Предупреждение") {
            yesButton { dialog ->
                dialog.dismiss()
                mPresenter.setNegative(rating)
            }
            noButton { dialog ->
                dialog.dismiss()
                setDefauldRating()
            }
        }.show()
    }

    private fun onClickRatingPositive(rating: Float) {
        alert("Вы действительно поставить хорошую оценку?", "Предупреждение") {
            yesButton { dialog ->
                dialog.dismiss()
                mPresenter.setPositive(rating)
            }
            noButton { dialog ->
                dialog.dismiss()
                setDefauldRating()
            }
        }.show()
    }

    private fun onClickCloseDeal(id: String) {
        alert("Вы действительно хотите закрыть кредит?", "Предупреждение") {
            yesButton { dialog ->
                dialog.dismiss()
                mPresenter.closeDeal(id)
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    private fun editDeal() {
        startActivity(ChangeCreditActivity.getIntent(this, deal))
    }
}
