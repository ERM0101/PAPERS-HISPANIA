using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexIBAN_OfficeCode = new Regex(@"^[0-9]{4}?$");

        private static Regex rexValidIBAN_OfficeCodeChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrIBAN_OfficeCodeError = "Format incorrecte del Codi d'Oficina de l'IBAN.";

        public static bool IsEmptyOrIBAN_OfficeCode(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsIBAN_OfficeCode(textToValidate);
        }

        public const string ValidationIBAN_OfficeCodeError = "Format incorrecte del Codi d'Oficina de l'IBAN. Valor no definit o format incorrecte.";

        public static bool IsIBAN_OfficeCode(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexIBAN_OfficeCode.IsMatch(textToValidate);
        }

        public static bool IsValidIBAN_OfficeCodeChar(string textToValidate)
        {
            return rexValidIBAN_OfficeCodeChars.IsMatch(textToValidate);
        }
    }
}
