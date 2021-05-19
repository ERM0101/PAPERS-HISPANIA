using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexSendType = new Regex(@"^[0-9]{1,2}$");

        private static Regex rexValidSendTypeChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrSendTypeError = "Format incorrecte del camp 'TIPUS' del Tipus d'Enviament.\r\nDetalls: S'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsEmptyOrSendType(string textToValidate, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsSendType(textToValidate, out msgError);
        }

        public const string ValidationSendTypeError = "Format incorrecte del camp 'TIPUS' del Tipus d'Enviament.\r\nDetalls: Valor no definit o s'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsSendType(string textToValidate, out string msgError)
        {
            if (!string.IsNullOrEmpty(textToValidate) && rexSendType.IsMatch(textToValidate))
            {
                msgError = string.Empty;
                return true;
            }
            else
            {
                msgError = ValidationSendTypeError;
                return (false);
            }
        }

        public static bool IsValidSendTypeChar(string textToValidate)
        {
            return rexValidSendTypeChars.IsMatch(textToValidate);
        }
    }
}
