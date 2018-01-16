package outlook.krasovsky.dima.nanobank.strategy.remote.request

import com.google.gson.annotations.SerializedName
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity

class PutMoneyModel {
    @SerializedName("DealId")
    lateinit var DealId: String

    @SerializedName("Amount")
    lateinit var Amount: String

}