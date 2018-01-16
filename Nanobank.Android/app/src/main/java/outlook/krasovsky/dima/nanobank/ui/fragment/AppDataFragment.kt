package outlook.krasovsky.dima.nanobank.ui.fragment


import android.content.Context
import android.os.Bundle
import android.support.v4.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.fragment_app_data.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.SignUpActivity
import outlook.krasovsky.dima.nanobank.ui.activity.intefrace.SignUpMoveInretface
import outlook.krasovsky.dima.nanobank.ui.fragment.`interface`.SignUpSaveParametor
import outlook.krasovsky.dima.nanobank.ui.listener.TextInputListener
import outlook.krasovsky.dima.nanobank.utils.CheckInput
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel

class AppDataFragment : Fragment(),
        SignUpSaveParametor {

    private lateinit var listener: SignUpMoveInretface
    private var validField = mutableListOf<Boolean>()

    override fun onCreateView(inflater: LayoutInflater?, container: ViewGroup?,
                              savedInstanceState: Bundle?): View? {
        if (validField.size == 0) {
            for (item in 0 until 3) {
                validField.add(false)
            }
            listener.enableNext(false)
        }
        return inflater!!.inflate(R.layout.fragment_app_data, container, false)
    }

    override fun onViewCreated(view: View?, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        initView()
    }

    private fun initView() {
        val check = CheckInput()
        edit_login.addTextChangedListener(TextInputListener(login_layout, this::enableNext, validField, 0, check::checkLogin))

        edit_password.addTextChangedListener(TextInputListener(password_layout, this::enableNext, validField, 1, check.checkPassword(edit_password_repeat)))
        edit_password_repeat.addTextChangedListener(TextInputListener(password_repeat_layout, this::enableNext, validField, 2, check.checkRepeatPassword(edit_password)))
    }


    override fun onAttach(context: Context?) {
        super.onAttach(context)
        listener = context as SignUpActivity
    }

    fun enableNext(isEnable: Boolean) {
        listener.enableNext(isEnable)
    }

    override fun saveParam(data: SignUpModel) {
        data.login = edit_login.text.toString()
        data.password = edit_password.text.toString()
        data.password_confirm = edit_password_repeat.text.toString()
    }

}
