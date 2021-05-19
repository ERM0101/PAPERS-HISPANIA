#region Librerias usadas por la clase

using iTextSharp.text;
using iTextSharp.text.pdf;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Source Report : 
    /// Report Description : Revisions
    /// Source Query : Revisions
    /// </summary>
    public static class RangesReportView
    {
        #region Attributes

        private static Font ForeFontHeaderTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK);

        private static Font ForeFontItemTable = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK);

        #endregion

        #region Methods

        /// <summary>
        /// Create the Report with the information of the Revisions selecteds.
        /// </summary>
        /// <param name="rangeFamily">Ranges selecteds filtereds by family.</param>
        public static void CreateReport(Dictionary<string, List<RangesView>> rangeFamily, string MonthValue, 
                                        int? MonthNumber)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, 
                      ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Listing, "Marges", out string PDF_FileName);
                doc.AddTitle("Llistat de Marges");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(1)
                {
                    new Tuple<string, int, string>("Numero de famílies : ", 4, rangeFamily.Count.ToString())
                };
                string Title = string.Format("LLISTAT DE MARGE COMERCIAL - MES : {0}",
                                             MonthValue is null ? DateTime.Now.ToString("MMMM", GlobalViewModel.UICulture).ToUpper() 
                                                                : MonthValue);
                CommonReportView.InsertTitle(doc, Title, Details, 55);
                decimal AmountCSCByMonth = 0;
                decimal AmountCSRPyMonth = 0;
                decimal AmountCSCByYear = 0;
                decimal AmountCSRPyYear = 0;
                foreach (KeyValuePair<string, List<RangesView>> family in rangeFamily)
                {
                    AddFamilyInfo(doc, family.Key, family.Value, MonthNumber, 
                                  out decimal AmountCSCByMonth_Family, out decimal AmountCSRPyMonth_Family,
                                  out decimal AmountCSCByYear_Family, out decimal AmountCSRPyYear_Family);
                    AmountCSCByMonth += AmountCSCByMonth_Family;
                    AmountCSRPyMonth += AmountCSRPyMonth_Family;
                    AmountCSCByYear += AmountCSCByYear_Family;
                    AmountCSRPyYear += AmountCSRPyYear_Family;
                }
                doc.Add(CreateAmountInfo(AmountCSCByMonth, AmountCSRPyMonth, AmountCSCByYear, AmountCSRPyYear));
                doc.Add(ReportView.NewLine());
                doc.Close();
                writer.Close();
                ReportView.AddPageNumber(PDF_FileName, PDF_Orientation.Horizontal);
                Process.Start(PDF_FileName);
            }
            catch (Exception ex)
            {
                if ((doc != null) && (doc.IsOpen())) doc.Close();
                if (writer != null) writer.Close();
                MsgManager.ShowMessage(string.Format("Error al construïr el PDF.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        private static void AddFamilyInfo(Document doc, 
                                          string FamilyName, 
                                          List<RangesView> ranges, 
                                          int? MonthNumber,
                                          out decimal AmountCSCByMonth_Family,
                                          out decimal AmountCSRPyMonth_Family,
                                          out decimal AmountCSCByYear_Family,
                                          out decimal AmountCSRPyYear_Family)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(8)
            {
                {new Tuple<string, int, PDF_Align>("ARTICLE", 2, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("DESCRIPCIÓ", 7, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("VENDES MES P.M.C.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES MES P.V.P.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PERCENT. DIF.", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES ANY P.M.C.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES ANY P.V.P.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PERCENT. DIF.", 2, PDF_Align.Center) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(ranges.Count);
            AmountCSCByMonth_Family = 0;
            AmountCSRPyMonth_Family = 0;
            AmountCSCByYear_Family = 0;
            AmountCSRPyYear_Family = 0;
            foreach (RangesView range in ranges)
            {
                decimal CSC_ByMonth = CumulativeSalesCostByMonth(range, MonthNumber);
                decimal CSRP_ByMonth = CumulativeSalesRetailPriceByMonth(range, MonthNumber);
                decimal CSC_ByYear = CumulativeSalesCostByYear(range, MonthNumber);
                decimal CSRP_ByYear = CumulativeSalesRetailPriceByYear(range, MonthNumber);
                string MonthPercent = CalculatePercent(CSRP_ByMonth, CSC_ByMonth);
                string YearPercent = CalculatePercent(CSRP_ByYear, CSC_ByYear);
                List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(8)
                {
                    new Tuple<string, PDF_Align>(range.Good_Code, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(range.Good_Description, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(CSC_ByMonth, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(CSRP_ByMonth, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(MonthPercent, PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(CSC_ByYear, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(CSRP_ByYear, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(YearPercent, PDF_Align.Right),
                };
                Items.Add(item);
                AmountCSCByMonth_Family += CSC_ByMonth;
                AmountCSRPyMonth_Family += CSRP_ByMonth;
                AmountCSCByYear_Family += CSC_ByYear;
                AmountCSRPyYear_Family += CSRP_ByYear;
            }
            doc.Add(ReportView.CreateTable(Columns, Items));
            doc.Add(CreateAmountFamilyInfo(FamilyName, AmountCSCByMonth_Family, AmountCSRPyMonth_Family,
                                           AmountCSCByYear_Family, AmountCSRPyYear_Family));
            doc.Add(ReportView.NewLine());
        }

        private static PdfPTable CreateAmountFamilyInfo(string FamilyName,
                                                        decimal AmountCSCByMonth_Family,
                                                        decimal AmountCSRPyMonth_Family,
                                                        decimal AmountCSCByYear_Family,
                                                        decimal AmountCSRPyYear_Family)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(7)
            {
                {new Tuple<string, int, PDF_Align>("TOTAL", 9, PDF_Align.Center)},
                {new Tuple<string, int, PDF_Align>("VENDES MES P.M.C.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES MES P.V.P.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PERCENT. DIF.", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES ANY P.M.C.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES ANY P.V.P.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PERCENT. DIF.", 2, PDF_Align.Center) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            string MonthPercent = CalculatePercent(AmountCSRPyMonth_Family, AmountCSCByMonth_Family);
            string YearPercent = CalculatePercent(AmountCSRPyYear_Family, AmountCSCByYear_Family);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
            {
                new Tuple<string, PDF_Align>(string.Format("FAMÍLIA : {0}", FamilyName), PDF_Align.Center),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSCByMonth_Family, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSRPyMonth_Family, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(MonthPercent, PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSCByYear_Family, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSRPyYear_Family, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(YearPercent, PDF_Align.Right),
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable);
        }

        private static PdfPTable CreateAmountInfo(decimal AmountCSCByMonth,
                                                  decimal AmountCSRPyMonth,
                                                  decimal AmountCSCByYear,
                                                  decimal AmountCSRPyYear)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(7)
            {
                {new Tuple<string, int, PDF_Align>("TOTAL", 9, PDF_Align.Center)},
                {new Tuple<string, int, PDF_Align>("VENDES MES P.M.C.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES MES P.V.P.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PERCENT. DIF.", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES ANY P.M.C.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VENDES ANY P.V.P.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PERCENT. DIF.", 2, PDF_Align.Center) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            string MonthPercent = CalculatePercent(AmountCSRPyMonth, AmountCSCByMonth);
            string YearPercent = CalculatePercent(AmountCSRPyYear, AmountCSCByYear);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
            {
                new Tuple<string, PDF_Align>("GENERAL", PDF_Align.Center),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSCByMonth, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSRPyMonth, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(MonthPercent, PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSCByYear, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(AmountCSRPyYear, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(YearPercent, PDF_Align.Right),
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable);
        }

        #region Helpers

        private static string CalculatePercent(decimal Sales_Retail_Price, decimal Sales_Cost)
        {
            if ((Sales_Cost == 0) && (Sales_Retail_Price == 0)) return GlobalViewModel.GetStringFromDecimalValue(0, DecimalType.Percent, true);
            else if ((Sales_Cost == 0) && (Sales_Retail_Price != 0)) return GlobalViewModel.GetStringFromDecimalValue(100, DecimalType.Percent, true);
            else
            {
                return GlobalViewModel.GetStringFromDecimalValue(((Sales_Retail_Price - Sales_Cost) * 100) / Sales_Cost, DecimalType.Percent, true);
            }
        }

        private static decimal CumulativeSalesCostByMonth(RangesView range, int? MonthNumber)
        {
            if (MonthNumber is null) MonthNumber = DateTime.Now.Month;
            decimal value = 0;
            switch (MonthNumber)
            {
                case 1: value = range.Cumulative_Sales_Cost_1;
                        break;
                case 2: value = range.Cumulative_Sales_Cost_2;
                        break;
                case 3: value = range.Cumulative_Sales_Cost_3;
                        break;
                case 4: value = range.Cumulative_Sales_Cost_4;
                        break;
                case 5: value = range.Cumulative_Sales_Cost_5;
                        break;
                case 6: value = range.Cumulative_Sales_Cost_6;
                        break;
                case 7: value = range.Cumulative_Sales_Cost_7;
                        break;
                case 8: value = range.Cumulative_Sales_Cost_8;
                        break;
                case 9: value = range.Cumulative_Sales_Cost_9;
                        break;
                case 10: value = range.Cumulative_Sales_Cost_10;
                         break;
                case 11: value = range.Cumulative_Sales_Cost_11;
                         break;
                case 12: value = range.Cumulative_Sales_Cost_12;
                         break;
                default: throw new ArgumentException(
                                      string.Format("Error, numero de mes '{0}' incorrecte.", MonthNumber));
            }
            return (Normalize(value));
        }

        private static decimal CumulativeSalesCostByYear(RangesView range, int? MonthNumber)
        {
            if (MonthNumber is null) MonthNumber = DateTime.Now.Month;
            decimal value = 0;
            if (MonthNumber >= 1) value += Normalize(range.Cumulative_Sales_Cost_1);
            if (MonthNumber >= 2) value += Normalize(range.Cumulative_Sales_Cost_2);
            if (MonthNumber >= 3) value += Normalize(range.Cumulative_Sales_Cost_3);
            if (MonthNumber >= 4) value += Normalize(range.Cumulative_Sales_Cost_4);
            if (MonthNumber >= 5) value += Normalize(range.Cumulative_Sales_Cost_5);
            if (MonthNumber >= 6) value += Normalize(range.Cumulative_Sales_Cost_6);
            if (MonthNumber >= 7) value += Normalize(range.Cumulative_Sales_Cost_7);
            if (MonthNumber >= 8) value += Normalize(range.Cumulative_Sales_Cost_8);
            if (MonthNumber >= 9) value += Normalize(range.Cumulative_Sales_Cost_9);
            if (MonthNumber >= 10) value += Normalize(range.Cumulative_Sales_Cost_10);
            if (MonthNumber >= 11) value += Normalize(range.Cumulative_Sales_Cost_11);
            if (MonthNumber >= 12) value += Normalize(range.Cumulative_Sales_Cost_12);
            return (value);
        }

        private static decimal CumulativeSalesRetailPriceByMonth(RangesView range, int? MonthNumber)
        {
            if (MonthNumber is null) MonthNumber = DateTime.Now.Month;
            decimal value = 0;
            switch (MonthNumber)
            {
                case 1: value = range.Cumulative_Sales_Retail_Price_1;
                        break;
                case 2: value = range.Cumulative_Sales_Retail_Price_2;
                        break;
                case 3: value = range.Cumulative_Sales_Retail_Price_3;
                        break;
                case 4: value = range.Cumulative_Sales_Retail_Price_4;
                        break;
                case 5: value = range.Cumulative_Sales_Retail_Price_5;
                        break;
                case 6: value = range.Cumulative_Sales_Retail_Price_6;
                        break;
                case 7: value = range.Cumulative_Sales_Retail_Price_7;
                        break;
                case 8: value = range.Cumulative_Sales_Retail_Price_8;
                        break;
                case 9: value = range.Cumulative_Sales_Retail_Price_9;
                        break;
                case 10: value = range.Cumulative_Sales_Retail_Price_10;
                         break;
                case 11: value = range.Cumulative_Sales_Retail_Price_11;
                         break;
                case 12: value = range.Cumulative_Sales_Retail_Price_12;
                         break;
                default: throw new ArgumentException(
                                      string.Format("Error, numero de mes '{0}' incorrecte.", MonthNumber));
            }
            return (Normalize(value));
        }

        private static decimal CumulativeSalesRetailPriceByYear(RangesView range, int? MonthNumber)
        {
            if (MonthNumber is null) MonthNumber = DateTime.Now.Month;
            decimal value = 0;
            if (MonthNumber >= 1) value += Normalize(range.Cumulative_Sales_Retail_Price_1);
            if (MonthNumber >= 2) value += Normalize(range.Cumulative_Sales_Retail_Price_2);
            if (MonthNumber >= 3) value += Normalize(range.Cumulative_Sales_Retail_Price_3);
            if (MonthNumber >= 4) value += Normalize(range.Cumulative_Sales_Retail_Price_4);
            if (MonthNumber >= 5) value += Normalize(range.Cumulative_Sales_Retail_Price_5);
            if (MonthNumber >= 6) value += Normalize(range.Cumulative_Sales_Retail_Price_6);
            if (MonthNumber >= 7) value += Normalize(range.Cumulative_Sales_Retail_Price_7);
            if (MonthNumber >= 8) value += Normalize(range.Cumulative_Sales_Retail_Price_8);
            if (MonthNumber >= 9) value += Normalize(range.Cumulative_Sales_Retail_Price_9);
            if (MonthNumber >= 10) value += Normalize(range.Cumulative_Sales_Retail_Price_10);
            if (MonthNumber >= 11) value += Normalize(range.Cumulative_Sales_Retail_Price_11);
            if (MonthNumber >= 12) value += Normalize(range.Cumulative_Sales_Retail_Price_12);
            return (value);
        }

        private static decimal Normalize(decimal value)
        {
            return GlobalViewModel.GetValueDecimalForCalculations(value, DecimalType.Currency);
        }

        #endregion

        #endregion
    }
}
