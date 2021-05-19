using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexIBAN_CountryCode = new Regex(@"^[a-zA-Z]{2}?[0-9]{2}?$");

        private static Regex rexValidIBAN_CountryCodeChars = new Regex(@"[a-z]|[A-Z]|[0-9]");

        public const string ValidationEmptyOrIBAN_CountryCodeError = "Format incorrecte del Codi de País de l'IBAN.";

        public static bool IsEmptyOrIBAN_CountryCode(string textToValidate, string CCC, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            return (string.IsNullOrEmpty(textToValidate) || IsIBAN_CountryCode(textToValidate, CCC, out ErrMsg));
        }

        public const string ValidationIBAN_CountryCodeError = "Format incorrecte del Codi de País de l'IBAN. Valor no definit o format incorrecte.";

        public static bool IsIBAN_CountryCode(string textToValidate, string CCC, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            if (!string.IsNullOrEmpty(textToValidate) && rexIBAN_CountryCode.IsMatch(textToValidate))
            {
                try
                {
                    string IBAN = string.Format("{0}{1}", textToValidate, CCC);
                    GlobalViewModel.Instance.HispaniaViewModel.ValidateSpanishIBAN(IBAN);
                }
                catch (Exception ex)
                {
                    ErrMsg = MsgManager.ExcepMsg(ex);
                }
            }
            else
            {
                ErrMsg = GlobalViewModel.ValidationEmptyOrIBAN_CountryCodeError;
            }
            return (string.IsNullOrEmpty(ErrMsg));
        }

        public static bool IsValidIBAN_CountryCodeChar(string textToValidate)
        {
            return rexValidIBAN_CountryCodeChars.IsMatch(textToValidate);
        }
    }
}
