#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

#endregion

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexUnit = new Regex(@"^([-]|[+]){0,1}((([0-9]){1,15}$)|((([0-9]){1,15})\,(([0-9]){1,3})$))");

        private static Regex rexValidUnitChars = new Regex(@"[0-9]|\,|\.|[-]|[+]");

        public static bool IsEmptyOrUnit(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsEmptyOrUnit(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.Unit), fieldName, out msgError));
        }

        public static bool IsEmptyOrUnit(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsUnit(textToValidate, fieldName, out msgError);
        }

        private const string ValidationUnitError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsUnit(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsUnit(GlobalViewModel.GetStringFromDecimalValue(decimalToValidate, DecimalType.Unit), fieldName, out msgError));
        }

        public static bool IsUnit(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexUnit.IsMatch(textToValidate))
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
                    msgError = string.Format(ValidationUnitError, fieldName, "Valor fora de rang o amb caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationUnitError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidUnitChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            return rexValidUnitChars.IsMatch(textToValidate);
        }
    }
}
