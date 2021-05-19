using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexGoodCode = new Regex(@"^([a-zA-ZñÑ0-9]){1,10}$"); 

        private static Regex rexValidGoodCodeChars = new Regex(@"[a-z]|[A-Z]|[ñÑ]|[0-9]"); 

        public const string ValidationEmptyOrGoodCodeError = "Format incorrecte del Còdi d'Article.";

        public static bool IsEmptyOrGoodCode(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsGoodCode(textToValidate);
        }

        public const string ValidationGoodCodeError = "Format incorrecte del Còdi d'Article. Valor no definit, conté caràcters no vàlids o te més de tres caràcters.";

        public static bool IsGoodCode(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexGoodCode.IsMatch(textToValidate);
        }

        public static bool IsValidGoodCodeChar(string textToValidate)
        {
            return rexValidGoodCodeChars.IsMatch(textToValidate);
        }
    }
}
