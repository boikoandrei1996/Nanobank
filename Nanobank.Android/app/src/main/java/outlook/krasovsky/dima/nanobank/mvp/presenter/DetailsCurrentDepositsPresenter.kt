package outlook.krasovsky.dima.nanobank.mvp.presenter

import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.DepositCurrentViewInterface
import outlook.krasovsky.dima.nanobank.mvp.view.DepositOldViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class DetailsCurrentDepositsPresenter : MvpPresenter<DepositCurrentViewInterface>() {

    var positiveRating: Float = 0f
    var negativeRating: Float = 0f

    lateinit var id: String

    fun getDeposit(id: String) {
        viewState.weiting()
        getItemDeposit(id)
    }

    fun putMoney(id: String, amount: String) {
        viewState.weiting()
        putMoneyDeal(id, amount)
    }

    fun closeDeal(id: String) {
        viewState.weiting()
        onCloseDeal(id)
    }


    fun setPositive(rating: Float) {
        if (rating != positiveRating) {
            setRating(rating, negativeRating)

        }
    }

    fun setNegative(rating: Float) {
        if (rating != negativeRating) {
            setRating(positiveRating, rating)

        }
    }

    private fun setRating(pos: Float, neg: Float) {
        val api = ApiManager(ApiHelper())
        api.setRating(id, pos, neg)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(deposits: String?) {
                    }

                    override fun onCompleted() {
                        positiveRating = pos
                        negativeRating = neg
                        viewState.ratingOK()
                    }

                    override fun onError(e: Throwable?) {
                        viewState.errorRating(e?.message ?: "ошибка")
                    }
                })
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

    private fun putMoneyDeal(id: String, amount: String) {
        val api = ApiManager(ApiHelper())
        api.putMoney(id, amount)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(deposits: String?) {
                    }

                    override fun onCompleted() {
                        viewState.onPutMoney()
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }

    private fun onCloseDeal(id: String) {
        val api = ApiManager(ApiHelper())
        api.closeDeal(id)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(deposits: String?) {
                    }

                    override fun onCompleted() {
                        viewState.onClose()
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }

    private fun initView(deposit: DepositsResponse) {
        id = deposit.id
        positiveRating = deposit.ratingPositive?.toFloat()?: 0f
        negativeRating = deposit.ratingNegative?.toFloat()?: 0f
        viewState.setView(deposit)
    }

}