using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexBillingSerieName = new Regex(@"^[a-zA-Z]{1,2}$");

        private static Regex rexValidBillingSerieNameChars = new Regex(@"[a-zA-Z]");

        public const string ValidationEmptyOrBillingSerieNameError = "Format incorrecte del Nom de la Serie de Facturació.\r\nDetalls: S'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsEmptyOrBillingSerieName(string textToValidate, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsBillingSerieName(textToValidate, out msgError);
        }

        public const string ValidationBillingSerieNameError = "Format incorrecte del Nom de la Serie de Facturació.\r\nDetalls: Valor no definit o s'ha excedit la longitud màxima de 2 caràcters.";

        public static bool IsBillingSerieName(string textToValidate, out string msgError)
        {
            if (!string.IsNullOrEmpty(textToValidate) && rexBillingSerieName.IsMatch(textToValidate))
            {
                msgError = string.Empty;
                return true;
            }
            else
            {
                msgError = ValidationBillingSerieNameError;
                return (false);
            }
        }

        public static bool IsValidBillingSerieNameChar(string textToValidate)
        {
            return rexValidBillingSerieNameChars.IsMatch(textToValidate);
        }
    }
}
