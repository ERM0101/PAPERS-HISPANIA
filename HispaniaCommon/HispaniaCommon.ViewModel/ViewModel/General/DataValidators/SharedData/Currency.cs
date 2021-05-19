using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexCurrency = new Regex(@"^([-]|[+]){0,1}((([0-9]){1,15}$)|((([0-9]){1,15})\,(([0-9]){1,4})$))");

        private static Regex rexValidCurrencyChars = new Regex(@"[0-9]|\,|\.|[-]|[+]");

        public static bool IsEmptyOrCurrency(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsEmptyOrCurrency(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.Currency), fieldName, out msgError));
        }

        public static bool IsEmptyOrCurrency(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsCurrency(textToValidate, fieldName, out msgError);
        }

        private const string ValidationCurrencyError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsCurrency(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsCurrency(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.Currency), fieldName, out msgError));
        }

        public static bool IsCurrency(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexCurrency.IsMatch(textToValidate))
                {
                    decimal value = decimal.Parse(textToValidate);
                    if ((value <= -999999999999999) || (value >= 999999999999999))
                    {
                        msgError = string.Format("Error, el valor '{0}' està fora de rang (-999999999999999 <-> 999999999999999).", fieldName);
                        return (false);
                    }
                    return true;
                }
                else
                {
                    msgError = string.Format(ValidationCurrencyError, fieldName, "Valor fora de rang o amb caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationCurrencyError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidCurrencyChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            return rexValidCurrencyChars.IsMatch(textToValidate);
        }
    }
}
