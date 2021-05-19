using HispaniaCommon.ViewModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sample.FullClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.btnIsNumber.Click += BtnIsNumber_Click;
            this.btnValidatePhoneNumber.Click += BtnValidatePhoneNumber_Click;
            this.btnValidateMobilePhoneNumber.Click += BtnValidateMobilePhoneNumber_Click;
            this.btnIsCIF.Click += BtnIsCIF_Click;
            this.btnIsName.Click += BtnIsName_Click;
            this.btnIsAddress.Click += BtnIsAddress_Click;
            this.btnIsEmail.Click += BtnIsEmail_Click;
            this.btnIsByte.Click += BtnIsByte_Click;
            this.btnIsUint.Click += BtnIsUint_Click;
            this.btnIsInt.Click += BtnIsInt_Click;
            this.btnIsPostalCode.Click += BtnIsPostalCode_Click;
        }

        public enum TestType
        {
            IsNumeric,
            IsPhoneNumber,
            IsMobilePhoneNumber,
            IsCIF,
            IsName,
            IsAddress,
            IsEmail,
            IsByte,
            IsUint,
            IsInt,
            IsPostalCode,
        }

        private void BtnIsNumber_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraph = new Paragraph();
                RegisterTestTesult(TestType.IsNumeric, string.Empty, false, paragraph);
                rtbResult.Document.Blocks.Add(paragraph);
            }
            else RegisterTestTesult(TestType.IsNumeric, tbTextToValidate.Text); 
        }

        private void BtnValidatePhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsPhoneNumber, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "a", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "9", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "93", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "931", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "9312", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "93123", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "931234", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "9312345", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "93123456", false, paragraphFalse);
                RegisterTestTesult(TestType.IsPhoneNumber, "349312345678", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsPhoneNumber, "931234567", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "93 1234567", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "93 123 45 67", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34931234567", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34 931234567", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34 93 1234567", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34 93 123 45 67", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34 972 123456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34 972 12 34 56", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34-931234567", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34-93-1234567", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34-972-123456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34-93-123-45-67", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPhoneNumber, "34-972-12-34-56", true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnValidateMobilePhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsMobilePhoneNumber, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "a", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "6", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "65", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "650", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "6501", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "65012", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "650123", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "6501234", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "65012345", false, paragraphFalse);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "346501234567", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "650123456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "650 123 456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "34650123456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "34 650123456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "34-650123456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "34 650 123 456", true, paragraphTrue);
                RegisterTestTesult(TestType.IsMobilePhoneNumber, "34-650 123 456", true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnIsCIF_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsCIF, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsCIF, "a", false, paragraphFalse);
                RegisterTestTesult(TestType.IsCIF, "6", false, paragraphFalse);
                RegisterTestTesult(TestType.IsCIF, "08.949950", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsCIF, "B-08.949950", true, paragraphTrue);
                RegisterTestTesult(TestType.IsCIF, "B-08949950", true, paragraphTrue);
                RegisterTestTesult(TestType.IsCIF, "B08.949950", true, paragraphTrue);
                RegisterTestTesult(TestType.IsCIF, "B08949950", true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnIsName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsName, string.Empty, false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsName, "Papers Hispània S.L.", true, paragraphTrue);
                RegisterTestTesult(TestType.IsName, "08.949950", true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnIsAddress_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsAddress, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsAddress, "08.949950", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsAddress, "C./ Sant Lluc, 31-35", true, paragraphTrue);
                RegisterTestTesult(TestType.IsAddress, "C\\ Còrsega, 195, 1º 1ª", true, paragraphTrue);
                RegisterTestTesult(TestType.IsAddress, "Plaça Francesc Macià, 1, 1º 1ª", true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsAddress, tbTextToValidate.Text);
        }

        private void BtnIsEmail_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsEmail, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsEmail, "08.949950", false, paragraphFalse);
                RegisterTestTesult(TestType.IsEmail, "peter", false, paragraphFalse);
                RegisterTestTesult(TestType.IsEmail, "peter.", false, paragraphFalse);
                RegisterTestTesult(TestType.IsEmail, "peter.rowlings", false, paragraphFalse);
                RegisterTestTesult(TestType.IsEmail, "peter.rowlings@", false, paragraphFalse);
                RegisterTestTesult(TestType.IsEmail, "peter.rowlings@company", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsEmail, "peter@company.com", true, paragraphTrue);
                RegisterTestTesult(TestType.IsEmail, "peter.rowlings@company.com", true, paragraphTrue);
                RegisterTestTesult(TestType.IsEmail, "peter-rowlings@company.com", true, paragraphTrue);
                RegisterTestTesult(TestType.IsEmail, "prowlings@company.com", true, paragraphTrue);
                RegisterTestTesult(TestType.IsEmail, "p_rowlings@company.com", true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnIsByte_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsByte, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsByte, "-1", false, paragraphFalse);
                RegisterTestTesult(TestType.IsByte, "256", false, paragraphFalse);
                RegisterTestTesult(TestType.IsByte, ".", false, paragraphFalse);
                RegisterTestTesult(TestType.IsByte, "a", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsByte, "0", true, paragraphTrue);
                RegisterTestTesult(TestType.IsByte, "100", true, paragraphTrue);
                RegisterTestTesult(TestType.IsByte, byte.MaxValue.ToString(), true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnIsUint_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsUint, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsUint, "-1", false, paragraphFalse);
                RegisterTestTesult(TestType.IsUint, "4294967296", false, paragraphFalse);
                RegisterTestTesult(TestType.IsUint, ".", false, paragraphFalse);
                RegisterTestTesult(TestType.IsUint, "a", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsUint, "0", true, paragraphTrue);
                RegisterTestTesult(TestType.IsUint, "100", true, paragraphTrue);
                RegisterTestTesult(TestType.IsUint, uint.MaxValue.ToString(), true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnIsInt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsInt, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsInt, "-2147483649", false, paragraphFalse);
                RegisterTestTesult(TestType.IsInt, "2147483648", false, paragraphFalse);
                RegisterTestTesult(TestType.IsInt, ".", false, paragraphFalse);
                RegisterTestTesult(TestType.IsInt, "a", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsInt, int.MinValue.ToString(), true, paragraphTrue);
                RegisterTestTesult(TestType.IsInt, "0", true, paragraphTrue);
                RegisterTestTesult(TestType.IsInt, "100", true, paragraphTrue);
                RegisterTestTesult(TestType.IsInt, int.MaxValue.ToString(), true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private void BtnIsPostalCode_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTextToValidate.Text))
            {
                Paragraph paragraphFalse = new Paragraph();
                RegisterTestTesult(TestType.IsPostalCode, string.Empty, false, paragraphFalse);
                RegisterTestTesult(TestType.IsPostalCode, "-1", false, paragraphFalse);
                rtbResult.Document.Blocks.Add(paragraphFalse);
                Paragraph paragraphTrue = new Paragraph();
                RegisterTestTesult(TestType.IsPostalCode, ".", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPostalCode, "0", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPostalCode, ".08036", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPostalCode, "08036", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPostalCode, "08036.", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPostalCode, "WNZ-23", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPostalCode, "12-245", true, paragraphTrue);
                RegisterTestTesult(TestType.IsPostalCode, "A", true, paragraphTrue);
                rtbResult.Document.Blocks.Add(paragraphTrue);
            }
            else RegisterTestTesult(TestType.IsPhoneNumber, tbTextToValidate.Text);
        }

        private bool DoTestTesult(TestType testType, string textToValidate, out string msgError)
        {
            msgError = string.Empty;
            switch (testType)
            {
                case TestType.IsNumeric:
                     return GlobalViewModel.IsNumeric(textToValidate);
                case TestType.IsPhoneNumber:
                     return GlobalViewModel.IsPhoneNumber(textToValidate);
                case TestType.IsMobilePhoneNumber:
                     return GlobalViewModel.IsMobilePhoneNumber(textToValidate);
                case TestType.IsCIF:
                     return GlobalViewModel.IsCIF(textToValidate);
                case TestType.IsName:
                     return GlobalViewModel.IsName(textToValidate);
                case TestType.IsAddress:
                     return GlobalViewModel.IsAddress(textToValidate);
                case TestType.IsEmail:
                     return GlobalViewModel.IsEmail(textToValidate);
                case TestType.IsByte:
                     return GlobalViewModel.IsByte(textToValidate, "Test_Field", out msgError);
                case TestType.IsUint:
                     return GlobalViewModel.IsUint(textToValidate, "Test_Field", out msgError);
                case TestType.IsInt:
                     return GlobalViewModel.IsInt(textToValidate, "Test_Field", out msgError);
                case TestType.IsPostalCode:
                     return GlobalViewModel.IsPostalCode(textToValidate);
                default:
                     return false;
            }
        }

        private void RegisterTestTesult(TestType testType, bool testResult, string msgError, string textToValidate, Paragraph paragraph, Brush brush)
        {
            string msgDetails = (!string.IsNullOrEmpty(msgError)) ? "\r\n" + msgError : string.Empty;
            switch (testType)
            {
                case TestType.IsNumeric:
                     if (testResult) AppendText(string.Format("'{0}' is numeric.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is not numeric.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsPhoneNumber:
                     if (testResult) AppendText(string.Format("'{0}' is a valid phone number.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid phone number.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsMobilePhoneNumber:
                     if (testResult) AppendText(string.Format("'{0}' is a valid mobile phone number.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid mobile phone number.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsCIF:
                     if (testResult) AppendText(string.Format("'{0}' is a valid CIF.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid CIF.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsName:
                     if (testResult) AppendText(string.Format("'{0}' is a valid Name.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid Name.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsAddress:
                     if (testResult) AppendText(string.Format("'{0}' is a valid Address.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid Address.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsEmail:
                     if (testResult) AppendText(string.Format("'{0}' is a valid Email.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid Email.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsByte:
                     if (testResult) AppendText(string.Format("'{0}' is a valid Byte.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid Byte.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsUint:
                     if (testResult) AppendText(string.Format("'{0}' is a valid Uint.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid Uint.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsInt:
                     if (testResult) AppendText(string.Format("'{0}' is a valid Int.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid Int.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                case TestType.IsPostalCode:
                     if (testResult) AppendText(string.Format("'{0}' is a valid PostalCode.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     else AppendText(string.Format("'{0}' is an invalid PostalCode.{1}\r\n", textToValidate, msgDetails), paragraph, brush);
                     break;
                default:
                    break;
            }
        }

        #region GUI

        private void RegisterTestTesult(TestType testType, string textToValidate, bool waitedResult, Paragraph paragraph)
        {
            Brush brush;
            string msgError;
            bool testResult = DoTestTesult(testType, textToValidate, out msgError);
            if (testResult == waitedResult)
            {
                brush = (testResult) ? Brushes.DarkGreen : Brushes.DarkRed;
                RegisterTestTesult(testType, testResult, msgError, textToValidate, paragraph, brush);
            }
            else RegisterTestTesult(testType, textToValidate, testResult, msgError, Brushes.Red);
        }

        private void RegisterTestTesult(TestType testType, string textToValidate)
        {
            string msgError;
            bool testResult = DoTestTesult(testType, textToValidate, out msgError);
            RegisterTestTesult(testType, textToValidate, testResult, msgError, Brushes.SteelBlue);
        }

        private void RegisterTestTesult(TestType testType, string textToValidate, bool testResult, string msgError, Brush brush)
        {
            Paragraph paragraph = new Paragraph();
            RegisterTestTesult(testType, testResult, msgError, textToValidate, paragraph, brush);
            rtbResult.Document.Blocks.Add(paragraph);
        }

        private void AppendText(string textToAppend, Paragraph paragraph, Brush brush)
        {
            paragraph.Inlines.Add(textToAppend);
            paragraph.Foreground = brush;
        }

        #endregion
    }
}
