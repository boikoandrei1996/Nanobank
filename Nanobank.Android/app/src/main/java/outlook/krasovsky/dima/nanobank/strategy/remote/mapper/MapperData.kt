package outlook.krasovsky.dima.nanobank.strategy.remote.mapper

import android.util.Log
import okhttp3.MediaType
import okhttp3.RequestBody
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterModel
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel

class MapperData {
    companion object {
        fun mapSignUpData(data: SignUpModel, tokenGCM: String): RegisterModel {
            val body = RegisterModel()
            body.login = data.login
            body.password = data.password
            body.password_confirm = data.password_confirm
            body.first_name = data.first_name
            body.second_name = data.second_name
            body.third_name = data.third_name
            body.email = data.email
            body.phone = data.phone
            body.numberCard = data.infoCard.numberCard
            body.validityPeriodCard = data.infoCard.validityPeriodCard
            body.customer = data.infoCard.customer
            body.cvv2 = data.infoCard.cvv2
            body.passport = data.pasport
            body.passport_type = data.pasport_type
            body.tokenGCM = tokenGCM
            return body
        }
    }
}