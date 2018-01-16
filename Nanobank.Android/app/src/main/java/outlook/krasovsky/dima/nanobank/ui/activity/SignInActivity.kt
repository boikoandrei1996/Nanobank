package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.Activity
import android.app.ProgressDialog
import android.content.Context
import android.content.Intent
import android.Manifest;
import android.os.Bundle
import android.support.design.widget.TextInputLayout
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_sign_in.*
import org.jetbrains.anko.*
import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.SignInPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.SignInViewInterface
import outlook.krasovsky.dima.nanobank.strategy.remote.response.SignInResponse
import outlook.krasovsky.dima.nanobank.utils.CheckInput
import java.util.*
import android.content.pm.PackageManager
import android.support.v4.app.ActivityCompat
import android.Manifest.permission.READ_PHONE_STATE


class SignInActivity : MvpAppCompatActivity(), SignInViewInterface {

    companion object {
        val LOGIN = "LOGIN"
        val TOKEN = "TOKEN"
        val TOKEN_EXPIRE = "TOKEN_EXPIRE"
        val PERMISSION = "PERMISSION"
    }

    private val ACTIVITY_REGISTRATION = 1
    private val REQUEST_READ_STORAGE = 2

    private val READ_PHONE_STATE_PERMISSION = Manifest.permission.READ_EXTERNAL_STORAGE

    @InjectPresenter
    lateinit var mPresenter: SignInPresenter

    var dialogWaiting: ProgressDialog? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_sign_in)

        if (!isPermissionGranted(READ_PHONE_STATE_PERMISSION)) {
            requestPermission(READ_PHONE_STATE_PERMISSION, REQUEST_READ_STORAGE);
        }
        getToken()
        initBtn()
    }

    private fun isPermissionGranted(permission: String): Boolean {
        val permissionCheck = ActivityCompat.checkSelfPermission(this, permission)
        return permissionCheck == PackageManager.PERMISSION_GRANTED
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        if ((requestCode == REQUEST_READ_STORAGE) and (grantResults.isNotEmpty())) {
            if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                toast("Разрешения получены")
            } else {
                toast("Разрешения не получены")
            }
        } else {
            super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    private fun requestPermission(permission: String, requestCode: Int) {
        ActivityCompat.requestPermissions(this, arrayOf(permission), requestCode)
    }

    private fun getToken() {
        val shpr = getSharedPreferences(getString(R.string.app_name), Context.MODE_PRIVATE)
        val tokenExpire = shpr.getString(TOKEN_EXPIRE, "0")
        if ((tokenExpire != "0") and (validateTokenExpire(tokenExpire))) {
            MenuActivity.login = shpr.getString(LOGIN, "")
            MenuActivity.token = shpr.getString(TOKEN, "")
            getIn()
        }
    }

    private fun validateTokenExpire(tokenExpire: String): Boolean =
            (Calendar.getInstance().timeInMillis - tokenExpire.toLong()) < 0

    private fun initBtn() {
        btn_sign_in.setOnClickListener { validateData() }
        btn_sign_up.setOnClickListener {
            startActivityForResult(
                    Intent(this, SignUpActivity::class.java), ACTIVITY_REGISTRATION)
        }
    }

    private fun validateData() {
        val login = edit_login.text.toString().trim()
        val password = edit_text_password.text.toString()

        val isValidate = checkEmpty(login, login_layout).and(checkEmpty(password, password_layout))
        mPresenter.signIn(login, password)
        if (!isValidate) {
        }
    }

    override fun writeParams(params: SignInResponse, login: String) {
        MenuActivity.token = params.access_token
        MenuActivity.login = login

        val editor = getSharedPreferences(getString(R.string.app_name), Context.MODE_PRIVATE).edit()
        editor.putString(LOGIN, login)
        editor.putString(TOKEN, params.access_token)
        editor.putString(TOKEN_EXPIRE, (Calendar.getInstance().timeInMillis + (params.expires_in * 1000)).toString())
        editor.apply()
    }

    private fun checkEmpty(values: String, layout: TextInputLayout): Boolean {
        val empty = CheckInput().isEmpty(values)
        if (empty) {
            layout.error = "пусто"
            return false
        }
        layout.error = null
        return true
    }

    override fun getIn() {
        dialogWaiting?.dismiss()
        startActivity<MenuActivity>()
        this.finish()
    }

    override fun error(message: String) {
        dialogWaiting?.dismiss()
        alert(message, "Ошибка") {
            yesButton { dialog -> dialog.dismiss() }
        }.show()
    }

    override fun weiting() {
        showDialogWaiting()
    }

    private fun showDialogWaiting() {
        dialogWaiting = indeterminateProgressDialog("подождите, соединение с сервером")
        dialogWaiting?.setCancelable(false)
        dialogWaiting?.show()
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        if (resultCode == Activity.RESULT_OK) {
            if (data != null) {
                alert(data.getStringExtra("message"), "Регистрация прошла успешно!") {
                    yesButton { dialog -> dialog.dismiss() }
                }.show()
            }
        }
    }

    override fun onDestroy() {
        super.onDestroy()
        dialogWaiting?.dismiss()
    }
}
