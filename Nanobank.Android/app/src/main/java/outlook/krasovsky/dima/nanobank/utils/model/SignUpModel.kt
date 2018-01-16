package outlook.krasovsky.dima.nanobank.utils.model

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName
import java.io.File

class SignUpModel {

    lateinit var login: String

    lateinit var password: String

    lateinit var password_confirm: String

    lateinit var first_name: String

    lateinit var second_name: String

    lateinit var third_name: String

    lateinit var email: String

    lateinit var phone: String

    var infoCard: SignUpCardModel = SignUpCardModel()

    lateinit var pasport: String

    val pasport_type: String = "image/png"

}