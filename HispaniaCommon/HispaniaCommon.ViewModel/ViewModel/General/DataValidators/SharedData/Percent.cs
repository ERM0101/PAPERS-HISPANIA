using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexPercent = new Regex(@"^(([0-9]){1,3}$)|^((([0-9]){1,3}),(([0-9]){1,3})$)");

        private static Regex rexValidPercentChars = new Regex(@"[0-9]|\,|\.");

        public static bool IsEmptyOrPercent(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsEmptyOrPercent(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.Percent), fieldName, out msgError));
        }

        public static bool IsEmptyOrPercent(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsPercent(textToValidate, fieldName, out msgError);
        }

        private const string ValidationPercentError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsPercent(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsPercent(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.Percent), fieldName, out msgError));
        }

        public static bool IsPercent(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexPercent.IsMatch(textToValidate))
                {
                    decimal value = decimal.Parse(textToValidate);
                    if (value > 100)
                    {
                        msgError = string.Format("Error, el percentatge '{0}' no pot ser mai superior a 100.", fieldName);
                        return (false);
                    }
                    return true;
                }
                else
                {
                    msgError = string.Format(ValidationPercentError, fieldName, "Valor fora de rang o amb caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationPercentError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidPercentChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            return rexValidPercentChars.IsMatch(textToValidate);
        }
    }
}
