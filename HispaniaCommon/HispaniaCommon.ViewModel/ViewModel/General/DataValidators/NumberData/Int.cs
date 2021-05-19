#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

#endregion

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexValidIntChars = new Regex(@"\d|[-]|[+]");

        private static Regex rexValidIntGreaterThanZeroChars = new Regex(@"\d");

        public static bool IsEmptyOrInt(int? intToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        { 
            return IsEmptyOrInt(GlobalViewModel.GetStringFromIntValue(intToValidate), fieldName, out msgError, OnlyGreaterThatZero);
        }

        public static bool IsEmptyOrInt(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsInt(textToValidate, fieldName, out msgError, OnlyGreaterThatZero);
        }

        private const string ValidationIntError = "Format incorrecte del camp '{0}'. Els valors correctes corresponen a números entre -2.147.483.648 i 2.147.483.647.\r\nDetalls: {1}";

        private const string ValidationIntGreaterThanZeroError = "Format incorrecte del camp '{0}'. Els valors correctes corresponen a números entre 0 i 2.147.483.647.\r\nDetalls: {1}";

        public static bool IsInt(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            try
            {
                msgError = string.Empty;
                int value = int.Parse(textToValidate);
                if ((OnlyGreaterThatZero) && (value < 0))
                {
                    msgError = ValidationIntGreaterThanZeroError;
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                msgError = string.Format((OnlyGreaterThatZero) ? ValidationIntGreaterThanZeroError : ValidationIntError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidIntChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            if (OnlyGreaterThatZero) return rexValidIntGreaterThanZeroChars.IsMatch(textToValidate);
            else return rexValidIntChars.IsMatch(textToValidate);
        }
    }
}
