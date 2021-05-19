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
    public enum CustomersSalesReportType
    {
        Stipple,
        Send
    }

    /// <summary>
    /// Source Report : 
    /// Report Description : CustomersSales
    /// Source Query : CustomersSales
    /// </summary>
    public static class CustomersSalesReportView
    {
        #region Methods

        /// <summary>
        /// Create the Report with the information of the Diary Bandages selecteds.
        /// </summary>
        /// <param name="customersSales">Ranges selecteds filtereds by family.</param>
        public static void CreateReport(CustomersSalesReportType ReportType, decimal Upper_Limit_Sales, DateTime ReportData,
                                        SortedDictionary<int, CustomersSalesView> customersSales)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, 
                      ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Listing, "VendesClients", out string PDF_FileName);
                doc.AddTitle("Informe de Vendes Superior a...");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(1)
                {
                    new Tuple<string, int, string>("Numero de registres : ", 4, GlobalViewModel.GetStringFromIntValue(customersSales.Count))
                };
                string Title = string.Format("INFORME DE VENDES SUPERIOR A '{0}'", 
                                             GlobalViewModel.GetStringFromDecimalValue(Upper_Limit_Sales, DecimalType.Normal));
                CommonReportView.InsertTitle(doc, Title, Details, 55);
                AddCustomersSalesInfo(doc, ReportType, customersSales);
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

        private static void AddCustomersSalesInfo(Document doc, CustomersSalesReportType ReportType,
                                                  SortedDictionary<int, CustomersSalesView> customersSales)
        {
            List<Tuple<string, int, PDF_Align>> Columns;
            List<List<Tuple<string, PDF_Align>>> Items;
            if (ReportType == CustomersSalesReportType.Send)
            {
                Columns = new List<Tuple<string, int, PDF_Align>>(7)
                {
                    {new Tuple<string, int, PDF_Align>("Nº CLIENT", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("EMPRESA", 7, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("N.I.F.", 3, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("ADREÇA", 7, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("CODI POSTAL", 2, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("POBLACIÓ", 4, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("TOTAL", 3, PDF_Align.Center) },
                };
                Items = new List<List<Tuple<string, PDF_Align>>>(customersSales.Count);
                foreach (CustomersSalesView customerSale in customersSales.Values)
                {
                    string Customer_Id = customerSale.Customer_Id.ToString();
                    decimal TotalAmount = Normalize(customerSale.Customer_Sales_Acum);
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(10)
                    {
                        new Tuple<string, PDF_Align>(Customer_Id, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(customerSale.Company_Name, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(customerSale.Company_Cif, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customerSale.Company_Address, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customerSale.Postal_Code, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customerSale.City, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true), PDF_Align.Right),
                    };
                    Items.Add(item);
                }
            }
            else 
            {
                Columns = new List<Tuple<string, int, PDF_Align>>(7)
                {
                    {new Tuple<string, int, PDF_Align>("Nº CLIENT", 2, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("EMPRESA", 7, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("N.I.F.", 3, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("TOTAL", 3, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("TELÈFON 1", 3, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("TELÈFON 2", 3, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("E-MAIL", 7, PDF_Align.Center) },
                };
                Items = new List<List<Tuple<string, PDF_Align>>>(customersSales.Count);
                foreach (CustomersSalesView customerSale in customersSales.Values)
                {
                    string Customer_Id = customerSale.Customer_Id.ToString();
                    decimal TotalAmount = Normalize(customerSale.Customer_Sales_Acum);
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(10)
                    {
                        new Tuple<string, PDF_Align>(Customer_Id, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(customerSale.Company_Name, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(customerSale.Company_Cif, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(customerSale.Company_Phone_1, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customerSale.Company_Phone_2, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customerSale.Company_EMail, PDF_Align.Left),
                    };
                    Items.Add(item);
                }
            }
            doc.Add(ReportView.CreateTable(Columns, Items));
            doc.Add(ReportView.NewLine());
        }

        #region Utils

        private static decimal Normalize(decimal value)
        {
            return GlobalViewModel.GetValueDecimalForCalculations(value, DecimalType.Currency);
        }

        #endregion

        #endregion
    }
}
