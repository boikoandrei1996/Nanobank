package outlook.krasovsky.dima.nanobank.ui.activity

import android.content.Context
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import kotlinx.android.synthetic.main.activity_menu.*
import org.jetbrains.anko.startActivity

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.SignInActivity.Companion.LOGIN
import outlook.krasovsky.dima.nanobank.ui.activity.SignInActivity.Companion.TOKEN
import outlook.krasovsky.dima.nanobank.ui.activity.SignInActivity.Companion.TOKEN_EXPIRE
import outlook.krasovsky.dima.nanobank.ui.fragment.DepositsFragment
import outlook.krasovsky.dima.nanobank.ui.fragment.FeedbackFragment
import outlook.krasovsky.dima.nanobank.ui.fragment.RequestFragment
import outlook.krasovsky.dima.nanobank.ui.fragment.RoomFragment
import java.util.zip.Inflater

class MenuActivity : AppCompatActivity() {

    companion object {
        var token: String? = null
        var login: String? = null
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_menu)
        Log.e("MYLOG", "token = ${token}")
        initBtn()

        if (savedInstanceState == null)
            initView()

    }

    private fun initView() {
        bottom_navigation.selectedItemId = R.id.action_request
    }

    private fun initBtn() {
        bottom_navigation.setOnNavigationItemSelectedListener { item ->
            val ft = supportFragmentManager.beginTransaction()

            when (item.itemId) {
                R.id.action_deposits ->
                    ft.replace(R.id.fl_root, DepositsFragment())
                R.id.action_request ->
                    ft.replace(R.id.fl_root, RequestFragment())
                R.id.action_room ->
                    ft.replace(R.id.fl_root, RoomFragment())
                R.id.action_feedback ->
                    ft.replace(R.id.fl_root, FeedbackFragment())
            }

            ft.commit()
            return@setOnNavigationItemSelectedListener true
        }
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.menu_navigation, menu)
        return super.onCreateOptionsMenu(menu)
    }

    override fun onOptionsItemSelected(item: MenuItem?): Boolean {
        when (item?.itemId) {
            R.id.action_exit -> exit()
        }
        return super.onOptionsItemSelected(item)
    }

    private fun exit() {
        clearParams()
        startActivity<SignInActivity>()
        this.finish()
    }

    private fun clearParams() {
        token = null
        login = null

        val editor = getSharedPreferences(getString(R.string.app_name), Context.MODE_PRIVATE).edit()
        editor.putString(LOGIN, "")
        editor.putString(TOKEN, "")
        editor.putString(TOKEN_EXPIRE, "0")
        editor.apply()
    }
}
