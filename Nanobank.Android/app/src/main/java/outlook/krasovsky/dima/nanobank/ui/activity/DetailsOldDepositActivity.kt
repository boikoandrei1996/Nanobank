package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.ProgressDialog
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.view.View
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_details_old_deposit.*
import org.jetbrains.anko.alert
import org.jetbrains.anko.backgroundResource
import org.jetbrains.anko.indeterminateProgressDialog
import org.jetbrains.anko.yesButton

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.DetailsOldDepositsPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.DepositOldViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login

class DetailsOldDepositActivity : MvpAppCompatActivity(), DepositOldViewInterface {

    private lateinit var id: String

    var dialogWaiting: ProgressDialog? = null

    @InjectPresenter
    lateinit var mPresenter: DetailsOldDepositsPresenter

    companion object {
        private val ID = "ID_DEAL"

        fun getIntent(context: Context, id: String): Intent {
            val intent = Intent(context, DetailsOldDepositActivity::class.java)
            intent.putExtra(ID, id)
            return intent
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_details_old_deposit)

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

    override fun weiting() {
        showDialogWaiting()
    }

    private fun showDialogWaiting() {
        dialogWaiting = indeterminateProgressDialog("подождите, соединение с сервером")
        dialogWaiting?.setCancelable(false)
        dialogWaiting?.show()
    }


    override fun setView(response: DepositsResponse) {
        dialogWaiting?.dismiss()
        if (response.ownerUserName == login) {
            tv_old_owner.text = response.creditorUserName
            tv_old_owner.setOnClickListener { startActivity(InfoUserActivity.getIntent(this, response.creditorUserName!!)) }
        } else {
            tv_old_owner.text = response.ownerUserName
            tv_old_owner.setOnClickListener { startActivity(InfoUserActivity.getIntent(this, response.ownerUserName)) }
        }
        tv_old_about.text = response.title
        old_start_value_details.text = "${response.startAmount} руб."
        old_finish_value_details.text = "${response.startAmount * (1 + response.dealDurationInMonth * (response.percentRate / 100))} руб."
        old_duration_details.text = "${response.dealDurationInMonth} мес."
        old_percent_details.text = response.percentRate.toString()
        old_deposit_start_data.text = response.dealStartDate.split("T")[0]
        old_deposit_end_data.text = response.dealClosedDate!!.split("T")[0]
        line_role.backgroundResource = if (response.ownerUserName == login) R.color.colorOwner else R.color.colorCreditor
    }
}
