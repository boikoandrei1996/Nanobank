package outlook.krasovsky.dima.nanobank.ui.fragment


import android.content.Context
import android.os.Bundle
import android.support.v4.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.fragment_credit_card_data.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.SignUpActivity
import outlook.krasovsky.dima.nanobank.ui.activity.intefrace.SignUpMoveInretface
import outlook.krasovsky.dima.nanobank.ui.fragment.`interface`.SignUpSaveParametor
import outlook.krasovsky.dima.nanobank.ui.listener.TextInputListener
import outlook.krasovsky.dima.nanobank.utils.CheckInput
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel


class CreditCardDataFragment : Fragment(),
        SignUpSaveParametor {

    private lateinit var listener: SignUpMoveInretface
    private var validField = mutableListOf<Boolean>()

    override fun onCreateView(inflater: LayoutInflater?, container: ViewGroup?,
                              savedInstanceState: Bundle?): View? {
        if (validField.size == 0) {
            for (item in 0 until 4) {
                validField.add(false)
            }
            listener.enableNext(false)
        }
        return inflater!!.inflate(R.layout.fragment_credit_card_data, container, false)
    }

   /* private fun initData() {
        listener.getDataCustomer().infoCard.numberCard = ""
        listener.getDataCustomer().infoCard.validityPeriodCard = ""
        listener.getDataCustomer().infoCard.customer = ""
        listener.getDataCustomer().infoCard.cvv2 = ""
    }*/

    override fun onViewCreated(view: View?, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        initView()
        //initData()
    }

    private fun initView() {
        val check = CheckInput()
        edit_number_card.addTextChangedListener(TextInputListener(number_card_layout, this::disableNext, validField, 0, check::checkNumberCard))
        edit_validity_period_card.addTextChangedListener(TextInputListener(validity_period_card_layout, this::disableNext, validField, 1, check::checkValidityPeriodCard))
        edit_customer.addTextChangedListener(TextInputListener(customer_layout, this::disableNext, validField, 2, check::checkCustomerCard))
        edit_cvv2.addTextChangedListener(TextInputListener(cvv2_layout, this::disableNext, validField, 3, check::checkCVV2))
    }

    override fun onAttach(context: Context?) {
        super.onAttach(context)
        listener = context as SignUpActivity
    }

    fun disableNext(isEnable: Boolean) {
        listener.enableNext(isEnable)
    }

    override fun saveParam(data: SignUpModel) {
        data.infoCard.numberCard = edit_number_card.text.toString()
        data.infoCard.validityPeriodCard = edit_validity_period_card.text.toString()
        data.infoCard.customer = edit_customer.text.toString()
        data.infoCard.cvv2 = edit_cvv2.text.toString()
    }

}
