using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexDescription = new Regex(@"[.|\.]*$"); 

        private static Regex rexValidDescriptionChars = new Regex(@".|\."); 

        public const string ValidationEmptyOrDescriptionError = "Format incorrecte de la descripció.";

        public static bool IsEmptyOrDescription(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsDescription(textToValidate);
        }

        public const string ValidationDescriptionError = "Format incorrecte de la descripció. Conté caràcters no vàlids.";

        public static bool IsDescription(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexDescription.IsMatch(textToValidate);
        }

        public static bool IsValidDescriptionChar(string textToValidate)
        {
            return rexValidDescriptionChars.IsMatch(textToValidate);
        }
    }
}
