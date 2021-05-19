#region Librerias usadas por la clase

using iTextSharp.text;
using iTextSharp.text.pdf;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Source Report : 
    /// Report Description : Revisions
    /// Source Query : Revisions
    /// </summary>
    public static class DeliveryNoteLinesReportView
    {
        /// <summary>
        /// Create the Report with the information of the Revisions selecteds.
        /// </summary>
        /// <param name="revisions">Customers selecteds.</param>
        public static void CreateReport()
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                List<HispaniaCompData.DeliveryNoteLine> info = GlobalViewModel.Instance.HispaniaViewModel.ReadInfoForDeliveryNoteLines();
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Listing, "LiniesAlbara", out string PDF_FileName);
                doc.AddTitle("Llistat de línies d'albarà");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(1)
                {
                    new Tuple<string, int, string>("Numero de registres : ", 4, info.Count.ToString())
                };
                CommonReportView.InsertTitle(doc, "LLISTAT DE LÍNIES D'ALBARÀ", Details, 55);
                List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(10)
                {
                    {new Tuple<string, int, PDF_Align>("Nº ALB.", 1, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("CONF.", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("Nº CLIENT", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("EMPRESA", 3, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("ARTICLE", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("DESCRIPCIÓ", 3, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("UFAC", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("UEXP", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("TIPUS ENVIAMENT", 4, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("DATA", 1, PDF_Align.Center) },
                };
                List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(info.Count);
                foreach (HispaniaCompData.DeliveryNoteLine deliveryNoteLine in info)
                {
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(10)
                    {
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromIntIdValue(deliveryNoteLine.DeliveryNoteId), PDF_Align.Left),
                        new Tuple<string, PDF_Align>(deliveryNoteLine.According, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromIntIdValue(deliveryNoteLine.CustomerId), PDF_Align.Center),
                        new Tuple<string, PDF_Align>(deliveryNoteLine.CustomerCompanyName, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(deliveryNoteLine.GoodCode, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(deliveryNoteLine.GoodDescription, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(deliveryNoteLine.UnitBilling, DecimalType.Unit), PDF_Align.Center),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(deliveryNoteLine.UnitShipping, DecimalType.Unit), PDF_Align.Center),
                        new Tuple<string, PDF_Align>(deliveryNoteLine.SendType, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDateTimeValue((DateTime) deliveryNoteLine.Date), PDF_Align.Center),
                    };
                    Items.Add(item);
                }
                doc.Add(ReportView.CreateTable(Columns, Items));
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
    }
}
