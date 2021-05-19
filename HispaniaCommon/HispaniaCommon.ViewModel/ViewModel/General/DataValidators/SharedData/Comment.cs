using System.Text.RegularExpressions;

namespace HispaniaCommon.ViewModel
{
    public partial class GlobalViewModel
    {
        private static Regex rexComment = new Regex(@"[\.\n\r]*$"); 

        private static Regex rexValidCommentChars = new Regex(".|\n|\r"); 

        public const string ValidationEmptyOrCommentError = "Format incorrecte del Nom.";

        public static bool IsEmptyOrComment(string textToValidate)
        {
            return string.IsNullOrEmpty(textToValidate) || IsComment(textToValidate);
        }

        public const string ValidationCommentError = "Format incorrecte del comentari. Conté caràcters no vàlids.";

        public static bool IsComment(string textToValidate)
        {
            return !string.IsNullOrEmpty(textToValidate) && rexComment.IsMatch(textToValidate);
        }

        public static bool IsValidCommentChar(string textToValidate)
        {
            return rexValidCommentChars.IsMatch(textToValidate);
        }
    }
}
