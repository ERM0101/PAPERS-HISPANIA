using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexIBAN_CheckDigits = new Regex(@"^[0-9]{2}?$");

        private static Regex rexValidIBAN_CheckDigitsChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrIBAN_CheckDigitsError = "Format incorrecte dels dígits de control de l'IBAN.";

        public static bool IsEmptyOrIBAN_CheckDigits(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsIBAN_CheckDigits(textToValidate);
        }

        public const string ValidationIBAN_CheckDigitsError = "Format incorrecte dels dígits de control de l'IBAN. Valor no definit o format incorrecte.";

        public static bool IsIBAN_CheckDigits(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexIBAN_CheckDigits.IsMatch(textToValidate);
        }

        public static bool IsValidIBAN_CheckDigitsChar(string textToValidate)
        {
            return rexValidIBAN_CheckDigitsChars.IsMatch(textToValidate);
        }
    }
}
