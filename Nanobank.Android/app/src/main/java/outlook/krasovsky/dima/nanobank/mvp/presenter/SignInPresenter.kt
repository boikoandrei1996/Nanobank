package outlook.krasovsky.dima.nanobank.mvp.presenter

import android.util.Log
import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.SignInViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.request.SignInModel
import outlook.krasovsky.dima.nanobank.strategy.remote.response.SignInResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class SignInPresenter : MvpPresenter<SignInViewInterface>() {

    fun signIn(login: String, password: String) {
        viewState.weiting()
        val model = SignInModel()
        model.username = login
        model.password = password
        singIn(model, login)
    }

    private fun singIn(model: SignInModel, login: String) {
        val api = ApiManager(ApiHelper())
        api.signIn(model)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<SignInResponse>() {
                    override fun onNext(params: SignInResponse?) {
                        viewState.writeParams(params!!, login)
                        viewState.getIn()
                    }

                    override fun onCompleted() {
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }

}