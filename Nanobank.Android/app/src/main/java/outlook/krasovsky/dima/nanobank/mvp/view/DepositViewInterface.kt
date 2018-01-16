package outlook.krasovsky.dima.nanobank.mvp.view

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter

interface DepositViewInterface : MvpView {
    @StateStrategyType(SingleStateStrategy::class)
    fun setView(response: DepositsResponse)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun error(message: String)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun close()

    @StateStrategyType(SingleStateStrategy::class)
    fun weiting()
}