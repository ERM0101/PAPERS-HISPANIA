#region Librerias usadas por la clase

using System;
using System.Collections.Generic;

#endregion

namespace HispaniaCommon.ViewModel
{
    public partial class MaintenanceForViewModel
    {
        #region Attributes

        private Dictionary<decimal, string> ConversionValues = new Dictionary<decimal, string>()
        {
            { 0, "CERO" },
            { 1, "UNO" },
            { 2, "DOS" },
            { 3, "TRES" },
            { 4, "CUATRO" },
            { 5, "CINCO" },
            { 6, "SEIS" },
            { 7, "SIETE" },
            { 8, "OCHO" },
            { 9, "NUEVE" },
            { 10, "DIEZ" },
            { 11, "ONCE" },
            { 12, "DOCE" },
            { 13, "TRECE" },
            { 14, "CATORCE" },
            { 15, "QUINCE" },
            { 20, "VEINTE" },
            { 30, "TREINTA" },
            { 40, "CUARENTA" },
            { 50, "CINCUENTA" },
            { 60, "SESENTA" },
            { 70, "SETENTA" },
            { 80, "OCHENTA" },
            { 90, "NOVENTA" },
            { 100, "CIEN" },
            { 500, "QUINIENTOS" },
            { 700, "SETECIENTOS" },
            { 900, "NOVECIENTOS" },
        };

        #endregion

        #region Conversion Functions

        public string ConvertMoneyToString(string MoneyValue)
        {
            if (decimal.TryParse(MoneyValue, out decimal MoneyDecimalValue))
            {
                decimal IntegerPart = Math.Truncate(MoneyDecimalValue);
                decimal DecimalPart = Math.Round((MoneyDecimalValue - IntegerPart) * 100, 2);
                return string.Format("{0}{1}",
                                     GetStringValue(IntegerPart),
                                     (DecimalPart > 0) ? string.Format(" CON {0} CENTIMOS", GetStringValue(DecimalPart)) : string.Empty);
            }
            else throw new ArgumentException(string.Format("Error, Wrong money value '{0}'.", MoneyDecimalValue));
        }

        public string ConvertMoneyToString(decimal MoneyDecimalValue)
        {
            if (MoneyDecimalValue <= 0) MoneyDecimalValue = -MoneyDecimalValue;
            decimal IntegerPart = Math.Truncate(MoneyDecimalValue);
            decimal DecimalPart = Math.Round((MoneyDecimalValue - IntegerPart) * 100, 2);
            return string.Format("{0}{1}",
                                    GetStringValue(IntegerPart),
                                    (DecimalPart > 0) ? string.Format(" CON {0} CENTIMOS", GetStringValue(DecimalPart)) : string.Empty);
        }

        private string GetStringValue(decimal value)
        {
            string Num2Text = string.Empty;
            value = Math.Truncate(value);
            if (value <= 15) Num2Text = ConversionValues[value];
            else if (value < 20) Num2Text = "DIECI" + GetStringValue(value - 10);
            else if (value == 20) Num2Text = ConversionValues[value];
            else if (value < 30) Num2Text = "VEINTI" + GetStringValue(value - 20);
            else if ((value == 30) || (value == 40) || (value == 50) || (value == 60) || (value == 70) || (value == 80) || (value == 90))
            {
                Num2Text = ConversionValues[value];
            }
            else if (value < 100) Num2Text = GetStringValue(Math.Truncate(value / 10) * 10) + " Y " + GetStringValue(value % 10);
            else if (value == 100) Num2Text = ConversionValues[value];
            else if (value < 200) Num2Text = "CIENTO " + GetStringValue(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
            {
                Num2Text = GetStringValue(Math.Truncate(value / 100)) + "CIENTOS";
            }
            else if ((value == 500) || (value == 700) || (value == 900))
            {
                Num2Text = ConversionValues[value];
            }
            else if (value < 1000) Num2Text = GetStringValue(Math.Truncate(value / 100) * 100) + " " + GetStringValue(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + GetStringValue(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = GetStringValue(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + GetStringValue(value % 1000);
            }
            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + GetStringValue(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = GetStringValue(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + GetStringValue(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + GetStringValue(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                Num2Text = GetStringValue(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + GetStringValue(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }

        #endregion
    }
}