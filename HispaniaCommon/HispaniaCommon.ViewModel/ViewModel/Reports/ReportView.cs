#region Libraries used for this class

using iTextSharp.text;
using iTextSharp.text.pdf;
using MBCode.Framework.Managers;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#endregion

namespace HispaniaCommon.ViewModel
{
    public enum PDF_Report_Types
    {
        Agent,
        Customer,
        Provider,
        CustomerOrder,
        ProviderOrder,
        DeliveryNote,
        Bill,
        Revision,
        Listing,
        BadDebt
    }

    public enum PDF_Orientation
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// 0=Left, 1=Centre, 2=Right
    /// </summary>
    public enum PDF_Align 
    {
        Left = 0, 
        Center = 1, 
        Right = 2
    }

    /// <summary>
    /// 0=Top, 1=Centre, 2=Down
    /// </summary>
    public enum PDF_Vert_Align
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    public enum PDF_RowTable_Padding_Style
    {
        Small,

        Normal,

        BillingReports,
    }

    public class ReportView
    {
        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static ReportView instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static ReportView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReportView();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private ReportView()
        {
        }

        #endregion

        #region Constants

        private const float PaddingSmallTop_Cell = 2f;

        private const float PaddingSmallBottom_Cell = 3f;

        private const float PaddingTop_Cell = 3f;

        private const float PaddingBottom_Cell = 4f;

        private const float PaddingTop_TitleCell = PaddingTop_Cell;

        private const float PaddingBottom_TitleCell = PaddingBottom_Cell;

        private const float PaddingTop_DataCell = 5f;

        private const float PaddingBottom_DataCell = 5f;

        #endregion

        #region Attributes

        #region Directories and Files Management

        private static string _GetApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static string _GetAgentReportDirectory = string.Format("{0}\\Informes\\Representats\\", _GetApplicationPath);

        private static string _GetCustomerReportDirectory = string.Format("{0}\\Informes\\Clients\\", _GetApplicationPath);

        private static string _GetListingReportDirectory = string.Format("{0}\\Informes\\Llistats\\", _GetApplicationPath);

        private static string _GetRevisionReportDirectory = string.Format("{0}\\Informes\\Revisions\\", _GetApplicationPath);

        private static string _GetProviderReportDirectory = string.Format("{0}\\Informes\\Proveïdors\\", _GetApplicationPath);

        private static string _GetCustomerOrderReportDirectory = string.Format("{0}\\Informes\\Comandes de Client\\", _GetApplicationPath);

        private static string _GetProviderOrderReportDirectory = string.Format("{0}\\Informes\\Comandes de Proveidor\\", _GetApplicationPath);        

        private static string _GetDeliveryNoteReportDirectory = string.Format("{0}\\Informes\\Albarans\\", _GetApplicationPath);

        private static string _GetBadDebtReportDirectory = string.Format("{0}\\Informes\\Impagats\\", _GetApplicationPath);

        private static string _GetBillReportDirectory = string.Format("{0}\\Informes\\Factures\\", _GetApplicationPath);

        public static int MinBottomMarginDoc = 25;

        #endregion

        #region Shared Attributes

        private static BaseColor ForeColor = CMYKColor.BLACK;

        private static Font ForeFont = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, ForeColor);

        #endregion

        #region Tables

        private static BaseColor BackHeaderTableColor = new BaseColor(247, 217, 173);

        private static BaseColor BorderTableColor = CMYKColor.BLACK;

