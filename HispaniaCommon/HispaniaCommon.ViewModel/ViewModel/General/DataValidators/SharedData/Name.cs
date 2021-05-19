using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexName = new Regex(@"^.*(?=.*[À-ÿa-zA-ZñÑ0-9\s\p{P}]).*$");  // Vowels with accents : [À-ÿ]

        private static Regex rexValidNameChars = new Regex(@"[À-ÿ]|[a-z]|[A-Z]|[ñÑ]|[0-9]|\s|\p{P}|\.]"); 

        public const string ValidationEmptyOrNameError = "Format incorrecte del Nom.";

        public static bool IsEmptyOrName(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsName(textToValidate);
        }

        public const string ValidationNameError = "Format incorrecte del Nom. Valor no definit o que conté caràcters no vàlids.";

        public static bool IsName(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexName.IsMatch(textToValidate);
        }

        public static bool IsValidNameChar(string textToValidate)
        {
            return rexValidNameChars.IsMatch(textToValidate);
        }
    }
}
