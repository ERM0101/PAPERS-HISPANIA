using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexMobilePhoneNumber = new Regex(string.Format("{0}{1}",
                                                                            "((^[0-9]{2}?(-| )*[0-9]{3}?(-| )*)|(^[0-9]{3}?(-| )*))",
                                                                            "(([0-9]{6}?$)|([0-9]{3}?(-| )*[0-9]{3}?$))"));



        private static Regex rexValidMobilePhoneNumberChars = new Regex("[0-9]|-| ");

        public const string ValidationEmptyOrMobilePhoneNumberError = "Format incorrecte de mòbil. Normalment es degut a un nombre incorrecte de digits.";

        public static bool IsEmptyOrMobilePhoneNumber(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsMobilePhoneNumber(textToValidate);
        }

        public const string ValidationMobilePhoneNumberError = "Format incorrecte de telèfon. Valor no definit o nombre de digits incorrecte.";

        public static bool IsMobilePhoneNumber(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexMobilePhoneNumber.IsMatch(textToValidate);
        }

        public static bool IsValidMobilePhoneNumberChar(string textToValidate)
        {
            return rexValidMobilePhoneNumberChars.IsMatch(textToValidate);
        }
    }
}
