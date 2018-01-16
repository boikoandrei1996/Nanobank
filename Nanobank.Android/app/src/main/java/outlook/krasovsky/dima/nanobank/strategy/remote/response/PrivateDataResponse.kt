package outlook.krasovsky.dima.nanobank.strategy.remote.response

import com.google.gson.annotations.SerializedName

class PrivateDataResponse {

    @SerializedName("userName")
    lateinit var userName: String

    @SerializedName("email")
    lateinit var email: String

    @SerializedName("phoneNumber")
    lateinit var phoneNumber: String

    @SerializedName("firstName")
    lateinit var firstName: String

    @SerializedName("lastName")
    lateinit var lastName: String

    @SerializedName("patronymic")
    lateinit var patronymic: String

    @SerializedName("ratingPositive")
    var ratingPositive: Int? = 0

    @SerializedName("ratingNegative")
    var ratingNegative: Int? = 0

    @SerializedName("cardNumber")
    lateinit var cardNumber: String

}