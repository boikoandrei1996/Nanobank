package outlook.krasovsky.dima.nanobank.ui.activity

import android.os.Bundle
import com.arellomobile.mvp.MvpAppCompatActivity

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.mvp.view.SignUpViewInterface
import com.arellomobile.mvp.presenter.InjectPresenter
import kotlinx.android.synthetic.main.activity_sign_up.*
import android.app.ProgressDialog
import android.content.Intent
import android.support.v4.app.Fragment
import org.jetbrains.anko.*
import outlook.krasovsky.dima.nanobank.mvp.presenter.SignUpPresenter
import outlook.krasovsky.dima.nanobank.ui.activity.intefrace.SignUpMoveInretface
import outlook.krasovsky.dima.nanobank.ui.fragment.`interface`.SignUpSaveParametor
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel


class SignUpActivity : MvpAppCompatActivity(), SignUpViewInterface,
        SignUpMoveInretface {

    @InjectPresenter
    lateinit var mPresenter: SignUpPresenter

    var dialogWaiting: ProgressDialog? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_sign_up)

        initBtn()
    }

    private fun initBtn() {
        btn_sign_up_back.setOnClickListener { mPresenter.moveBack() }
        btn_sign_up_next.setOnClickListener { mPresenter.moveNext(applicationContext) }
    }

    override fun getDataCustomer(): SignUpModel = mPresenter.getData()

    override fun replaceFragment(fragment: Fragment) {
        val ft = supportFragmentManager.beginTransaction()
        ft.replace(R.id.fl_sign_up, fragment)
        ft.commit()
    }

    override fun setData(fragment: Fragment) {
        (fragment as SignUpSaveParametor).saveParam(mPresenter.getData())
    }

    override fun moveToNextIndicator(positionOld: Int, positionNew: Int) {
        root_indicator.getChildAt(positionOld).backgroundResource = R.color.signUpScreenDone
        root_indicator.getChildAt(positionNew).backgroundResource = R.color.signUpScreenCurrent
    }

    override fun moveToBackIndicator(positionOld: Int, positionNew: Int) {
        root_indicator.getChildAt(positionOld).backgroundResource = R.color.signUpScreenFuture
        root_indicator.getChildAt(positionNew).backgroundResource = R.color.signUpScreenCurrent
    }

    override fun renameBtnBack(text: String) {
        btn_sign_up_back.text = text
    }

    override fun close() {
        this.finish()
    }

    override fun enableNext(isEnable: Boolean) {
        btn_sign_up_next.isEnabled = isEnable
    }

    override fun renameNameBtnNext(name: String) {
        btn_sign_up_next.text = name
    }

    override fun weiting() {
        showDialogWaiting()
    }

    override fun responseError(message: String) {
        dialogWaiting?.dismiss()
        alert(message, "Ошибка") {
            yesButton { dialog -> dialog.dismiss() }
        }.show()
    }

    override fun responseOk(message: String) {
        dialogWaiting?.dismiss()

        setResult(RESULT_OK, generateIntentResult(message))
        finish()
    }

    private fun generateIntentResult(message: String): Intent {
        val intent = Intent()
        intent.putExtra("message", message)
        return intent
    }

    private fun showDialogWaiting() {
        dialogWaiting = indeterminateProgressDialog("подождите, соединение с сервером")
        dialogWaiting?.setCancelable(false)
        dialogWaiting?.show()
    }

    override fun onDestroy() {
        super.onDestroy()
        dialogWaiting?.dismiss()
    }

}