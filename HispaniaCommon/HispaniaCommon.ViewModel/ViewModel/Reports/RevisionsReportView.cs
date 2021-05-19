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
    public static class RevisionsReportView
    {
        /// <summary>
        /// Create the Report with the information of the Revisions selecteds.
        /// </summary>
        /// <param name="revisions">Customers selecteds.</param>
        public static void CreateReport(List<RevisionsView> revisions, RevisionsType RevisionType)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                string Title, FileName;
                switch (RevisionType)
                {
                    case RevisionsType.MismatchesAvailable:
                         Title = "Revisió d'Estocs Disponibles (desquadres)";
                         FileName = "RevisioEstocsDisponible";
                         break;
                    case RevisionsType.MismatchesStocks:
                         Title = "Revisió d'Estocs Existències (desquadres)";
                         FileName = "RevisioEstocsExistencies";
                         break;
                    case RevisionsType.GeneralAvailable:
                         Title = "Revisió d'Estocs Disponibles";
                         FileName = "GeneralDisponible";
                         break;
                    case RevisionsType.GeneralStocks:
                         Title = "Revisió d'Estocs Existències";
                         FileName = "GeneralEstocs";
                         break;
                    default:
                         throw new ArgumentException(string.Format("Error, tipus de revisió '{0}' no reconegut", (int) RevisionType));
                }
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, 
                      ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Listing, FileName, out string PDF_FileName);
                doc.AddTitle(Title);
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(1);
                Details.Add(new Tuple<string, int, string>("Numero de registres : ", 4, revisions.Count.ToString()));
                CommonReportView.InsertTitle(doc, Title.ToUpper(), Details, 55);
                List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(11)
                {
                    {new Tuple<string, int, PDF_Align>("ARTICLE", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("DESCRIPCIÓ", 8, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("INICIAL (UE)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("ENTRADES (UE)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("SORTIDES (UE)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("ESTIMACIÓ (UE)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("REAL (UE)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("INICIAL (UF)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("ENTRADES (UF)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("SORTIDES (UF)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("ESTIMACIÓ (UF)", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("REAL (UF)", 2, PDF_Align.Left) },
                };
                List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(11);
                foreach (RevisionsView revision in revisions)
                {
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
                    {
                        new Tuple<string, PDF_Align>(revision.GoodCode, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(revision.GoodDescription, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.InitialUE, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.EntryUE, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.OutputUE, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.StockExpectedUE, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.StockRealUE, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.InitialUF, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.EntryUF, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.OutputUF, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.StockExpectedUF, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(revision.StockRealUF, DecimalType.Unit), PDF_Align.Right),
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
