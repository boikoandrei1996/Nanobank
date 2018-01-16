package outlook.krasovsky.dima.nanobank.data.cash.realm.model

import io.realm.RealmObject
import io.realm.annotations.PrimaryKey

open class UserInfoModel : RealmObject() {

    @PrimaryKey
    lateinit var userName: String

    lateinit var email: String

    lateinit var phoneNumber: String

    lateinit var firstName: String

    lateinit var lastName: String

    lateinit var patronymic: String
}