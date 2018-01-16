package outlook.krasovsky.dima.nanobank.ui.fragment


import android.app.Activity
import android.app.ProgressDialog
import android.content.Intent
import android.os.Bundle
import android.support.v4.app.Fragment
import android.support.v4.widget.SwipeRefreshLayout
import android.support.v7.widget.LinearLayoutManager
import android.support.v7.widget.RecyclerView
import android.support.v7.widget.RecyclerView.ViewHolder
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import com.arellomobile.mvp.MvpAppCompatFragment
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.fragment_deposits.*
import org.jetbrains.anko.indeterminateProgressDialog
import org.jetbrains.anko.startActivityForResult
import org.jetbrains.anko.support.v4.*
import org.jetbrains.anko.yesButton

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.DepositsPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.DepositsViewInterface
import outlook.krasovsky.dima.nanobank.ui.activity.DetailsDepositActivity
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter


class DepositsFragment : MvpAppCompatFragment(), DepositsViewInterface {

    private val DETAILS = 1

    companion object {
        val LOCAL_ID = "LOCAL_ID"
    }

    @InjectPresenter
    lateinit var mPresenter: DepositsPresenter

    var dialogWaiting: ProgressDialog? = null

    override fun onCreateView(inflater: LayoutInflater?, container: ViewGroup?,
                              savedInstanceState: Bundle?): View? {
        return inflater!!.inflate(R.layout.fragment_deposits, container, false)
    }

    override fun onViewCreated(view: View?, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        if (savedInstanceState == null) {
            mPresenter.getDeposits()
        }

        swipe_layout.setOnRefreshListener {
            mPresenter.getDepositsRefresh()
        }
    }

    override fun setAdapterRV(adapter: DepositsAdapter) {
        if (dialogWaiting?.isShowing != true) {
            swipe_layout.isRefreshing = false
        } else {
            dialogWaiting?.dismiss()
        }

        rv_deposits.layoutManager = LinearLayoutManager(context, LinearLayoutManager.VERTICAL, false)
        rv_deposits.adapter = adapter
    }

    override fun error(message: String) {
        if (dialogWaiting?.isShowing != true) {
            swipe_layout.isRefreshing = false
        } else {
            dialogWaiting?.dismiss()
        }
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
        startActivityForResult(DetailsDepositActivity.getIntent(context, id), DETAILS)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        if (requestCode == DETAILS) {
            if (resultCode == Activity.RESULT_OK) {
                if (data != null) {
                    val localId = data.getStringExtra(LOCAL_ID)
                    mPresenter.refresh(localId)
                    toast("Вы откликнулись на займ, подробности в кабинете пользователя")
                }
            }
        }
    }

    override fun onDestroy() {
        super.onDestroy()
        dialogWaiting?.dismiss()
    }

}
