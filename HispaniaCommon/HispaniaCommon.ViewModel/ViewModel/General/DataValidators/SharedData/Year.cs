using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexYear = new Regex(@"^(([0-9]){4,5}$)");

        private static Regex rexValidYearChars = new Regex(@"[0-9]");

        public static bool IsEmptyOrYear(decimal yearToValidate, string fieldName, out string msgError)
        {
            return (IsEmptyOrYear(GlobalViewModel.GetStringFromYearValue(yearToValidate, true), fieldName, out msgError));
        }

        public static bool IsEmptyOrYear(string textToValidate, string fieldName, out string msgError)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsYear(textToValidate, fieldName, out msgError);
        }

        private const string ValidationYearError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        public static bool IsYear(decimal decimalToValidate, string fieldName, out string msgError)
        {
            return (IsYear(GlobalViewModel.GetStringFromYearValue(decimalToValidate, true), fieldName, out msgError));
        }

        public static bool IsYear(string textToValidate, string fieldName, out string msgError)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexYear.IsMatch(textToValidate))
                {
                    decimal value = decimal.Parse(textToValidate);
                    if ((value < 1900) || (value > 99999))
                    {
                        msgError = string.Format("Error, l'any del camp '{0}' no pot ser mai superior a 99999 ni inferior a 1900.", fieldName);
                        return (false);
                    }
                    return true;
                }
                else
                {
                    msgError = string.Format(ValidationYearError, fieldName, "S'han introduït caràcters incorrectes o un valor més petit que 1900.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format(ValidationYearError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidYearChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            return rexValidYearChars.IsMatch(textToValidate);
        }
    }
}
