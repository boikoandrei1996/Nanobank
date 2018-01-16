package outlook.krasovsky.dima.nanobank.strategy.remote.request

import com.google.gson.annotations.SerializedName
import outlook.krasovsky.dima.nanobank.utils.model.SignUpCardModel

class RegisterModel {
    @SerializedName("username")
    lateinit var login: String

    @SerializedName("password")
    lateinit var password: String

    @SerializedName("confirmPassword")
    lateinit var password_confirm: String

    @SerializedName("email")
    lateinit var email: String

    @SerializedName("phoneNumber")
    lateinit var phone: String

    @SerializedName("firstName")
    lateinit var first_name: String

    @SerializedName("lastName")
    lateinit var second_name: String

    @SerializedName("patronymic")
    lateinit var third_name: String

    @SerializedName("cardNumber")
    lateinit var numberCard: String

    @SerializedName("cardDateOfExpire")
    lateinit var validityPeriodCard: String

    @SerializedName("cardOwnerFullName")
    lateinit var customer: String

    @SerializedName("cardCVV2Key")
    lateinit var cvv2: String

    @SerializedName("FCMPushNotificationToken")
    lateinit var tokenGCM: String

    @SerializedName("PassportImage")
    lateinit var passport: String

    @SerializedName("ImageMimeType")
    lateinit var passport_type: String

}