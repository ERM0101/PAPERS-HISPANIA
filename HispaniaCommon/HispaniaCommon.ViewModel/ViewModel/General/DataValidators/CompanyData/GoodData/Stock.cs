using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexStock = new Regex(@"^([-]|[+]){0,1}((([0-9]){1,15}$)|((([0-9]){1,15}),(([0-9]){1,4})$))");

        private static Regex rexValidStockChars = new Regex(@"[0-9]|\,|\.|[-]|[+]");

        private static Regex rexValidStockGreaterThanZeroChars = new Regex(@"[0-9]|\,");
        public static bool IsEmptyOrStock(decimal decimalToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            return IsEmptyOrStock(GlobalViewModel.GetStringFromStockValue(decimalToValidate), fieldName, out msgError, OnlyGreaterThatZero);
        }

        public static bool IsEmptyOrStock(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            msgError = string.Empty;
            return string.IsNullOrEmpty(textToValidate) || IsStock(textToValidate, fieldName, out msgError, OnlyGreaterThatZero);
        }

        private const string ValidationStockError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";

        private const string ValidationStockGreaterThanZeroError = "Format incorrecte del camp '{0}'. Valor incorrecte.\r\nDetalls: {1}";
        public static bool IsStock(decimal decimalToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            return IsStock(GlobalViewModel.GetStringFromStockValue(decimalToValidate), fieldName, out msgError, OnlyGreaterThatZero);
        }

        public static bool IsStock(string textToValidate, string fieldName, out string msgError, bool OnlyGreaterThatZero = false)
        {
            try
            {
                msgError = string.Empty;
                if (!string.IsNullOrEmpty(textToValidate) && rexStock.IsMatch(textToValidate))
                {
                    float value = float.Parse(textToValidate);
                    if ((OnlyGreaterThatZero) && (value < 0))
                    {
                        msgError = ValidationStockGreaterThanZeroError;
                        return false;
                    }
                    else return true;
                }
                else
                {
                    msgError = string.Format((OnlyGreaterThatZero) ? ValidationStockGreaterThanZeroError : ValidationStockError, fieldName, 
                                             "S'han introduït més de tres decimals o caràcters incorrectes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgError = string.Format((OnlyGreaterThatZero) ? ValidationStockGreaterThanZeroError : ValidationStockError, fieldName, MsgManager.ExcepMsg(ex));
                return false;
            }
        }

        public static bool IsValidStockChar(string textToValidate, bool OnlyGreaterThatZero = false)
        {
            if (OnlyGreaterThatZero) return rexValidStockGreaterThanZeroChars.IsMatch(textToValidate);
            else return rexValidStockChars.IsMatch(textToValidate);
        }
    }
}
