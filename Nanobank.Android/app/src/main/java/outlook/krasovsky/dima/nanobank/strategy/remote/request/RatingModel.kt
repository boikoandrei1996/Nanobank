package outlook.krasovsky.dima.nanobank.strategy.remote.request

import com.google.gson.annotations.SerializedName
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity

class RatingModel {
    @SerializedName("positive")
    var positive: Int = 0

    @SerializedName("negative")
    var negative: Int = 0

}