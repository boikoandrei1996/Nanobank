package outlook.krasovsky.dima.nanobank.ui.activity

import android.app.ProgressDialog
import android.content.Context
import android.content.Intent
import android.os.Bundle
import com.arellomobile.mvp.MvpAppCompatActivity
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_complain.*
import org.jetbrains.anko.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.presenter.ComplainPresenter
import outlook.krasovsky.dima.nanobank.mvp.view.ComplainViewInterface
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login

class ComplainActivity : MvpAppCompatActivity(), ComplainViewInterface {

    var dialogWaiting: ProgressDialog? = null
    lateinit var id: String

    @InjectPresenter
    lateinit var mPresenter: ComplainPresenter

    companion object {
        val ID = "ID"

        fun getIntent(context: Context, id: String): Intent {
            val intent = Intent(context, ComplainActivity::class.java)
            intent.putExtra(ID, id)
            return intent
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_complain)

        id = intent.getStringExtra(ID)

        initBtn()
    }

    private fun initBtn() {
        btn_complain.setOnClickListener { onClickComplain() }
        btn_close_complain.setOnClickListener { onClickCancelComplain() }
    }


    private fun onClickComplain() {
        alert("Вы действительно хотите подать жалобу?", "Предупреждение") {
            yesButton {
                dialog -> dialog.dismiss()
                validateData()
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    private fun onClickCancelComplain() {
        alert("Вы действительно хотите выйти? Все изменения будут утрачены.", "Предупреждение") {
            yesButton {
                dialog -> dialog.dismiss()
                this@ComplainActivity.finish()
            }
            noButton { dialog -> dialog.dismiss() }
        }.show()
    }

    private fun validateData() {
        var text = ed_complain_text.text.toString()
        if (text == "") text = "standart complain"
        mPresenter.complain(id, text)
    }

    override fun complainOK() {
        dialogWaiting?.dismiss()
        toast("жалоба подана")
        this.finish()
    }

    override fun errorComplain(message: String) {
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

    override fun onBackPressed() {
        onClickCancelComplain()
    }

}
