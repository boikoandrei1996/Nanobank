package outlook.krasovsky.dima.nanobank.mvp.presenter

import android.util.Log
import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.DepositViewInterface
import outlook.krasovsky.dima.nanobank.mvp.view.DepositsViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class DetailsDepositsPresenter : MvpPresenter<DepositViewInterface>() {

    fun getDeposit(id: String) {
        viewState.weiting()
        getItemDeposit(id)

    }

    fun respond(id: String) {
        viewState.weiting()
        setRespond(id)
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

    private fun setRespond(id: String) {
        val api = ApiManager(ApiHelper())
        api.setRespond(id)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(deposits: String?) {
                    }

                    override fun onCompleted() {
                        viewState.close()
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