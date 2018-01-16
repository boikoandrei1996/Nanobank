package outlook.krasovsky.dima.nanobank.strategy.remote.request

import com.google.gson.annotations.SerializedName

class SignInModel {
    @SerializedName("grant_type")
    private val grant_type: String = "password"

    @SerializedName("username")
    lateinit var username: String

    @SerializedName("password")
    lateinit var password: String

}