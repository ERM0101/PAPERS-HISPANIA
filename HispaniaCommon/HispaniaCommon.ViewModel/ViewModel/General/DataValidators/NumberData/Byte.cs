using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexValidByteChars = new Regex(@"\d");

        public static bool IsEmptyOrByte(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsByte(textToValidate, fieldName, out msgError);
        }

        private const string ValidationByteError = "Format incorrecte del camp numèric '{0}'. Els valors correctes corresponen a números entre 0 i 255.\r\nDetalls: {1}";

        public static bool IsByte(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                byte value = byte.Parse(textToValidate);
                return true;
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationByteError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidByteChar(string textToValidate)
        {
            return rexValidByteChars.IsMatch(textToValidate);
        }
    }
}
