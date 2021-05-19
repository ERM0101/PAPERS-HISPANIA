using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexFloat = new Regex(@"^([-]|[+]){0,1}((([0-9])+$)|(([0-9])+,([0-9]){1,3}$))");

        private static Regex rexValidFloatChars = new Regex(@"[0-9]|,|[-]|[+]");

        private static Regex rexValidFloatGreaterThanZeroChars = new Regex(@"[0-9]|,");

        public static bool IsEmptyOrFloat(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsFloat(textToValidate, fieldName, out msgError, OnlyGreaterThatZero);
        }

        private const string ValidationFloatError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        private const string ValidationFloatGreaterThanZeroError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsFloat(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexFloat.IsMatch(textToValidate))
                {
                    float value = float.Parse(textToValidate);
                    if ((OnlyGreaterThatZero) && (value < 0))
                    {
                        msgError = ValidationFloatGreaterThanZeroError;
                        return false;
                    }
                    else return true;
                }
                else
                {
                    msgError = string.Format((OnlyGreaterThatZero) ? ValidationFloatGreaterThanZeroError : ValidationFloatError, fieldName, 
                                             "S'han introduït més de tres decimals o caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format((OnlyGreaterThatZero) ? ValidationFloatGreaterThanZeroError : ValidationFloatError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidFloatChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            if (OnlyGreaterThatZero) return rexValidFloatGreaterThanZeroChars.IsMatch(textToValidate);
            else return rexValidFloatChars.IsMatch(textToValidate);
        }
    }
}
