package outlook.krasovsky.dima.nanobank.ui.fragment


import android.content.Context
import android.os.Bundle
import android.support.v4.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.CheckBox
import kotlinx.android.synthetic.main.fragment_contract_data.*

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.SignUpActivity
import outlook.krasovsky.dima.nanobank.ui.activity.intefrace.SignUpMoveInretface


class ContractDataFragment : Fragment() {

    private lateinit var listener: SignUpMoveInretface
    private var flag = false

    override fun onCreateView(inflater: LayoutInflater?, container: ViewGroup?,
                              savedInstanceState: Bundle?): View? {
        return inflater!!.inflate(R.layout.fragment_contract_data, container, false)
    }

    override fun onViewCreated(view: View?, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        listener.enableNext(flag)
        initView()
    }

    private fun initView() {
        chb_contract.setOnClickListener { view -> checkChBox((view as CheckBox).isChecked) }
    }

    private fun checkChBox(isCheched: Boolean) {
        listener.enableNext(isCheched)
        flag = !flag
    }

    override fun onAttach(context: Context?) {
        super.onAttach(context)
        listener = context as SignUpActivity
    }

}
