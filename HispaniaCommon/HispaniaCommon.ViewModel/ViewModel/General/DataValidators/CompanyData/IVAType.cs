using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexIVAType = new Regex(@"^[0-9]{1,2}$");

        private static Regex rexValidIVATypeChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrIVATypeError = "Format incorrecte del camp Tipus del Tipus d'IVA.\r\nDetalls: S'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsEmptyOrIVAType(string textToValidate, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsIVAType(textToValidate, out msgError);
        }

        public const string ValidationIVATypeError = "Format incorrecte del camp Tipus del Tipus d'IVA.\r\nDetalls: Valor no definit o s'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsIVAType(string textToValidate, out string msgError)
        {
            if (!string.IsNullOrEmpty(textToValidate) && rexIVAType.IsMatch(textToValidate))
            {
                msgError = string.Empty;
                return true;
            }
            else
            {
                msgError = ValidationIVATypeError;
                return (false);
            }
        }

        public static bool IsValidIVATypeChar(string textToValidate)
        {
            return rexValidIVATypeChars.IsMatch(textToValidate);
        }
    }
}
