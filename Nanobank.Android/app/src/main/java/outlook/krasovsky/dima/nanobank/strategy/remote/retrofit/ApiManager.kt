package outlook.krasovsky.dima.nanobank.strategy.remote.retrofit

import android.util.Log
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.MediaType
import okhttp3.RequestBody
import okhttp3.ResponseBody
import org.json.JSONObject
import outlook.krasovsky.dima.nanobank.strategy.remote.mapper.MapperData
import outlook.krasovsky.dima.nanobank.strategy.remote.request.SignInModel
import outlook.krasovsky.dima.nanobank.strategy.remote.mapper.MapperData.Companion.mapSignUpData
import outlook.krasovsky.dima.nanobank.strategy.remote.request.PutMoneyModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RatingModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterDealModel
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.response.PrivateDataResponse
import outlook.krasovsky.dima.nanobank.strategy.remote.response.SignInResponse
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.token
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel
import retrofit2.Call
import retrofit2.Response
import rx.Observable
import rx.Subscriber


class ApiManager(private val apiHelper: ApiHelper) {

    fun registry(data: SignUpModel, tokenGCM: String): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().register(MapperData.mapSignUpData(data, tokenGCM))
                var response: Response<ResponseBody>? = null
                var retryPolice = 5
                var flagEOF = false
                do {
                    try {
                        response = request.execute()
                        break
                    } catch (e: Exception) {
                        if (e.message.equals("Already executed.")) {
                            flagEOF = true
                            break
                        }
                        retryPolice -= 1
                        e.printStackTrace()
                    }
                } while (retryPolice != 0)

                if ((response != null) or (flagEOF)) {
                    if ((response?.isSuccessful?: false) or (flagEOF)) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response?.errorBody()?.string()?: "")))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun signIn(data: SignInModel): Observable<SignInResponse> {
        return Observable.create { t: Subscriber<in SignInResponse>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().signIn("password", data.username, data.password)
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext(Gson().fromJson(response.body().string(), SignInResponse::class.java))
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(JSONObject(response.errorBody().string()).getString("error_description")))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun complain(dealId: String, complainText: String): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().complain(dealId, complainText, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun registerDeal(data: RegisterDealModel): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().registerDeal(data, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun getDeposits(): Observable<MutableList<DepositsResponse>> {
        return Observable.create { t: Subscriber<in MutableList<DepositsResponse>>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().getDeposits("Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext(Gson().fromJson<MutableList<DepositsResponse>>(response.body().string()))
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                e.printStackTrace()
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun getDealsOfUser(username: String): Observable<MutableList<DepositsResponse>> {
        return Observable.create { t: Subscriber<in MutableList<DepositsResponse>>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().getDealsOfUser(username, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        val jsonObject = JSONObject(response.body().string())
                        val jsonAsOwnerDeposits = jsonObject.getString("asOwner")
                        val asOwnerDeposits = Gson().fromJson<MutableList<DepositsResponse>>(jsonAsOwnerDeposits)

                        val jsonasCreditorDeposits = jsonObject.getString("asCreditor")
                        val asCreditorDeposits = Gson().fromJson<MutableList<DepositsResponse>>(jsonasCreditorDeposits)
                        asCreditorDeposits.addAll(asOwnerDeposits)

                        t!!.onNext(asCreditorDeposits)
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    inline fun <reified T> Gson.fromJson(json: String) = this.fromJson<T>(json, object : TypeToken<T>() {}.type)

    fun getDeposit(id: String): Observable<DepositsResponse> {
        return Observable.create { t: Subscriber<in DepositsResponse>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().getDeposit(id, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext(Gson().fromJson(response.body().string(), DepositsResponse::class.java))
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun changeDeal(id: String, model: RegisterDealModel): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().changeDeal(id, model, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun closeDeal(id: String): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().closeDeal(id, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun putMoney(id: String, amount: String): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                val model = PutMoneyModel()
                model.DealId = id
                model.Amount = amount
                request = apiHelper.getApi().transit(model, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun setRating(id: String, pos: Float, neg: Float): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                val model = RatingModel()
                model.positive = pos.toInt()
                model.negative = neg.toInt()
                request = apiHelper.getApi().setRating(id, model, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }


    fun getUser(username: String): Observable<PrivateDataResponse> {
        return Observable.create { t: Subscriber<in PrivateDataResponse>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().getUser(username, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext(Gson().fromJson(response.body().string(), PrivateDataResponse::class.java))
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    fun setRespond(id: String): Observable<String> {
        return Observable.create { t: Subscriber<in String>? ->
            var request: Call<ResponseBody>? = null
            try {
                request = apiHelper.getApi().respond(id, "Bearer ${token!!}")
                val response = sendRequest(request)
                if (response != null) {
                    if (response.isSuccessful) {
                        t!!.onNext("")
                        t.onCompleted()
                    } else {
                        try {
                            t!!.onError(Exception(parceError(response.errorBody().string())))
                        } catch (e: Exception) {
                            t!!.onError(Exception("Ошибка"))
                        }
                    }

                } else {
                    t!!.onError(Exception("Проверьте интенет соединение"))
                }
            } catch (e: Exception) {
                t!!.onError(Exception("Проверьте интенет соединение"))
            }
        }
    }

    private fun sendRequest(request: Call<ResponseBody>): Response<ResponseBody>? {
        var retryPolice = 5
        do {
            try {
                return request.execute()
            } catch (e: Exception) {
                retryPolice -= 1
                e.printStackTrace()
            }
        } while (retryPolice != 0)

        return null
    }

    private fun parceError(error: String): String {
        var message = ""
        val modelState = JSONObject(error).getString("modelState")
        val jsonState = JSONObject(modelState)
        val fields = jsonState.keys()
        for (field in fields) {
            message += jsonState.getString(field)
        }
        return message
    }
}