package outlook.krasovsky.dima.nanobank.strategy.remote.response

import com.google.gson.annotations.SerializedName

class DepositsResponse {

    @SerializedName("id")
    lateinit var id: String

    @SerializedName("title")
    lateinit var title: String

    @SerializedName("startAmount")
    var startAmount: Double = 0.0

    @SerializedName("returnedAmount")
    var returnedAmount: Double? = 0.0

    @SerializedName("dealDurationInMonth")
    var dealDurationInMonth: Int = 0

    @SerializedName("percentRate")
    var percentRate: Double = 0.0

    @SerializedName("ownerUserName")
    lateinit var ownerUserName: String

    @SerializedName("creditorUserName")
    var creditorUserName: String? = null

    @SerializedName("dealStartDate")
    lateinit var dealStartDate: String

    @SerializedName("dealClosedDate")
    var dealClosedDate: String? = null

    @SerializedName("isClosed")
    var isClosed: Boolean = false

    @SerializedName("ratingPositive")
    var ratingPositive: Int? = null

    @SerializedName("ratingNegative")
    var ratingNegative: Int? = null
}