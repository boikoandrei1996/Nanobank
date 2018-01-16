package outlook.krasovsky.dima.nanobank.utils.model

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class SignUpCardModel {

    @SerializedName("number_card")
    lateinit var numberCard: String

    @SerializedName("validity_period_card")
    lateinit var validityPeriodCard: String

    @SerializedName("customer")
    lateinit var customer: String

    @SerializedName("cvv2")
    lateinit var cvv2: String

}