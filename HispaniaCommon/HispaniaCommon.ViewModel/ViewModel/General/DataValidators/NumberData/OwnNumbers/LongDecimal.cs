using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexLongDecimal = new Regex(@"^(([0-9]){0,10}$)");

        private static Regex rexValidLongDecimalChars = new Regex(@"[0-9]");

        public static bool IsEmptyOrLongDecimal(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsEmptyOrLongDecimal(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.WithoutDecimals), fieldName, out msgError));
        }

        public static bool IsEmptyOrLongDecimal(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsLongDecimal(textToValidate, fieldName, out msgError);
        }

        private const string ValidationLongDecimalError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsLongDecimal(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsLongDecimal(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.WithoutDecimals), fieldName, out msgError));
        }

        public static bool IsLongDecimal(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexLongDecimal.IsMatch(textToValidate))
                {
                    decimal value = decimal.Parse(textToValidate);
                    if (value > 9999999999)
                    {
                        msgError = string.Format("Error, el Nombre '{0}' no pot ser mai superior a 9999999999.", fieldName);
                        return (false);
                    }
                    return true;
                }
                else
                {
                    msgError = string.Format(ValidationLongDecimalError, fieldName, "S'han introduït més de deu dígits o caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationLongDecimalError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidLongDecimalChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            return rexValidLongDecimalChars.IsMatch(textToValidate);
        }
    }
}
