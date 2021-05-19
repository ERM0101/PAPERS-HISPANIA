using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexEffectType = new Regex(@"^[0-9]{1,2}$");

        private static Regex rexValidEffectTypeChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrEffectTypeError = "Format incorrecte del camp Tipus del Tipus d'Efecte.\r\nDetalls: S'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsEmptyOrEffectType(string textToValidate, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsEffectType(textToValidate, out msgError);
        }

        public const string ValidationEffectTypeError = "Format incorrecte del camp Tipus del Tipus d'Efecte.\r\nDetalls: Valor no definit o s'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsEffectType(string textToValidate, out string msgError)
        {
            if (!string.IsNullOrEmpty(textToValidate) && rexEffectType.IsMatch(textToValidate))
            {
                msgError = string.Empty;
                return true;
            }
            else
            {
                msgError = ValidationEffectTypeError;
                return (false);
            }
        }

        public static bool IsValidEffectTypeChar(string textToValidate)
        {
            return rexValidEffectTypeChars.IsMatch(textToValidate);
        }
    }
}
