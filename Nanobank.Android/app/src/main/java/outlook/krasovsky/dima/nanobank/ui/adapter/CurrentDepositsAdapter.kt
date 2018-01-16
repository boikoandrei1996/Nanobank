package outlook.krasovsky.dima.nanobank.ui.adapter

import android.support.v7.widget.RecyclerView
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.item_current_deposit.view.*
import kotlinx.android.synthetic.main.layout_current_creditor.view.*
import kotlinx.android.synthetic.main.layout_current_owner.view.*
import kotlinx.android.synthetic.main.layout_current_wait.view.*
import org.jetbrains.anko.backgroundResource
import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.R.id.viewStub_wait
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.ui.activity.MenuActivity.Companion.login
import outlook.krasovsky.dima.nanobank.utils.TransformData

class CurrentDepositsAdapter(private var deposits: MutableList<DepositsResponse>,
                             private val details: (String) -> Unit) :
        RecyclerView.Adapter<RecyclerView.ViewHolder>() {

    enum class Type(val type: Int) {
        HEADER(0), ITEM(1)
    }

    override fun getItemCount(): Int = if (deposits.size == 0) 1 else deposits.size

    override fun getItemViewType(position: Int): Int {
        return if (position == 0) {
            if (deposits.size == 0) {
                Type.HEADER.type
            } else {
                Type.ITEM.type
            }
        } else {
            Type.ITEM.type
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup?, viewType: Int): RecyclerView.ViewHolder {
        var vh: RecyclerView.ViewHolder? = null
        if (viewType == Type.HEADER.type) {
            val v = LayoutInflater.from(parent!!.context).inflate(R.layout.empty_deposit, parent, false)
            vh = VHHeader(v)
        } else {
            val v = LayoutInflater.from(parent!!.context).inflate(R.layout.item_current_deposit, parent, false)
            vh = VHItem(v)
        }
        return vh
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder?, position: Int) {
        if (holder is VHItem) {
            holder.bind(deposits[position])
        }
    }

    inner class VHHeader(view: View) : RecyclerView.ViewHolder(view)

    inner class VHItem(val view: View) : RecyclerView.ViewHolder(view) {

        fun bind(deposit: DepositsResponse) {
            if (deposit.creditorUserName == null) {
                setVisible(View.VISIBLE, View.GONE, View.GONE)
                view.line_current.backgroundResource = R.color.colorWaitOwner
                view.root_current_deposit_about.text = TransformData.transformAboutCreditOutput(deposit.title)
                view.current_deposit_amount.text = "${deposit.startAmount} руб."
                view.current_deposit_period.text = "${deposit.dealDurationInMonth} мес."
                view.current_deposit_percent.text = "${deposit.percentRate}"
            } else {
                if (deposit.ownerUserName == login) {
                    setVisible(View.GONE, View.VISIBLE, View.GONE)
                    view.line_current.backgroundResource = R.color.colorOwner
                    view.owner_about.text = TransformData.transformAboutCreditOutput(deposit.title)
                    view.current_deposit_owner_start.text = deposit.dealStartDate.split("T")[0]
                    view.current_deposit_owner_period.text = "${deposit.dealDurationInMonth} мес."
                    view.current_deposit_owner_percent.text = deposit.percentRate.toString()
                } else {
                    setVisible(View.GONE, View.GONE, View.VISIBLE)
                    view.line_current.backgroundResource = R.color.colorCreditor
                    view.creditor_about.text = TransformData.transformAboutCreditOutput(deposit.title)
                    view.current_deposit_creditor_start.text = deposit.dealStartDate.split("T")[0]
                    view.current_deposit_creditor_period.text = "${deposit.dealDurationInMonth} мес."
                }
            }

            view.current_root_deposit.setOnClickListener { details(deposit.id) }
        }

        private fun setVisible(wait: Int, owner: Int, creditor: Int) {
            view.viewStub_wait.visibility = wait
            view.viewStub_owner.visibility = owner
            view.viewStub_creditor.visibility = creditor
        }
    }
}