package outlook.krasovsky.dima.nanobank.ui.adapter

import android.support.v7.widget.RecyclerView
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.item_deposit.view.*
import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.strategy.remote.response.DepositsResponse
import outlook.krasovsky.dima.nanobank.utils.TransformData

class DepositsAdapter(private var deposits: MutableList<DepositsResponse>,
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
            val v = LayoutInflater.from(parent!!.context).inflate(R.layout.item_deposit, parent, false)
            vh = VHItem(v)
        }
        return vh
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder?, position: Int) {
        if (holder is VHItem) {
            holder.bind(deposits[position])
        }
    }

    fun removeItem(localId: String) {
        deposits = deposits.filter { it.id != localId }.toMutableList()
        notifyDataSetChanged()
    }

    inner class VHHeader(view: View) : RecyclerView.ViewHolder(view)

    inner class VHItem(val view: View) : RecyclerView.ViewHolder(view) {

        fun bind(deposit: DepositsResponse) {
            view.deposit_about.text = TransformData.transformAboutCreditOutput(deposit.title)
            view.deposit_amount.text = "${deposit.startAmount.toString()} руб."
            view.deposit_percent.text = "${deposit.percentRate.toString()}"
            view.root_deposit.setOnClickListener { details(deposit.id) }
        }
    }
}