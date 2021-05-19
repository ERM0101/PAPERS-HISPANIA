using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexAddress = new Regex(@"^.*(?=.*)(?=.*[0-9À-ÿa-zA-ZñÑçÇ/,ºª/-/_/.\'\s]).*$"); // Vowels with accents : [À-ÿ]

        private static Regex rexValidAddressChars = new Regex(@"[0-9]|[À-ÿ]|[a-z]|[A-Z]|[ñÑ]|[çÇ]|º|ª|-|_|\s|\.|\d|/|,|\'|/."); 

        public const string ValidationEmptyOrAddressError = "Format incorrecte de l'adreça.";

        public static bool IsEmptyOrAddress(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsAddress(textToValidate);
        }

        public const string ValidationAddressError = "Format incorrecte de l'adreça. Valor no definit o que conté caràcters no vàlids.";

        public static bool IsAddress(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexAddress.IsMatch(textToValidate);
        }

        public static bool IsValidAddressChar(string textToValidate)
        {
            return rexValidAddressChars.IsMatch(textToValidate);
        }
    }
}
