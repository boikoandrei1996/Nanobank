package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.Activity
import android.app.ProgressDialog
import android.content.Intent
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.v7.widget.LinearLayoutManager
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_current_deals.*
import org.jetbrains.anko.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.CurrentCreditsPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.CurrentCreditsViewInterface
import outlook.krasovsky.dima.nanobank.ui.activity.DetailsCurrentDepositActivity.Companion.LOCAL_ID
import outlook.krasovsky.dima.nanobank.ui.adapter.CurrentDepositsAdapter
import outlook.krasovsky.dima.nanobank.ui.dialogs.Dialogs.Companion.startAlertInfo

class CurrentDealsActivity : MvpAppCompatActivity(), CurrentCreditsViewInterface {

    private val DETAILS = 1

    enum class Sort {
        ALL, CREDITOR, OWNER, OWNER_WAIT
    }

    var dialogWaiting: ProgressDialog? = null

    @InjectPresenter
    lateinit var mPresenter: CurrentCreditsPresenter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_current_deals)

        if (savedInstanceState == null) {
            mPresenter.getCurrentCredits()
        }
    }

    override fun onNewIntent(intent: Intent?) {
        super.onNewIntent(intent)
        mPresenter.getCurrentCredits()
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        val inflater = menuInflater
        inflater.inflate(R.menu.menu_sort_current, menu)
        when (mPresenter.selectedFilter) {
            0 -> {
                menu!!.findItem(R.id.action_current_all).isChecked = true
            }
            1 -> {
                menu!!.findItem(R.id.action_current_credirot).isChecked = true
            }
            2 -> {
                menu!!.findItem(R.id.action_current_owner).isChecked = true
            }
            3 -> {
                menu!!.findItem(R.id.action_current_owner_wait).isChecked = true
            }
        }
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem?): Boolean {
        when (item!!.getItemId()) {
            R.id.action_current_all -> {
                mPresenter.sort(Sort.ALL)
                mPresenter.selectedFilter = 0
                item.isChecked = true
                return true
            }
            R.id.action_current_credirot -> {
                mPresenter.sort(Sort.CREDITOR)
                mPresenter.selectedFilter = 1
                item.isChecked = true
                return true
            }
            R.id.action_current_owner -> {
                mPresenter.sort(Sort.OWNER)
                mPresenter.selectedFilter = 2
                item.isChecked = true
                return true
            }
            R.id.action_current_owner_wait -> {
                mPresenter.sort(Sort.OWNER_WAIT)
                mPresenter.selectedFilter = 3
                item.isChecked = true
                return true
            }
            R.id.action_current_info -> {
                startAlertInfo(this, true)
                return true
            }
            else -> return super.onOptionsItemSelected(item)
        }
    }

    override fun setAdapterRV(adapter: CurrentDepositsAdapter) {
        dialogWaiting?.dismiss()

        current_rv_deposits.layoutManager = LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false)
        current_rv_deposits.adapter = adapter
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
        startActivityForResult(DetailsCurrentDepositActivity.getIntent(this, id), DETAILS)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        if (requestCode == DETAILS) {
            if (resultCode == Activity.RESULT_OK) {
                if (data != null) {
                    val localId = data.getStringExtra(LOCAL_ID)
                    mPresenter.refresh(localId)
                    toast("Вы закрыли сделку")
                }
            }
        }
    }
}
