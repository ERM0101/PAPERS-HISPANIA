using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexRetentionCode = new Regex(@"^[0-9]{1,2}$");

        private static Regex rexValidRetentionCodeChars = new Regex(@"[0-9]");

        public const string ValidationEmptyOrRetentionCodeError = "Format incorrecte del camp Tipus del Tipus de Retenció.\r\nDetalls: S'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsEmptyOrRetentionCode(string textToValidate, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsRetentionCode(textToValidate, out msgError);
        }

        public const string ValidationRetentionCodeError = "Format incorrecte del camp Tipus del Tipus de Retenció.\r\nDetalls: Valor no definit o s'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsRetentionCode(string textToValidate, out string msgError)
        {
            if (!string.IsNullOrEmpty(textToValidate) && rexRetentionCode.IsMatch(textToValidate))
            {
                msgError = string.Empty;
                return true;
            }
            else
            {
                msgError = ValidationRetentionCodeError;
                return (false);
            }
        }

        public static bool IsValidRetentionCodeChar(string textToValidate)
        {
            return rexValidRetentionCodeChars.IsMatch(textToValidate);
        }
    }
}
