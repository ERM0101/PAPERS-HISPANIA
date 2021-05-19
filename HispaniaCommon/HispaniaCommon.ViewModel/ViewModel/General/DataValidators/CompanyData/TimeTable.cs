using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexTimeTable = new Regex(@"^.*(?=.*[0-9a-zA-ZñÑ\d\s\p{P}]).*$");  // Vowels with accents : [À-ÿ]

        private static Regex rexValidTimeTableChars = new Regex(@"[0-9a-z]|[A-Z]|[ñÑ]|\s|\p{P}|\.|\d]");

        public const string ValidationEmptyOrTimeTableError = "Format incorrecte del Horari.";

        public static bool IsEmptyOrTimeTable(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsTimeTable(textToValidate);
        }

        public const string ValidationTimeTableError = "Format incorrecte del Horari. Valor no definit o que conté caràcters no vàlids.";

        public static bool IsTimeTable(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexTimeTable.IsMatch(textToValidate);
        }

        public static bool IsValidTimeTableChar(string textToValidate)
        {
            return rexValidTimeTableChars.IsMatch(textToValidate);
        }
    }
}
