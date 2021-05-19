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
    /// Report Description : list the Providers
    /// Source Query : Providers selecteds
    /// </summary>
    public static class BadDebtsReportView
    {
        #region Attributes

        private static Font ForeFontHeaderTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK);

        private static Font ForeFontItemTable = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK);

        #endregion

        /// <summary>
        /// Create the Report with the information of the Providers selecteds.
        /// </summary>
        /// <param name="badDebts">Providers selecteds.</param>
        public static void CreateReport(List<BadDebtsView> badDebts)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.BadDebt, "Impagats", out string PDF_FileName);
                doc.AddTitle("Informe d'impagats seleccionats de la finestra d'impagats");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(2);
                Details.Add(new Tuple<string, int, string>("Numero d'Impagats seleccionats :", 4, badDebts.Count.ToString()));
                CommonReportView.InsertTitle(doc, "INFORME D'IMPAGATS", Details, 60);

                List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>
                {
                    { new Tuple<string, int, PDF_Align>("Nº CLIENT", 2, PDF_Align.Left) },
                    { new Tuple<string, int, PDF_Align>("CLIENT", 6, PDF_Align.Left) },
                    { new Tuple<string, int, PDF_Align>("Nº REBUT", 2, PDF_Align.Center) },
                    { new Tuple<string, int, PDF_Align>("DATA EXPEDICIÓ", 3, PDF_Align.Center) },
                    { new Tuple<string, int, PDF_Align>("DATA VENCIMENT", 3, PDF_Align.Center) },
                    { new Tuple<string, int, PDF_Align>("IMPORT PENDENT", 2, PDF_Align.Center) },
                };
                List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(7);
                decimal TotalAmount = 0;
                foreach (BadDebtsView badDebt in badDebts)
                {
                    TotalAmount += badDebt.Amount_Pending;
                    BillsView bill = GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(badDebt.Bill_Id, badDebt.Bill_Year);
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
                    {
                        new Tuple<string, PDF_Align>(bill.Customer_Id_Str, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(bill.Customer.Company_Name, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromIntIdValue(badDebt.Receipt_Id), PDF_Align.Center),
                        new Tuple<string, PDF_Align>(badDebt.Bill_Date_Str, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(badDebt.Receipt_Expiration_Date_Str, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(badDebt.Amount_Pending_Str, PDF_Align.Right),
                    };
                    Items.Add(item);
                }
                doc.Add(ReportView.CreateTable(Columns, Items));
                doc.Add(CreateAmountInfo(TotalAmount));
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

        #region Bad Debt Amount

        private static PdfPTable CreateAmountInfo(decimal AmountValue)
        {
            string Title = "TOTAL VALOR PENDENT DE COBRAR";
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
