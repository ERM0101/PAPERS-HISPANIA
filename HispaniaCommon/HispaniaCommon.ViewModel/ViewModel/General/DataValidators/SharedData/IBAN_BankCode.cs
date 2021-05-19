using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexIBAN_BankCode = new Regex(@"^[0-9]{4}?$");

        private static Regex rexValidIBAN_BankCodeChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrIBAN_BankCodeError = "Format incorrecte del Codi de Banc de l'IBAN.";

        public static bool IsEmptyOrIBAN_BankCode(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsIBAN_BankCode(textToValidate);
        }

        public const string ValidationIBAN_BankCodeError = "Format incorrecte del Codi de Banc de l'IBAN. Valor no definit o format incorrecte.";

        public static bool IsIBAN_BankCode(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexIBAN_BankCode.IsMatch(textToValidate);
        }

        public static bool IsValidIBAN_BankCodeChar(string textToValidate)
        {
            return rexValidIBAN_BankCodeChars.IsMatch(textToValidate);
        }
    }
}
