#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
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
    /// Source Report : infocli
    /// Report Description : list the customers associates at the Agent
    /// Source Query : SELECT Val([codcli]) AS Expr1, CLIENTES.activo, CLIENTES.EMPRESA, CLIENTES.CODREP, CLIENTES.ACU1, CLIENTES.ACU2, CLIENTES.ACU3, 
    ///                       CLIENTES.ACU4, CLIENTES.ACU5, CLIENTES.ACU6, CLIENTES.ACU7, CLIENTES.ACU8, CLIENTES.ACU9, CLIENTES.ACU10, CLIENTES.ACU11, 
    ///                       CLIENTES.ACU12
    ///                FROM CLIENTES
    ///                WHERE(((CLIENTES.activo)<>True) AND((CLIENTES.CODREP)=[Formularios]![REPRESENTANTES]![codrep]))
    ///                ORDER BY Val([codcli]);
    /// </summary>
    public static class AgentReportView
    {
        public static void CreateReport(AgentsView agent)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                List<HispaniaComptabilitat.Data.Customer> CustomersOfTheAgentSelected = HispaniaDataAccess.Instance.GetCustomersForAgentReport(agent.Agent_Id);
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Horizontal,
                                                0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.Agent, string.Format("Representant_{0}", agent.Name), out string PDF_FileName);
                doc.AddTitle("Informe de Clients del comercial {0}" + agent.Name);
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                List<Tuple<string, int, string>> Details = new List<Tuple<string, int, string>>(2)
                {
                    new Tuple<string, int, string>("Comercial :", 1, agent.Name),
                    new Tuple<string, int, string>("Numero :", 1, agent.Agent_Id.ToString())
                };
                CommonReportView.InsertTitle(doc, "INFORME DE REPRESENTANT", Details, 75);
                List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(14)
                {
                    {new Tuple<string, int, PDF_Align>("CLIENTS", 5, PDF_Align.Left) },
                    {new Tuple<string, int, PDF_Align>("GENER", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("FEBRER", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("MARÇ", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("ABRIL", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("MAIG", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("JUNY", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("JULIOL", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("AGOST", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("SET.", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("OCT.", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("NOV.", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("DES.", 1, PDF_Align.Center) },
                    {new Tuple<string, int, PDF_Align>("TOTAL", 1, PDF_Align.Center) }
                };
                List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>();
                decimal[] totalxColumnes = new decimal[13];
                foreach (HispaniaComptabilitat.Data.Customer customer in CustomersOfTheAgentSelected)
                {
                    decimal totalxFiles = 0;
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(14);
                    for (int ind = 0; ind < 14; ind++)
                    {
                        string value = ManageItem(ind, customer, totalxColumnes, ref totalxFiles);
                        if (ind == 0) item.Add(new Tuple<string, PDF_Align>(value, PDF_Align.Left));
                        else item.Add(new Tuple<string, PDF_Align>(value, PDF_Align.Center));
                    }
                    Items.Add(item);
                }
                List<Tuple<string, PDF_Align>> itemAcum = new List<Tuple<string, PDF_Align>>(14);
                for (int ind = 0; ind < 14; ind++)
                {
                    if (ind == 0) itemAcum.Add(new Tuple<string, PDF_Align>("TOTAL", PDF_Align.Left));
                    else
                    {
                        itemAcum.Add(new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(totalxColumnes[ind - 1], DecimalType.Currency, true), PDF_Align.Center));
                    }
                }
                Items.Add(itemAcum);
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

        private static string ManageItem(int index, HispaniaComptabilitat.Data.Customer customer, decimal[] totalPerColumnes, ref decimal totalPerFiles)
        {
            string ItemValue = string.Empty;
            if (index == 0) ItemValue = string.Format("{0} - {1}", customer.Customer_Id, customer.Company_Name);
            else if ((index >= 1) && (index <= 12))
            {
                decimal acum;
                if (index == 1) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_1);
                else if (index == 2) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_2);
                else if (index == 3) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_3);
                else if (index == 4) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_4);
                else if (index == 5) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_5);
                else if (index == 6) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_6);
                else if (index == 7) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_7);
                else if (index == 8) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_8);
                else if (index == 9) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_9);
                else if (index == 10) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_10);
                else if (index == 11) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_11);
                else if (index == 12) acum = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_12);
                else throw new Exception("Index out of range.");
                totalPerColumnes[index - 1] += acum;
                totalPerFiles += acum;
                ItemValue = GlobalViewModel.GetStringFromDecimalValue(acum, DecimalType.Currency, true); 
            }
            else if (index == 13)
            {
                totalPerColumnes[12] += totalPerFiles;
                ItemValue = GlobalViewModel.GetStringFromDecimalValue(totalPerFiles, DecimalType.Currency, true);
            }
            return ItemValue;
        }
    }
}
