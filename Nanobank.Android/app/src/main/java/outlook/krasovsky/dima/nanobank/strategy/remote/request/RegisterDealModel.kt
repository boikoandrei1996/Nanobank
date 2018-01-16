package outlook.krasovsky.dima.nanobank.strategy.remote.request

import com.google.gson.annotations.SerializedName
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.token

class RegisterDealModel {

    @SerializedName("title")
    lateinit var title: String

    @SerializedName("startAmount")
    var startAmount: Double = 0.0

    @SerializedName("DealDurationInMonth")
    var dealDurationMonth: Int = 0

    @SerializedName("percentRate")
    var percentRate: Double = 0.0

}