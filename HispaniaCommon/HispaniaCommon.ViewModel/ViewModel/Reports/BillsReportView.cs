#region Librerias usadas por la clase

using iTextSharp.text;
using iTextSharp.text.pdf;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    public enum BillFlag
    {
        Print,
        SendByEMail,
    }
    public enum ReceiptFlag
    {
        Print,
        SendByEMail,
    }

    /// <summary>
    /// Source Report : 
    /// Report Description : list the CustomerOrders
    /// Source Query : CustomerOrders selecteds
    /// </summary>
    public static class BillsReportView
    {
        #region Attributes

        private static BaseColor BackColumnTableBillInfoColor = new BaseColor(216, 239, 255);

        private static BaseColor BackColumnTableColor = new BaseColor(247, 217, 173);

        private static Font ForeFontTable = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, CMYKColor.BLACK);

        private static Font ForeFontRegisterInfo = new Font(Font.FontFamily.TIMES_ROMAN, 6f, Font.NORMAL, CMYKColor.BLACK);

        private static Font ForeFontHeaderDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 6.5f, Font.NORMAL, CMYKColor.BLACK);

        private static Font ForeFontDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL, CMYKColor.BLACK);

        private static Font ForeFontMovementDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL, CMYKColor.BLACK);

        private static Font ForeFontReceiptDataTable = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL, CMYKColor.BLACK);

        private static BaseColor ForeColorHispaniaCompany = new BaseColor(181, 133, 63);

        private static string AmericanTypewriterFileFontPath = string.Format("{0}\\ReportsFont\\AmericanTypewriterRegular.ttf",
                                                                             System.IO.Path.GetDirectoryName(Assembly.GetAssembly(typeof(CommonReportView)).Location));

        private static BaseFont AmericanTypewriter = BaseFont.CreateFont(AmericanTypewriterFileFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        private static Font ForeFontHispaniaCompany = new Font(AmericanTypewriter, 20, Font.BOLD, ForeColorHispaniaCompany);

        private static Font ForeFontFootHispaniaCompany = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, ForeColorHispaniaCompany);

        private static BaseColor ForeColorAddress = CMYKColor.BLACK;

        //private static Font ForeFontHispaniaAddress = new Font(AmericanTypewriter, 6f, Font.BOLD, ForeColorAddress);
        private static Font ForeFontHispaniaAddress = new Font(Font.FontFamily.TIMES_ROMAN, 6f, Font.BOLD, ForeColorAddress);

        #endregion

        #region Main Methods

        /// <summary>
        /// Create the Report with the information of the Customers selecteds.
        /// </summary>
        /// <param name="bill">Customers selecteds.</param>
        public static bool CreateReport(BillsView bill, string Bill_PDF_FileName, out Dictionary<int, string> Receipts_PDF_Filename, out string ErrMsg,
                                        bool DuplicateReceipt = true)
        {
            if (CreateBillReport(bill, Bill_PDF_FileName, out ErrMsg))
            {
                int NumReceipt = 1;
                bool ReceiptsCreatedOk = true;
                IEnumerable<ReceiptsView> query = bill.Receipts.OrderBy(ReceiptsView => ReceiptsView.Expiration_Date);
                Receipts_PDF_Filename = new Dictionary<int, string>(bill.Receipts.Count);
                foreach (ReceiptsView receipt in query)
                {
                    ReceiptsCreatedOk &= 
                       CreateReceiptReport(bill, receipt, NumReceipt++, Bill_PDF_FileName, out string Receipt_PDF_Filename, out ErrMsg, DuplicateReceipt);
                    if (ReceiptsCreatedOk)
                    {
                        Receipts_PDF_Filename.Add(receipt.Receipt_Id, Receipt_PDF_Filename);
                    }
                    else break;
                }
                return ReceiptsCreatedOk;
            }
            else
            {
                Receipts_PDF_Filename = new Dictionary<int, string>();
                return false;
            }
        }

        public static bool CheckAndContinueIfExistReport(BillsView bill, out string PDF_FileName, out string ErrMsg)
        {
            PDF_FileName = null;
            ErrMsg = string.Empty;
            try
            {
                decimal Year = bill.Year;
                string BasePdfFileName = string.Format("Factura_{0}-{1}", bill.Year, bill.Bill_Id);
                if (ReportView.ExistPDF_FileName(PDF_Report_Types.Bill, BasePdfFileName, out string[] OldPdfReports, 
                                                 Year))
                {
                    string Question = string.Format("Hi ha un o més informes creats per la factura {0} vol continuar i esborrar-los ?", bill.Bill_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        ReportView.DeleteReportFiles(OldPdfReports);
                        PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.Bill, BasePdfFileName, Year);
                    }
                    else
                    {
                        ErrMsg = string.Format("Informació, s'ha cancel·lat la construcció de l'informe de la factura número {0}.", bill.Bill_Id);
                    }
                }
                else
                {
                    PDF_FileName = ReportView.GetPDF_FileName(PDF_Report_Types.Bill, BasePdfFileName, Year);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al construïr el PDF de la factura número {0}.\r\nDetalls: {1}",
                                       bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool SendByEMail(BillsView bill, string PDF_FileName, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(bill.Customer.Company_EMail))
                {
                    string EMAIL_Address = bill.Customer.Company_EMail;
                    string Subject = string.Format("Factura número ({0})", bill.Bill_Id);
                    string BodyMessage = string.Format("Factura número ({0})\r\nPapers Hispània S.L.\r\n", bill.Bill_Id);
                    List<Tuple<string, string>> FileAttachments = new List<Tuple<string, string>>(1)
                    {
                        new Tuple<string, string>(PDF_FileName, string.Format("Factura_{0}", bill.Bill_Id))
                    };
                    ReportView.SendReport(EMAIL_Address, Subject, BodyMessage, FileAttachments, out ErrMsg);
                }
                else
                {
                    ErrMsg = string.Format("Error, a l'enviar per email el PDF de la factura número {0}.\r\nDetalls: Manca l'adreça electrònica del client.",
                                           bill.Bill_Id);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, a l'enviar per email el PDF de la factura número {0}.\r\nDetalls: {1}",
                                       bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool UpdateFileNameInBill(BillsView bill, string PDF_FileName, Dictionary<int, string> Receipts_PDF_Name, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                HispaniaCompData.Bill Bill =
                        GlobalViewModel.Instance.HispaniaViewModel.GetBill(bill.Bill_Id, bill.Year);
                Bill.FileNamePDF = PDF_FileName;
                BillsView BillToUpdate = new BillsView(Bill);
                foreach (ReceiptsView receipt in BillToUpdate.Receipts)
                {
                    receipt.FileNamePDF = Receipts_PDF_Name[receipt.Receipt_Id];
                }
                GlobalViewModel.Instance.HispaniaViewModel.UpdateBill(BillToUpdate);
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al guardar el nom del fitxer PDF generat per la factura número '{0}' a la Base de Dades.\r\nDetalls: {1}",
                                       bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool GetFileNameInBill(BillsView bill, out string PDF_FileName, out string ErrMsg)
        {
            try
            {
                HispaniaCompData.Bill Bill = GlobalViewModel.Instance.HispaniaViewModel.GetBill(bill.Bill_Id, bill.Year);
                PDF_FileName = Bill.FileNamePDF;
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                PDF_FileName = null;
                ErrMsg = string.Format("Error, al obtindre el nom del fitxer PDF generat per la factura número {0} a la Base de Dades.\r\nDetalls: {1}",
                                       bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }
        
        public static bool UpdateBillFlag(BillsView bill, BillFlag FlagToUpdate, object newFlagValue, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                BillsView billForUpdate = new BillsView(bill);
                switch (FlagToUpdate)
                {
                    case BillFlag.Print:
                         billForUpdate.Print = (bool) newFlagValue;
                         break;
                    case BillFlag.SendByEMail:
                         billForUpdate.SendByEMail = (bool) newFlagValue;
                         break;
                    default:
                         throw new ArgumentException(string.Format("Flag {0} no rerconegut.", FlagToUpdate));
                }
                GlobalViewModel.Instance.HispaniaViewModel.UpdateBill(billForUpdate);
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al actualitzar el flag '{0}' per la factura numero '{1}' a la Base de Dades.\r\nDetalls: {2}",
                                       FlagToUpdate, bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool GetFileNameInReceipt(ReceiptsView receipt, out string PDF_FileName, out string ErrMsg)
        {
            try
            {
                HispaniaCompData.Receipt Receipt = GlobalViewModel.Instance.HispaniaViewModel.GetReceipt(receipt.Receipt_Id);
                PDF_FileName = Receipt.FileNamePDF;
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                PDF_FileName = null;
                ErrMsg = string.Format("Error, al obtindre el nom del fitxer PDF generat pel rebut numero {0} de la factura {1} a la Base de Dades.\r\nDetalls: {2}",
                                       receipt.Receipt_Id, receipt.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool SendByEMail(BillsView bill, List<ReceiptsView> ReceiptsListToSend, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(bill.Customer.Company_EMail))
                {
                    string EMAIL_Address = bill.Customer.Company_EMail;
                    string Subject = string.Format("Rebuts de la factura número {0}", bill.Bill_Id);
                    StringBuilder BodyMessage = new StringBuilder(string.Empty);
                    BodyMessage.AppendFormat("Rebuts de la factura número {0}\r\n", bill.Bill_Id);
                    foreach (ReceiptsView Receipt in ReceiptsListToSend)
                    {
                        BodyMessage.AppendFormat("Rebut número {0} de la data {1}\r\n", 
                                                  Receipt.Receipt_Id, 
                                                  GlobalViewModel.GetStringFromDateTimeValue(Receipt.Expiration_Date));
                    };
                    BodyMessage.AppendLine("Papers Hispània S.L.\r\n");
                    List<Tuple<string, string>> FileAttachments = new List<Tuple<string, string>>(ReceiptsListToSend.Count);
                    foreach(ReceiptsView Receipt in ReceiptsListToSend)
                    {
                        if (!string.IsNullOrEmpty(Receipt.FileNamePDF))
                        {
                            FileAttachments.Add(new Tuple<string, string>(Receipt.FileNamePDF, string.Format("Rebut_{0}", Receipt.Receipt_Id)));
                        }
                    };
                    ReportView.SendReport(EMAIL_Address, Subject, BodyMessage.ToString(), FileAttachments, out ErrMsg);
                }
                else
                {
                    ErrMsg = string.Format("Error, a l'enviar per email els PDFs dels rebuts de la factura número {0}.\r\n" +
                                           "Detalls: Manca l'adreça electrònica del client.", bill.Bill_Id);
                }
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, a l'enviar per email els PDFs dels rebuts de la factura número {0}.\r\nDetalls: {1}",
                                       bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        public static bool UpdateReceiptFlag(ReceiptsView receipt, ReceiptFlag FlagToUpdate, object newFlagValue, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                ReceiptsView receiptForUpdate = new ReceiptsView(receipt);
                switch (FlagToUpdate)
                {
                    case ReceiptFlag.Print:
                         receiptForUpdate.Print = (bool) newFlagValue;
                         break;
                    case ReceiptFlag.SendByEMail:
                         receiptForUpdate.SendByEMail = (bool) newFlagValue;
                         break;
                    default:
                         throw new ArgumentException(string.Format("Flag {0} no rerconegut.", FlagToUpdate));
                }
                GlobalViewModel.Instance.HispaniaViewModel.UpdateReceipt(receiptForUpdate);
            }
            catch (Exception ex)
            {
                ErrMsg = string.Format("Error, al actualitzar el flag '{0}' per la factura número '{1}' a la Base de Dades.\r\nDetalls: {2}",
                                       FlagToUpdate, receipt.Receipt_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        #endregion

        #region Create Bill Report

        /// <summary>
        /// Create the Report with the information of the Customers selecteds.
        /// </summary>
        /// <param name="bill">Customers selecteds.</param>
        private static bool CreateBillReport(BillsView bill, string PDF_FileName, out string ErrMsg)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Vertical, 15, 15, 15, 
                                                ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.CustomerOrder, PDF_FileName);
                doc.AddTitle(string.Format("Factura Numero {0}", bill.Bill_Id));
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                CommonReportView.InsertHispaniaHeader(doc);
                CommonReportView.InsertCustomerInfo(doc, bill.Customer, true);
                InsertBillMovementTable(doc, bill);
                InsertBillAmount(doc, bill);
                InsertBillDataBankInfo(doc, bill);
                InsertBillReceiptsAndSendInfo(doc, bill);
                InsertBillAndDeliveryNoteCommentFoot(doc, bill);
                doc.Close();
                writer.Close();
                ReportView.AddPageNumber(PDF_FileName, PDF_Orientation.Horizontal);
                ErrMsg = string.Empty;                
            }
            catch (Exception ex)
            {
                if ((doc != null) && (doc.IsOpen())) doc.Close();
                if (writer != null) writer.Close();
                ErrMsg = string.Format("Error, al construïr el PDF de la factura número {0}.\r\nDetalls: {1}",
                                       bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        #region Bill Movement Table

        private static void InsertBillMovementTable(Document doc, BillsView bill)
        {
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(8)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("N.ALB", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT EXPEDICIÓ", 4, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("QUANT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("UNITAT FACTURACIÓ", 4, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("TIPUS DE MERCADERIA", 11, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("PREU UNITARI", 3, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("IMPORT", 3, PDF_Align.Center, BackColumnTableColor) }
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>();
            foreach (CustomerOrdersView customerOrder in bill.CustomerOrders.OrderBy(x => x.DeliveryNote_Date))
            {
                int CustomerOrder_Id = customerOrder.CustomerOrder_Id;
                foreach (CustomerOrderMovementsView movement in GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderMovements(CustomerOrder_Id).OrderBy(y => y.CustomerOrderMovement_Id))
                {
                    string InfoType = string.Format("{0}{1}{3}{3}{2}", movement.Good is null ? string.Empty : movement.Good.Good_Code + " / ",
                                                                       movement.Description, movement.Remark, Environment.NewLine);
                    List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(8)
                    {
                        new Tuple<string, PDF_Align>(customerOrder.DeliveryNote_Id.ToString(), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Shipping, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(movement.Unit_Shipping_Definition, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Billing, DecimalType.Unit), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(movement.Unit_Billing_Definition, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(InfoType, PDF_Align.Left),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.RetailPrice, DecimalType.Currency, true), PDF_Align.Right),
                        new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(movement.Amount, DecimalType.Currency, true), PDF_Align.Right)
                    };
                    Items.Add(item);
                }
            }
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontMovementDataTable, ForeFontHeaderDataTable, 98,
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Bill Amount

        private static void InsertBillAmount(Document doc, BillsView bill)
        {
            string EarlyPaymentDiscountTitle =
                      string.Format("DTE. P. I. ({0})",
                                    GlobalViewModel.GetStringFromDecimalValue(bill.BillingData_EarlyPaymentDiscount, DecimalType.Percent, true));
            string IVAAmountTitle =
                      string.Format("IVA ({0})",
                                    GlobalViewModel.GetStringFromDecimalValue(bill.IVAPercent, DecimalType.Percent, true));
            string SurchargeAmountTitle =
                      string.Format("RECÀRREC ({0})",
                                    GlobalViewModel.GetStringFromDecimalValue(bill.SurchargePercent, DecimalType.Percent, true));
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(8)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("IMPORT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>(EarlyPaymentDiscountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("BASE IMPONIBLE", 2, PDF_Align.Center, BackColumnTableColor) },
                { new Tuple<string, int, PDF_Align, BaseColor>(IVAAmountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>(SurchargeAmountTitle, 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("NUM FACTURA", 2, PDF_Align.Center, BackColumnTableBillInfoColor) },
                { new Tuple<string, int, PDF_Align, BaseColor>("DATA FACTURA", 2, PDF_Align.Center, BackColumnTableBillInfoColor) },
                { new Tuple<string, int, PDF_Align, BaseColor>("TOTAL FACTURA", 2, PDF_Align.Center, BackColumnTableColor) },
            };
            GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(bill,
                                                                           out decimal GrossAmount,
                                                                           out decimal EarlyPayementDiscountAmount,
                                                                           out decimal TaxableBaseAmount,
                                                                           out decimal IVAAmount,
                                                                           out decimal SurchargeAmount,
                                                                           out decimal TotalAmount);
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(8)
            {
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(GrossAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(EarlyPayementDiscountAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TaxableBaseAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(IVAAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(SurchargeAmount, DecimalType.Currency, true), PDF_Align.Right),
                new Tuple<string, PDF_Align>(bill.Bill_Id_Str, PDF_Align.Center),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDateTimeValue(bill.Date), PDF_Align.Center),
                new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true), PDF_Align.Right),
            };
            Items.Add(item);
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontDataTable, ForeFontHeaderDataTable, 98,
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
        }

        #endregion

        #region Bill Data Bank Info

        private static void InsertBillDataBankInfo(Document doc, BillsView bill)
        {
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(3)  
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("NUM CLIENT", 2, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("FORMA DE PAGAMENT", 4, PDF_Align.Center, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("PAGABLE A", 10, PDF_Align.Left, BaseColor.WHITE) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            string DataBank_Bank = string.IsNullOrEmpty(bill.DataBank_Bank) ? string.Empty : bill.DataBank_Bank.Trim();
            string IBANFromBill = bill.GetStringIBANFromBill();
            string PayableTo = string.Format("{0}\r\n{1}", 
                                             DataBank_Bank, 
                                             string.IsNullOrEmpty(IBANFromBill) ? string.Empty : IBANFromBill.Trim());
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(3)
            {
                new Tuple<string, PDF_Align>(bill.Customer_Id_Str, PDF_Align.Center),
                new Tuple<string, PDF_Align>(bill.DataBank_EffectType.Description, PDF_Align.Left),
                new Tuple<string, PDF_Align>(PayableTo, PDF_Align.Left),
            };
            Items.Add(item);
            doc.Add(ReportView.CreateTable(Columns, Items, ForeFontDataTable, ForeFontHeaderDataTable, 98,
                                           null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Receipts and Send Info

        private static void InsertBillReceiptsAndSendInfo(Document doc, BillsView bill)
        {
            List<Tuple<string, int, PDF_Align>> Columns = new List<Tuple<string, int, PDF_Align>>(2)
            {
                { new Tuple<string, int, PDF_Align>("VENCIMENT", 8, PDF_Align.Center) },
                { new Tuple<string, int, PDF_Align>("IMPORT", 6, PDF_Align.Center) }
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(bill.Receipts.Count);
            IEnumerable<ReceiptsView> query = bill.Receipts.OrderBy(ReceiptsView => ReceiptsView.Expiration_Date);
            foreach (ReceiptsView Receipt in query)
            {
                Items.Add(new List<Tuple<string, PDF_Align>>(2)
                          {
                              new Tuple<string, PDF_Align>(Receipt.Expiration_Date_Str, PDF_Align.Center),
                              new Tuple<string, PDF_Align>(GlobalViewModel.GetStringFromDecimalValue(Receipt.Amount, DecimalType.Currency, true), PDF_Align.Right),
                          });
            }
            List<PdfPCell> Cells = new List<PdfPCell>(1)
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Columns, Items, ForeFontTable), 14),
            };
            List<Tuple<string, int, PDF_Align>> SendColumns = new List<Tuple<string, int, PDF_Align>>(2)
            {
                { new Tuple<string, int, PDF_Align>("ENVIAMENT", 6, PDF_Align.Left) },
            };
            string AgentInfo = (bill.BillingData_Agent != null) ? string.Format("({0})", bill.BillingData_Agent.Agent_Id) : string.Empty;
            List<List<Tuple<string, PDF_Align>>> SendItems = new List<List<Tuple<string, PDF_Align>>>
            {
                new List<Tuple<string, PDF_Align>>(1)
                {
                    new Tuple<string, PDF_Align>(AgentInfo, PDF_Align.Left),
                },
                new List<Tuple<string, PDF_Align>>(1)
                {
                    new Tuple<string, PDF_Align>(bill.CustomerOrders[0].SendType.Description, PDF_Align.Left),
                },
            };
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

        #region Delivery Note Comment Foot

        private static void InsertBillAndDeliveryNoteCommentFoot(Document doc, BillsView bill)
        {
            StringBuilder cbBillFoodRemarks = new StringBuilder(string.Empty);
            foreach (CustomerOrdersView customerOrder in bill.CustomerOrders.OrderBy(x => x.DeliveryNote_Date))
            {
                if (!String.IsNullOrEmpty(customerOrder.Remarks))
                {
                    cbBillFoodRemarks.AppendLine(customerOrder.Remarks);
                }
            }
            if (!String.IsNullOrEmpty(bill.Remarks))
            {
                cbBillFoodRemarks.AppendLine(bill.Remarks);
            }
            if (!String.IsNullOrEmpty(cbBillFoodRemarks.ToString()))
            {
                List<PdfPCell> CellsDeliveryNoteFoot = new List<PdfPCell>()
                {
                    ReportView.CreateEmptyRow(1),
                    ReportView.CreateRow(cbBillFoodRemarks.ToString(), 9, ForeFontDataTable, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left),
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
        }

        #endregion

        #endregion

        #region Create Receipt Report

        /// <summary>
        /// Create the Report with the information of the Customers selecteds.
        /// </summary>
        /// <param name="bill">Customers selecteds.</param>
        private static bool CreateReceiptReport(BillsView bill, ReceiptsView receipt, int NumReceipt, string Bill_PDF_FileName, 
                                                out string Receipt_FileName, out string ErrMsg, bool DuplicateReceipt = true)
        {
            Document doc = null;
            PdfWriter writer = null;
            try
            {
                Receipt_FileName = string.Format("{0}_Rebut-{1}_{2}.pdf",
                                                 Bill_PDF_FileName.Replace(".pdf", string.Empty),
                                                 NumReceipt,
                                                 GlobalViewModel.GetStringFromDateTimeValue(receipt.Expiration_Date, DateTimeFormat.Report));
                doc = ReportView.CreateDocument(iTextSharp.text.PageSize.A4, PDF_Orientation.Vertical, 15, 15, 15,
                                                ReportView.MinBottomMarginDoc);
                writer = ReportView.GetPDF_PdfWriter(doc, PDF_Report_Types.CustomerOrder, Receipt_FileName);
                doc.AddTitle(string.Format("Rebut Numero {0} de la factura Numero {1}", NumReceipt, bill.Bill_Id));
                doc.AddCreator("Hispania Papers S.L.");
                doc.Open();
                InsertHeaderReceiptInfo(doc, bill, receipt);
                if (DuplicateReceipt)
                {
                    doc.Add(ReportView.DrawLine());
                    doc.Add(ReportView.NewLine());
                    InsertHeaderReceiptInfo(doc, bill, receipt);
                    doc.Add(ReportView.DrawLine());
                }
                doc.Add(ReportView.NewLine());
                doc.Close();
                writer.Close();
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                if ((doc != null) && (doc.IsOpen())) doc.Close();
                if (writer != null) writer.Close();
                Receipt_FileName = string.Empty;
                ErrMsg = string.Format("Error, al construïr el PDF de la factura número {0}.\r\nDetalls: {1}",
                                       bill.Bill_Id, MsgManager.ExcepMsg(ex));
            }
            return (ErrMsg == string.Empty);
        }

        #region Header Receipt Info

        private static void InsertHeaderReceiptInfo(Document doc, BillsView bill, ReceiptsView receipt)
        {
            PdfPCell Company_Cell = ReportView.CreateRow("Papers Hispània, S.L.", 1, ForeFontHispaniaCompany, BaseColor.WHITE, BaseColor.WHITE,
                                                         PDF_Align.Center, PDF_Vert_Align.Center, 0f);
            Company_Cell.Rotation = 90;
            Company_Cell.BorderWidth = 0;
            Company_Cell.FixedHeight = 230f;
            string Address = "Sant Lluc, 31-35 (cant. Indústria)  08918 BADALONA (Barcelona) Telèfon 93 387 50 58" +
                             "Fax 93 388 12 83  E-mail: papershispania@papershispania.com";
            PdfPCell AddressCell = ReportView.CreateRow(Address, 1, ForeFontHispaniaAddress, BaseColor.WHITE, BaseColor.WHITE,
                                                        PDF_Align.Center, PDF_Vert_Align.Center, 0f);
            AddressCell.Rotation = 90;
            AddressCell.BorderWidth = 0;
            AddressCell.FixedHeight = 230f;
            PdfPCell Legend = ReportView.CreateRow("en el domicili de pagament següent: ", 27, ForeFontHeaderDataTable,
                                                   BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left);
            Legend.PaddingTop = 3f;
            Legend.PaddingBottom = 4f;
            PdfPCell AdditionalInfo = ReportView.CreateRow("REGISTRE MERCANTIL DE BARCELONA TOM 22476 FOLI 85, FULL B-37663, INSC 5a - NIF B-08.949.950", 
                                                           27, ForeFontRegisterInfo, BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Left);
            List<PdfPCell> InfoReceipt = new List<PdfPCell>()
            {
                ReportView.CreateNestedTable(CreateFirstLineReceipt(bill, receipt), 27),
                ReportView.CreateNestedTable(CreateSecondLineReceipt(bill, receipt), 27),
                ReportView.CreateNestedTable(CreateThirdLineReceipt(bill, receipt), 27),
                Legend,
                ReportView.CreateNestedTable(CreateFourthLineReceipt(bill, receipt), 27),
                ReportView.CreateNestedTable(CreateFifthLineReceipt(bill, receipt), 27),
                AdditionalInfo
            };
            List<PdfPCell> InfoReceiptCells = new List<PdfPCell>(2)
            {
                ReportView.CreateNestedTable(InfoReceipt, BaseColor.WHITE, BaseColor.WHITE, 27),
            };
            List<PdfPCell> HispHeaderCell = new List<PdfPCell>()
            {
                Company_Cell,
                AddressCell,
                ReportView.CreateNestedTable(ReportView.CreateTable(InfoReceiptCells, 27), 16)
            };
            doc.Add(ReportView.CreateBasicTable(HispHeaderCell, 18, 95));
        }

        #region Create First Line Receipt

        private static PdfPTable CreateFirstLineReceipt(BillsView bill, ReceiptsView receipt)
        {
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(3)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("NUM REBUT", 2, PDF_Align.Left, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("LLOC DE LLIURAMENT", 8, PDF_Align.Left, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("IMPORT", 6, PDF_Align.Left, BaseColor.WHITE) }
            };
            string ReceiptNum = string.Format("{0}/{1}", bill.Bill_Id, bill.DataBank_EffectType.EffectType_Id);
            string PlaceOfDelivery = (bill.CustomerOrders[0].PostalCode is null) ? string.Empty : bill.CustomerOrders[0].PostalCode_Str;
            string ReceiptAmount = GlobalViewModel.GetStringFromDecimalValue(receipt.Amount, DecimalType.Currency, true);
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(3)
            {
                new Tuple<string, PDF_Align>(ReceiptNum, PDF_Align.Left),
                new Tuple<string, PDF_Align>(PlaceOfDelivery, PDF_Align.Center),
                new Tuple<string, PDF_Align>(ReceiptAmount, PDF_Align.Right)
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontReceiptDataTable, ForeFontHeaderDataTable, 98,
                                          null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports);
        }

        #endregion

        #region Create Second Line Receipt

        private static PdfPTable CreateSecondLineReceipt(BillsView bill, ReceiptsView receipt)
        {
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(2)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("DATA DE LLIURAMENT", 8, PDF_Align.Left, BaseColor.WHITE) },
                { new Tuple<string, int, PDF_Align, BaseColor>("VENCIMENT", 8, PDF_Align.Left, BaseColor.WHITE) }
            };
            string DateOfDelivery = GlobalViewModel.GetStringFromDateTimeValue(bill.CustomerOrders[0].DeliveryNote_Date);
            string ReceiptExpirationDate = GlobalViewModel.GetStringFromDateTimeValue(receipt.Expiration_Date);
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(2)
            {
                new Tuple<string, PDF_Align>(DateOfDelivery, PDF_Align.Center),
                new Tuple<string, PDF_Align>(ReceiptExpirationDate, PDF_Align.Center)
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontReceiptDataTable, ForeFontHeaderDataTable, 98,
                                          null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports);
        }

        #endregion

        #region Create Third Line Receipt

        private static PdfPTable CreateThirdLineReceipt(BillsView bill, ReceiptsView receipt)
        {
            string info = string.Format("Per aquest REBUT {0} vostè al venciment expressat la quantitat de", receipt.Amount > 0 ? "pagarà" : "rebrà");
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(1)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>(info, 8, PDF_Align.Left, BaseColor.WHITE) },
            };
            string AmountText = GlobalViewModel.Instance.HispaniaViewModel.ConvertMoneyToString(receipt.Amount);
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(1)
            {
                new Tuple<string, PDF_Align>(AmountText, PDF_Align.Left),
            };
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontReceiptDataTable, ForeFontHeaderDataTable, 98,
                                          BaseColor.WHITE, BaseColor.WHITE, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports);
        }

        #endregion

        #region Create Fourth Line Receipt

        private static PdfPTable CreateFourthLineReceipt(BillsView bill, ReceiptsView receipt)
        {
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(4)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("FORMA DE PAGAMENT", 3, PDF_Align.Center, BaseColor.WHITE) },
            };
            if (bill.DataBank_EffectType.EffectType_Id != 7) // Totes les opcions menys el Comptat (bill.DataBank_EffectType.EffectType_Id == 6)
            {
                Columns.Add(new Tuple<string, int, PDF_Align, BaseColor>("ENTITAT", 3, PDF_Align.Center, BaseColor.WHITE));
                Columns.Add(new Tuple<string, int, PDF_Align, BaseColor>("ADREÇA", 5, PDF_Align.Center, BaseColor.WHITE));
                Columns.Add(new Tuple<string, int, PDF_Align, BaseColor>("NUM COMPTE CORRENT", 5, PDF_Align.Center, BaseColor.WHITE));
            }
            string PayementType = bill.DataBank_EffectType.Description;
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(2)
            {
                new Tuple<string, PDF_Align>(PayementType, PDF_Align.Center),
            };
            if (bill.DataBank_EffectType.EffectType_Id != 7) // Totes les opcions menys el Comptat (bill.DataBank_EffectType.EffectType_Id == 6)
            {
                string DataBank = string.IsNullOrEmpty(bill.DataBank_Bank) ? string.Empty : bill.DataBank_Bank.Trim();
                string DataBankAddress = string.IsNullOrEmpty(bill.DataBank_BankAddress) ? string.Empty : bill.DataBank_BankAddress.Trim();
                string IBANFromBill = bill.GetStringIBANFromBill();
                string CCC = string.IsNullOrEmpty(IBANFromBill) ? string.Empty : IBANFromBill.Trim();
                item.Add(new Tuple<string, PDF_Align>(DataBank, PDF_Align.Left));
                item.Add(new Tuple<string, PDF_Align>(DataBankAddress, PDF_Align.Left));
                item.Add(new Tuple<string, PDF_Align>(CCC, PDF_Align.Left));
            }
            Items.Add(item);
            return ReportView.CreateTable(Columns, Items, ForeFontReceiptDataTable, ForeFontHeaderDataTable, 98,
                                          null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.BillingReports);
        }

        #endregion

        #region Create Fifth Line Receipt

        private static PdfPTable CreateFifthLineReceipt(BillsView bill, ReceiptsView receipt)
        {
            List<Tuple<string, int, PDF_Align, BaseColor>> Columns = new List<Tuple<string, int, PDF_Align, BaseColor>>(1)
            {
                { new Tuple<string, int, PDF_Align, BaseColor>("NOM I DOMICILI DEL LLIURAT", 3, PDF_Align.Left, BaseColor.WHITE) },
            };
            List<List<Tuple<string, PDF_Align>>> Items = new List<List<Tuple<string, PDF_Align>>>(1);
            string CustomerBasicInfo = string.Format("{0} / {1}", bill.Customer.Customer_Id, bill.Customer.Company_Cif);
            string PostalCodeInfo, ProvinceInfo;
            if (bill.Company_PostalCode is null) PostalCodeInfo = ProvinceInfo = string.Empty;
            else
            {
                ProvinceInfo = (bill.Customer.Company_PostalCode.Province is null) ? 
                               string.Empty : 
                               string.Format("({0})", bill.Customer.Company_PostalCode.Province);
                PostalCodeInfo = string.Format("{0}, {1} {2}", bill.Customer.Company_PostalCode_Str, bill.Customer.Company_City_Str, ProvinceInfo);
            }
            List<Tuple<string, PDF_Align>> item = new List<Tuple<string, PDF_Align>>(1)
            {
                new Tuple<string, PDF_Align>(CustomerBasicInfo, PDF_Align.Left),
            };
            Items.Add(item);
            item = new List<Tuple<string, PDF_Align>>(1)
            {
                new Tuple<string, PDF_Align>(bill.Customer.Company_Name, PDF_Align.Left),
            };
            Items.Add(item);
            item = new List<Tuple<string, PDF_Align>>(1)
            {
                new Tuple<string, PDF_Align>(bill.Customer.Company_Address, PDF_Align.Left),
            };
            Items.Add(item);
            item = new List<Tuple<string, PDF_Align>>(1)
            {
                new Tuple<string, PDF_Align>(PostalCodeInfo, PDF_Align.Left),
            };
            Items.Add(item);
            PdfPCell Company_Cell = ReportView.CreateRow("Papers Hispània, S.L.", 4, ForeFontFootHispaniaCompany, BaseColor.WHITE,
                                                         BaseColor.BLACK, PDF_Align.Center, PDF_Vert_Align.Center, 0f);
            Company_Cell.BorderColorRight = BaseColor.WHITE;
            List<PdfPCell> CustomerInfoCell = new List<PdfPCell>()
            {
                ReportView.CreateNestedTable(ReportView.CreateTable(Columns, Items, ForeFontReceiptDataTable, ForeFontHeaderDataTable, 98,
                                                                    null, null, PDF_Align.Center, PDF_RowTable_Padding_Style.Small), 6),
                Company_Cell,
            };
            return ReportView.CreateTable(CustomerInfoCell, 10);
        }

        #endregion
        
        #endregion

        #endregion
    }
}
