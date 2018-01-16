package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.ProgressDialog
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.v7.widget.LinearLayoutManager
import android.view.Menu
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_history_credits.*
import org.jetbrains.anko.alert
import org.jetbrains.anko.indeterminateProgressDialog
import org.jetbrains.anko.yesButton

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.HistoryCreditsPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.HistoryCreditsViewInterface
import outlook.krasovsky.dima.nanobank.ui.adapter.OldDepositsAdapter
import android.view.MenuInflater
import android.view.MenuItem
import outlook.krasovsky.dima.nanobank.ui.dialogs.Dialogs


class HistoryCreditsActivity : MvpAppCompatActivity(), HistoryCreditsViewInterface {

    var dialogWaiting: ProgressDialog? = null

    enum class Sort {
        ALL, CREDITOR, OWNER
    }

    @InjectPresenter
    lateinit var mPresenter: HistoryCreditsPresenter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_history_credits)

        if (savedInstanceState == null) {
            mPresenter.getOldCredits()
        }
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        val inflater = menuInflater
        inflater.inflate(R.menu.menu_sort_old, menu)
        when (mPresenter.selectedFilter) {
            0 -> {
                menu!!.findItem(R.id.action_all).isChecked = true
            }
            1 -> {
                menu!!.findItem(R.id.action_credirot).isChecked = true
            }
            2 -> {
                menu!!.findItem(R.id.action_owner).isChecked = true
            }
        }
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem?): Boolean {
        when (item!!.getItemId()) {
            R.id.action_all -> {
                mPresenter.sort(Sort.ALL)
                mPresenter.selectedFilter = 0
                item.isChecked = true
                return true
            }
            R.id.action_credirot -> {
                mPresenter.sort(Sort.CREDITOR)
                mPresenter.selectedFilter = 1
                item.isChecked = true
                return true
            }
            R.id.action_owner -> {
                mPresenter.sort(Sort.OWNER)
                mPresenter.selectedFilter = 2
                item.isChecked = true
                return true
            }
            R.id.action_info -> {
                Dialogs.startAlertInfo(this, false)
                return true
            }
            else -> return super.onOptionsItemSelected(item)
        }
    }

    override fun setAdapterRV(adapter: OldDepositsAdapter) {
        dialogWaiting?.dismiss()

        rv_old_deposits.layoutManager = LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false)
        rv_old_deposits.adapter = adapter
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

    override fun startDetailsDeposit(id: String) {
        startActivity(DetailsOldDepositActivity.getIntent(this, id))
    }
}
