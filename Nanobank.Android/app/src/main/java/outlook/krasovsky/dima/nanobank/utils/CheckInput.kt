package outlook.krasovsky.dima.nanobank.utils

import android.widget.EditText
import java.io.IOException
import java.util.regex.Pattern

class CheckInput {

    fun isEmpty(value: String): Boolean = value == ""

    fun isEmptyField(value: String): Boolean {
        if (!isEmpty(value)) {
            return true
        } else throw IOException("поле не может быть пустым")
    }

    fun checkLogin(value: String): Boolean {
        if (Regex("[\\w_]+").matches(value)) {
            if (!Regex("[а-яА-Я]+").matches(value)) {
                return true
            } else throw IOException("логин не может содержать буквы кирилловского алфавита")
        } else throw IOException("содержит: буквы латинского алфавита, цифры и _")
    }

    fun checkEmail(value: String): Boolean {
        if (Regex("\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*\\.\\w{2,4}").matches(value)) {
            return true
        } else throw IOException("некорректный email")
    }

    fun checkPhone(value: String): Boolean {
        if (Regex("(29)?(44)?(17)?(33)?(25)?\\d{7}").matches(value) and (value.length < 10)) {
            if (value.length == 7) {
                throw IOException("введите телефон включая код")
            }
            return true
        } else throw IOException("некорректный телефон")
    }

    fun checkPassword(repeatPasswor: EditText): (String) -> Boolean {
        return fun(value: String): Boolean {
            repeatPasswor.setText(repeatPasswor.text.toString() + "")
            if (value.length < 8) {
                throw IOException("пароль должен содержать не менее 8 символов")
            }
            if (!Pattern.compile("[A-Z]+").matcher(value).find()) {
                throw IOException("хотябы 1 большая буква латинского алфавита")
            }
            if (!Pattern.compile("[a-z]+").matcher(value).find()) {
                throw IOException("хотябы 1 маленькая буква латинского алфавита")
            }
            if (Pattern.compile("[а-яА-Я]+").matcher(value).find()) {
                throw IOException("пароль не может содержать буквы кирилловского алфавита")
            }
            if (!Pattern.compile("[0-9]+").matcher(value).find()) {
                throw IOException("хотябы 1 цифра")
            }
            if (Regex("[\\w_]+").matches(value)) {
                return true
            } else throw IOException("некорректный логи")
        }
    }

    fun checkFIO(value: String): Boolean {
        if (Regex("[а-яА-Я]+").matches(value)) {
            return true
        } else throw IOException("некорректные инициалы, только киррилица ")
    }

    fun checkRepeatPassword(edit_password: EditText): (String) -> Boolean {
        return fun(value: String): Boolean {
            val password = edit_password.text.toString()
            if (password.equals(value)) {
                return true
            } else throw IOException("пароли не совпадают")
        }
    }

    fun checkNumberCard(value: String): Boolean {
        if (Regex("\\d{16}").matches(value)) {
            return true
        } else throw IOException("некорректный номер карты");
    }

    fun checkCustomerCard(value: String): Boolean {
        if (Regex("[a-zA-z]+ [a-zA-z]+").matches(value)) {
            return true
        } else throw IOException("некорректная имя и фамилия владельца карты");
    }

    fun checkValidityPeriodCard(value: String): Boolean {
        if (Regex("\\d{2}/\\d{2}").matches(value)) {
            val firstPeriod = value.split("/")[0].toInt()
            val secondPeriod = value.split("/")[1].toInt()
            if ((firstPeriod > 0) and (firstPeriod < 13)) {
                if ((secondPeriod > 17) and (secondPeriod < 24)) {
                    return true
                } else throw IOException("год действия некорректен")
            } else throw IOException("такого месяца не существует")
        } else throw IOException("некорректный формат. mm/yy")
    }

    fun checkCVV2(value: String): Boolean {
        if (Regex("\\d{3}").matches(value)) {
            return true
        } else throw IOException("некорректный формат")
    }

    companion object {

        fun checkPutMoney(value: String): Boolean {
            try {
                if (value.toDouble() > 0) {
                    return true
                } else {
                    return false
                }
            } catch (e: Exception) {
                return false
            }
        }

        fun checkPeriod(value: String): Boolean {
            if (Regex("[1-9]{1}\\d*").matches(value) and (value.toInt() > 0) and (value.toInt() < 25)) {
                return true
            } else return false
        }

        fun checkPercentDeal(value: String): Boolean {
            try {
                if (value.toDouble() > 0) {
                    return true
                } else {
                    return false
                }
            } catch (e: Exception) {
                return false
            }
        }
    }

}