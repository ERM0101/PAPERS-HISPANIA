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
    /// Report Description : DiaryBandages
    /// Source Query : DiaryBandages
    /// </summary>
    public static class DiaryBandagesAndAccountingsReportView
    {
        #region Attributes

        private static Font ForeFontHeaderTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK);

        private static Font ForeFontItemTable = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK);

        #endregion

        #region Methods

        /// <summary>
        /// Create the Report with the information of the Diary Bandages selecteds.
        /// </summary>
        /// <param name="diaryBandage">Ranges selecteds filtereds by family.</param>
        public static void CreateReport(string Bill_Id_From, string Bill_Id_Until,
                                        SortedDictionary<int, DiaryBandagesAndAccountingsView> diaryBandagesAndAccountings)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, 
                      ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Listing, "DiariVendesComptabilitat", out string PDF_FileName);
                doc.AddTitle("Diari de Vendes / Comptabilitat");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(1)
                {
                    new Tuple<string, int, string>("Numero de registres : ", 4, diaryBandagesAndAccountings.Count.ToString())
                };
                string Title = string.Format("DIARI DE VENDES / COMPTABILITAT ENTRE LA FACTURA '{0}' I LA FACTURA '{1}'",
                                             Bill_Id_From, Bill_Id_Until);
                CommonReportView.InsertTitle(doc, Title, Details, 55);
                AddDiaryBandagesAndAccountingInfo(doc, diaryBandagesAndAccountings);
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

        private static void AddDiaryBandagesAndAccountingInfo(
                               Document doc, 
                               SortedDictionary<int, DiaryBandagesAndAccountingsView> diaryBandagesAndAccountings)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(10)
            {
                {new Tuple<string, int, PDF_Align>("Nº FACTURA", 2, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("DATA", 2, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("Nº CLIENT", 2, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("EMPRESA", 7, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("IMPORT BRUT", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("DESCOMPTE PI.", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("BASE IMPOSABLE", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("IVA", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("RECÀRREC", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("TOTAL", 3, PDF_Align.Center) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(diaryBandagesAndAccountings.Count);
            decimal NationalGrossAmount = 0;
            decimal NationalEarlyPayementDiscountAmount = 0;
            decimal NationalTaxableBaseAmount = 0;
            decimal NationalIVAAmount = 0;
            decimal NationalSurchargeAmount = 0;
            decimal NationalTotalAmount = 0;
            decimal ExportGrossAmount = 0;
            decimal ExportEarlyPayementDiscountAmount = 0;
            decimal ExportTaxableBaseAmount = 0;
            decimal ExportIVAAmount = 0;
            decimal ExportSurchargeAmount = 0;
            decimal ExportTotalAmount = 0;
            foreach (DiaryBandagesAndAccountingsView diaryBandagesAndAccounting in diaryBandagesAndAccountings.Values)
            {
                string Bill_Id = GlobalViewModel.GetStringFromIntIdValue(diaryBandagesAndAccounting.Bill_Id);
                string Bill_Date = GlobalViewModel.GetStringFromDateTimeValue(diaryBandagesAndAccounting.Bill_Date);
                string Customer_Id = GlobalViewModel.GetStringFromIntIdValue(diaryBandagesAndAccounting.Customer_Id);
                string Company_Name = diaryBandagesAndAccounting.Company_Name;
                decimal GrossAmount = diaryBandagesAndAccounting.GrossAmount;
                decimal EarlyPayementDiscountAmount = diaryBandagesAndAccounting.EarlyPayementDiscount;
                decimal TaxableBaseAmount = diaryBandagesAndAccounting.TaxableBaseAmount;
                decimal IVAAmount = diaryBandagesAndAccounting.IVAAmount;
                decimal SurchargeAmount = diaryBandagesAndAccounting.SurchargeAmount;
                decimal TotalAmount = diaryBandagesAndAccounting.Total;
                List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(10)
                {
                    new Tuple<string, PDF_Align>(Bill_Id, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Bill_Date, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Customer_Id, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Company_Name, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(GrossAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(EarlyPayementDiscountAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TaxableBaseAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(IVAAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(SurchargeAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true), PDF_Align.Right),
                };
                Items.Add(item);
                NationalGrossAmount += IVAAmount != 0 ? GrossAmount : 0;
                NationalEarlyPayementDiscountAmount += IVAAmount != 0 ? EarlyPayementDiscountAmount : 0;
                NationalTaxableBaseAmount += IVAAmount != 0 ? TaxableBaseAmount : 0;
                NationalIVAAmount += IVAAmount != 0 ? IVAAmount : 0;
                NationalSurchargeAmount += IVAAmount != 0 ? SurchargeAmount : 0;
                NationalTotalAmount += IVAAmount != 0 ? TotalAmount : 0;
                ExportGrossAmount += IVAAmount == 0 ? GrossAmount : 0;
                ExportEarlyPayementDiscountAmount += IVAAmount == 0 ? EarlyPayementDiscountAmount : 0;
                ExportTaxableBaseAmount += IVAAmount == 0 ? TaxableBaseAmount : 0;
                ExportIVAAmount += 0;
                ExportSurchargeAmount += IVAAmount == 0 ? SurchargeAmount : 0;
                ExportTotalAmount += IVAAmount == 0 ? TotalAmount : 0;
            }
            doc.Add(ReportView.CreateTable(Columns, Items));
            doc.Add(ReportView.NewLine());
            doc.Add(CreateAmountInfo(NationalGrossAmount, NationalEarlyPayementDiscountAmount,
                                     NationalTaxableBaseAmount, NationalIVAAmount, NationalSurchargeAmount,
                                     NationalTotalAmount, ExportGrossAmount, ExportEarlyPayementDiscountAmount,
                                     ExportTaxableBaseAmount, ExportIVAAmount, ExportSurchargeAmount,
                                     ExportTotalAmount));
        }

        private static PdfPTable CreateAmountInfo(decimal NationalGrossAmount,
                                                  decimal NationalEarlyPayementDiscountAmount,
                                                  decimal NationalTaxableBaseAmount,
                                                  decimal NationalIVAAmount,
                                                  decimal NationalSurchargeAmount,
                                                  decimal NationalTotalAmount,
                                                  decimal ExportGrossAmount,
                                                  decimal ExportEarlyPayementDiscountAmount,
                                                  decimal ExportTaxableBaseAmount,
                                                  decimal ExportIVAAmount,
                                                  decimal ExportSurchargeAmount,
                                                  decimal ExportTotalAmount)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(7)
            {
                {new Tuple<string, int, PDF_Align>(string.Empty, 7, PDF_Align.Center)},
                {new Tuple<string, int, PDF_Align>("IMPORT BRUT", 4, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("DESCOMPTE PI.", 4, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("BASE IMPOSABLE", 4, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("IVA", 4, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("RECÀRREC", 4, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("TOTAL", 4, PDF_Align.Center) },
            };
            decimal TotalGrossAmount = NationalGrossAmount + ExportGrossAmount;
            decimal TotalEarlyPayementDiscountAmount = NationalEarlyPayementDiscountAmount +
                                                       ExportEarlyPayementDiscountAmount;
            decimal TotalTaxableBaseAmount = NationalTaxableBaseAmount + ExportTaxableBaseAmount;
            decimal TotalIVAAmount = NationalIVAAmount + ExportIVAAmount;
            decimal TotalSurchargeAmount = NationalSurchargeAmount + ExportSurchargeAmount;
            decimal TotalTotalAmount = NationalTotalAmount + ExportTotalAmount;
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(3)
            {
                new List<Tuple<string, PDF_Align>>(7)
                {
                    new Tuple<string, PDF_Align>("TOTAL NACIONAL : ", PDF_Align.Left),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(NationalGrossAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(NationalEarlyPayementDiscountAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(NationalTaxableBaseAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(NationalIVAAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(NationalSurchargeAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(NationalTotalAmount, DecimalType.Currency, true), PDF_Align.Right),
                },
                new List<Tuple<string, PDF_Align>>(7)
                {
                    new Tuple<string, PDF_Align>("TOTAL EXPORTACIONS : ", PDF_Align.Left),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(ExportGrossAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(ExportEarlyPayementDiscountAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(ExportTaxableBaseAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(ExportIVAAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(ExportSurchargeAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(ExportTotalAmount, DecimalType.Currency, true), PDF_Align.Right),
                },
                new List<Tuple<string, PDF_Align>>(7)
                {
                    new Tuple<string, PDF_Align>("TOTAL GENERAL : ", PDF_Align.Left),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalGrossAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalEarlyPayementDiscountAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalTaxableBaseAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalIVAAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalSurchargeAmount, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalTotalAmount, DecimalType.Currency, true), PDF_Align.Right),
                }
            };
            return ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable);
        }

        #endregion
    }
}
