package outlook.krasovsky.dima.nanobank.ui.activity.intefrace

import android.support.v4.app.Fragment
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel

interface SignUpMoveInretface {
    fun getDataCustomer(): SignUpModel
    fun enableNext(isEnable: Boolean)
}