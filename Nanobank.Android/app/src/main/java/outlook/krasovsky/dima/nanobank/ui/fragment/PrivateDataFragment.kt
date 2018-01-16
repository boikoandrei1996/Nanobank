package outlook.krasovsky.dima.nanobank.ui.fragment


import android.content.Context
import android.os.Bundle
import android.support.v4.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.fragment_private_data.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.SignUpActivity
import outlook.krasovsky.dima.nanobank.ui.activity.intefrace.SignUpMoveInretface
import outlook.krasovsky.dima.nanobank.ui.fragment.`interface`.SignUpSaveParametor
import outlook.krasovsky.dima.nanobank.ui.listener.TextInputListener
import outlook.krasovsky.dima.nanobank.utils.CheckInput
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel


class PrivateDataFragment : Fragment(),
        SignUpSaveParametor {

    private lateinit var listener: SignUpMoveInretface
    private var validField = mutableListOf<Boolean>()

    override fun onCreateView(inflater: LayoutInflater?, container: ViewGroup?,
                              savedInstanceState: Bundle?): View? {
        if (validField.size == 0) {
            for (item in 0 until 5) {
                validField.add(false)
            }
            listener.enableNext(false)
        }
        return inflater!!.inflate(R.layout.fragment_private_data, container, false)
    }

    override fun onViewCreated(view: View?, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        initView()
    }

    private fun initView() {
        val check = CheckInput()
        edit_first_name.addTextChangedListener(TextInputListener(first_name_layout, this::enableNext, validField, 0, check::checkFIO))
        edit_second_name.addTextChangedListener(TextInputListener(second_name_layout, this::enableNext, validField, 1, check::checkFIO))
        edit_third_name.addTextChangedListener(TextInputListener(third_name_layout, this::enableNext, validField, 2, check::checkFIO))
        edit_email.addTextChangedListener(TextInputListener(email_layout, this::enableNext, validField, 3, check::checkEmail))
        edit_phone.addTextChangedListener(TextInputListener(phone_layout, this::enableNext, validField, 4, check::checkPhone))
    }

    override fun onAttach(context: Context?) {
        super.onAttach(context)
        listener = context as SignUpActivity
    }

    fun enableNext(isEnable: Boolean) {
        listener.enableNext(isEnable)
    }

    override fun saveParam(data: SignUpModel) {
        data.first_name = edit_first_name.text.toString()
        data.second_name = edit_second_name.text.toString()
        data.third_name = edit_third_name.text.toString()
        data.email = edit_email.text.toString()
        data.phone = edit_phone.text.toString()
    }
}
