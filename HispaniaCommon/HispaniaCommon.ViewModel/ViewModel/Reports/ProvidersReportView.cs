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
    public static class ProvidersReportView
    {
        /// <summary>
        /// Create the Report with the information of the Providers selecteds.
        /// </summary>
        /// <param name="providers">Providers selecteds.</param>
        public static void CreateReport(List<ProvidersView> providers)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Provider, "Proveidors", out string PDF_FileName);
                doc.AddTitle("Informe de Proveïdors seleccionats de la finestra de proveïdors");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(2);
                Details.Add(new Tuple<string, int, string>("Numero de Proveïdors seleccionats :", 4, providers.Count.ToString()));
                CommonReportView.InsertTitle(doc, "INFORME DE PROVEÏDORS", Details, 60);
                List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>
                {
                    { new Tuple<string, int, PDF_Align>("EMPRESA", 4, PDF_Align.Left) },
                    { new Tuple<string, int, PDF_Align>("ADREÇA", 4, PDF_Align.Left) },
                    { new Tuple<string, int, PDF_Align>("CP", 1, PDF_Align.Center) },
                    { new Tuple<string, int, PDF_Align>("CIUTAT", 3, PDF_Align.Left) },
                    { new Tuple<string, int, PDF_Align>("TELF. 2", 1, PDF_Align.Center) },
                    { new Tuple<string, int, PDF_Align>("REPRESENTANT", 2, PDF_Align.Left) },
                    { new Tuple<string, int, PDF_Align>("TELF. REP.", 1, PDF_Align.Center) }
                };
                List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(7);
                foreach (ProvidersView provider in providers)
                {
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
                    {
                        new Tuple<string, PDF_Align>(provider.Name, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(provider.Address, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(provider.PostalCode_Str, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(provider.City, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(provider.Phone, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(provider.Agent_Name, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(provider.Agent_Phone, PDF_Align.Center)
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
