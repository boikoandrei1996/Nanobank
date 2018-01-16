package outlook.krasovsky.dima.nanobank.ui.fragment


import android.os.Bundle
import android.support.v4.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.fragment_room.*
import org.jetbrains.anko.support.v4.startActivity

import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.ChangeCreditActivity
import outlook.krasovsky.dima.nanobank.ui.activity.CurrentDealsActivity
import outlook.krasovsky.dima.nanobank.ui.activity.HistoryCreditsActivity
import outlook.krasovsky.dima.nanobank.ui.activity.PrivateDataActivity


class RoomFragment : Fragment() {


    override fun onCreateView(inflater: LayoutInflater?, container: ViewGroup?,
                              savedInstanceState: Bundle?): View? {
        return inflater!!.inflate(R.layout.fragment_room, container, false)
    }

    override fun onViewCreated(view: View?, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        initBtn()
    }

    private fun initBtn() {
        private_data.setOnClickListener { startActivity<PrivateDataActivity>() }
        current_reauest.setOnClickListener { startActivity<CurrentDealsActivity>() }
        history_credits.setOnClickListener { startActivity<HistoryCreditsActivity>() }
    }

}

