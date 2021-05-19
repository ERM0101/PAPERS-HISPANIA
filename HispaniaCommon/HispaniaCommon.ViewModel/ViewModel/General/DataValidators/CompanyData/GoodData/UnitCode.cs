using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexUnitCode = new Regex(@"^[0-9]{1,2}$");

        private static Regex rexValidUnitCodeChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrUnitCodeError = "Format incorrecte del camp còdi d'Unitat.\r\nDetalls: S'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsEmptyOrUnitCode(string textToValidate, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsUnitCode(textToValidate, out msgError);
        }

        public const string ValidationUnitCodeError = "Format incorrecte del camp còdi d'Unitat.\r\nDetalls: Valor no definit o s'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsUnitCode(string textToValidate, out string msgError)
        {
            if (!string.IsNullOrEmpty(textToValidate) && rexUnitCode.IsMatch(textToValidate))
            {
                msgError = string.Empty;
                return true;
            }
            else
            {
                msgError = ValidationUnitCodeError;
                return (false);
            }
        }

        public static bool IsValidUnitCodeChar(string textToValidate)
        {
            return rexValidUnitCodeChars.IsMatch(textToValidate);
        }
    }
}
