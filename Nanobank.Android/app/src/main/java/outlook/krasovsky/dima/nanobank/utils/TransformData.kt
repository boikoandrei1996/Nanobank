package outlook.krasovsky.dima.nanobank.utils

class TransformData {
    companion object {
        fun transformAboutCreditOutput(text: String): String {
            var about = ""
            if (text.length > 70) {
                about = text.slice(IntRange(0, 70)) + "..."
            } else about = text

            return about
        }

        fun transformAboutCreditInput(text: String): String = text.replace("\n", "")
    }
}