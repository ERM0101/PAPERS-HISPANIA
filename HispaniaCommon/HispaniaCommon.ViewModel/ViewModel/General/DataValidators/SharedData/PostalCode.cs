using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexPostalCode = new Regex(@"(^(\.|-)$)|(^(\.)*([a-zA-Z0-9](\.|-)*){1,8}$)|(^([a-zA-Z0-9](\.|-)*)$)");

        private static Regex rexValidPostalCodeChars = new Regex(@"[a-zA-Z]|[0-9]|\.|[-]");

        public const string ValidationEmptyOrPostalCodeError = "Format incorrecte de Codi Postal.";

        public static bool IsEmptyOrPostalCode(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsPostalCode(textToValidate);
        }

        public const string ValidationPostalCodeError = "Format incorrecte del Codi Postal. Valor no definit o format incorrecte.";

        public static bool IsPostalCode(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexPostalCode.IsMatch(textToValidate);
        }

        public static bool IsValidPostalCodeChar(string textToValidate)
        {
            return rexValidPostalCodeChars.IsMatch(textToValidate);
        }
    }
}
