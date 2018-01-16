package outlook.krasovsky.dima.nanobank.mvp.presenter

import android.content.Context
import android.support.v4.app.Fragment
import com.arellomobile.mvp.InjectViewState
import com.arellomobile.mvp.MvpPresenter
import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.gcm.MyInstanceIDListenerService
import outlook.krasovsky.dima.nanobank.gcm.MyInstanceIDListenerService.Companion.TOKEN_GCM
import outlook.krasovsky.dima.nanobank.mvp.view.SignUpViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.cash.Cash
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.ui.fragment.*
import outlook.krasovsky.dima.nanobank.ui.fragment.`interface`.DataFramentInterface
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

@InjectViewState
class SignUpPresenter : MvpPresenter<SignUpViewInterface>(), DataFramentInterface {

    val model = SignUpModel()
    val stackFragments = mutableListOf<Fragment>()
    var position = 0

    init {
        initStack()
        replaceFragment(stackFragments[position])
    }

    private fun initStack() {
        stackFragments.add(AppDataFragment())
        stackFragments.add(PrivateDataFragment())
        stackFragments.add(CreditCardDataFragment())
        stackFragments.add(PasportDataFragment())
        stackFragments.add(ContractDataFragment())
    }

    override fun getData(): SignUpModel = model

    fun moveBack() {
        position -= 1
        if (position == -1) {
            viewState.close()
        } else if (position == 0) {
            replaceFragment(stackFragments[position])
            renameBtnBack("выйти")
            moveToBackIndicator()
        } else {
            replaceFragment(stackFragments[position])
            renameBtnNext("далее")
            moveToBackIndicator()
        }
    }

    fun moveNext(context: Context) {
        position += 1
        if (position == 4) {
            viewState.setData(stackFragments[position - 1])
            replaceFragment(stackFragments[position])
            renameBtnNext("регистрация")
            moveToNextIndicator()
        } else if (position == 5) {
            position -= 1
            viewState.weiting()
            sendData(getToken(context))
        } else {
            viewState.setData(stackFragments[position - 1])
            replaceFragment(stackFragments[position])
            renameBtnBack("назад")
            moveToNextIndicator()
        }
    }

    private fun sendData(tokenGCM: String) {
        val api = ApiManager(ApiHelper())
        api.registry(model, tokenGCM)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(object : Subscriber<String>() {
                    override fun onNext(text: String?) {
                    }

                    override fun onCompleted() {
                        Cash().saveUserInfo(model)
                        viewState.responseOk("Ожидайте письмо на почте с подтверждением регистрации!")
                    }

                    override fun onError(e: Throwable?) {
                        viewState.responseError(e?.message?: "ошибка")
                    }
                })
    }

    private fun getToken(context: Context): String {
        val shpref = context.getSharedPreferences(context.getString(R.string.app_name), Context.MODE_PRIVATE)
        return shpref.getString(TOKEN_GCM, "")
    }

    fun moveToNextIndicator() {
        val positionOld = position - 1
        val positionNew = position
        viewState.moveToNextIndicator(positionOld, positionNew)
    }

    fun moveToBackIndicator() {
        val positionOld = position + 1
        val positionNew = position
        viewState.moveToBackIndicator(positionOld, positionNew)
    }

    fun replaceFragment(fragment: Fragment) {
        viewState.replaceFragment(fragment)
    }

    fun renameBtnBack(name: String) {
        viewState.renameBtnBack(name)
    }

    fun renameBtnNext(name: String) {
        viewState.renameNameBtnNext(name)
    }

}