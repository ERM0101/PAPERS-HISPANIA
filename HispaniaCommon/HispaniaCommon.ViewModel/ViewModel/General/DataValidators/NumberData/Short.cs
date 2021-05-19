using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexValidShortChars = new Regex(@"\d|[-]|[+]");

        private static Regex rexValidShortGreaterThanZeroChars = new Regex(@"\d");

        public static bool IsEmptyOrShort(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsShort(textToValidate, fieldName, out msgError, OnlyGreaterThatZero);
        }

        private const string ValidationShortError = "Format incorrecte del camp '{0}'. Els valors correctes corresponen a números entre -32.678 i 32.677.\r\nDetalls: {1}";

        private const string ValidationShortGreaterThanZeroError = "Format incorrecte del camp '{0}'. Els valors correctes corresponen a números entre 0 i 32.677.\r\nDetalls: {1}";

        public static bool IsShort(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            try
            {
                msgError = string.Empty;
                short value = short.Parse(textToValidate);
                if ((OnlyGreaterThatZero) && (value < 0))
                {
                    msgError = ValidationShortGreaterThanZeroError;
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                msgError = string.Format((OnlyGreaterThatZero) ? ValidationShortGreaterThanZeroError : ValidationShortError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidShortChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            if (OnlyGreaterThatZero) return rexValidShortGreaterThanZeroChars.IsMatch(textToValidate);
            else return rexValidShortChars.IsMatch(textToValidate);
        }
    }
}
