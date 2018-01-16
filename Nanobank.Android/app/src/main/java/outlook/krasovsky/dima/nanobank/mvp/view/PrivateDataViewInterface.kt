package outlook.krasovsky.dima.nanobank.mvp.view

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.SkipStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse
import outlook.krasovsky.dima.nanobank.ui.adapter.DepositsAdapter

interface PrivateDataViewInterface : MvpView {
    @StateStrategyType(SingleStateStrategy::class)
    fun setView(data: PrivateDataResponse)

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun error(message: String)

    @StateStrategyType(SingleStateStrategy::class)
    fun weiting()

}