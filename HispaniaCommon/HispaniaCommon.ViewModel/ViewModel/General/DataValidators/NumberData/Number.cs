#region Librerias usadas por la clase

using System.Text.RegularExpressions;

#endregion

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexNumber = new Regex(@"^\d+$");

        private static Regex rexValidNumericChars = new Regex(@"\d");

        public static bool IsNumeric(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexNumber.IsMatch(textToValidate);  
        }

        public static bool IsValidNumericChar(string textToValidate)
        {
            return rexValidNumericChars.IsMatch(textToValidate);
        }
    }
}
