package outlook.krasovsky.dima.nanobank.strategy.remote.cash

import io.realm.Realm
import android.util.Log
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import io.realm.RealmResults
import okhttp3.ResponseBody
import org.json.JSONObject
import outlook.krasovsky.dima.nanobank.data.cash.realm.mapper.Mapper
import outlook.krasovsky.dima.nanobank.data.cash.realm.model.UserInfoModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.SignInModel
import outlook.krasovsky.dima.nanobank.strategy.remote.mapper.MapperData.Companion.mapSignUpData
import outlook.krasovsky.dima.nanobank.strategy.remote.request.PutMoneyModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RatingModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterDealModel
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.response.SignInResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiHelper
import outlook.krasovsky.dima.nanobank.strategy.remote.retrofit.ApiManager
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel
import retrofit2.Call
import retrofit2.Response
import rx.Observable
import rx.Observer
import rx.Subscriber
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers

class Cash {

    fun getUserInfo(login: String): Observable<UserInfoModel> {
        return Observable.create { t: Subscriber<in UserInfoModel>? ->
            Realm.getDefaultInstance().executeTransaction { realm: Realm? ->
                val model = realm!!.where(UserInfoModel::class.java).equalTo("userName", login).findFirst()
                if (model == null) {
                    val api = ApiManager(ApiHelper())
                    api.getUser(login)
                            .subscribe(object : Subscriber<PrivateDataResponse>() {
                                override fun onNext(data: PrivateDataResponse?) {
                                    val modelResponse = Mapper.mapUserInfo(data!!)
                                    realm.copyToRealm(modelResponse)
                                    t!!.onNext(modelResponse)
                                }

                                override fun onCompleted() {
                                }

                                override fun onError(e: Throwable?) {
                                    t!!.onError(e)
                                }
                            })
                } else {
                    val data = realm.copyFromRealm(model)
                    t!!.onNext(data)
                    t.onCompleted()
                }
            }
        }
    }

    fun saveUserInfo(model: SignUpModel) {
        Realm.getDefaultInstance().executeTransactionAsync { realm: Realm? ->
            val modelResponse = Mapper.mapUserInfo(model)
            realm!!.copyToRealm(modelResponse)
        }
    }
}