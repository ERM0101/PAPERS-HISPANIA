using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexValidUintChars = new Regex(@"\d");

        private const string ValidationEmptyOrUintError = "Format incorrecte del camp numèric '{0}'. Els valors correctes corresponen a números entre 0 i 4.294.967.295.\r\nDetalls: {1}";

        public static bool IsEmptyOrUint(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsUint(textToValidate, fieldName, out msgError);
        }

        private const string ValidationUintError = "Format incorrecte del camp numèric '{0}'. Els valors correctes corresponen a números entre 0 i 4.294.967.295.\r\nDetalls: {1}";

        public static bool IsUint(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                uint value = uint.Parse(textToValidate);
                return true;
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationUintError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidUintChar(string textToValidate)
        {
            return rexValidUintChars.IsMatch(textToValidate);
        }
    }
}
