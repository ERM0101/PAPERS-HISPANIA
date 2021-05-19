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
    public static class StockTakingsReportView
    {
        #region Attributes

        private static Font ForeFontHeaderTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK);

        private static Font ForeFontItemTable = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK);

        #endregion

        #region Methods

        /// <summary>
        /// Create the Report with the information of the Revisions selecteds.
        /// </summary>
        /// <param name="revisions">Customers selecteds.</param>
        public static void CreateReport(Dictionary<string, List<StockTakingsView>> stockTakingFamily)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, 
                      ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Listing, "Inventari", out string PDF_FileName);
                doc.AddTitle("Llistat d'Inventari");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(1);
                Details.Add(new Tuple<string, int, string>("Numero de famílies : ", 4, stockTakingFamily.Count.ToString()));
                CommonReportView.InsertTitle(doc, "LLISTAT D'INVENTARI", Details, 55);
                decimal AmountExpression_1 = 0;
                foreach (KeyValuePair<string, List<StockTakingsView>> family in stockTakingFamily)
                {
                    AddFamilyInfo(doc, family.Key, family.Value, out decimal AmountExpression_1_Family);
                    AmountExpression_1 += AmountExpression_1_Family;
                }
                doc.Add(CreateAmountInfo(AmountExpression_1));
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

        private static void AddFamilyInfo(Document doc, string FamilyName, List<StockTakingsView> stockTakings, out decimal AmountExpression_1_Family)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(13)
            {
                {new Tuple<string, int, PDF_Align>("ARTICLE", 2, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("DESCRIPCIÓ", 7, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("CODI UNITAT", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PREU COST", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("PREU MIG", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("ESTOC (UE)", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("ESTOC (UF)", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("VALOR EXIST.", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("MÍNIM", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("ENTRADES (UE)", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("ENTRADES (UF)", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("SORTIDES (UE)", 2, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("SORTIDES (UF)", 2, PDF_Align.Center) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(stockTakings.Count);
            AmountExpression_1_Family = 0;
            foreach (StockTakingsView stockTaking in stockTakings)
            {
                decimal Amount_Expression = GlobalViewModel.GetValueDecimalForCalculations(stockTaking.Expression_1, DecimalType.Currency);
                List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(13)
                {
                    new Tuple<string, PDF_Align>(stockTaking.GoodCode, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(stockTaking.Good_Description, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromIntValue(stockTaking.UnitType), PDF_Align.Center),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Price_Cost, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Average_Price_Cost, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Shipping_Unit_Stocks, DecimalType.Unit), PDF_Align.Center),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Billing_Unit_Stocks, DecimalType.Unit), PDF_Align.Center),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(Amount_Expression, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromIntValue(stockTaking.Minimum), PDF_Align.Center),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Shipping_Unit_Entrance, DecimalType.Unit), PDF_Align.Center),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Billing_Unit_Entrance, DecimalType.Unit), PDF_Align.Center),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Shipping_Unit_Departure, DecimalType.Unit), PDF_Align.Center),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(stockTaking.Billing_Unit_Departure, DecimalType.Unit), PDF_Align.Center),
                };
                Items.Add(item);
                AmountExpression_1_Family += Amount_Expression;
            }
            doc.Add(ReportView.CreateTable(Columns, Items));
            doc.Add(CreateAmountFamilyInfo(FamilyName, AmountExpression_1_Family));
            doc.Add(ReportView.NewLine());
        }

        private static PdfPTable CreateAmountFamilyInfo(string FamilyName, decimal AmountValue)
        {
            string Title = string.Format("TOTAL VALOR FAMÍLIA : {0}", FamilyName);
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(1)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>(Title, 8, PDF_Align.Center, BaseColor.WHITE) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(1)
            {
                new Tuple<string, PDF_Align>(AmountValue.ToString("0.00 €"), PDF_Align.Center),
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable);
        }

        private static PdfPTable CreateAmountInfo(decimal AmountValue)
        {
            string Title = "TOTAL VALOR DE L'INVENTARI";
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(1)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>(Title, 8, PDF_Align.Center, BaseColor.WHITE) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(1)
            {
                new Tuple<string, PDF_Align>(AmountValue.ToString("0.00 €"), PDF_Align.Center),
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable);
        }

        #endregion
    }
}
