using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexCIF = new Regex(@"([a-z]|[A-Z]|[0-9]|\.|[-])+");

        // Only accept Spanish CIF or NIF
        //private static Regex rexCIF = new Regex(@"(^[a-zA-Z]+(\.|-)*[0-9]{2}?(\.|-)*[0-9]{6}?$)|(^[a-zA-Z]+[0-9]{8}?$)|(^[0-9]{2}?(\.|-)*[0-9]{6}?(\.|-)*[a-zA-Z]+$)|(^[0-9]{8}?[a-zA-Z]+$)"); 

        private static Regex rexValidCIFChars = new Regex(@"[a-z]|[A-Z]|[0-9]|\.|[-]");

        public const string ValidationEmptyOrCIFError = "Format incorrecte de CIF.";

        public static bool IsEmptyOrCIF(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsCIF(textToValidate);
        }

        public const string ValidationCIFError = "Format incorrecte de CIF. Valor no definit o format incorrecte.";

        public static bool IsCIF(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexCIF.IsMatch(textToValidate);
        }

        public static bool IsValidCIFChar(string textToValidate)
        {
            return rexValidCIFChars.IsMatch(textToValidate);
        }
    }
}
