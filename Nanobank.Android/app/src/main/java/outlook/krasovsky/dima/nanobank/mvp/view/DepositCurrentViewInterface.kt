package outlook.krasovsky.dima.nanobank.mvp.view

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.SkipStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse

interface DepositCurrentViewInterface : MvpView {
    @StateStrategyType(SingleStateStrategy::class)
    fun setView(response: DepositsResponse)

    @StateStrategyType(SingleStateStrategy::class)
    fun onClose()

    @StateStrategyType(SkipStrategy::class)
    fun onPutMoney()

    @StateStrategyType(SkipStrategy::class)
    fun ratingOK()

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun error(message: String)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun errorRating(message: String)

    @StateStrategyType(SingleStateStrategy::class)
    fun weiting()
}