        private static Font ForeFontTable = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, ForeColor);

        private static Font ForeFontItemTable = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL, ForeColor);

        #endregion

        #endregion

        #region Properties

        private static string GetAgentReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetAgentReportDirectory)) Directory.CreateDirectory(_GetAgentReportDirectory);
                return (_GetAgentReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetAgentReportDirectory = value;
            }
        }

        private static string GetCustomerReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetCustomerReportDirectory)) Directory.CreateDirectory(_GetCustomerReportDirectory);
                return (_GetCustomerReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetCustomerReportDirectory = value;
            }
        }

        private static string GetListingReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetListingReportDirectory)) Directory.CreateDirectory(_GetListingReportDirectory);
                return (_GetListingReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetListingReportDirectory = value;
            }
        }

        private static string GetRevisionReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetRevisionReportDirectory)) Directory.CreateDirectory(_GetRevisionReportDirectory);
                return (_GetRevisionReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetRevisionReportDirectory = value;
            }
        }

        private static string GetProviderReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetProviderReportDirectory)) Directory.CreateDirectory(_GetProviderReportDirectory);
                return (_GetProviderReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetProviderReportDirectory = value;
            }
        }

        private static string GetBadDebtReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetBadDebtReportDirectory)) Directory.CreateDirectory(_GetBadDebtReportDirectory);
                return (_GetBadDebtReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetBadDebtReportDirectory = value;
            }
        }

        private static string GetCustomerOrderReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetCustomerOrderReportDirectory)) Directory.CreateDirectory(_GetCustomerOrderReportDirectory);
                return (_GetCustomerOrderReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetCustomerOrderReportDirectory = value;
            }
        }
                
        private static string GetProviderOrderReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetProviderOrderReportDirectory)) Directory.CreateDirectory(_GetProviderOrderReportDirectory);
                return (_GetProviderOrderReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetProviderOrderReportDirectory = value;
            }
        }

        private static string GetDeliveryNoteReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetDeliveryNoteReportDirectory)) Directory.CreateDirectory(_GetDeliveryNoteReportDirectory);
                return (_GetDeliveryNoteReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetDeliveryNoteReportDirectory = value;
            }
        }

        private static string GetBillReportDirectory
        {
            get
            {
                if (!Directory.Exists(_GetBillReportDirectory)) Directory.CreateDirectory(_GetBillReportDirectory);
                return (_GetBillReportDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetBillReportDirectory = value;
            }
        }

        #endregion

        #region PDF Create File Methods

        public static Document CreateDocument(Rectangle PageSize, PDF_Orientation Orientation,
                                              float Margin_Left, float Margin_Right, float Margin_Top, 
                                              float Margin_Bottom)
        {
            if (Margin_Bottom < MinBottomMarginDoc) Margin_Bottom = MinBottomMarginDoc;
            Document doc = new Document(PageSize, Margin_Left, Margin_Right, Margin_Top, Margin_Bottom);
            if (Orientation == PDF_Orientation.Horizontal) doc.SetPageSize(PageSize.Rotate());
            return doc;
        }

        public static PdfWriter GetPDF_PdfWriter(Document doc, PDF_Report_Types reportType, string pdfFileName)
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.Create));
            return writer;
        }

        public static PdfWriter GetPDF_PdfWriter(Document doc, PDF_Report_Types reportType, 
                                                 string pdfReportFileName, out string pdfFileName)
        {
            return (GetPDF_PdfWriter(doc, reportType, pdfFileName = GetPDF_FileName(reportType, pdfReportFileName)));
        }

        public static string GetPDF_FileName(PDF_Report_Types reportType, string pdfFileName, decimal? Year = null)
        {
            switch (reportType)
            {
                case PDF_Report_Types.Agent:
                     return string.Format("{0}{1}.pdf", GetAgentReportDirectory, pdfFileName);
                case PDF_Report_Types.Customer:
                     return string.Format("{0}{1}.pdf", GetCustomerReportDirectory, pdfFileName);
                case PDF_Report_Types.Listing:
                     return string.Format("{0}{1}.pdf", GetListingReportDirectory, pdfFileName);
                case PDF_Report_Types.Revision:
                     return string.Format("{0}{1}.pdf", GetRevisionReportDirectory, pdfFileName);
                case PDF_Report_Types.Provider:
                     return string.Format("{0}{1}.pdf", GetProviderReportDirectory, pdfFileName);
                case PDF_Report_Types.BadDebt:
                     return string.Format("{0}{1}.pdf", GetBadDebtReportDirectory, pdfFileName);
                case PDF_Report_Types.CustomerOrder:
                     string BaseCustomerOrderReportDirectory = string.Format("{0}{1}\\", GetCustomerOrderReportDirectory, Year);
                     if (!Directory.Exists(BaseCustomerOrderReportDirectory)) Directory.CreateDirectory(BaseCustomerOrderReportDirectory);
                     return string.Format("{0}{1}.pdf", BaseCustomerOrderReportDirectory, pdfFileName);
                case PDF_Report_Types.ProviderOrder:
                    string BaseProviderOrderReportDirectory = string.Format("{0}{1}\\", GetProviderOrderReportDirectory, Year);
                    if (!Directory.Exists(BaseProviderOrderReportDirectory)) Directory.CreateDirectory(BaseProviderOrderReportDirectory);
                    return string.Format("{0}{1}.pdf", BaseProviderOrderReportDirectory, pdfFileName);
                case PDF_Report_Types.DeliveryNote:
                     string BaseDeliveryNoteReportDirectory = string.Format("{0}{1}\\", GetDeliveryNoteReportDirectory, Year);
                     if (!Directory.Exists(BaseDeliveryNoteReportDirectory)) Directory.CreateDirectory(BaseDeliveryNoteReportDirectory);
                     return string.Format("{0}{1}.pdf", BaseDeliveryNoteReportDirectory, pdfFileName);
                case PDF_Report_Types.Bill:
                     string BaseBillReportDirectory = string.Format("{0}{1}\\", GetBillReportDirectory, Year);
                     if (!Directory.Exists(BaseBillReportDirectory)) Directory.CreateDirectory(BaseBillReportDirectory);
                     return string.Format("{0}{1}.pdf", BaseBillReportDirectory, pdfFileName);
                default:
                     return string.Format("{0}.pdf", pdfFileName);
            }
        }

        #endregion

        #region  Exist File Report

        public static bool ExistPDF_FileName(PDF_Report_Types report_Type, string pdfFileName, out string[] OldPdfReports, decimal? Year = null)
        {
            OldPdfReports = new string[0];
            string BaseErrMsg = "Error, tipus de informe '{0}' incorrecte per aquesta la operació no existeix el tipus.";
            string FileNamePattern = string.Format("{0}*.*", pdfFileName);
            switch (report_Type)
            {
                case PDF_Report_Types.Agent:
                     throw new ArgumentException(string.Format(BaseErrMsg, report_Type));
                case PDF_Report_Types.Customer:
                     throw new ArgumentException(string.Format(BaseErrMsg, report_Type));
                case PDF_Report_Types.Provider:
                     throw new ArgumentException(string.Format(BaseErrMsg, report_Type));
                case PDF_Report_Types.BadDebt:
                     throw new ArgumentException(string.Format(BaseErrMsg, report_Type));
                case PDF_Report_Types.CustomerOrder:
                     string BaseCustomerOrderReportDirectory = string.Format("{0}{1}\\", GetCustomerOrderReportDirectory, Year);
                     if (Directory.Exists(BaseCustomerOrderReportDirectory))
                     {
                        OldPdfReports = Directory.GetFiles(BaseCustomerOrderReportDirectory, FileNamePattern);
                        return OldPdfReports.Length > 0;
                     }
                     else return false;
                case PDF_Report_Types.ProviderOrder:
                    string BaseProviderOrderReportDirectory = string.Format("{0}{1}\\", GetProviderOrderReportDirectory, Year);
                    if (Directory.Exists(BaseProviderOrderReportDirectory))
                    {
                        OldPdfReports = Directory.GetFiles(BaseProviderOrderReportDirectory, FileNamePattern);
                        return OldPdfReports.Length > 0;
                    }
                    else return false;
                case PDF_Report_Types.DeliveryNote:
                     string BaseDeliveryNoteReportDirectory = string.Format("{0}{1}\\", GetDeliveryNoteReportDirectory, Year);
                     if (Directory.Exists(BaseDeliveryNoteReportDirectory))
                     {
                        OldPdfReports = Directory.GetFiles(BaseDeliveryNoteReportDirectory, FileNamePattern);
                        return OldPdfReports.Length > 0;
                     }
                     else return false;
                case PDF_Report_Types.Bill:
                     string BaseBillReportDirectory = string.Format("{0}{1}\\", GetBillReportDirectory, Year);
                     if (Directory.Exists(BaseBillReportDirectory))
                     {
                        OldPdfReports = Directory.GetFiles(BaseBillReportDirectory, FileNamePattern);
                        return OldPdfReports.Length > 0;
                     }
                     else return false;
                default:
                     throw new ArgumentException(string.Format(BaseErrMsg, report_Type));
            }
        }

        #endregion 

        #region  Delete Report Files

        public static void DeleteReportFiles(string[] OldPdfReports)
        {
            foreach (string PathOldPdfReport in OldPdfReports)
            {
                File.Delete(PathOldPdfReport);
            }
        }

        #endregion 

        #region Add Page Number

        private static Font ForeFontPageNumber = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, ForeColor);

        public static void AddPageNumber(string fileName, PDF_Orientation Orientation)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(bytes);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    float position;
                    if (Orientation == PDF_Orientation.Horizontal) position = reader.GetPageSizeWithRotation(1).Width / 2;
                    else position = reader.GetPageSize(1).Width / 2;
                    for (int i = 1; i <= pages; i++)
                    {
                        string PageNumber = string.Format("Plana {0} de {1}", i, reader.NumberOfPages);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER,
                                                   new Phrase(PageNumber, ForeFontPageNumber), position, 15f, 0);
                    }
                }
                bytes = stream.ToArray();
            }
            File.WriteAllBytes(fileName, bytes);
        }

        #endregion

        #region Tables

        public static PdfPTable CreateTable(List<Tuple<string, int, PDF_Align>> Columns,
                                            List<List<Tuple<string, PDF_Align>>> Items = null,
                                            Font ForeFontTableIn = null,
                                            Font ForeFontHeaderTableIn = null,
                                            float WidthDocumentTablePercentage = 95,
                                            BaseColor BackHeaderColorTable = null,
                                            BaseColor BorderColorTable = null,
                                            PDF_Align HorizontalTableAlignement = PDF_Align.Center,
                                            PDF_RowTable_Padding_Style PaddingRowStyle = PDF_RowTable_Padding_Style.Normal)
        {
            List<Tuple<string, int, PDF_Align, BaseColor>> ColoredColumns = null;
            if ((Columns != null) && (Columns.Count > 0))
            {
                ColoredColumns = new List<Tuple<string, int, PDF_Align, BaseColor>>(Columns.Count);
                foreach (Tuple<string, int, PDF_Align> column in Columns)
                {
                    ColoredColumns.Add(new Tuple<string, int, PDF_Align, BaseColor>(column.Item1, column.Item2, column.Item3, BaseColor.WHITE));
                }
            }
            return CreateTable(ColoredColumns, Items, ForeFontTableIn, ForeFontHeaderTableIn, WidthDocumentTablePercentage, BackHeaderColorTable,
                               BorderColorTable, HorizontalTableAlignement, PaddingRowStyle);
        }
        
        public static PdfPTable CreateTable(List<Tuple<string, int, PDF_Align, BaseColor>> Columns,
                                            List<List<Tuple<string, PDF_Align>>> Items = null,
                                            Font ForeFontTableIn = null,
                                            Font ForeFontHeaderTableIn = null,
                                            float WidthDocumentTablePercentage = 95,
                                            BaseColor BackHeaderColorTable = null,
                                            BaseColor BorderColorTable = null,
                                            PDF_Align HorizontalTableAlignement = PDF_Align.Center,
                                            PDF_RowTable_Padding_Style PaddingRowStyle = PDF_RowTable_Padding_Style.Normal)
        {
            if ((Columns == null) || (Columns.Count == 0))
            {
                throw new ArgumentNullException("Error, al crear la taula al pdf.\r\nDetalls: manquen les columnes.");
            }
            int colSpan = 0, indexColumn = 0;
            int[] RowColSpan = new int[Columns.Count];
            BaseColor[] ColumnBackColor = new BaseColor[Columns.Count];
            foreach (Tuple<string, int, PDF_Align, BaseColor> itemValue in Columns)
            {
                colSpan += itemValue.Item2;
                RowColSpan[indexColumn] = Columns[indexColumn].Item2;
                ColumnBackColor[indexColumn] = Columns[indexColumn].Item4 ?? BaseColor.WHITE;
                indexColumn++;
            }
            float PaddingTop_Title, PaddingBottom_Title, PaddingTop_Data, PaddingBottom_Data;
            switch (PaddingRowStyle)
            {
                case PDF_RowTable_Padding_Style.BillingReports:
                     PaddingTop_Title = PaddingTop_TitleCell;
                     PaddingBottom_Title = PaddingBottom_TitleCell;
                     PaddingTop_Data = PaddingTop_DataCell;
                     PaddingBottom_Data = PaddingBottom_DataCell;
                     break;
                case PDF_RowTable_Padding_Style.Small:
                     PaddingTop_Title = PaddingSmallTop_Cell;
                     PaddingBottom_Title = PaddingSmallBottom_Cell;
                     PaddingTop_Data = PaddingSmallTop_Cell;
                     PaddingBottom_Data = PaddingSmallBottom_Cell;
                     break;
                case PDF_RowTable_Padding_Style.Normal:
                default:
                     PaddingTop_Title = PaddingTop_Cell;
                     PaddingBottom_Title = PaddingBottom_Cell;
                     PaddingTop_Data = PaddingTop_Cell;
                     PaddingBottom_Data = PaddingBottom_Cell;
                    break;
            }
            List<PdfPCell> Cells = new List<PdfPCell>(Columns.Count);
            foreach (Tuple<string, int, PDF_Align, BaseColor> column in Columns)
            {
                PdfPCell cell = CreateRow(column.Item1, column.Item2, ForeFontHeaderTableIn ?? ForeFontTable,
                                          BackHeaderColorTable ?? BackHeaderTableColor,
                                          BorderColorTable ?? BorderTableColor, column.Item3, PDF_Vert_Align.Center);
                cell.PaddingTop = PaddingTop_Title;
                cell.PaddingBottom = PaddingBottom_Title;
                Cells.Add(cell);
            }
            PdfPTable Table = CreateTable(Cells, colSpan, WidthDocumentTablePercentage, HorizontalTableAlignement);
            if (Items != null)
            {
                foreach (List<Tuple<string, PDF_Align>> item in Items)
                {
                    for (indexColumn = 0; indexColumn < Columns.Count; indexColumn++)
                    {
                        PdfPCell cell = CreateRow(item[indexColumn].Item1, RowColSpan[indexColumn],
                                                  ForeFontTableIn ?? ForeFontItemTable, ColumnBackColor[indexColumn],
                                                  BorderTableColor, item[indexColumn].Item2, PDF_Vert_Align.Center,
                                                  0.10f);
                        cell.PaddingTop = PaddingTop_Data;
                        cell.PaddingBottom = PaddingBottom_Data;
                        Table.AddCell(cell);
                    }
                }
            }
            return (Table);
        }

        public static PdfPTable CreateTable(List<PdfPCell> Cells = null, int ColSpanTable = 1,
                                            float WidthDocumentTablePercentage = 95,
                                            PDF_Align HorizontalTableAlignement = PDF_Align.Center)
        {
            PdfPTable NewTable = new PdfPTable(ColSpanTable)
            {
                WidthPercentage = WidthDocumentTablePercentage,
                HorizontalAlignment = (int)HorizontalTableAlignement,
            };
            if (Cells != null)
            {
                foreach (PdfPCell cell in Cells)
                {
                    cell.VerticalAlignment = (int)PDF_Vert_Align.Center;
                    NewTable.AddCell(cell);
                }
            }
            return NewTable;
        }

        public static PdfPTable CreateBasicTable(List<PdfPCell> Cells = null, int ColSpanTable = 1,
                                                 float WidthDocumentTablePercentage = 95)
        {
            PdfPTable NewTable = new PdfPTable(ColSpanTable)
            {
                WidthPercentage = WidthDocumentTablePercentage,
            };
            if (Cells != null)
            {
                foreach (PdfPCell cell in Cells)
                {
                    NewTable.AddCell(cell);
                }
            }
            return NewTable;
        }

        #endregion

        #region New Line in Document

        internal static IElement NewLine()
        {
            return new Paragraph(Environment.NewLine);
        }

        #endregion

        #region Draw Line in Document

        internal static IElement DrawLine()
        {
            return new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
        }

        #endregion

        #region New Page in Document

        internal static void NewPage(Document doc)
        {
            doc.NewPage();
        }

        #endregion

        #region Print Document

        public static bool PrintReport(string PDF_FileName, string FileDescription, out string InfoPrintExecution)
        {
            try
            {
                Process.Start(PDF_FileName);
                InfoPrintExecution = string.Format("Document asociat a '{0}' presentat correctament per la seva impressió.", FileDescription);
                return (true);
            }
            catch (Exception ex)
            {
                InfoPrintExecution = string.Format("Error, al presentar el document associat a '{0}' a imprimir.\r\nDetalls: {1}\r\n",
                                                   FileDescription, MsgManager.ExcepMsg(ex));
                return (false);
            }
        }

        #endregion

        #region Send EMail

        internal static bool SendReport(string EMail_Address, string Subject, string BodyMessgage, 
                                        List<Tuple<string, string>> FileAttachments, out string ErrMsg)
        {
            return (OutlookManager.SendEmail(EMail_Address, Subject, BodyMessgage, FileAttachments, out ErrMsg) == 0);
        }

        #endregion

        #region Supplier Methods

        internal static PdfPCell CreateRow(string RowText, int ColSpan = 10, Font RowForeFont = null, 
                                           BaseColor BackgroundColor = null, BaseColor BorderColor = null,
                                           PDF_Align HorizontalAlignment = PDF_Align.Center,
                                           PDF_Vert_Align VerticalAlignment = PDF_Vert_Align.Center,
                                           float BorderWidthBottom = 0.75f)
        {
            return CreateRowInternal(RowText, RowForeFont ?? ForeFont, BackgroundColor ?? BaseColor.WHITE, 
                                     BorderColor ?? BaseColor.WHITE, ColSpan, HorizontalAlignment, 
                                     VerticalAlignment, BorderWidthBottom);
        }

        internal static PdfPCell CreateEmptyRow()
        {
            Font RowForeFontEmpty = new Font(Font.FontFamily.TIMES_ROMAN, 5, Font.NORMAL, BaseColor.WHITE);
            return CreateRowInternal(Environment.NewLine, RowForeFontEmpty, BaseColor.WHITE, BaseColor.WHITE, 10);
        }

        internal static PdfPCell CreateEmptyRow(int ColSpan = 10)
        {
            Font RowForeFontEmpty = new Font(Font.FontFamily.TIMES_ROMAN, 5, Font.NORMAL, BaseColor.WHITE);
            return CreateRowInternal(Environment.NewLine, RowForeFontEmpty, BaseColor.WHITE, BaseColor.WHITE, ColSpan);
        }

        internal static PdfPCell CreateEmptyRow(BaseColor BackgroundColor, BaseColor BorderColor, int ColSpan = 10)
        {
            Font RowForeFontEmpty = new Font(Font.FontFamily.TIMES_ROMAN, 5, Font.NORMAL, BaseColor.WHITE);
            return CreateRowInternal(Environment.NewLine, RowForeFontEmpty, BackgroundColor, BorderColor, ColSpan);
        }

        internal static PdfPCell CreateRowInternal(string RowText, Font RowForeFont, BaseColor BackgroundColor, 
                                                   BaseColor BorderColor, int ColSpan = 10, 
                                                   PDF_Align HorizontalAlignment = PDF_Align.Center, 
                                                   PDF_Vert_Align VerticalAlignment = PDF_Vert_Align.Center,
                                                   float BorderWidthBottom = 0.75f)
        {
            return (new PdfPCell(new Phrase(RowText, RowForeFont))
                    {
                        Colspan = ColSpan,
                        BackgroundColor = BackgroundColor,
                        BorderColor = BorderColor,
                        BorderWidthBottom = BorderWidthBottom,
                        HorizontalAlignment = (int) HorizontalAlignment,
                        VerticalAlignment = (int) VerticalAlignment
            });
        }

        internal static PdfPCell CreateNestedTable(List<PdfPCell> Cells, BaseColor BackgroundColor,
                                                   BaseColor BorderColor, int ColSpan = 10, int ColSpanTable = 1,
                                                   PDF_Align HorizontalAlignment = PDF_Align.Center,
                                                   PDF_Vert_Align VerticalAlignment = PDF_Vert_Align.Center,
                                                   float BorderWidthBottom = 0.75f)
        {
            return (CreateNestedTable(CreateTable(Cells, ColSpanTable), ColSpan, BackgroundColor, BorderColor,
                                      HorizontalAlignment, VerticalAlignment, BorderWidthBottom));
        }

        internal static PdfPCell CreateNestedTable(PdfPTable Table, int ColSpan = 10, 
                                                   BaseColor BackgroundColor = null, BaseColor BorderColor = null, 
                                                   PDF_Align HorizontalAlignment = PDF_Align.Center,
                                                   PDF_Vert_Align VerticalAlignment = PDF_Vert_Align.Center,
                                                   float BorderWidthBottom = 0.75f)
        {
            return (new PdfPCell(Table)
            {
                Colspan = ColSpan,
                BackgroundColor = BackgroundColor ?? BaseColor.WHITE,
                BorderColor = BorderColor ?? BaseColor.WHITE,
                BorderWidthBottom = BorderWidthBottom,
                HorizontalAlignment = (int)HorizontalAlignment,
                VerticalAlignment = (int)VerticalAlignment
            });
        }

        #endregion
    }
}
