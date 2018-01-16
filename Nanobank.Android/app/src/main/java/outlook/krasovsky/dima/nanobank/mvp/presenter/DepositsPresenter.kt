package outlook.krasovsky.dima.nanobank.mvp.presenter

import android.util.Log
import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import org.json.JSONObject
import outlook.krasovsky.dima.nanobank.mvp.view.DepositsViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.request.SignInModel
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class DepositsPresenter : MvpPresenter<DepositsViewInterface>() {

    private lateinit var mAdapter: DepositsAdapter
    private lateinit var currentCredits: MutableList<DepositsResponse>

    fun getDeposits() {
        viewState.weiting()
        getAll()
    }

    fun getDepositsRefresh() {
        getAll()
    }

    fun refresh(localId: String) {
        currentCredits = currentCredits.filter { it.id != localId }.toMutableList()
        initRV(currentCredits)
    }

    private fun getAll() {
        val api = ApiManager(ApiHelper())
        api.getDeposits()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<MutableList<DepositsResponse>>() {
                    override fun onNext(deposits: MutableList<DepositsResponse>?) {
                        currentCredits = deposits!!
                        initRV(currentCredits)
                    }

                    override fun onCompleted() {
                    }

                    override fun onError(e: Throwable?) {
                        viewState.error(e?.message ?: "ошибка")
                    }
                })
    }

    private fun initRV(deposits: MutableList<DepositsResponse>) {
        mAdapter = DepositsAdapter(deposits, this::detailsDeposit)
        viewState.setAdapterRV(mAdapter)
    }

    private fun detailsDeposit(id: String) {
        viewState.startDetailsDeposit(id)
    }
}