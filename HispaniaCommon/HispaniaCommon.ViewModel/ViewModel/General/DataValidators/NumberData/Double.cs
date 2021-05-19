using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexDouble = new Regex(@"^([-]|[+]){0,1}((([0-9])+$)|(([0-9])+,([0-9]){1,3}$))");

        private static Regex rexValidDoubleChars = new Regex(@"[0-9]|,|[-]|[+]");

        private static Regex rexValidDoubleGreaterThanZeroChars = new Regex(@"[0-9]|,");

        public static bool IsEmptyOrDouble(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsDouble(textToValidate, fieldName, out msgError, OnlyGreaterThatZero);
        }

        private const string ValidationDoubleError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        private const string ValidationDoubleGreaterThanZeroError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsDouble(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexDouble.IsMatch(textToValidate))
                {
                    double value = double.Parse(textToValidate);
                    if ((OnlyGreaterThatZero) && (value < 0))
                    {
                        msgError = ValidationDoubleGreaterThanZeroError;
                        return false;
                    }
                    else return true;
                }
                else
                {
                    msgError = string.Format((OnlyGreaterThatZero) ? ValidationDoubleGreaterThanZeroError : ValidationDoubleError, fieldName, 
                                             "S'han introduït més de tres decimals o caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format((OnlyGreaterThatZero) ? ValidationDoubleGreaterThanZeroError : ValidationDoubleError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidDoubleChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            if (OnlyGreaterThatZero) return rexValidDoubleGreaterThanZeroChars.IsMatch(textToValidate);
            else return rexValidDoubleChars.IsMatch(textToValidate);
        }
    }
}
