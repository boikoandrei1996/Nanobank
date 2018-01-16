package outlook.krasovsky.dima.nanobank.mvp.view

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.AddToEndSingleStrategy
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.SkipStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.data.cash.realm.model.UserInfoModel
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy

interface GetCreditViewInterface : MvpView {
    @StateStrategyType(AddToEndSingleStrategy::class)
    fun requestDone()

    @StateStrategyType(SingleStateStrategy::class)
    fun setUserInfo(model: UserInfoModel)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun error(message: String)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun errorInfo(message: String)

    @StateStrategyType(AddToEndSingleStrategy::class)
    fun weiting()
}