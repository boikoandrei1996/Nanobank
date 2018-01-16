package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.Activity
import android.app.ProgressDialog
import android.content.Context
import android.content.Intent
import android.os.Build.ID
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_details_deposit.*
import org.jetbrains.anko.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.DepositsPresenter
import outlook.krasovsky.dima.nanobank.mvp.presenter.DetailsDepositsPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.DepositViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.ui.fragment.DepositsFragment
import java.util.Arrays.asList

class DetailsDepositActivity : MvpAppCompatActivity(), DepositViewInterface {

    private lateinit var id: String

    var dialogWaiting: ProgressDialog? = null

    @InjectPresenter
    lateinit var mPresenter: DetailsDepositsPresenter

    companion object {
        private val ID = "ID_DEAL"

        fun getIntent(context: Context, id: String): Intent {
            val intent = Intent(context, DetailsDepositActivity::class.java)
            intent.putExtra(ID, id)
            return intent
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_details_deposit)

        id = intent.getStringExtra(ID)
        if (savedInstanceState == null) {
            mPresenter.getDeposit(id)
        }
        initBtn()
    }

    private fun initBtn() {
        btn_apointment.setOnClickListener { onRepsond() }
    }

    private fun onRepsond() {
        alert("Вы действительно хотите отозваться на данное предложение?", "Предупреждение") {
            yesButton {
                dialog -> dialog.dismiss()
                mPresenter.respond(id)
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    override fun error(message: String) {
        dialogWaiting?.dismiss()
        btn_apointment.isEnabled = false
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

    override fun close() {
        val intent = Intent()
        intent.putExtra(DepositsFragment.LOCAL_ID, id)
        setResult(Activity.RESULT_OK, intent)
        this.finish()
    }

    override fun setView(response: DepositsResponse) {
        dialogWaiting?.dismiss()
        btn_apointment.isEnabled = true
        deposit_owner.text = response.ownerUserName
        deposit_owner.setOnClickListener { startActivity(InfoUserActivity.getIntent(this, response.ownerUserName)) }
        deposit_about.text = response.title
        deposit_value.text = "${response.startAmount} руб."
        deposit_period.text = "${response.dealDurationInMonth} мес."
        deposit_percent.text = "${response.percentRate}"
    }
}
