#region Librerias usadas porm la clase

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
    public enum ProviderOrderFlag
    {
        Print,
        SendByEMail,
    }

    /// <summary>
    /// Source Report : 
    /// Report Description : list the ProviderOrders
    /// Source Query : ProviderOrders selecteds
    /// </summary>
    public static class ProviderOrdersReportView
    {
        #region Attributes

        private static BaseColor BackColumnTableColor = new BaseColor(247, 217, 173);

        private static Font ForeFontTable = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, CMYKColor.BLACK);

        private static Font ForeFontHeaderDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 6.5f, Font.NORMAL, CMYKColor.BLACK);

        private static Font ForeFontDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL, CMYKColor.BLACK);

        #endregion

        #region Main Methods

        /// <summary>
        /// Create the Report with the information of the Providers selecteds.
        /// </summary>
        /// <param name="providerOrder">Providers selecteds.</param>
        public static bool CreateReport(ProviderOrdersView providerOrder, string PDF_FileName, out string ErrMsg, bool IsProforma = true)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Vertical, 0, 0, 15, ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.ProviderOrder, PDF_FileName);
                doc.AddTitle(string.Format("Comanda de Client Numero {0}", providerOrder.ProviderOrder_Id));
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                CommonReportView.InsertHispaniaHeader(doc);
                if(IsProforma)
                {
                    CommonReportView.InsertProviderInfo(doc, providerOrder.Provider, false, CustomerInfoType.CustomerOrderProforma);
                }
                else
                {
                    CommonReportView.InsertProviderInfo(doc, providerOrder.Provider, false, CustomerInfoType.CustomerOrderComanda);
                }
                
                InsertProviderOrderHeader(doc, providerOrder);
                InsertProviderOrderMovementTable(doc, providerOrder);
                InsertProviderOrderAmount(doc, providerOrder);
                InsertProviderOrderCommentFoot(doc, providerOrder);
                InsertProviderOrderFoot(doc, providerOrder);
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
                ErrMsg = string.Format("Error, al construïr el PDF de la Comanda de Client número {0}.\r\nDetalls: {1}",
                                       providerOrder.ProviderOrder_Id, MsgManager.ExcepMsg(ex));
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
                    string Subject = string.Format("Comanda de Client número ({0})", providerOrder.ProviderOrder_Id);
                    string BodyMessage = string.Format("Comanda de Client número ({0})\r\nPapers Hispània S.L.\r\n", providerOrder.ProviderOrder_Id);
                    List<Tuple<string, string>> FileAttachments = new List<Tuple<string, string>>(1)
                    {
                        new Tuple<string, string>(PDF_FileName, string.Format("Comanda_Client_{0}", providerOrder.ProviderOrder_Id))
                    };
                    ReportView.SendReport(EMAIL_Address, Subject, BodyMessage, FileAttachments, out ErrMsg);
                }
                else
                {
                    ErrMsg = string.Format("Error, a l'enviar per email el PDF de la Comanda de Client número {0}.\r\nDetalls: Manca l'adreça electrònica del client.",
                                           providerOrder.ProviderOrder_Id);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, a l'enviar per email el PDF de la Comanda de Client número {0}.\r\nDetalls: {1}",
                                       providerOrder.ProviderOrder_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool UpdateProviderOrderFlag(ProviderOrdersView providerOrder, ProviderOrderFlag FlagToUpdate, object newFlagValue, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                ProviderOrdersView providerOrderForUpdate = new ProviderOrdersView(providerOrder);
                switch (FlagToUpdate)
                {
                    case ProviderOrderFlag.Print:
                         providerOrderForUpdate.Print_ProviderOrder = (bool) newFlagValue;
                         break;
                    case ProviderOrderFlag.SendByEMail:
                         providerOrderForUpdate.SendByEMail_ProviderOrder = (bool) newFlagValue;
                         break;
                    default:
                            throw new ArgumentException(string.Format("Flag {0} no rerconegut.", FlagToUpdate));
                }
                GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrder(providerOrderForUpdate);
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al actualitzar el flag '{0}' per la Comanda de Client número '{1}' a la Base de Dades.\r\nDetalls: {2}",
                                        FlagToUpdate, providerOrder.DeliveryNote_Id, MsgManager.ExcepMsg(ex));
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
                decimal Year = providerOrder.Date.Year;
                string BasePdfFileName = string.Format("ComandaClient_({0})", providerOrder.ProviderOrder_Id);
                if (ReportView.ExistPDF_FileName(PDF_Report_Types.ProviderOrder, BasePdfFileName, 
                                                 out string[] OldPdfReports, Year))
                {
                    string Question = string.Format("Hi ha un o més informes creats per la Comanda de Client {0} vol continuar i esborrar-los ?",
                                                    providerOrder.ProviderOrder_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        ReportView.DeleteReportFiles(OldPdfReports);
                        PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.ProviderOrder, BasePdfFileName, Year);
                    }
                    else
                    {
                        ErrMsg = string.Format("Informació, s'ha cancel·lat la construcció de l'informe de la Comanda de Client número {0}.",
                                                providerOrder.ProviderOrder_Id);
                    }
                }
                else
                {
                    PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.ProviderOrder, BasePdfFileName, Year);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al construïr el PDF de la Comanda de Client número {0}.\r\nDetalls: {1}",
                                       providerOrder.ProviderOrder_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        #endregion

        #region Provider Order Header

        private static void InsertProviderOrderHeader(Document doc, ProviderOrdersView providerOrder)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(2)
                    {
                        { new Tuple<string, int, PDF_Align>("DATA", 8, PDF_Align.Center) },
                        { new Tuple<string, int, PDF_Align>("NÚMERO COMANDA", 6, PDF_Align.Center) }
                    };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1)
                    {
                        new List<Tuple<string, PDF_Align>>(2)
                        {
                            new Tuple<string, PDF_Align>(providerOrder.Date_Str, PDF_Align.Center),
                            new Tuple<string, PDF_Align>(providerOrder.ProviderOrder_Id.ToString(), PDF_Align.Center),
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

        #region Provider Order Movement Table

        private static void InsertProviderOrderMovementTable(Document doc, ProviderOrdersView providerOrder)
        {
            bool Valued = providerOrder.Valued;
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(7)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT EXPEDICIÓ", 3, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT FACTURACIÓ", 3, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("TIPUS DE MERCADERIA", Valued ? 8 : 12, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("PREU UNITARI", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("IMPORT", 2, PDF_Align.Center, BackColumnTableColor) }
            };
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
                    new Tuple<string, PDF_Align>(InfoType, PDF_Align.Left),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.RetailPrice, DecimalType.Currency, true), PDF_Align.Right),
                    new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.Amount, DecimalType.Currency, true), PDF_Align.Right)
                };
                Items.Add(item);
            }
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontDataTable, ForeFontHeaderDataTable, 98,
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Provider Order Amount

        private static void InsertProviderOrderAmount(Document doc, ProviderOrdersView providerOrder)
        {
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

        #region Provider Order Comment Foot

        private static void InsertProviderOrderCommentFoot(Document doc, ProviderOrdersView providerOrder)
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

        #region Provider Order Foot

        public static void InsertProviderOrderFoot(Document doc, ProviderOrdersView providerOrder)
        {
            GetProviderOrderFootInfo(providerOrder, out string AgentInfo, out string EffectInfo, out string PayDaysInfo, out string BankInfo, out string IBANInfo);
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
            doc.Add(new Paragraph(Environment.NewLine));
        }

        private static void GetProviderOrderFootInfo(ProviderOrdersView providerOrder, out string AgentInfo, out string EffectInfo,
                                                     out string PayDaysInfo, out string BankInfo, out string IBANInfo)
        {
            // Complete AgentInfo thats include text 'REPRESENTANT' and the Name of the name of the Agent. The provider has indicated that this
            // informations ara not needed for him.
            //AgentInfo = (providerOrder.BillingData_Agent != null) ?
            //             string.Format("REPRESENTAT ({0}) : {1}", providerOrder.BillingData_Agent.Agent_Id, providerOrder.BillingData_Agent.Name) :
            //             string.Empty;
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
                string IBANFromProviderOrder = providerOrder.GetStringIBANFromProviderOrder();
                IBANInfo = string.Format("IBAN : {0}", string.IsNullOrEmpty(IBANFromProviderOrder) ? "No Informat" : IBANFromProviderOrder.Trim());
            }
            else BankInfo = IBANInfo = string.Empty;
        }

        #endregion
    }
}