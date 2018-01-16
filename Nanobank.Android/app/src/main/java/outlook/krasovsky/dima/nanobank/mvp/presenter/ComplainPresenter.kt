package outlook.krasovsky.dima.nanobank.mvp.presenter

import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.ComplainViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class ComplainPresenter : MvpPresenter<ComplainViewInterface>() {

    fun complain(dealId: String, complainText: String) {
        viewState.weiting()
        val api = ApiManager(ApiHelper())
        api.complain(dealId, complainText)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(deposits: String?) {
                    }

                    override fun onCompleted() {
                        viewState.complainOK()
                    }

                    override fun onError(e: Throwable?) {
                        viewState.errorComplain(e?.message ?: "ошибка")
                    }
                })
    }
}