package outlook.krasovsky.dima.nanobank.mvp.presenter

import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.CurrentCreditsViewInterface
import outlook.krasovsky.dima.nanobank.mvp.view.HistoryCreditsViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.CurrentDealsActivity
import outlook.krasovsky.dima.nanobank.ui.activity.CurrentDealsActivity.*
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login
import outlook.krasovsky.dima.nanobank.ui.adapter.CurrentDepositsAdapter
import outlook.krasovsky.dima.nanobank.ui.adapter.OldDepositsAdapter
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class CurrentCreditsPresenter : MvpPresenter<CurrentCreditsViewInterface>() {

    private lateinit var mAdapter: CurrentDepositsAdapter
    private var currentDeposits: MutableList<DepositsResponse>? = null

    var selectedFilter = 0

    fun getCurrentCredits() {
        viewState.weiting()
        getCurrentCredits(MenuActivity.login!!)
    }

    fun sort(type: Sort) {
        if (currentDeposits != null) {
            when (type) {
                Sort.ALL -> initRV(currentDeposits!!)
                Sort.CREDITOR -> initRV(currentDeposits!!.filter { it.creditorUserName == login }.toMutableList())
                Sort.OWNER -> initRV(currentDeposits!!.filter { (it.ownerUserName == login) and (it.creditorUserName != null) }.toMutableList())
                Sort.OWNER_WAIT -> initRV(currentDeposits!!.filter { it.creditorUserName == null }.toMutableList())
            }
        }
    }

    fun refresh(localId: String) {
        currentDeposits = currentDeposits!!.filter { it.id != localId }.toMutableList()
        initRV(currentDeposits!!)
    }

    private fun getCurrentCredits(login: String) {
        val api = ApiManager(ApiHelper())
        api.getDealsOfUser(login)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<MutableList<DepositsResponse>>() {
                    override fun onNext(credits: MutableList<DepositsResponse>?) {
                        currentDeposits = credits!!.filter { !it.isClosed }.toMutableList()
                        initRV(currentDeposits!!)
                    }

                    override fun onCompleted() {
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }


    private fun initRV(currentDeposits: MutableList<DepositsResponse>) {
        mAdapter = CurrentDepositsAdapter(currentDeposits, this::detailsDeposit)
        viewState.setAdapterRV(mAdapter)
    }

    private fun detailsDeposit(id: String) {
        viewState.startDetailsDeposit(id)
    }
}