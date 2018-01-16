package outlook.krasovsky.dima.nanobank.mvp.presenter

import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.InfoUserViewInterface
import outlook.krasovsky.dima.nanobank.mvp.view.PrivateDataViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class InfoUserPresenter : MvpPresenter<InfoUserViewInterface>() {

    fun getPrivateData(login: String) {
        viewState.weiting()
        getData(login)
    }

    private fun getData(login: String) {
        val api = ApiManager(ApiHelper())
        api.getUser(login)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<PrivateDataResponse>() {
                    override fun onNext(data: PrivateDataResponse?) {
                        viewState.setView(data!!)
                    }

                    override fun onCompleted() {
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }
}