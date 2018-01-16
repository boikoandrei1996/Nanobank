package outlook.krasovsky.dima.nanobank.mvp.presenter

import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.data.cash.realm.model.UserInfoModel
import outlook.krasovsky.dima.nanobank.mvp.view.ChangeCreditViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.cash.Cash
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterDealModel
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class ChangeCreditPresenter : MvpPresenter<ChangeCreditViewInterface>() {

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

    fun changeCredit(id: String, model: RegisterDealModel) {
        viewState.weiting()
        getItemDeposit(id, model)
    }

    private fun getItemDeposit(id: String, model: RegisterDealModel) {
        val api = ApiManager(ApiHelper())
        api.changeDeal(id, model)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(deposits: String?) {
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