package outlook.krasovsky.dima.nanobank.mvp.view

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.SkipStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy
import outlook.krasovsky.dima.nanobank.ui.adapter.CurrentDepositsAdapter
import outlook.krasovsky.dima.nanobank.ui.adapter.OldDepositsAdapter

interface CurrentCreditsViewInterface : MvpView {

    @StateStrategyType(SingleStateStrategy::class)
    fun setAdapterRV(adapter: CurrentDepositsAdapter)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun error(message: String)

    @StateStrategyType(SingleStateStrategy::class)
    fun weiting()

    @StateStrategyType(SkipStrategy::class)
    fun startDetailsDeposit(id: String)
}