package outlook.krasovsky.dima.nanobank.mvp.strategy

import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.ViewCommand
import com.arellomobile.mvp.viewstate.strategy.StateStrategy

class CleanAndSkipStrategy : StateStrategy {

    override fun <View : MvpView> beforeApply(currentState: MutableList<ViewCommand<View>>, incomingCommand: ViewCommand<View>) {
        if (currentState.size != 0) {
            currentState.removeAt(currentState.size - 1)
        }
        //do nothing to skip
    }

    override fun <View : MvpView> afterApply(currentState: List<ViewCommand<View>>, incomingCommand: ViewCommand<View>) {
        // pass
    }
}