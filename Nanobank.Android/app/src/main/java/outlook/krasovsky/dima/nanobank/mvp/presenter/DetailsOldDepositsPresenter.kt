package outlook.krasovsky.dima.nanobank.mvp.presenter

import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.DepositOldViewInterface
import outlook.krasovsky.dima.nanobank.mvp.view.DepositViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class DetailsOldDepositsPresenter : MvpPresenter<DepositOldViewInterface>() {

    fun getDeposit(id: String) {
        viewState.weiting()
        getItemDeposit(id)

    }

    private fun getItemDeposit(id: String) {
        val api = ApiManager(ApiHelper())
        api.getDeposit(id)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<DepositsResponse>() {
                    override fun onNext(deposits: DepositsResponse?) {
                        initView(deposits!!)
                    }

                    override fun onCompleted() {
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }

    private fun initView(deposit: DepositsResponse) {
       viewState.setView(deposit)
    }

}