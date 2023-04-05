#region Librerias usadas por la clase

using iTextSharp.text;
using iTextSharp.text.pdf;
using MBCode.Framework.Managers.Messages;
using System;
using System.Windows;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;
using System.Diagnostics;

#endregion

namespace HispaniaCommon.ViewModel
{
    public enum DeliveryNoteFlag
    {
        Print,
        SendByEMail,
    }

    /// <summary>
    /// Source Report : 
    /// Report Description : list the CustomerOrders
    /// Source Query : CustomerOrders selecteds
    /// </summary>
    public static class DeliveryNotesReportView
    {
        #region Attributes

        private static BaseColor BackColumnTableColor = new BaseColor(247, 217, 173);

        private static Font ForeFontTable = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, CMYKColor.BLACK);

        private static Font ForeFontHeaderDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 6.5f, Font.NORMAL, CMYKColor.BLACK);

        private static Font ForeFontDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL, CMYKColor.BLACK);

        #endregion

        #region Main Methods

        /// <summary>
        /// Create the Report with the information of the Customers selecteds.
        /// </summary>
        /// <param name="customerOrder">Customers selecteds.</param>
        public static bool CreateReport(CustomerOrdersView customerOrder, string PDF_FileName, out string ErrMsg)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Vertical, 0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.DeliveryNote, PDF_FileName);
                doc.AddTitle(string.Format("Albarà Numero {0}", customerOrder.DeliveryNote_Id));
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                CommonReportView.InsertHispaniaHeader(doc);
                CommonReportView.InsertCustomerInfo(doc, customerOrder.Customer, false, CustomerInfoType.DeliveryNote);
                InsertDeliveryNoteHeader(doc, customerOrder);
                InsertDeliveryNoteMovementTable(doc, customerOrder);
                InsertDeliveryNoteAmount(doc, customerOrder);
                InsertDeliveryNoteCommentFoot(doc, customerOrder);
                InsertDeliveryNoteFoot(doc, customerOrder);
                doc.Close();
                writer.Close();
                ReportView.AddPageNumber(PDF_FileName, PDF_Orientation.Horizontal);
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                if ((doc != null) && (doc.IsOpen())) doc.Close();
                if (writer != null) writer.Close();
                PDF_FileName = null;
                ErrMsg = string.Format("Error, al construïr el PDF de l'albarà número {0}.\r\nDetalls: {1}",
                                       customerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        /// <summary>
        /// Create the Report with the information of the Providers selecteds.
        /// </summary>
        /// <param name="providerOrder">Customers selecteds.</param>
        public static bool CreateReport(ProviderOrdersView providerOrder, string PDF_FileName, out string ErrMsg)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Vertical, 0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.DeliveryNote, PDF_FileName);
                doc.AddTitle(string.Format("Albarà Numero {0}", providerOrder.DeliveryNote_Id));
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                CommonReportView.InsertHispaniaHeader(doc);
                CommonReportView.InsertProviderInfo(doc, providerOrder.Provider, false, CustomerInfoType.DeliveryNote);
                InsertDeliveryNoteHeader(doc, providerOrder);
                InsertDeliveryNoteMovementTable(doc, providerOrder);
                InsertDeliveryNoteAmount(doc, providerOrder);
                InsertDeliveryNoteCommentFoot(doc, providerOrder);
                InsertDeliveryNoteFoot(doc, providerOrder);
                doc.Close();
                writer.Close();
                ReportView.AddPageNumber(PDF_FileName, PDF_Orientation.Horizontal);
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                if ((doc != null) && (doc.IsOpen())) doc.Close();
                if (writer != null) writer.Close();
                PDF_FileName = null;
                ErrMsg = string.Format("Error, al construïr el PDF de l'albarà número {0}.\r\nDetalls: {1}",
                                       providerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool SendByEMail(CustomerOrdersView customerOrder, string PDF_FileName, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(customerOrder.Customer.Company_EMail))
                {
                    string EMAIL_Address = customerOrder.Customer.Company_EMail;
                    string Subject = string.Format("Albarà número ({0})", customerOrder.DeliveryNote_Id);
                    string BodyMessage = string.Format("Albarà número ({0})\r\nPapers Hispània S.L.\r\n", customerOrder.DeliveryNote_Id);
                    List<Tuple<string, string>> FileAttachments = new List<Tuple<string, string>>(1)
                    {
                        new Tuple<string, string>(PDF_FileName, string.Format("Albarà_{0}", customerOrder.DeliveryNote_Id))
                    };
                    ReportView.SendReport(EMAIL_Address, Subject, BodyMessage, FileAttachments, out ErrMsg);
                }
                else
                {
                    ErrMsg = string.Format("Error, a l'enviar per email el PDF de l'albarà número {0}.\r\nDetalls: Manca l'adreça electrònica del client.",
                                           customerOrder.DeliveryNote_Id);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, a l'enviar per email el PDF de l'albarà número {0}.\r\nDetalls: {1}",
                                       customerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool SendByEMail(ProviderOrdersView providerOrder, string PDF_FileName, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(providerOrder.Provider.Company_EMail))
                {
                    string EMAIL_Address = providerOrder.Provider.Company_EMail;
                    string Subject = string.Format("Albarà número ({0})", providerOrder.DeliveryNote_Id);
                    string BodyMessage = string.Format("Albarà número ({0})\r\nPapers Hispània S.L.\r\n", providerOrder.DeliveryNote_Id);
                    List<Tuple<string, string>> FileAttachments = new List<Tuple<string, string>>(1)
                    {
                        new Tuple<string, string>(PDF_FileName, string.Format("Albarà_{0}", providerOrder.DeliveryNote_Id))
                    };
                    ReportView.SendReport(EMAIL_Address, Subject, BodyMessage, FileAttachments, out ErrMsg);
                }
                else
                {
                    ErrMsg = string.Format("Error, a l'enviar per email el PDF de l'albarà número {0}.\r\nDetalls: Manca l'adreça electrònica del client.",
                                           providerOrder.DeliveryNote_Id);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, a l'enviar per email el PDF de l'albarà número {0}.\r\nDetalls: {1}",
                                       providerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool UpdateDeliveryNoteFlag(CustomerOrdersView customerOrder, DeliveryNoteFlag FlagToUpdate, object newFlagValue, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                CustomerOrdersView customerOrderForUpdate = new CustomerOrdersView(customerOrder);
                switch (FlagToUpdate)
                {
                    case DeliveryNoteFlag.Print:
                         customerOrderForUpdate.Print = (bool) newFlagValue;
                         break;
                    case DeliveryNoteFlag.SendByEMail:
                         customerOrderForUpdate.SendByEMail = (bool) newFlagValue;
                         break;
                    default:
                         throw new ArgumentException(string.Format("Flag {0} no rerconegut.", FlagToUpdate));
                }
                GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrder(customerOrderForUpdate);
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al actualitzar el flag '{0}' per l'albarà número '{1}' a la Base de Dades.\r\nDetalls: {2}",
                                       FlagToUpdate, customerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool UpdateDeliveryNoteFlag(ProviderOrdersView providerOrder, DeliveryNoteFlag FlagToUpdate, object newFlagValue, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                ProviderOrdersView providerOrderForUpdate = new ProviderOrdersView(providerOrder);
                switch (FlagToUpdate)
                {
                    case DeliveryNoteFlag.Print:
                        providerOrderForUpdate.Print = (bool)newFlagValue;
                        break;
                    case DeliveryNoteFlag.SendByEMail:
                        providerOrderForUpdate.SendByEMail = (bool)newFlagValue;
                        break;
                    default:
                        throw new ArgumentException(string.Format("Flag {0} no rerconegut.", FlagToUpdate));
                }
                GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrder(providerOrderForUpdate);
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al actualitzar el flag '{0}' per l'albarà número '{1}' a la Base de Dades.\r\nDetalls: {2}",
                                       FlagToUpdate, providerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool GetFileNameInDeliveryNote(CustomerOrdersView customerOrder, out string PDF_FileName, out string ErrMsg)
        {
            try
            {
                HispaniaCompData.DeliveryNote DeliveryNote =
                                      GlobalViewModel.Instance.HispaniaViewModel.GetDeliveryNote(customerOrder.DeliveryNote_Id, customerOrder.DeliveryNote_Year);
                PDF_FileName = DeliveryNote.FileNamePDF;
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                PDF_FileName = null;
                ErrMsg = string.Format("Error, a l'obtindre el nom del fitxer PDF generat per l'albarà número {0} a la Base de Dades.\r\nDetalls: {1}",
                                       customerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool CheckAndContinueIfExistReport(CustomerOrdersView customerOrder, out string PDF_FileName,
                                                         out string ErrMsg)
        {
            PDF_FileName = null;
            ErrMsg = string.Empty;
            try
            {
                decimal Year = customerOrder.DeliveryNote_Year;
                string BasePdfFileName = string.Format("Albara_({0})", customerOrder.DeliveryNote_Id);
                if (ReportView.ExistPDF_FileName(PDF_Report_Types.DeliveryNote, BasePdfFileName, 
                                                 out string[] OldPdfReports, Year)) 
                {
                    string Question = string.Format("Hi ha un o més informes creats per l'albarà {0} vol continuar i esborrar-los ?",
                                                    customerOrder.DeliveryNote_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        ReportView.DeleteReportFiles(OldPdfReports);
                        PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.DeliveryNote, BasePdfFileName, Year);
                    }
                    else
                    {
                        ErrMsg = string.Format("Informació, s'ha cancel·lat la construcció de l'informe de l'albarà número {0}.",
                                                customerOrder.DeliveryNote_Id);
                    }
                }
                else
                {
                    PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.DeliveryNote, BasePdfFileName, Year);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al construïr el PDF de l'albarà número {0}.\r\nDetalls: {1}",
                                       customerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool CheckAndContinueIfExistReport(ProviderOrdersView providerOrder, out string PDF_FileName,
                                                        out string ErrMsg)
        {
            PDF_FileName = null;
            ErrMsg = string.Empty;
            try
            {
                decimal Year = providerOrder.DeliveryNote_Year;
                string BasePdfFileName = string.Format("Albara_({0})", providerOrder.DeliveryNote_Id);
                if (ReportView.ExistPDF_FileName(PDF_Report_Types.DeliveryNote, BasePdfFileName,
                                                 out string[] OldPdfReports, Year))
                {
                    string Question = string.Format("Hi ha un o més informes creats per l'albarà {0} vol continuar i esborrar-los ?",
                                                    providerOrder.DeliveryNote_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        ReportView.DeleteReportFiles(OldPdfReports);
                        PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.DeliveryNote, BasePdfFileName, Year);
                    }
                    else
                    {
                        ErrMsg = string.Format("Informació, s'ha cancel·lat la construcció de l'informe de l'albarà número {0}.",
                                                providerOrder.DeliveryNote_Id);
                    }
                }
                else
                {
                    PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.DeliveryNote, BasePdfFileName, Year);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al construïr el PDF de l'albarà número {0}.\r\nDetalls: {1}",
                                       providerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }


        #endregion

        #region Delivery Note Header

        private static void InsertDeliveryNoteHeader(Document doc, CustomerOrdersView customerOrder)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(2)
            {
                { new Tuple<string, int, PDF_Align>("DATA", 8, PDF_Align.Center) },
                { new Tuple<string, int, PDF_Align>("NÚMERO ALBARÀ", 6, PDF_Align.Center) }
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1)
            {
                new List<Tuple<string, PDF_Align>>(2)
                {
                    new Tuple<string, PDF_Align>(customerOrder.DeliveryNote_Date_Str, PDF_Align.Center),
                    new Tuple<string, PDF_Align>(customerOrder.DeliveryNote_Id_Str, PDF_Align.Center),
                }
            };
            List<PdfPCell> Cells = new List<PdfPCell>(1)
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Columns, Items, ForeFontTable), 14),
            };
            List<Tuple<string, int, PDF_Align>> SendColumns = new List<Tuple<string, int, PDF_Align>>(2)
            {
                { new Tuple<string, int, PDF_Align>("ENVIAMENT", 6, PDF_Align.Left) },
            };
            List<List<Tuple<string, PDF_Align>>> SendItems = new List<List<Tuple<string, PDF_Align>>>()
            {
                new List<Tuple<string, PDF_Align>>(1)
                {
                    new Tuple<string, PDF_Align>(customerOrder.SendType.Description, PDF_Align.Left),
                }
            };
            if (!customerOrder.Address.ToUpper().Equals(customerOrder.Customer.Company_Address.ToUpper()))
            {
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                  new Tuple<string, PDF_Align>(customerOrder.Address, PDF_Align.Left),
                              });
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                  new Tuple<string, PDF_Align>(customerOrder.PostalCode.Postal_Code_Info, PDF_Align.Left),
                              });
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                 new Tuple<string, PDF_Align>(customerOrder.PostalCode.Province, PDF_Align.Left),
                              });
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                 new Tuple<string, PDF_Align>(customerOrder.TimeTable, PDF_Align.Left),
                              });
            }
            List<PdfPCell> SendCells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(ReportView.CreateTable(SendColumns, SendItems, ForeFontTable), 8),
                ReportView.CreateEmptyRow(1),
            };
            List<PdfPCell> CellsDeliveryNoteHeader = new List<PdfPCell>(Columns.Count)
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Cells, 14), 4),
                ReportView.CreateNestedTable(ReportView.CreateTable(SendCells, 10), 6)
            };
            doc.Add(ReportView.CreateTable(CellsDeliveryNoteHeader, 10));
            doc.Add(ReportView.NewLine());
        }

        private static void InsertDeliveryNoteHeader(Document doc, ProviderOrdersView providerOrder)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(2)
            {
                { new Tuple<string, int, PDF_Align>("DATA", 8, PDF_Align.Center) },
                { new Tuple<string, int, PDF_Align>("NÚMERO ALBARÀ", 6, PDF_Align.Center) }
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1)
            {
                new List<Tuple<string, PDF_Align>>(2)
                {
                    new Tuple<string, PDF_Align>(providerOrder.DeliveryNote_Date_Str, PDF_Align.Center),
                    new Tuple<string, PDF_Align>(providerOrder.DeliveryNote_Id_Str, PDF_Align.Center),
                }
            };
            List<PdfPCell> Cells = new List<PdfPCell>(1)
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Columns, Items, ForeFontTable), 14),
            };
            List<Tuple<string, int, PDF_Align>> SendColumns = new List<Tuple<string, int, PDF_Align>>(2)
            {
                { new Tuple<string, int, PDF_Align>("ENVIAMENT", 6, PDF_Align.Left) },
            };
            List<List<Tuple<string, PDF_Align>>> SendItems = new List<List<Tuple<string, PDF_Align>>>()
            {
                new List<Tuple<string, PDF_Align>>(1)
                {
                    new Tuple<string, PDF_Align>(providerOrder.SendType.Description, PDF_Align.Left),
                }
            };
            if (!providerOrder.Address.ToUpper().Equals(providerOrder.Provider.Company_Address.ToUpper()))
            {
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                  new Tuple<string, PDF_Align>(providerOrder.Address, PDF_Align.Left),
                              });
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                  new Tuple<string, PDF_Align>(providerOrder.PostalCode.Postal_Code_Info, PDF_Align.Left),
                              });
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                 new Tuple<string, PDF_Align>(providerOrder.PostalCode.Province, PDF_Align.Left),
                              });
                SendItems.Add(new List<Tuple<string, PDF_Align>>(1)
                              {
                                 new Tuple<string, PDF_Align>(providerOrder.TimeTable, PDF_Align.Left),
                              });
            }
            List<PdfPCell> SendCells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(ReportView.CreateTable(SendColumns, SendItems, ForeFontTable), 8),
                ReportView.CreateEmptyRow(1),
            };
            List<PdfPCell> CellsDeliveryNoteHeader = new List<PdfPCell>(Columns.Count)
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Cells, 14), 4),
                ReportView.CreateNestedTable(ReportView.CreateTable(SendCells, 10), 6)
            };
            doc.Add(ReportView.CreateTable(CellsDeliveryNoteHeader, 10));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Delivery Note Movement Table

        private static void InsertDeliveryNoteMovementTable(Document doc, CustomerOrdersView customerOrder)
        {
            bool Valued = customerOrder.Valued;
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(7)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT EXPEDICIÓ", 3, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT FACTURACIÓ", 3, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("TIPUS DE MERCADERIA", Valued ? 8 : 12, PDF_Align.Center, BaseColor.WHITE) }
            };
            if (Valued)
            {
                Columns.Add(new Tuple<string, int, PDF_Align, BaseColor>("PREU UNITARI", 2, PDF_Align.Center, BaseColor.WHITE));
                Columns.Add(new Tuple<string, int, PDF_Align, BaseColor>("IMPORT", 2, PDF_Align.Center, BackColumnTableColor));
            }
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>();
            foreach (CustomerOrderMovementsView movement in GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderMovements(customerOrder.CustomerOrder_Id))
            {
                string InfoType = string.Format("{0}{1}{2}{2}{3}", movement.Good is null? string.Empty : movement.Good.Good_Code + " / ", 
                                                                   movement.Description, Environment.NewLine, movement.Remark);
                bool DontShowUnitShipping = String.IsNullOrEmpty(movement.Unit_Shipping_Definition) || (movement.Unit_Shipping == 0);
                string Unit_Shipping_ToPrint = DontShowUnitShipping ? string.Empty 
                                                                   :  GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Shipping, DecimalType.Unit);
                bool DontShowUnitBilling = (movement.Unit_Billing == 0);
                string Unit_Billing_ToPrint = DontShowUnitBilling ? string.Empty
                                                                   : GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Billing, DecimalType.Unit);
                List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
                {
                    new Tuple<string, PDF_Align>(Unit_Shipping_ToPrint, PDF_Align.Right),
                    new Tuple<string, PDF_Align>(movement.Unit_Shipping_Definition, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Unit_Billing_ToPrint, PDF_Align.Right),
                    new Tuple<string, PDF_Align>(movement.Unit_Billing_Definition, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(InfoType, PDF_Align.Left)
                };
                if (Valued)
                {
                    item.Add(new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.RetailPrice, DecimalType.Currency, true), PDF_Align.Right));
                    item.Add(new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.Amount, DecimalType.Currency, true), PDF_Align.Right));
                }
                Items.Add(item);
            }
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontDataTable, ForeFontHeaderDataTable, 98, 
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
            doc.Add(ReportView.NewLine());
        }

        private static void InsertDeliveryNoteMovementTable(Document doc, ProviderOrdersView providerOrder)
        {
            bool Valued = providerOrder.Valued;
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(7)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT EXPEDICIÓ", 3, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT FACTURACIÓ", 3, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("TIPUS DE MERCADERIA", Valued ? 8 : 12, PDF_Align.Center, BaseColor.WHITE) }
            };
            if (Valued)
            {
                Columns.Add(new Tuple<string, int, PDF_Align, BaseColor>("PREU UNITARI", 2, PDF_Align.Center, BaseColor.WHITE));
                Columns.Add(new Tuple<string, int, PDF_Align, BaseColor>("IMPORT", 2, PDF_Align.Center, BackColumnTableColor));
            }
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>();
            foreach (ProviderOrderMovementsView movement in GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderMovements(providerOrder.ProviderOrder_Id))
            {
                string InfoType = string.Format("{0}{1}{2}{2}{3}", movement.Good is null ? string.Empty : movement.Good.Good_Code + " / ",
                                                                   movement.Description, Environment.NewLine, movement.Remark);
                bool DontShowUnitShipping = String.IsNullOrEmpty(movement.Unit_Shipping_Definition) || (movement.Unit_Shipping == 0);
                string Unit_Shipping_ToPrint = DontShowUnitShipping ? string.Empty
                                                                   : GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Shipping, DecimalType.Unit);
                bool DontShowUnitBilling = (movement.Unit_Billing == 0);
                string Unit_Billing_ToPrint = DontShowUnitBilling ? string.Empty
                                                                   : GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Billing, DecimalType.Unit);
                List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(7)
                {
                    new Tuple<string, PDF_Align>(Unit_Shipping_ToPrint, PDF_Align.Right),
                    new Tuple<string, PDF_Align>(movement.Unit_Shipping_Definition, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(Unit_Billing_ToPrint, PDF_Align.Right),
                    new Tuple<string, PDF_Align>(movement.Unit_Billing_Definition, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(InfoType, PDF_Align.Left)
                };
                if (Valued)
                {
                    item.Add(new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.RetailPrice, DecimalType.Currency, true), PDF_Align.Right));
                    item.Add(new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.Amount, DecimalType.Currency, true), PDF_Align.Right));
                }
                Items.Add(item);
            }
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontDataTable, ForeFontHeaderDataTable, 98,
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Delivery Note Amount

        private static void InsertDeliveryNoteAmount(Document doc, CustomerOrdersView customerOrder)//, decimal GrossAmount)
        {
            if (!customerOrder.Valued) return; // If the Customer Order is not valued this information shoud not appear in the report.
            string EarlyPaymentDiscountTitle =
                        string.Format("DTE. P. I. ({0})",
                                      GlobalViewModel.GetStringFromDecimalValue(customerOrder.BillingData_EarlyPaymentDiscount, DecimalType.Percent, true));
            string IVAAmountTitle =
                        string.Format("IVA ({0})",
                                      GlobalViewModel.GetStringFromDecimalValue(customerOrder.IVAPercent, DecimalType.Percent, true));
            string SurchargeAmountTitle =
                        string.Format("RECÀRREC ({0})",
                                      GlobalViewModel.GetStringFromDecimalValue(customerOrder.SurchargePercent, DecimalType.Percent, true));
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(6)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("SUMA", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>(EarlyPaymentDiscountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("BASE IMPONIBLE", 2, PDF_Align.Center, BackColumnTableColor) },
                { new Tuple<string, int, PDF_Align, BaseColor>(IVAAmountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>(SurchargeAmountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("TOTAL", 2, PDF_Align.Center, BackColumnTableColor) },
            };
            GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(customerOrder,
                                                                           out decimal GrossAmount,
                                                                           out decimal EarlyPayementDiscountAmount,
                                                                           out decimal TaxableBaseAmount,
                                                                           out decimal IVAAmount,
                                                                           out decimal SurchargeAmount,
                                                                           out decimal TotalAmount);
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(6)
            {
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(GrossAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(EarlyPayementDiscountAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TaxableBaseAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(IVAAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(SurchargeAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true), PDF_Align.Right),
            };
            Items.Add(item);
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontDataTable, ForeFontHeaderDataTable, 98,
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
            doc.Add(ReportView.NewLine());
        }

        private static void InsertDeliveryNoteAmount(Document doc, ProviderOrdersView providerOrder)//, decimal GrossAmount)
        {
            if (!providerOrder.Valued) return; // If the Customer Order is not valued this information shoud not appear in the report.
            string EarlyPaymentDiscountTitle =
                        string.Format("DTE. P. I. ({0})",
                                      GlobalViewModel.GetStringFromDecimalValue(providerOrder.BillingData_EarlyPaymentDiscount, DecimalType.Percent, true));
            string IVAAmountTitle =
                        string.Format("IVA ({0})",
                                      GlobalViewModel.GetStringFromDecimalValue(providerOrder.IVAPercent, DecimalType.Percent, true));
            string SurchargeAmountTitle =
                        string.Format("RECÀRREC ({0})",
                                      GlobalViewModel.GetStringFromDecimalValue(providerOrder.SurchargePercent, DecimalType.Percent, true));
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(6)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("SUMA", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>(EarlyPaymentDiscountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("BASE IMPONIBLE", 2, PDF_Align.Center, BackColumnTableColor) },
                { new Tuple<string, int, PDF_Align, BaseColor>(IVAAmountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>(SurchargeAmountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("TOTAL", 2, PDF_Align.Center, BackColumnTableColor) },
            };
            GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(providerOrder,
                                                                           out decimal GrossAmount,
                                                                           out decimal EarlyPayementDiscountAmount,
                                                                           out decimal TaxableBaseAmount,
                                                                           out decimal IVAAmount,
                                                                           out decimal SurchargeAmount,
                                                                           out decimal TotalAmount);
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(6)
            {
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(GrossAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(EarlyPayementDiscountAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TaxableBaseAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(IVAAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(SurchargeAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true), PDF_Align.Right),
            };
            Items.Add(item);
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontDataTable, ForeFontHeaderDataTable, 98,
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Delivery Note Comment Foot

        private static void InsertDeliveryNoteCommentFoot(Document doc, CustomerOrdersView customerOrder)
        {
            List<PdfPCell> CellsDeliveryNoteFoot = new List<PdfPCell>()
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(customerOrder.Remarks, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
            };
            List<PdfPCell> Cells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(CellsDeliveryNoteFoot, BaseColor.WHITE, BaseColor.WHITE, 8, 10),
                ReportView.CreateEmptyRow(1),
            };
            doc.Add(ReportView.CreateTable(Cells, 10));
            doc.Add(new Paragraph(Environment.NewLine));
        }

        private static void InsertDeliveryNoteCommentFoot(Document doc, ProviderOrdersView providerOrder)
        {
            List<PdfPCell> CellsDeliveryNoteFoot = new List<PdfPCell>()
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(providerOrder.Remarks, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
            };
            List<PdfPCell> Cells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(CellsDeliveryNoteFoot, BaseColor.WHITE, BaseColor.WHITE, 8, 10),
                ReportView.CreateEmptyRow(1),
            };
            doc.Add(ReportView.CreateTable(Cells, 10));
            doc.Add(new Paragraph(Environment.NewLine));
        }

        #endregion

        #region Delivery Note Foot

        public static void InsertDeliveryNoteFoot(Document doc, CustomerOrdersView customerOrder)
        {
            GetDeliveryNoteFootInfo(customerOrder, out string AgentInfo, out string EffectInfo, out string PayDaysInfo, out string BankInfo, out string IBANInfo);
            List<PdfPCell> CellsDeliveryNoteFoot = new List<PdfPCell>()
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(AgentInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(EffectInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(PayDaysInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(BankInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(IBANInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
            };
            List<PdfPCell> Cells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(CellsDeliveryNoteFoot, BaseColor.WHITE, BaseColor.WHITE, 8, 10),
                ReportView.CreateEmptyRow(1),
            };
            doc.Add(ReportView.CreateTable(Cells, 10));
        }

        public static void InsertDeliveryNoteFoot(Document doc, ProviderOrdersView providerOrder)
        {
            GetDeliveryNoteFootInfo(providerOrder, out string AgentInfo, out string EffectInfo, out string PayDaysInfo, out string BankInfo, out string IBANInfo);
            List<PdfPCell> CellsDeliveryNoteFoot = new List<PdfPCell>()
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(AgentInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(EffectInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(PayDaysInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(BankInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateRow(IBANInfo, 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
            };
            List<PdfPCell> Cells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(CellsDeliveryNoteFoot, BaseColor.WHITE, BaseColor.WHITE, 8, 10),
                ReportView.CreateEmptyRow(1),
            };
            doc.Add(ReportView.CreateTable(Cells, 10));
        }

        private static void GetDeliveryNoteFootInfo(CustomerOrdersView customerOrder, out string AgentInfo, out string EffectInfo,
                                                    out string PayDaysInfo, out string BankInfo, out string IBANInfo)
        {
            // Complete AgentInfo thats include text 'REPRESENTANT' and the Name of the name of the Agent. The customer has indicated that this
            // informations ara not needed for him.
            //AgentInfo = (customerOrder.BillingData_Agent != null) ?
            //            string.Format("REPRESENTAT ({0}) : {1}", customerOrder.BillingData_Agent.Agent_Id, customerOrder.BillingData_Agent.Name) :
            //            string.Empty;
            AgentInfo = (customerOrder.BillingData_Agent != null) ? string.Format("({0})", customerOrder.BillingData_Agent.Agent_Id) : string.Empty;
            string ExpirationDays = GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals);
            string ExpirationDays_2 = string.Format(" , {0} + {1}",
                                                    GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals),
                                                    GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals));
            string ExpirationDays_3 = string.Format(" , {0} + {1} + {2}",
                                                    GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals),
                                                    GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals),
                                                    GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals));
            EffectInfo = string.Format("{0} {1} A : {2} {3} {4} DIES.",
                                       customerOrder.DataBank_NumEffect,
                                       (customerOrder.EffectType != null) ? customerOrder.EffectType.Description : string.Empty,
                                       ((customerOrder.DataBank_NumEffect >= 1) && (customerOrder.DataBank_NumEffect <= 3)) ? ExpirationDays : string.Empty,
                                       ((customerOrder.DataBank_NumEffect >= 2) && (customerOrder.DataBank_NumEffect <= 3)) ? ExpirationDays_2 : string.Empty,
                                       ((customerOrder.DataBank_NumEffect == 3)) ? ExpirationDays_3 : string.Empty);
            string PayDay_1 = GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_Payday_1, DecimalType.WithoutDecimals);
            string PayDay_2 = GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_Payday_2, DecimalType.WithoutDecimals);
            string PayDay_3 = GlobalViewModel.GetStringFromDecimalValue(customerOrder.DataBank_Payday_3, DecimalType.WithoutDecimals);
            PayDaysInfo = ((customerOrder.DataBank_Payday_1 <= 0) && (customerOrder.DataBank_Payday_2 <= 0) && (customerOrder.DataBank_Payday_3 <= 0)) ?
                          string.Empty :
                          string.Format("DIA(ES) FIXE(ES) DE PAGAMENT : {0} {1} {2} DE CADA MES",
                                        (customerOrder.DataBank_Payday_1 > 0) ? PayDay_1 : string.Empty,
                                        (customerOrder.DataBank_Payday_2 > 0) ? string.Format(" - {0}", PayDay_2) : string.Empty,
                                        (customerOrder.DataBank_Payday_3 > 0) ? string.Format(" - {0}", PayDay_3) : string.Empty);
            if ((customerOrder.EffectType != null) && (customerOrder.EffectType.EffectType_Id == 6))
            {
                BankInfo = string.IsNullOrEmpty(customerOrder.DataBank_Bank) ? "No Informat" : string.Format("BANC o CAIXA : {0}", customerOrder.DataBank_Bank);
                string IBANFromCustomerOrder = customerOrder.GetStringIBANFromCustomerOrder();
                IBANInfo = string.Format("IBAN : {0}", string.IsNullOrEmpty(IBANFromCustomerOrder) ? "No Informat" : IBANFromCustomerOrder.Trim());
            }
            else BankInfo = IBANInfo = string.Empty;
        }

        private static void GetDeliveryNoteFootInfo(ProviderOrdersView providerOrder, out string AgentInfo, out string EffectInfo,
                                                  out string PayDaysInfo, out string BankInfo, out string IBANInfo)
        {
            // Complete AgentInfo thats include text 'REPRESENTANT' and the Name of the name of the Agent. The customer has indicated that this
            // informations ara not needed for him.
            //AgentInfo = (customerOrder.BillingData_Agent != null) ?
            //            string.Format("REPRESENTAT ({0}) : {1}", customerOrder.BillingData_Agent.Agent_Id, customerOrder.BillingData_Agent.Name) :
            //            string.Empty;
            AgentInfo = (providerOrder.BillingData_Agent != null) ? string.Format("({0})", providerOrder.BillingData_Agent.Agent_Id) : string.Empty;
            string ExpirationDays = GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals);
            string ExpirationDays_2 = string.Format(" , {0} + {1}",
                                                    GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals),
                                                    GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals));
            string ExpirationDays_3 = string.Format(" , {0} + {1} + {2}",
                                                    GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals),
                                                    GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals),
                                                    GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals));
            EffectInfo = string.Format("{0} {1} A : {2} {3} {4} DIES.",
                                       providerOrder.DataBank_NumEffect,
                                       (providerOrder.EffectType != null) ? providerOrder.EffectType.Description : string.Empty,
                                       ((providerOrder.DataBank_NumEffect >= 1) && (providerOrder.DataBank_NumEffect <= 3)) ? ExpirationDays : string.Empty,
                                       ((providerOrder.DataBank_NumEffect >= 2) && (providerOrder.DataBank_NumEffect <= 3)) ? ExpirationDays_2 : string.Empty,
                                       ((providerOrder.DataBank_NumEffect == 3)) ? ExpirationDays_3 : string.Empty);
            string PayDay_1 = GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_Payday_1, DecimalType.WithoutDecimals);
            string PayDay_2 = GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_Payday_2, DecimalType.WithoutDecimals);
            string PayDay_3 = GlobalViewModel.GetStringFromDecimalValue(providerOrder.DataBank_Payday_3, DecimalType.WithoutDecimals);
            PayDaysInfo = ((providerOrder.DataBank_Payday_1 <= 0) && (providerOrder.DataBank_Payday_2 <= 0) && (providerOrder.DataBank_Payday_3 <= 0)) ?
                          string.Empty :
                          string.Format("DIA(ES) FIXE(ES) DE PAGAMENT : {0} {1} {2} DE CADA MES",
                                        (providerOrder.DataBank_Payday_1 > 0) ? PayDay_1 : string.Empty,
                                        (providerOrder.DataBank_Payday_2 > 0) ? string.Format(" - {0}", PayDay_2) : string.Empty,
                                        (providerOrder.DataBank_Payday_3 > 0) ? string.Format(" - {0}", PayDay_3) : string.Empty);
            if ((providerOrder.EffectType != null) && (providerOrder.EffectType.EffectType_Id == 6))
            {
                BankInfo = string.IsNullOrEmpty(providerOrder.DataBank_Bank) ? "No Informat" : string.Format("BANC o CAIXA : {0}", providerOrder.DataBank_Bank);
                string IBANFromCustomerOrder = providerOrder.GetStringIBANFromProviderOrder();
                IBANInfo = string.Format("IBAN : {0}", string.IsNullOrEmpty(IBANFromCustomerOrder) ? "No Informat" : IBANFromCustomerOrder.Trim());
            }
            else BankInfo = IBANInfo = string.Empty;
        }

        #endregion
    }
}