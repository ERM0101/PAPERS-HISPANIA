using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexEmail = new Regex(@"^([\w\.\-]+)@([\w\.\-]+)((\.(\w)+))$");

        private static Regex rexValidEmailChars = new Regex(@"[a-z]|[A-Z]|[ñÑ]|\.|\d|-|_|@");

        public const string ValidationEmptyOrEmailError = "Format incorrecte de l'adreça de correu electrònic.";

        public static bool IsEmptyOrEmail(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsEmail(textToValidate);
        }

        public const string ValidationEmailError = "Format incorrecte de l'adreça de correu electònic. Valor no definit o que conté caràcters no vàlids.";

        public static bool IsEmail(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate); // Introdueixen més d'un e-mail en el camp e-mail per tant no podem vaidar res.
            //return !string.IsNullOrEmpty(textToValidate) && rexEmail.IsMatch(textToValidate);
        }

        public static bool IsValidEmailChar(string textToValidate)
        {
            return rexValidEmailChars.IsMatch(textToValidate);
        }
    }
}
