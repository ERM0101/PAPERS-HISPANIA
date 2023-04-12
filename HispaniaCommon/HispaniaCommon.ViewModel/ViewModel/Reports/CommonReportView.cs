#region Librerias usadas por la clase

using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace HispaniaCommon.ViewModel
{
    public enum CustomerInfoType
    {
        CustomerOrderProforma,
        CustomerOrderComanda,
        DeliveryNote,
        
    }

    public class CommonReportView
    {
        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static CommonReportView instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static CommonReportView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommonReportView();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private CommonReportView()
        {
        }

        #endregion

        #region Attributes

        #region Shared Attributes

        private static BaseColor ForeColor = CMYKColor.BLACK;

        private static Font ForeFontTable = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, CMYKColor.BLACK);

        #endregion

        #region Document Title

        private static Font ForeColorTitle = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.WHITE);

        private static Font ForeColorTitleData = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, BaseColor.WHITE);

        private static Font ForeColorValueData = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL, BaseColor.BLACK);

        private static BaseColor BackColorTitleData = new BaseColor(130, 88, 31);

        private static BaseColor BackColorValueData = new BaseColor(255, 242, 157);

        #endregion

        #region Hispania Header

        private static BaseColor ForeColorHispaniaCompany = new BaseColor(181, 133, 63);

        private static string AmericanTypewriterFileFontPath = string.Format("{0}\\ReportsFont\\AmericanTypewriterRegular.ttf",
                                                                System.IO.Path.GetDirectoryName(Assembly.GetAssembly(typeof(CommonReportView)).Location));

        private static BaseFont AmericanTypewriter = BaseFont.CreateFont(AmericanTypewriterFileFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        private static Font ForeFontHispaniaCompany = new Font(AmericanTypewriter, 48, Font.BOLD, ForeColorHispaniaCompany);

        private static BaseColor ForeColorAddress = CMYKColor.BLACK;

        //private static Font ForeFontHispaniaAddress = new Font(AmericanTypewriter, 10f, Font.NORMAL, ForeColorAddress);
        private static Font ForeFontHispaniaAddress = new Font(Font.FontFamily.TIMES_ROMAN, 10f, Font.NORMAL, ForeColorAddress);

        private static Font ForeFontHispaniaCommercialRegister = new Font(Font.FontFamily.TIMES_ROMAN, 6f, Font.NORMAL, ForeColorAddress);

        #endregion

        #region Customer Info 

        private static BaseColor ForeColorCustomerInfo = BaseColor.BLACK;

        private static Font ForeFontCustomerOrder = new Font(Font.FontFamily.TIMES_ROMAN, 17, Font.BOLD, ForeColorCustomerInfo);

        private static Font ForeFontDeliveryNote = new Font(Font.FontFamily.TIMES_ROMAN, 24, Font.BOLD, ForeColorCustomerInfo);

        private static Font ForeFontCustomerInfo = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, ForeColorCustomerInfo);

        private static BaseColor BackColorCustomerInfo = new BaseColor(242, 213, 176);

        #endregion

        #endregion

        #region Document Title 

        public static void InsertTitle(Document doc, string Title, List<Tuple<string, int, string>> Details, 
                                       float WidthDocumentTablePercentage = 75,
                                       PDF_Align HorizontalTableAlignement = PDF_Align.Center)
        {
            int ColSpanTable = (Details.Count == 2) ? 12 : 10;
            List<PdfPCell> CellsTitle = new List<PdfPCell>(3);
            CellsTitle.Add(ReportView.CreateRow(Title, ColSpanTable, ForeColorTitle, BackColorTitleData, BackColorTitleData));
            CellsTitle.Add(ReportView.CreateEmptyRow(BackColorTitleData, BackColorTitleData, ColSpanTable));
            if (Details.Count == 2) CellsTitle.Add(ReportView.CreateEmptyRow(BackColorTitleData, BackColorTitleData, 1));
            int ColSpanDetails_1 = (Details.Count == 1) ? Details[0].Item2 : 1;
            int ColSpanDetails_2 = (Details.Count == 1) ? 5 - Details[0].Item2 : 4 - Details[0].Item2;
            CellsTitle.Add(ReportView.CreateRow(Details[0].Item1, ColSpanDetails_1, ForeColorTitleData, 
                                                BackColorTitleData, BackColorTitleData));
            CellsTitle.Add(ReportView.CreateRow(Details[0].Item3, ColSpanDetails_2, ForeColorValueData, 
                                                BackColorValueData, BackColorValueData));
            if (Details.Count == 2)
            {
                CellsTitle.Add(ReportView.CreateRow(Details[1].Item1, 1, ForeColorTitleData, BackColorTitleData,
                                                    BackColorTitleData));
                CellsTitle.Add(ReportView.CreateRow(Details[1].Item3, 2 - Details[1].Item2, ForeColorValueData, 
                                                    BackColorValueData, BackColorValueData));
            }
            CellsTitle.Add(ReportView.CreateRow("Data de creació : ", 2, ForeColorTitleData, BackColorTitleData,
                                                BackColorTitleData));
            CellsTitle.Add(ReportView.CreateRow(DateTime.Now.ToString(), 2, ForeColorValueData, BackColorValueData,
                                                BackColorValueData));
            CellsTitle.Add(ReportView.CreateEmptyRow(BackColorTitleData, BackColorTitleData, 1));
            doc.Add(ReportView.CreateTable(CellsTitle, ColSpanTable));
            doc.Add(ReportView.NewLine());
        }

        #endregion

        #region Hispania Header

        public static void InsertHispaniaHeader(Document doc)
        {
            string Address = "Sant Lluc, 31-35 (cant. Indústria)  08918 BADALONA (Barcelona)\r\n" +
                             "Telèfon 93 387 50 58  Fax 93 388 12 83  E-mail: papershispania@papershispania.com";
            string CommercialRegister = "Registre mercantil de Barcelona Tom 22476 Foli 85, Full B-37663, INSC 5a - NIF B-08.949.950";
            List<PdfPCell> CellsHispHeader = new List<PdfPCell>(3)
            {
                ReportView.CreateRow("Papers Hispània, S.L.", 10, ForeFontHispaniaCompany),
                ReportView.CreateEmptyRow(),
                ReportView.CreateRow(Address, 10, ForeFontHispaniaAddress),
                ReportView.CreateRow(CommercialRegister, 10, ForeFontHispaniaCommercialRegister)
            };
            doc.Add(ReportView.CreateTable(CellsHispHeader, 10));
            doc.Add(new Paragraph(Environment.NewLine));
        }

        #endregion

        #region Customer Info

        public static void InsertCustomerInfo(Document doc, CustomersView Customer, bool IsBill = false, 
                                              CustomerInfoType Type = CustomerInfoType.CustomerOrderProforma)
        {
            List<PdfPCell> CellsDeliveryNote;
            List<Tuple<string, int, PDF_Align>> Columns = null;
            List<List<Tuple<string, PDF_Align>>> Items = null;
            bool NumProvExist = !String.IsNullOrEmpty(Customer.Company_NumProv);
            if (NumProvExist)
            {
                string NumProvInfo = 
                          string.Format("{0}{1}", 
                                        Customer.Company_NumProv,
                                        Customer.Customer_Id == 233 || Customer.Customer_Id == 3893 ? " (ESB08949950)" 
                                                                                                    : string.Empty);
                Columns = new List<Tuple<string, int, PDF_Align>>(1)
                {
                    { new Tuple<string, int, PDF_Align>("NÚMERO PROVEÏDOR", 14, PDF_Align.Center) }
                };
                Items = new List<List<Tuple<string, PDF_Align>>>(1)
                {
                    new List<Tuple<string, PDF_Align>>(1)
                    {
                        new Tuple<string, PDF_Align>(NumProvInfo, PDF_Align.Center),
                    }
                };
            }
            string AdditionalText = IsBill ? "FACTURA" : (Type == CustomerInfoType.CustomerOrderProforma ? "PRO FORMA" : (Type == CustomerInfoType.CustomerOrderComanda ? "COMANDA" : "ALBARÀ"));
            CellsDeliveryNote = NumProvExist ? new List<PdfPCell>(8)
                                               {
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateRow(AdditionalText, 14, ForeFontDeliveryNote),
                                                    ReportView.CreateEmptyRow(3),
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateNestedTable(ReportView.CreateTable(Columns, Items, ForeFontTable), 14),
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateEmptyRow(4),
                                               } :
                                               new List<PdfPCell>(8)
                                               {
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(2),
                                                    ReportView.CreateRow(AdditionalText, 5, ForeFontCustomerOrder),
                                                    ReportView.CreateEmptyRow(1),
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(6),
                                               };
            string TimeTable = IsBill ? string.Empty : Customer.Company_TimeTable;
            List<PdfPCell> CellsCustomerInfo = new List<PdfPCell>()
            {
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Customer.Customer_Id.ToString(), 2, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateRow("D.N.I./C.I.F.: ", 3, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo),
                ReportView.CreateRow(Customer.Company_Cif, 4, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Customer.Company_Name, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Customer.Company_Address, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Customer.Company_City_Str, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Customer.Company_PostalCode_Str, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(TimeTable, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left), 
            };
            List<PdfPCell> Cells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(CellsCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, 8, 10),
                ReportView.CreateEmptyRow(1),
            };
            List<PdfPCell> CellsCustInfo = new List<PdfPCell>(4)
            {
                ReportView.CreateNestedTable(CellsDeliveryNote, BaseColor.WHITE, BaseColor.WHITE, 3, 6),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(ReportView.CreateTable(Cells, 10), 6),
            };
            doc.Add(ReportView.CreateTable(CellsCustInfo, 10));
            doc.Add(new Paragraph(Environment.NewLine));
        }

        #endregion

        #region Provider Info

        public static void InsertProviderInfo(Document doc, ProvidersView Provider, bool IsBill = false,
                                              CustomerInfoType Type = CustomerInfoType.CustomerOrderProforma)
        {
            List<PdfPCell> CellsDeliveryNote;
            List<Tuple<string, int, PDF_Align>> Columns = null;
            List<List<Tuple<string, PDF_Align>>> Items = null;
            bool NumProvExist = !String.IsNullOrEmpty(Provider.Company_NumProv);
            if (NumProvExist)
            {
                string NumProvInfo =
                          string.Format("{0}{1}",
                                        Provider.Company_NumProv,
                                        Provider.Provider_Id == 233 || Provider.Provider_Id == 3893 ? " (ESB08949950)"
                                                                                                    : string.Empty);
                Columns = new List<Tuple<string, int, PDF_Align>>(1)
                {
                    { new Tuple<string, int, PDF_Align>("NÚMERO PROVEÏDOR", 14, PDF_Align.Center) }
                };
                Items = new List<List<Tuple<string, PDF_Align>>>(1)
                {
                    new List<Tuple<string, PDF_Align>>(1)
                    {
                        new Tuple<string, PDF_Align>(NumProvInfo, PDF_Align.Center),
                    }
                };
            }
            string AdditionalText = IsBill ? "FACTURA" : (Type == CustomerInfoType.CustomerOrderProforma ? "PRO FORMA" : (Type == CustomerInfoType.CustomerOrderComanda ? "COMANDA" : "ALBARÀ"));
            CellsDeliveryNote = NumProvExist ? new List<PdfPCell>(8)
                                               {
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateRow(AdditionalText, 14, ForeFontDeliveryNote),
                                                    ReportView.CreateEmptyRow(3),
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateNestedTable(ReportView.CreateTable(Columns, Items, ForeFontTable), 14),
                                                    ReportView.CreateEmptyRow(4),
                                                    ReportView.CreateEmptyRow(4),
                                               } :
                                               new List<PdfPCell>(8)
                                               {
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(2),
                                                    ReportView.CreateRow(AdditionalText, 5, ForeFontCustomerOrder),
                                                    ReportView.CreateEmptyRow(1),
                                                    ReportView.CreateEmptyRow(6),
                                                    ReportView.CreateEmptyRow(6),
                                               };
            string TimeTable = IsBill ? string.Empty : Provider.Company_TimeTable;
            List<PdfPCell> CellsCustomerInfo = new List<PdfPCell>()
            {
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Provider.Provider_Id.ToString(), 2, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateRow("D.N.I./C.I.F.: ", 3, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo),
                ReportView.CreateRow(Provider.Company_Cif, 4, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Provider.Company_Name, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Provider.Company_Address, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Provider.Company_City_Str, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(Provider.Company_PostalCode_Str, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
                ReportView.CreateEmptyRow(BackColorCustomerInfo, BackColorCustomerInfo, 1),
                ReportView.CreateRow(TimeTable, 9, ForeFontCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, PDF_Align.Left),
            };
            List<PdfPCell> Cells = new List<PdfPCell>(3)
            {
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(CellsCustomerInfo, BackColorCustomerInfo, BackColorCustomerInfo, 8, 10),
                ReportView.CreateEmptyRow(1),
            };
            List<PdfPCell> CellsCustInfo = new List<PdfPCell>(4)
            {
                ReportView.CreateNestedTable(CellsDeliveryNote, BaseColor.WHITE, BaseColor.WHITE, 3, 6),
                ReportView.CreateEmptyRow(1),
                ReportView.CreateNestedTable(ReportView.CreateTable(Cells, 10), 6),
            };
            doc.Add(ReportView.CreateTable(CellsCustInfo, 10));
            doc.Add(new Paragraph(Environment.NewLine));
        }

        #endregion
    }
}
