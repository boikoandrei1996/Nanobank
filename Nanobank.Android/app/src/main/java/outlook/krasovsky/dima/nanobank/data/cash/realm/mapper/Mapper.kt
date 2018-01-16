package outlook.krasovsky.dima.nanobank.data.cash.realm.mapper

import outlook.krasovsky.dima.nanobank.data.cash.realm.model.UserInfoModel
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel

class Mapper {
    companion object {
        fun mapUserInfo(data: PrivateDataResponse): UserInfoModel {
            val model = UserInfoModel()
            model.userName = data.userName
            model.firstName = data.firstName
            model.lastName = data.lastName
            model.patronymic = data.patronymic
            model.phoneNumber = data.phoneNumber
            model.email = data.email
            return model
        }

        fun mapUserInfo(data: SignUpModel): UserInfoModel {
            val model = UserInfoModel()
            model.userName = data.login
            model.firstName = data.first_name
            model.lastName = data.second_name
            model.patronymic = data.third_name
            model.phoneNumber = data.phone
            model.email = data.email
            return model
        }
    }
}