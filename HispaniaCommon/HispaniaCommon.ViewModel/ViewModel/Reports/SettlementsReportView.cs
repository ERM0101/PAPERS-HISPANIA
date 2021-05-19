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
    public static class SettlementsReportView
    {
        #region Attributes

        private static Font ForeFontHeaderTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK);

        private static Font ForeFontItemTable = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK);

        #endregion

        #region Methods

        /// <summary>
        /// Create the Report with the information of the Revisions selecteds.
        /// </summary>
        /// <param name="agents">Ranges selecteds filtereds by family.</param>
        public static void CreateReport(string Bill_Id_From, string Bill_Id_Until,
                                        SortedDictionary<int, Dictionary<int, List<SettlementsView>>> agents)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15,
                      ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Listing, "LiquidacioComissions", out string PDF_FileName);
                doc.AddTitle("Liquidació de Comissions");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                foreach (Dictionary<int, List<SettlementsView>> agent_bills in agents.Values)
                {
                    bool HeaderShowed = false;
                    decimal BaseAcum = 0;
                    decimal ComissionAcum = 0;
                    foreach (KeyValuePair<int, List<SettlementsView>> bill in agent_bills)
                    {
                        if (!HeaderShowed)
                        {
                            HeaderAgent(doc, bill.Value[0].Agent_Name, Bill_Id_From, Bill_Id_Until);
                            HeaderShowed = true;
                        }
                        AddAgentBillInfo(doc, bill.Value, out decimal BaseBillAcum, out decimal ComissionBillAcum);
                        BaseAcum += BaseBillAcum;
                        ComissionAcum += ComissionBillAcum;
                    }
                    doc.Add(CreateAmounAgentInfo(BaseAcum, ComissionAcum));
                    ReportView.NewPage(doc);
                }
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

        #region Header Agent

        private static void HeaderAgent(Document doc, string Agent_Name, string Bill_Id_From, string Bill_Id_Until)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(1)
            {
                { new Tuple<string, int, PDF_Align>("INFORME DE COMISSIONS CORRESPONENTS A", 10, PDF_Align.Center) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1)
            {
                new List<Tuple<string, PDF_Align>>(1)
                {
                    new Tuple<string, PDF_Align>(Agent_Name, PDF_Align.Center),
                },
            };
            List<Tuple<string, int, PDF_Align>> BillColumns = new List<Tuple<string, int, PDF_Align>>(1)
            {
                { new Tuple<string, int, PDF_Align>("INTERVAL DE FACTURES", 10, PDF_Align.Center) },
            };
            string BillInfo = string.Format("DESDE LA FACTURA '{0}' FINS A LA FACTURA '{1}'.",
                                            Bill_Id_From, Bill_Id_Until);
            List<List<Tuple<string, PDF_Align>>> BillItems = new List<List<Tuple<string, PDF_Align>>>(1)
            {
                new List<Tuple<string, PDF_Align>>(1)
                {
                    new Tuple<string, PDF_Align>(BillInfo, PDF_Align.Center),
                }
            };
            List<PdfPCell> Cells = new List<PdfPCell>(2)
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable), 10),
                ReportView.CreateNestedTable(ReportView.CreateTable(BillColumns, BillItems, ForeFontItemTable, ForeFontHeaderTable), 10),
            };
            List<Tuple<string, int, PDF_Align>> DateColumns = new List<Tuple<string, int, PDF_Align>>(2)
            {
                { new Tuple<string, int, PDF_Align>("DATA", 6, PDF_Align.Center) },
            };
            string Data = GlobalViewModel.GetStringFromDateTimeValue(DateTime.Now, DateTimeFormat.LongFormat);
            List<List<Tuple<string, PDF_Align>>> DateItems = new List<List<Tuple<string, PDF_Align>>>()
            {
                new List<Tuple<string, PDF_Align>>(1)
                {
                    new Tuple<string, PDF_Align>(Data, PDF_Align.Center),
                }
            };
            List<PdfPCell> DateCells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(2),
                ReportView.CreateNestedTable(ReportView.CreateTable(DateColumns, DateItems, ForeFontItemTable, ForeFontHeaderTable), 7),
                ReportView.CreateEmptyRow(1),
            };
            List<PdfPCell> HeaderCell = new List<PdfPCell>(Columns.Count)
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Cells, 10), 7),
                ReportView.CreateNestedTable(ReportView.CreateTable(DateCells, 10), 3)
            };
            doc.Add(ReportView.CreateTable(HeaderCell, 10));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Agent Bill Info

        private static void AddAgentBillInfo(Document doc, List<SettlementsView> settlements, out decimal BaseAcum, out decimal ComissionAcum)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(7)
            {
                {new Tuple<string, int, PDF_Align>("Nº FACTURA", 2, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("Nº CLIENT", 2, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("EMPRESA", 7, PDF_Align.Left) },
                {new Tuple<string, int, PDF_Align>("DATA", 2, PDF_Align.Left) },
                { new Tuple<string, int, PDF_Align>("IMPORT", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("% COMISSIÓ", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("COMISSIÓ", 3, PDF_Align.Center) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(settlements.Count);
            BaseAcum = 0;
            ComissionAcum = 0;
            string ComissionPercentInfo = string.Empty;
            foreach (SettlementsView settlement in settlements)
            {
                string Bill_Id = settlement.Bill_Id.ToString();
                string Bill_Date = GlobalViewModel.GetStringFromDateTimeValue(settlement.Bill_Date);
                string Customer_Id = settlement.Customer_Id.ToString();
                string Company_Name = settlement.Company_Name;
                decimal Base = settlement.Base;
                decimal ComissionPercent = settlement.ComissionPercent;
                decimal Comission = settlement.Comission;
                List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
                {
                    new Tuple<string, PDF_Align>(Bill_Id, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Customer_Id, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Company_Name, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Bill_Date, PDF_Align.Center),
                    new Tuple<string, PDF_Align>(Base.ToString("0.00 €"), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(ComissionPercent.ToString("0.00") + " %", PDF_Align.Center),
                    new Tuple<string, PDF_Align>(Comission.ToString("0.00 €"), PDF_Align.Right),
                };
                Items.Add(item);
                BaseAcum += Base;
                ComissionAcum += Comission;
            }
            doc.Add(ReportView.CreateTable(Columns, Items));
            doc.Add(CreateAmounBillInfo(BaseAcum, ComissionAcum));
            doc.Add(ReportView.NewLine());
        }

        #region Create Amount Bill

        private static PdfPTable CreateAmounBillInfo(decimal BaseAcum, decimal ComissionAcum)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(4)
            {
                {new Tuple<string, int, PDF_Align>("SUBTOTAL", 13, PDF_Align.Center)},
                { new Tuple<string, int, PDF_Align>("IMPORT", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("% MIG COMISSIÓ", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("COMISSIÓ", 3, PDF_Align.Center) }
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            decimal BillComissionPercent = BaseAcum == 0 ? 100 : Normalize((ComissionAcum / BaseAcum) * 100) ;
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(4)
            {
                new Tuple<string, PDF_Align>("Subtotals", PDF_Align.Right),
                new Tuple<string, PDF_Align>(BaseAcum.ToString("0.00 €"), PDF_Align.Right),
                new Tuple<string, PDF_Align>(BillComissionPercent.ToString("0.00") + " %", PDF_Align.Center),
                new Tuple<string, PDF_Align>(ComissionAcum.ToString("0.00 €"), PDF_Align.Right)
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable);
        }

        #endregion

        #endregion

        #region Create Amount Agent

        private static PdfPTable CreateAmounAgentInfo(decimal BaseAcum, decimal ComissionAcum)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(4)
            {
                {new Tuple<string, int, PDF_Align>(string.Empty, 13, PDF_Align.Center)},
                { new Tuple<string, int, PDF_Align>("TOTAL VENDES", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("COMISSIÓ BRUTA", 3, PDF_Align.Center) },
                {new Tuple<string, int, PDF_Align>("% MIG COMISSIÓ", 3, PDF_Align.Center) }
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            decimal BillComissionPercent = BaseAcum == 0 ? 100 : Normalize((ComissionAcum / BaseAcum) * 100);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(4)
            {
                new Tuple<string, PDF_Align>("Total", PDF_Align.Right),
                new Tuple<string, PDF_Align>(BaseAcum.ToString("0.00 €"), PDF_Align.Right),
                new Tuple<string, PDF_Align>(ComissionAcum.ToString("0.00 €"), PDF_Align.Right),
                new Tuple<string, PDF_Align>(BillComissionPercent.ToString("0.00") + " %", PDF_Align.Center)
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontItemTable, ForeFontHeaderTable);
        }

        #endregion

        #region Utils

        private static decimal Normalize(decimal value)
        {
            return GlobalViewModel.GetValueDecimalForCalculations(value, DecimalType.Currency);
        }

        #endregion

        #endregion
    }
}
