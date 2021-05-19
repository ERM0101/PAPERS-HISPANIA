using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexCod_Fam = new Regex(@"^([a-zA-ZñÑ]){0,3}$"); 

        private static Regex rexValidCod_FamChars = new Regex(@"[a-z]|[A-Z]|[ñÑ]"); 

        public const string ValidationEmptyOrCod_FamError = "Format incorrecte del Cod_Fam.";

        public static bool IsEmptyOrCod_Fam(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsCod_Fam(textToValidate);
        }

        public const string ValidationCod_FamError = "Format incorrecte del Cod_Fam. Valor no definit, conté caràcters no vàlids o te més de tres caràcters.";

        public static bool IsCod_Fam(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexCod_Fam.IsMatch(textToValidate);
        }

        public static bool IsValidCod_FamChar(string textToValidate)
        {
            return rexValidCod_FamChars.IsMatch(textToValidate);
        }
    }
}
