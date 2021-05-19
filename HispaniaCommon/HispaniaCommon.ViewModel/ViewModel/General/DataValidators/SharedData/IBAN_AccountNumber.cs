using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexIBAN_AccountNumber = new Regex(@"^[0-9]{10}?$");

        private static Regex rexValidIBAN_AccountNumberChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrIBAN_AccountNumberError = "Format incorrecte del número de compte de l'IBAN.";

        public static bool IsEmptyOrIBAN_AccountNumber(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsIBAN_AccountNumber(textToValidate);
        }

        public const string ValidationIBAN_AccountNumberError = "Format incorrecte del número de compte de l'IBAN. Valor no definit o format incorrecte.";

        public static bool IsIBAN_AccountNumber(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexIBAN_AccountNumber.IsMatch(textToValidate);
        }

        public static bool IsValidIBAN_AccountNumberChar(string textToValidate)
        {
            return rexValidIBAN_AccountNumberChars.IsMatch(textToValidate);
        }
    }
}
