package outlook.krasovsky.dima.nanobank.strategy.remote.retrofit

import okhttp3.RequestBody
import okhttp3.ResponseBody
import outlook.krasovsky.dima.nanobank.strategy.remote.request.PutMoneyModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RatingModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterDealModel
import outlook.krasovsky.dima.nanobank.strategy.remote.request.RegisterModel
import retrofit2.Call
import retrofit2.http.*

interface ApiServerRequest {
    @Headers("content-type: application/json", "accept: application/json")
    @POST("account/register")
    @Streaming
    fun register(@Body registerData: RegisterModel): Call<ResponseBody>

    @FormUrlEncoded
    @Headers("content-type: application/x-www-form-urlencoded", "accept: application/json")
    @POST("token")
    fun signIn(@Field("grant_type") type: String,
               @Field("username") username: String,
               @Field("password") password: String): Call<ResponseBody>

    @FormUrlEncoded
    @Headers("content-type: application/x-www-form-urlencoded", "accept: application/json")
    @POST("complain/add")
    fun complain(@Field("DealId") dealId: String,
                 @Field("ComplainText") complainText: String,
                 @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("content-type: application/json", "accept: application/json")
    @POST("deal/register")
    fun registerDeal(@Body registerDeal: RegisterDealModel, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("content-type: application/json", "accept: application/json")
    @PUT("deal/{id}")
    fun changeDeal(@Path("id") id: String, @Body changeDeal: RegisterDealModel, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("content-type: application/json", "accept: application/json")
    @PUT("deal/{id}/set/rating")
    fun setRating(@Path("id") id: String, @Body model: RatingModel, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("content-type: application/json", "accept: application/json")
    @PUT("creditcard/transit")
    fun transit(@Body model: PutMoneyModel, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("content-type: application/json", "accept: application/json")
    @PUT("deal/respond/{id}")
    fun respond(@Path("id") id: String, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("content-type: application/json", "accept: application/json")
    @PUT("deal/close/{id}")
    fun closeDeal(@Path("id") id: String, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("Content-Type: application/json", "Accept: application/json")
    @GET("deal/all/opened")
    fun getDeposits(@Header("Authorization") token: String): Call<ResponseBody>

    @Headers("Content-Type: application/json", "Accept: application/json")
    @GET("user/{username}")
    fun getUser(@Path("username") username: String, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("Content-Type: application/json", "Accept: application/json")
    @GET("deal/{username}/all")
    fun getDealsOfUser(@Path("username") username: String, @Header("Authorization") token: String): Call<ResponseBody>

    @Headers("Content-Type: application/json", "Accept: application/json")
    @GET("deal/{id}")
    fun getDeposit(@Path("id") id: String, @Header("Authorization") token: String): Call<ResponseBody>

}