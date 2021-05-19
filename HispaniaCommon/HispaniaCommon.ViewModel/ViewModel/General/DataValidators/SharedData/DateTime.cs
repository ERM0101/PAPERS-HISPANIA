using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        public static bool CompareNullDateTimeValues(DateTime date_1, DateTime date_2)
        {
            return ((DateTime)date_1).Date == ((DateTime)date_2).Date;
        }

        public static bool CompareNullDateTimeValues(DateTime? date_1, DateTime? date_2)
        {
            if ((date_1 == null) && (date_2 == null)) return true;
            else if ((date_1 == null) && (date_2 != null)) return false;
            else if ((date_1 != null) && (date_2 == null)) return false;
            else return ((DateTime)date_1).Date == ((DateTime)date_2).Date;
        }

        private static Regex rexValidDateTimeChars = new Regex(@"[0-9]|/");

        public static bool IsEmptyOrDateTime(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsDateTime(textToValidate, fieldName, out msgError);
        }

        private const string ValidationDateTimeError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsDateTime(DateTime dateToValidate, string fieldName, out string msgError)
        {
            #pragma warning disable IDE0031 // DateTime types can't implements this optimization.
            return IsDateTime((dateToValidate == null ? null : dateToValidate.ToString()), fieldName, out msgError);
            #pragma warning restore IDE0031 
        }

        public static bool IsDateTime(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            try
            {
                if (!(string.IsNullOrEmpty(textToValidate)))
                {
                    DateTime value = DateTime.Parse(textToValidate);
                }
                else msgError = string.Format(ValidationDateTimeError, fieldName, "Format de data incorrecte.");
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationDateTimeError, fieldName, MsgManager.ExcepMsg(ex));
            }
            return (String.IsNullOrEmpty(msgError));
        }

        public static bool IsValidDateTimeChar(string textToValidate)
        {
            return rexValidDateTimeChars.IsMatch(textToValidate);
        }
    }
}
