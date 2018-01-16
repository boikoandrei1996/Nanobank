package outlook.krasovsky.dima.nanobank.mvp.view

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.AddToEndSingleStrategy
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.SkipStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter

interface DepositsViewInterface : MvpView {
    @StateStrategyType(SingleStateStrategy::class)
    fun setAdapterRV(adapter: DepositsAdapter)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun error(message: String)

    @StateStrategyType(AddToEndSingleStrategy::class)
    fun weiting()

    @StateStrategyType(SkipStrategy::class)
    fun startDetailsDeposit(id: String)
}