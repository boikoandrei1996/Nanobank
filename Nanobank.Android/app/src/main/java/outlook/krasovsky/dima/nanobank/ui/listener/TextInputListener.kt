package outlook.krasovsky.dima.nanobank.ui.listener

import android.support.design.widget.TextInputLayout
import android.text.Editable
import android.text.TextWatcher
import android.util.Log
import android.widget.Button
import java.io.IOException

class TextInputListener(
        private val edit_layout: TextInputLayout,
        private val enableNext: (Boolean) -> Unit,
        private val validField: MutableList<Boolean>,
        private val currentField: Int,
        private val isValid: (String) -> Boolean) : TextWatcher {

    override fun afterTextChanged(p0: Editable?) {}

    override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
    }

    override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
        check(p0.toString())
    }

    private fun check(s: String) {
        try {
            if (isValid(s)) {
                edit_layout.error = null
                validField[currentField] = true
                checkValidFields()
            }
        } catch (e: IOException) {
            edit_layout.error = e.message
            validField[currentField] = false
            checkValidFields()
        }
    }

    private fun checkValidFields() {
        if (validField.all { it }) enableNext(true) else enableNext(false)
    }
}