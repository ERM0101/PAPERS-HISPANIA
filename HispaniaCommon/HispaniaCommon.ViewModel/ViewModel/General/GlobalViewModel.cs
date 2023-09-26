#region Librerias usadas por la clase

using MBCode.Framework.Credentials;
using MBCode.Framework.Managers.Culture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace HispaniaCommon.ViewModel
{
    public enum DecimalType
    {
        Percent,
        Currency,
        Unit,
        Stock,
        Normal,
        WithoutDecimals,
    }

    public enum DateTimeFormat
    {
        UI,
        Report,
        LongFormat,
    }

    public partial class GlobalViewModel
    {
        #region Attributes

        /// <summary>
        /// Cultura to use in the reports and UI and in the filter of dates.
        /// </summary>
        public static CultureInfo UICulture = new CultureInfo("ca-ES");

        #region Credentials

        /// <summary>
        /// Store the Credential that use the user for access to Closing Menu.
        /// </summary>
        public Credential CredentialAccessClosingMenu = null;

        /// <summary>
        /// Store the Credential that use the user for access to Closing Menu.
        /// </summary>
        public Dictionary<string, Credential> AllCredentials = null;

        #endregion

        #endregion

        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static GlobalViewModel instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static GlobalViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalViewModel();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private GlobalViewModel()
        {
            CredentialAccessClosingMenu = new Credential(Guid.NewGuid(), Guid.NewGuid(), "HISPANIA", "ec831cf4ca11c34b75366e834d004e75");
            AllCredentials = new Dictionary<string, Credential>
            {
                { CredentialAccessClosingMenu.UserName, CredentialAccessClosingMenu }
            };
            CultureManager.ActualCulture = new CultureInfo(UICulture.Name);
        }

        #endregion

        #region ViewModel to use

        /// <summary>
        /// Store the reference at the Application Type defined for the instance of the class, by default : Undefined.
        /// </summary>
        private MaintenanceForViewModel _HispaniaViewModel = null;

        /// <summary>
        /// Allow the application to access or change the reference at the Application Type defined for the instance of the class.
        /// </summary>
        public MaintenanceForViewModel HispaniaViewModel
        {
            get
            {
                return _HispaniaViewModel;
            }
            set
            {
                _HispaniaViewModel = value;
            }
        }

        #endregion

        #region Get Value Operations

        #region String

        public const byte PERCENT_ACCURANCY = 2;

        public const byte CURRENCY_ACCURANCY = 2;

        public const byte UNIT_ACCURANCY = 3;

        public const byte DEFAULT_ACCURANCY = 2;

        public const byte STOCK_ACCURANCY = 4;

        public static string NormalizeDecimalValue(string DecimalInput_Str, DecimalType decimalType)
        {
            switch (decimalType)
            {
                case DecimalType.Percent:
                     return NormalizeDecimalPercentDecimalValue(DecimalInput_Str);
                case DecimalType.Currency:
                     return NormalizeDecimalCurrencyDecimalValue(DecimalInput_Str);
                case DecimalType.Unit:
                     return NormalizeDecimalUnitDecimalValue(DecimalInput_Str);
                case DecimalType.Stock:
                     return NormalizeDecimalStockValue(DecimalInput_Str);
                case DecimalType.WithoutDecimals:
                     return NormalizeDecimalValueInternal(DecimalInput_Str, 0);
                case DecimalType.Normal:
                default:
                     return NormalizeDecimalValueInternal(DecimalInput_Str);
            }
        }

        public static string GetStringFromDecimalValue(decimal? DecimalInput, DecimalType decimalType, bool WithSign = false)
        {
            switch (decimalType)
            {
                case DecimalType.Percent:
                     return GetStringFromPercentDecimalValue(DecimalInput) + (WithSign ? " %" : string.Empty);
                case DecimalType.Currency:
                     return GetStringFromCurrencyDecimalValue(DecimalInput) + (WithSign ? " €" : string.Empty);
                case DecimalType.Unit:
                     return GetStringFromUnitDecimalValue(DecimalInput);
                case DecimalType.Stock:
                     return GetStringFromStockValue(DecimalInput);
                case DecimalType.WithoutDecimals:
                     return GetStringFromDecimalValueInternal(DecimalInput, 0);
                case DecimalType.Normal:
                default:
                     return GetStringFromDecimalValueInternal(DecimalInput);
            }
        }

        public static string GetStringFromDateTimeValue(DateTime? DateTimeInput, DateTimeFormat Format = DateTimeFormat.UI)
        {
            string pattern;

            switch( Format )
            {
                case DateTimeFormat.Report:
                     pattern = "dd-MM-yyyy";
                     break;
                case DateTimeFormat.LongFormat:
                     pattern = "dddd, dd MMMM yyyy";
                     break;
                case DateTimeFormat.UI:
                default:
                     pattern = "dd/MM/yyyy";
                     break;
            }
            string result;

            if(DateTimeInput is null || DateTimeInput == DateTime.MinValue )
            {
                result = (Format == DateTimeFormat.UI ? "No Informat" : string.Empty);
            }
            else
            {
                result = DateTimeInput.Value.ToString( pattern ); //, UICulture );
            }

            return result;
        }

        public static string GetStringFromPostalCode(PostalCodesView PostalCode)
        {
            return (PostalCode is null) ? "No Informat"
                                        : string.Format("{0}, {1}", PostalCode.Postal_Code, PostalCode.City);
        }

        public static string GetStringFromYearValue(decimal? DecimalInput, bool ForValidate = false)
        {
            return DecimalInput is null || DecimalInput == YearInitValue ? ForValidate ? string.Empty : "No Informat"
                                                                         : GetStringFromDecimalValueInternal(DecimalInput, 0);
        }

        public static string GetStringFromBillingSerieValue(BillingSeriesView BillingSerie)
        {
            return (BillingSerie is null) ? "No Informat" : BillingSerie.Name;
        }

        public static string GetStringFromIntValue(int? IntInput)
        {
            return IntInput?.ToString();
        }

        public static string GetStringFromIntIdValue(int? IntInput)
        {
            return (IntInput == IntIdInitValue) ? "No Informat" : IntInput?.ToString("0");
        }

        public static decimal GetValueDecimalForCalculations(decimal inputValue, DecimalType decimalType = DecimalType.Currency)
        {
            return GetValueDecimalForCalculations(inputValue, GetDecimalDigits(decimalType));
        }


        #region Helpers

        private static string GetStringFromPercentDecimalValue(decimal? DecimalInput)
        {
            return GetStringFromDecimalValueInternal(DecimalInput, PERCENT_ACCURANCY);
        }

        private static string GetStringFromCurrencyDecimalValue(decimal? DecimalInput)
        {
            return GetStringFromDecimalCurrencyValueInternal(DecimalInput, CURRENCY_ACCURANCY);
        }

        private static string GetStringFromUnitDecimalValue(decimal? DecimalInput)
        {
            return GetStringFromDecimalValueInternal(DecimalInput, UNIT_ACCURANCY);
        }

        private static string GetStringFromStockValue(decimal? DecimalInput)
        {
            return GetStringFromDecimalValueInternal(DecimalInput, STOCK_ACCURANCY);
        }

        private static string GetStringFromDecimalValueInternal(decimal? DecimalInput, byte NumDecimals = DEFAULT_ACCURANCY)
        {
            string Str = string.Empty;
            if (DecimalInput != null)
            {
                string Mask = string.Format("F{0}", NumDecimals);
                Str = GetValueDecimalForCalculations((decimal)DecimalInput, NumDecimals).ToString(Mask, CultureInfo.CurrentCulture);
                int IndexQuota = Str.IndexOf(',');
                if (IndexQuota != -1)
                {
                    string fraction = Str.Substring(IndexQuota + 1, Str.Length - IndexQuota - 1);
                    if (fraction.Replace("0", string.Empty).Length == 0)
                    {
                        if (IndexQuota != -1) Str = Str.Remove(IndexQuota);
                    }
                    else Str = Str.Remove(IndexQuota + 1) + fraction.TrimEnd(new char[] { '0' });
                }
            }
            return Str;
        }

        private static string GetStringFromDecimalCurrencyValueInternal(decimal? DecimalInput, byte NumDecimals = DEFAULT_ACCURANCY)
        {
            string Str = string.Empty;
            if (DecimalInput != null)
            {
                string Mask = string.Format("F{0}", NumDecimals);
                Str = GetValueDecimalForCalculations((decimal)DecimalInput, NumDecimals).ToString(Mask, CultureInfo.CurrentCulture);
            }
            return Str;
        }

        private static string NormalizeDecimalPercentDecimalValue(string DecimalInput_Str)
        {
            return NormalizeDecimalValueInternal(DecimalInput_Str, PERCENT_ACCURANCY);
        }

        private static string NormalizeDecimalCurrencyDecimalValue(string DecimalInput_Str)
        {
            return NormalizeDecimalCurrencyValueInternal(DecimalInput_Str, CURRENCY_ACCURANCY);
        }

        private static string NormalizeDecimalUnitDecimalValue(string DecimalInput_Str)
        {
            return NormalizeDecimalValueInternal(DecimalInput_Str, UNIT_ACCURANCY);
        }

        private static string NormalizeDecimalStockValue(string DecimalInput_Str)
        {
            return NormalizeDecimalValueInternal(DecimalInput_Str, STOCK_ACCURANCY);
        }

        private static string NormalizeDecimalValueInternal(string DecimalInput_Str, byte NumDecimals = DEFAULT_ACCURANCY)
        {
            if (!String.IsNullOrEmpty(DecimalInput_Str))
            {
                int IndexQuota = DecimalInput_Str.IndexOf(',');
                string fraction = DecimalInput_Str.Substring(IndexQuota + 1, DecimalInput_Str.Length - IndexQuota - 1);
                if (fraction.Length > NumDecimals)
                {
                    DecimalInput_Str = DecimalInput_Str.Substring(0, IndexQuota + 1) + fraction.Substring(0, NumDecimals);
                }
                //else if ((fraction.Length == NumDecimals) && (fraction[NumDecimals - 1] == '0')) 
                //{
                //    fraction = fraction.TrimEnd(new char[] { '0' });
                //    DecimalInput_Str = DecimalInput_Str.Remove(IndexQuota + 1) +
                //                       fraction.Substring(0, Math.Min(fraction.Length, NumDecimals));
                //}
            }
            return DecimalInput_Str;
        }

        private static string NormalizeDecimalCurrencyValueInternal(string DecimalInput_Str, byte NumDecimals = DEFAULT_ACCURANCY)
        {
            if (!String.IsNullOrEmpty(DecimalInput_Str))
            {
                int IndexQuota = DecimalInput_Str.IndexOf(',');
                string fraction = DecimalInput_Str.Substring(IndexQuota + 1, DecimalInput_Str.Length - IndexQuota - 1);
                if (fraction.Length > NumDecimals)
                {
                    DecimalInput_Str = DecimalInput_Str.Substring(0, IndexQuota + 1) + fraction.Substring(0, NumDecimals);
                }
            }
            return DecimalInput_Str;
        }

        private static decimal GetValueDecimalForCalculations(decimal inputValue, byte NumDecimals = 2)
        {
            return Math.Round(inputValue, NumDecimals, MidpointRounding.AwayFromZero);
        }

        private static byte GetDecimalDigits(DecimalType decimalType)
        {
            switch (decimalType)
            {
                case DecimalType.Percent:
                     return PERCENT_ACCURANCY;
                case DecimalType.Currency:
                     return CURRENCY_ACCURANCY;
                case DecimalType.Unit:
                     return UNIT_ACCURANCY;
                case DecimalType.WithoutDecimals:
                     return 0;
                case DecimalType.Normal:
                default:
                     return DEFAULT_ACCURANCY;
            }
        }

        #endregion

        #endregion

        #region Bool

        public static bool BoolInitValue = false;

        public static bool GetBoolNullValue(bool? Value)
        {
            return Value is null ? BoolInitValue : (bool)Value;
        }

        public static bool? GetBoolNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return bool.Parse(ViewFieldText);
        }

        public static bool GetBoolValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return false;
            else return bool.Parse(ViewFieldText);
        }

        public static bool GetBoolValue(bool? BoolInput)
        {
            return (bool)(BoolInput ?? false);
        }

        #endregion

        #region Byte

        public static byte? GetByteNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return byte.Parse(ViewFieldText);
        }

        public static byte GetByteValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return 0;
            else return byte.Parse(ViewFieldText);
        }

        public static byte GetByteValue(byte? ByteInput)
        {
            return (byte)(ByteInput ?? 0);
        }

        #endregion

        #region Short

        public static short? GetShortNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return short.Parse(ViewFieldText);
        }

        public static short GetShortValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return 0;
            else return short.Parse(ViewFieldText);
        }

        public static short GetShortValue(short? ShortInput)
        {
            return (short)(ShortInput ?? 0);
        }

        #endregion

        #region Uint

        public static uint? GetUintNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return uint.Parse(ViewFieldText);
        }

        public static uint GetUintValue(string ViewFieldText)
        {
            return uint.Parse((string.IsNullOrEmpty(ViewFieldText)) ? "0" : ViewFieldText);
        }

        public static uint GetUintValue(uint? IntInput)
        {
            return (uint)(IntInput ?? 0);
        }

        #endregion

        #region Int

        public static int? GetIntNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return int.Parse(ViewFieldText);
        }

        public static int IntInitValue = -1; // Aquest valor no es pot modificar
        public static int GetIntValue(string ViewFieldText)
        {
            return string.IsNullOrEmpty(ViewFieldText) ? IntInitValue : int.Parse(ViewFieldText);
            //return int.Parse((string.IsNullOrEmpty(ViewFieldText)) ? "0" : ViewFieldText);
        }

        public static int IntIdInitValue = -1; // Aquest valor no es pot modificar

        public static int GetIntFromIntIdValue(int? IntValue)
        {
            return (IntValue is null) ? IntIdInitValue : (int)IntValue;
        }

        public static int? GetIntIdDatabaseValue(int? IntValue)
        {
            return IntValue is null || IntValue == IntIdInitValue ? null : IntValue;
        }

        public static int GetIntValue(int? IntInput)
        {
            return (int)(IntInput ?? 0);
        }

        public static int GetIntFromIntFromStringValue(string sValue)
        {
            
            return int.TryParse(sValue, out int iValue) ? iValue : IntInitValue;
        }

        #endregion

        #region Decimal

        public static decimal? GetDecimalNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return decimal.Parse(ViewFieldText);
        }

        public static decimal GetDecimalValue(string ViewFieldText)
        {
            return decimal.Parse((string.IsNullOrEmpty(ViewFieldText)) ? "0" : ViewFieldText);
        }

        public static decimal DecimalInitValue = 0;

        public static decimal GetDecimalValue(decimal? DecimalInput)
        {
            return ((DecimalInput is null) ? DecimalInitValue : (decimal)DecimalInput);
        }

        //public static decimal YearInitValue = 0;

        public static decimal YearInitValue = (decimal)DateTimeInitValue.Year;

        public static decimal GetDecimalYearValue(decimal? DecimalInput)
        {
            return ((DecimalInput is null) ? YearInitValue : (decimal)DecimalInput);
        }

        public static decimal GetCurrencyValueFromDecimal(decimal DecimalInput)
        {
            return decimal.Parse(string.Format("{0:0.00}", DecimalInput));
        }

        #endregion

        #region Float

        public static float? GetFloatNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return float.Parse(ViewFieldText);
        }

        public static float GetFloatValue(string ViewFieldText)
        {
            return float.Parse((string.IsNullOrEmpty(ViewFieldText)) ? "0" : ViewFieldText);
        }

        public static float GetFloatValue(float? FloatInput)
        {
            return ((FloatInput == null) ? 0 : (float)FloatInput);
        }

        #endregion

        #region Double

        public static double? GetDoubleNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return double.Parse(ViewFieldText);
        }

        public static double GetDoubleValue(string ViewFieldText)
        {
            return double.Parse((string.IsNullOrEmpty(ViewFieldText)) ? "0" : ViewFieldText);
        }

        public static double GetDoubleValue(double? DoubleInput)
        {
            return ((DoubleInput == null) ? 0 : (double)DoubleInput);
        }

        #endregion

        #region DateTime

        public static DateTime? GetDateTimeNullValue(string ViewFieldText)
        {
            if (string.IsNullOrEmpty(ViewFieldText)) return null;
            else return DateTime.Parse(ViewFieldText);
        }

        public static DateTime GetDateTimeValue(string ViewFieldText)
        {
            return ((string.IsNullOrEmpty(ViewFieldText)) ? DateTime.Now : DateTime.Parse(ViewFieldText));
        }

        public static DateTime DateTimeInitValue = DateTime.MinValue;

        public static DateTime GetDateTimeValue(DateTime? DateTimeInput)
        {
            return ((DateTimeInput is null) ? DateTimeInitValue : (DateTime)DateTimeInput);
        }

        public static DateTime? GetDateTimeDatabaseValue(DateTime? DateTimeInput)
        {
            return (((DateTimeInput == DateTimeInitValue) || (DateTimeInput == null)) ? null : DateTimeInput);
        }

        public static Decimal? GetYearDatabaseValue(DateTime? DateTimeInput, Decimal Year)
        {
            return (DateTimeInput == GlobalViewModel.DateTimeInitValue) ? (decimal?)null : Year;
        }

        public static string GetDateForUI(DateTime ViewFieldDate)
        {
            #pragma warning disable IDE0031 // Dont't apply at DateTime type
            return (ViewFieldDate == null) ? null : ViewFieldDate.ToString("dd MMMM yyyy", UICulture);
            #pragma warning restore IDE0031 
        }

        public static string GetLongDateString(DateTime ViewFieldDate)
        {
            #pragma warning disable IDE0031 // Dont't apply at DateTime type
            return (ViewFieldDate == null) ? null : ViewFieldDate.ToString("D", UICulture).Replace(@"/", "de");
            #pragma warning restore IDE0031
        }

        #endregion

        #region Enum

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return value.ToString();
        }

        #endregion

        #endregion

        #region Manage Decimal Values

        public static decimal GetUIDecimalValue(string ViewFieldText)
        {
            return decimal.Parse(string.IsNullOrEmpty(ViewFieldText) || ViewFieldText == "-" ? "0" :  ViewFieldText.Replace(".", ","));
        }

        public static void NormalizeTextBox(object sender, TextChangedEventArgs e, DecimalType decimalType = DecimalType.Normal)
        {
            var tb = (TextBox)sender;
            using (tb.DeclareChangeBlock())
            {
                int CaretPosition = tb.CaretIndex;
                foreach (var c in e.Changes)
                {
                    if (c.AddedLength == 0) continue;
                    tb.Select(c.Offset, c.AddedLength);
                    if (tb.SelectedText.Contains("."))
                    {
                        tb.SelectedText = tb.SelectedText.Replace('.', ',');
                    }
                    tb.Select(c.Offset + c.AddedLength, 0);
                }
                if (tb.Text.Contains(","))
                {
                    tb.Text = NormalizeDecimalValue(tb.Text, decimalType);
                    tb.CaretIndex = CaretPosition; 
                }
            }
        }

        #endregion

        #region Extended Operations With Strings

        /// <summary>
        /// If string_1 == string_2 returns 0
        /// If string_1 > string_2 returns 1
        /// If string_1 < string_2 returns 2
        /// </summary>
        /// <param name="string_1">First string to compare</param>
        /// <param name="string_2">Second string to compare</param>
        /// <returns></returns>
        public static int CompareStringValues(string string_1, string string_2)
        {
            if (String.IsNullOrEmpty(string_1) && String.IsNullOrEmpty(string_2)) return 0;
            else if (String.IsNullOrEmpty(string_1) && (!String.IsNullOrEmpty(string_2))) return 2;
            else if ((!String.IsNullOrEmpty(string_1)) && String.IsNullOrEmpty(string_2)) return 1;
            else
            {
                StringBuilder sb_1 = new StringBuilder(string_1.ToUpper());
                StringBuilder sb_2 = new StringBuilder(string_2.ToUpper());
                for (int i = 0; i < string_1.Length; i++)
                {
                    if (i >= sb_2.Length) return 1;
                    else if (sb_1[i] == sb_2[i]) continue;
                    else if (sb_1[i] < sb_2[i]) return 2;
                    else return 1;
                }
                if (sb_2.Length > sb_1.Length) return 2;
                else return 0;
            }
        }

        #endregion

        #region Action with Controls

        public void SelectAllTextInGotFocusEvent(object sender, RoutedEventArgs e)
        {
            //  Set the event as handled
                e.Handled = true;
            //  Select the Text
                (sender as TextBox).SelectAll();
        }

        #endregion
    }
}
