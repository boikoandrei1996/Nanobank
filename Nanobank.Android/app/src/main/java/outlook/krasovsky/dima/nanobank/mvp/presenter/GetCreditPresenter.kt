package outlook.krasovsky.dima.nanobank.mvp.presenter

import android.os.Handler
import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.data.cash.realm.model.UserInfoModel
import outlook.krasovsky.dima.nanobank.mvp.view.GetCreditViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.cash.Cash
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterDealModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.SignInModel
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class GetCreditPresenter : MvpPresenter<GetCreditViewInterface>() {

    fun getData() {
        viewState.weiting()
        Cash().getUserInfo(MenuActivity.login!!)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<UserInfoModel>() {
                    override fun onNext(model: UserInfoModel?) {
                        viewState.setUserInfo(model!!)
                    }

                    override fun onCompleted() {

                    }

                    override fun onError(e: Throwable?) {
                        viewState.errorInfo(e?.message ?: "ошибка")
                    }
                })
    }

    fun getCredit(about: String, amount: Double, distance: Int, percent: Double) {
        viewState.weiting()

        val model = RegisterDealModel()
        model.title = about
        model.startAmount = amount
        model.dealDurationMonth = distance
        model.percentRate = percent
        registerCredit(model)
    }

    private fun registerCredit(model: RegisterDealModel) {
        val api = ApiManager(ApiHelper())
        api.registerDeal(model)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(text: String?) {
                    }

                    override fun onCompleted() {
                        viewState.requestDone()
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }

}