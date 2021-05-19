using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        //private static Regex rexPhoneNumber = new Regex(string.Format("{0}{1}{2}{3}",
        //                                                              "(((^[0-9]{2}?(-| )*[0-9]{2}?(-| )*)|(^[0-9]{2}?(-| )*))",
        //                                                              "(([0-9]{7}?$)|([0-9]{3}?(-| )*[0-9]{2}?(-| )*[0-9]{2}?$)))|",
        //                                                              "(((^[0-9]{2}?(-| )*[0-9]{3}?(-| )*)|(^[0-9]{3}?(-| )*))",
        //                                                              "(([0-9]{6}?$)|([0-9]{2}?(-| )*[0-9]{2}?(-| )*[0-9]{2}?$)))"));

        //private static Regex rexValidPhoneNumberChars = new Regex("[0-9]|-| ");

        private static Regex rexPhoneNumber = new Regex("[0-9]*");

        private static Regex rexValidPhoneNumberChars = new Regex("[0-9]");

        public const string ValidationEmptyOrPhoneNumberError = "Format incorrecte de telèfon. Normalment es degut a un nombre incorrecte de digits.";

        public static bool IsEmptyOrPhoneNumber(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsPhoneNumber(textToValidate);
        }

        public const string ValidationPhoneNumberError = "Format incorrecte de telèfon. Valor no definit o nombre de digits incorrecte.";

        public static bool IsPhoneNumber(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexPhoneNumber.IsMatch(textToValidate); 
        }

        public static bool IsValidPhoneNumberChar(string textToValidate)
        {
            return rexValidPhoneNumberChars.IsMatch(textToValidate);
        }
    }
}
