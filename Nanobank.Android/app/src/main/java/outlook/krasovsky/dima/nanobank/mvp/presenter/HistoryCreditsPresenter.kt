package outlook.krasovsky.dima.nanobank.mvp.presenter

import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.HistoryCreditsViewInterface
import outlook.krasovsky.dima.nanobank.mvp.view.PrivateDataViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.HistoryCreditsActivity
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login
import outlook.krasovsky.dima.nanobank.ui.adapter.OldDepositsAdapter
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class HistoryCreditsPresenter : MvpPresenter<HistoryCreditsViewInterface>() {

    private lateinit var mAdapter: OldDepositsAdapter
    private var oldCredits: MutableList<DepositsResponse>? = null

    var selectedFilter = 0

    fun getOldCredits() {
        viewState.weiting()
        getOldCredits(MenuActivity.login!!)
    }

    fun sort(type: HistoryCreditsActivity.Sort) {
        if (oldCredits != null) {
            when (type) {
                HistoryCreditsActivity.Sort.ALL -> initRV(oldCredits!!)
                HistoryCreditsActivity.Sort.CREDITOR -> initRV(oldCredits!!.filter { it.creditorUserName == login }.toMutableList())
                HistoryCreditsActivity.Sort.OWNER -> initRV(oldCredits!!.filter { it.ownerUserName == login }.toMutableList())
            }
        }
    }

    private fun getOldCredits(login: String) {
        val api = ApiManager(ApiHelper())
        api.getDealsOfUser(login)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<MutableList<DepositsResponse>>() {
                    override fun onNext(credits: MutableList<DepositsResponse>?) {
                        oldCredits = credits!!.filter { it.isClosed }.toMutableList()
                        initRV(oldCredits!!)
                    }

                    override fun onCompleted() {
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }


    private fun initRV(oldDeposits: MutableList<DepositsResponse>) {
        mAdapter = OldDepositsAdapter(oldDeposits, this::detailsDeposit)
        viewState.setAdapterRV(mAdapter)
    }

    private fun detailsDeposit(id: String) {
        viewState.startDetailsDeposit(id)
    }
}