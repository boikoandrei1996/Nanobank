package outlook.krasovsky.dima.nanobank.mvp.view

import android.graphics.Bitmap
import android.support.v4.app.Fragment
import com.arellomobile.mvp.MvpView
import com.arellomobile.mvp.viewstate.strategy.AddToEndSingleStrategy
import com.arellomobile.mvp.viewstate.strategy.SingleStateStrategy
import com.arellomobile.mvp.viewstate.strategy.SkipStrategy
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType
import outlook.krasovsky.dima.nanobank.mvp.strategy.CleanAndSkipStrategy

interface SignUpViewInterface : MvpView {
    fun weiting()

    fun responseOk(message: String)

    fun responseError(message: String)

    fun replaceFragment(fragment: Fragment)

    fun moveToBackIndicator(positionOld: Int, positionNew: Int)
    fun moveToNextIndicator(positionOld: Int, positionNew: Int)

    fun renameBtnBack(text: String)

    fun setData(fragment: Fragment)

    fun close()

    fun renameNameBtnNext(name: String)
}