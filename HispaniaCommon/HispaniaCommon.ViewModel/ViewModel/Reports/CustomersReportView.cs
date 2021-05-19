﻿#region librerias usadas por la clase

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
    /// Report Description : list the Customers
    /// Source Query : Customers selecteds
    /// </summary>
    public static class CustomersReportView
    {
        /// <summary>
        /// Create the Report with the information of the Customers selecteds.
        /// </summary>
        /// <param name="customers">Customers selecteds.</param>
        public static void CreateReport(List<CustomersView> customers)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal, 0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Customer, "Clients", out string PDF_FileName);
                doc.AddTitle("Informe de Clients seleccionats de la finestra de clients");
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(1)
                {
                    new Tuple<string, int, string>("Numero de Clients seleccionats : ", 4, customers.Count.ToString())
                };
                CommonReportView.InsertTitle(doc, "INFORME DE CLIENTS", Details, 55);
                List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(7)
                {
                    {new Tuple<string, int, PDF_Align>("EMPRESA", 4, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("ADREÇA", 4, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("CP", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("CIUTAT", 3, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("TELF. 1", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("TELF. 2", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("CONTACTE", 2, PDF_Align.Left) }
                };
                List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(7);
                foreach (CustomersView customer in customers)
                {
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
                    {
                        new Tuple<string, PDF_Align>(customer.Company_Name, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(customer.Company_Address, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(customer.Company_PostalCode_Str, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customer.Company_City_Str, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(customer.Company_Phone_1, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customer.Company_Phone_2, PDF_Align.Center),
                        new Tuple<string, PDF_Align>(customer.Company_ContactPerson, PDF_Align.Left)
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
