package outlook.krasovsky.dima.nanobank.strategy.remote.response

import com.google.gson.annotations.SerializedName

class SignInResponse {
    @SerializedName("access_token")
    lateinit var access_token: String

    @SerializedName("token_type")
    lateinit var token_type: String

    @SerializedName("expires_in")
    var expires_in: Int = 0
}