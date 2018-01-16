package outlook.krasovsky.dima.nanobank.mvp.view

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.SkipStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse

interface ComplainViewInterface : MvpView {

    @StateStrategyType(SkipStrategy::class)
    fun complainOK()

    @StateStrategyType(CleanAndSkipStrategy::class)
    fun errorComplain(message: String)

    @StateStrategyType(SingleStateStrategy::class)
    fun weiting()
}