using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexShortDecimal = new Regex(@"^(([0-9]){0,3}$)");

        private static Regex rexValidShortDecimalChars = new Regex(@"[0-9]");

        public static bool IsEmptyOrShortDecimal(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsEmptyOrShortDecimal(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.WithoutDecimals), fieldName, out msgError));
        }

        public static bool IsEmptyOrShortDecimal(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsShortDecimal(textToValidate, fieldName, out msgError);
        }

        private const string ValidationShortDecimalError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsShortDecimal(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsShortDecimal(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.WithoutDecimals), fieldName, out msgError));
        }

        public static bool IsShortDecimal(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexShortDecimal.IsMatch(textToValidate))
                {
                    decimal value = decimal.Parse(textToValidate);
                    if (value > 999)
                    {
                        msgError = string.Format("Error, el Nombre '{0}' no pot ser mai superior a 999.", fieldName);
                        return (false);
                    }
                    return true;
                }
                else
                {
                    msgError = string.Format(ValidationShortDecimalError, fieldName, "S'han introduït més de tres dígits o caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationShortDecimalError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidShortDecimalChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            return rexValidShortDecimalChars.IsMatch(textToValidate);
        }
    }
}
