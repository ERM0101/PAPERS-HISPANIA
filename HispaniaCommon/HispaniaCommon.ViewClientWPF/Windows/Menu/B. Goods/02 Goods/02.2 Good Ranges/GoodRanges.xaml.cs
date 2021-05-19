#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for GoodRanges.xaml
    /// </summary>
    public partial class GoodRanges : Window
    {
        #region Constants

        /// <summary>
        /// Defines the theme for the main Hispania Application.
        /// </summary>
        private ThemeInfo HispaniaApp = new ThemeInfo("HispaniaCommon.ViewClientWPF", "component/Recursos/Themes/", "HispaniaComptabilitat");

        /// <summary>
        /// Defines the theme for the support Hispania Application.
        /// </summary>
        private ThemeInfo HispaniaSupportApp = new ThemeInfo("HispaniaCommon.ViewClientWPF", "component/Recursos/Themes/", "HispaniaHelp");

        /// <summary>
        /// Show the button.
        /// </summary>
        private GridLength ViewButton = new GridLength(120.0);

        /// <summary>
        /// Show the button.
        /// </summary>
        private GridLength ViewExtendedColumn = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Hide the button.
        /// </summary>
        private GridLength HideColumn = new GridLength(0.0);

        /// <summary>
        /// Store normal color.
        /// </summary>
        private Brush brNormalForeColor = new SolidColorBrush(Color.FromArgb(255, 98, 103, 106));

        /// <summary>
        /// Store normal color.
        /// </summary>
        private Brush brNormalBackColor = new SolidColorBrush(Color.FromArgb(255, 198, 203, 206));

        /// <summary>
        /// Store editable control fore color.
        /// </summary>
        private Brush brEditableCtrlForeColor = new SolidColorBrush(Colors.White);

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Accept is pressed and the data of the
        /// Good is correct.
        /// </summary>
        /// <param name="NewOrEditedGood">New or Edited Good.</param>
        public delegate void dlgAcceptGoodRangeChanged(GoodsView EditedRangesGood);

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Good is correct.
        /// </summary>
        public event dlgAcceptGoodRangeChanged EvAcceptGoodRangeChanged;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private GoodsView m_Data= null;

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Stotre if the data of the Customer has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store the Operation of the UserControl.
        /// </summary>
        private Operation m_CtrlOperation = Operation.Show;
        
        #region GUI

        /// <summary>
        /// Store the background color for the search text box.
        /// </summary>
        private Brush m_AppColor = null;

        /// <summary>
        /// Store the list of Editable Controls.
        /// </summary>
        private List<Control> EditableControls = null;

        /// <summary>
        /// Store the list of Non Editable Controls.
        /// </summary>
        private List<Control> NonEditableControls = null;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        public ApplicationType AppType
        {
            get
            {
                return (m_AppType);
            }
            set
            {
                m_AppType = value;
                ActualizeUserControlComponents();
            }
        }

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        public GoodsView Data
        {
            get
            {
                return (m_Data);
            }
            set
            {
                if (value != null)
                {
                    m_Data = value;
                    CtrlOperation = Operation.Show;
                }
                else throw new ArgumentNullException("Error, no s'ha trobat l'Article a carregar.");
            }
        }

        /// <summary>
        /// Get or Set the Operation of the UserControl.
        /// </summary>
        public Operation CtrlOperation
        {
            get
            {
                return (m_CtrlOperation);
            }
            set
            {
                m_CtrlOperation = value;
                switch (m_CtrlOperation)
                {
                    case Operation.Show :
                         cbAcceptButton.Width = HideColumn;
                         cbAcceptButtonExt.Width = HideColumn;
                         cbEditButton.Width = ViewButton;
                         cbCancelButtonExt.Width = HideColumn;
                         cbCancelButton.Width = HideColumn;
                         LoadDataInWindow(m_Data);
                         break;
                    case Operation.Edit:
                         EditedGood = new GoodsView(m_Data);
                         AreDataChanged = false;
                         break;
                }
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is GroupBox)
                    {
                        ((GroupBox)control).SetResourceReference(Control.StyleProperty,
                                                                 ((m_CtrlOperation == Operation.Show) ? "NonEditableGroupBox" : "EditableGroupBox"));
                    }
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
            }
        }

        /// <summary>
        /// Get or Set the Edited Company information.
        /// </summary>
        private GoodsView EditedGood
        {
            get;
            set;
        }


        /// <summary>
        /// Get or Set if the data of the Customer has changed.
        /// </summary>
        private bool AreDataChanged
        {
            get
            {
                return (m_AreDataChanged);
            }
            set
            {
                m_AreDataChanged = value;
                if (m_AreDataChanged)
                {
                    cbAcceptButton.Width = ViewButton;
                    cbAcceptButtonExt.Width = ViewExtendedColumn;
                    cbEditButton.Width = HideColumn;
                    cbCancelButtonExt.Width = ViewExtendedColumn;
                    cbCancelButton.Width = ViewButton;
                }
                else
                {
                    cbAcceptButton.Width = HideColumn;
                    cbAcceptButtonExt.Width = HideColumn;
                    cbEditButton.Width = HideColumn;
                    cbCancelButtonExt.Width = HideColumn;
                    cbCancelButton.Width = ViewButton;
                }
            }
        }

        /// <summary>
        /// Get or Set if the manager of the data change for the Customer has active.
        /// </summary>
        private bool DataChangedManagerActive
        {
            get;
            set;
        }

        #region GUI

        /// <summary>
        /// Get the ThemeInfo to apply 
        /// </summary>
        private ThemeInfo AppTheme
        {
            get
            {
                if (AppType == ApplicationType.Comptabilitat) return (HispaniaApp);
                else if (AppType == ApplicationType.Help) return (HispaniaSupportApp);
                else throw new Exception("Error, undefined app type.");
            }
        }

        /// <summary>
        /// Get the background color for the search text box.
        /// </summary>
        private Brush BrAppColor
        {
            get
            {
                if (m_AppColor == null)
                {
                    if (AppType == ApplicationType.Comptabilitat) m_AppColor = new SolidColorBrush(Color.FromArgb(255, 155, 33, 28));
                    else if (AppType == ApplicationType.Help)
                    {
                        m_AppColor = new SolidColorBrush(Color.FromArgb(255, 8, 70, 124));
                    }
                    else m_AppColor = new SolidColorBrush(Colors.WhiteSmoke);
                }
                return (m_AppColor);
            }
        }

        #endregion

        #endregion

        #region Buiders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public GoodRanges(ApplicationType AppType)
        {
            InitializeComponent();
            InitNonEditableControls();
            InitEditableControls();
            Initialize(AppType);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType);
            //  Load the managers of the controls of the Window.
                LoadManagers();
        }

        #endregion

        #region Standings

        /// <summary>
        /// Method that actualize the Window components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void ActualizeWindowComponents(ApplicationType AppType)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
            //  Actualize state of Window controls
                CtrlOperation = Operation.Show;       
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                tbCumulativeSalesRetailPrice1,
                tbCumulativeSalesRetailPrice2,
                tbCumulativeSalesRetailPrice3,
                tbCumulativeSalesRetailPrice4,
                tbCumulativeSalesRetailPrice5,
                tbCumulativeSalesRetailPrice6,
                tbCumulativeSalesRetailPrice7,
                tbCumulativeSalesRetailPrice8,
                tbCumulativeSalesRetailPrice9,
                tbCumulativeSalesRetailPrice10,
                tbCumulativeSalesRetailPrice11,
                tbCumulativeSalesRetailPrice12,
                tbCumulativeSalesCost1,
                tbCumulativeSalesCost2,
                tbCumulativeSalesCost3,
                tbCumulativeSalesCost4,
                tbCumulativeSalesCost5,
                tbCumulativeSalesCost6,
                tbCumulativeSalesCost7,
                tbCumulativeSalesCost8,
                tbCumulativeSalesCost9,
                tbCumulativeSalesCost10,
                tbCumulativeSalesCost11,
                tbCumulativeSalesCost12,
                gbAcumSalesCosts
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitNonEditableControls()
        {
            NonEditableControls = new List<Control>
            {
                lblGoodCode,
                tbGoodCode,
                lblGoodDescription,
                tbGoodDescription,
                tbGoodRange1,
                tbGoodRange2,
                tbGoodRange3,
                tbGoodRange4,
                tbGoodRange5,
                tbGoodRange6,
                tbGoodRange7,
                tbGoodRange8,
                tbGoodRange9,
                tbGoodRange10,
                tbGoodRange11,
                tbGoodRange12,
                tbRetailPriceRange,
                tbSalesCostRange,
                tbGoodRangeRange,
                tbGoodRange1Real,
                tbGoodRange2Real,
                tbGoodRange3Real,
                tbGoodRange4Real,
                tbGoodRange5Real,
                tbGoodRange6Real,
                tbGoodRange7Real,
                tbGoodRange8Real,
                tbGoodRange9Real,
                tbGoodRange10Real,
                tbGoodRange11Real,
                tbGoodRange12Real,
                tbGoodRangeRangeReal
            };
        }

        /// <summary>
        /// Method that actualize the UserControl components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void ActualizeUserControlComponents()
        {
            //  Apply Theme to UserControl.
                ThemeManager.ActualTheme = AppTheme;
        }

        /// <summary>
        /// Load the data of the Company into the Window
        /// </summary>
        private void LoadDataInWindow(GoodsView goodView)
        {
            if (goodView == null) return;
            DataChangedManagerActive = false;
            tbGoodCode.Text = goodView.Good_Code;
            tbGoodDescription.Text = goodView.Good_Description;
            LoadInControls_CumulativeSalesRetailPrice(goodView);
            LoadInControls_CumulativeSalesCost(goodView);
            CalculateRanges(goodView);
            DataChangedManagerActive = true;
        }

        private void LoadInControls_CumulativeSalesRetailPrice(GoodsView goodView)
        {
            tbCumulativeSalesRetailPrice1.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_1, DecimalType.Currency);
            tbCumulativeSalesRetailPrice2.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_2, DecimalType.Currency);
            tbCumulativeSalesRetailPrice3.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_3, DecimalType.Currency);
            tbCumulativeSalesRetailPrice4.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_4, DecimalType.Currency);
            tbCumulativeSalesRetailPrice5.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_5, DecimalType.Currency);
            tbCumulativeSalesRetailPrice6.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_6, DecimalType.Currency);
            tbCumulativeSalesRetailPrice7.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_7, DecimalType.Currency);
            tbCumulativeSalesRetailPrice8.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_8, DecimalType.Currency);
            tbCumulativeSalesRetailPrice9.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_9, DecimalType.Currency);
            tbCumulativeSalesRetailPrice10.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_10, DecimalType.Currency);
            tbCumulativeSalesRetailPrice11.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_11, DecimalType.Currency);
            tbCumulativeSalesRetailPrice12.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Retail_Price_12, DecimalType.Currency);
        }
        private void LoadInControls_CumulativeSalesCost(GoodsView goodView)
        {
            tbCumulativeSalesCost1.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_1, DecimalType.Currency);
            tbCumulativeSalesCost2.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_2, DecimalType.Currency);
            tbCumulativeSalesCost3.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_3, DecimalType.Currency);
            tbCumulativeSalesCost4.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_4, DecimalType.Currency);
            tbCumulativeSalesCost5.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_5, DecimalType.Currency);
            tbCumulativeSalesCost6.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_6, DecimalType.Currency);
            tbCumulativeSalesCost7.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_7, DecimalType.Currency);
            tbCumulativeSalesCost8.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_8, DecimalType.Currency);
            tbCumulativeSalesCost9.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_9, DecimalType.Currency);
            tbCumulativeSalesCost10.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_10, DecimalType.Currency);
            tbCumulativeSalesCost11.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_11, DecimalType.Currency);
            tbCumulativeSalesCost12.Text = GlobalViewModel.GetStringFromDecimalValue(goodView.Cumulative_Sales_Cost_12, DecimalType.Currency);
        }

        private void CalculateRanges(GoodsView goodView)
        {
            CalculatAcumRanges(goodView, out decimal RetailPriceRange, out decimal SalesCostRange);
            CalculateRanges(goodView, RetailPriceRange, SalesCostRange);
            CalculateRangesReal(goodView, RetailPriceRange, SalesCostRange);
        }

        private void CalculatAcumRanges(GoodsView goodView, out decimal RetailPriceRange, out decimal SalesCostRange)
        {
            RetailPriceRange = goodView.Cumulative_Sales_Retail_Price_1 + goodView.Cumulative_Sales_Retail_Price_2 +
                               goodView.Cumulative_Sales_Retail_Price_3 + goodView.Cumulative_Sales_Retail_Price_4 +
                               goodView.Cumulative_Sales_Retail_Price_5 + goodView.Cumulative_Sales_Retail_Price_6 +
                               goodView.Cumulative_Sales_Retail_Price_7 + goodView.Cumulative_Sales_Retail_Price_8 +
                               goodView.Cumulative_Sales_Retail_Price_9 + goodView.Cumulative_Sales_Retail_Price_10 +
                               goodView.Cumulative_Sales_Retail_Price_11 + goodView.Cumulative_Sales_Retail_Price_12;
            tbRetailPriceRange.Text = GlobalViewModel.GetStringFromDecimalValue(RetailPriceRange, DecimalType.Currency);
            SalesCostRange = goodView.Cumulative_Sales_Cost_1 + goodView.Cumulative_Sales_Cost_2 +
                             goodView.Cumulative_Sales_Cost_3 + goodView.Cumulative_Sales_Cost_4 +
                             goodView.Cumulative_Sales_Cost_5 + goodView.Cumulative_Sales_Cost_6 +
                             goodView.Cumulative_Sales_Cost_7 + goodView.Cumulative_Sales_Cost_8 +
                             goodView.Cumulative_Sales_Cost_9 + goodView.Cumulative_Sales_Cost_10 +
                             goodView.Cumulative_Sales_Cost_11 + goodView.Cumulative_Sales_Cost_12;
            tbSalesCostRange.Text = GlobalViewModel.GetStringFromDecimalValue(SalesCostRange, DecimalType.Currency);
        }

        private void CalculateRanges(GoodsView goodView, decimal RetailPriceRange, decimal SalesCostRange)
        {
            tbGoodRange1.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_1, goodView.Cumulative_Sales_Cost_1);
            tbGoodRange2.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_2, goodView.Cumulative_Sales_Cost_2);
            tbGoodRange3.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_3, goodView.Cumulative_Sales_Cost_3);
            tbGoodRange4.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_4, goodView.Cumulative_Sales_Cost_4);
            tbGoodRange5.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_5, goodView.Cumulative_Sales_Cost_5);
            tbGoodRange6.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_6, goodView.Cumulative_Sales_Cost_6);
            tbGoodRange7.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_7, goodView.Cumulative_Sales_Cost_7);
            tbGoodRange8.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_8, goodView.Cumulative_Sales_Cost_8);
            tbGoodRange9.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_9, goodView.Cumulative_Sales_Cost_9);
            tbGoodRange10.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_10, goodView.Cumulative_Sales_Cost_10);
            tbGoodRange11.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_11, goodView.Cumulative_Sales_Cost_11);
            tbGoodRange12.Text = MonthRange(goodView.Cumulative_Sales_Retail_Price_12, goodView.Cumulative_Sales_Cost_12);
            tbGoodRangeRange.Text = MonthRange(RetailPriceRange, SalesCostRange);
        }

        private string MonthRange(decimal Sales_Retail_Price, decimal Sales_Cost)
        {
            if ((Sales_Cost == 0) && (Sales_Retail_Price == 0)) return GlobalViewModel.GetStringFromDecimalValue(0, DecimalType.Percent, true);
            else if ((Sales_Cost == 0) && (Sales_Retail_Price != 0)) return GlobalViewModel.GetStringFromDecimalValue(100, DecimalType.Percent, true);
            else
            {
                return GlobalViewModel.GetStringFromDecimalValue(((Sales_Retail_Price - Sales_Cost) * 100) / Sales_Cost, DecimalType.Percent, true);
            }
        }

        private void CalculateRangesReal(GoodsView goodView, decimal RetailPriceRange, decimal SalesCostRange)
        {
            tbGoodRange1Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_1, goodView.Cumulative_Sales_Cost_1);
            tbGoodRange2Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_2, goodView.Cumulative_Sales_Cost_2);
            tbGoodRange3Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_3, goodView.Cumulative_Sales_Cost_3);
            tbGoodRange4Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_4, goodView.Cumulative_Sales_Cost_4);
            tbGoodRange5Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_5, goodView.Cumulative_Sales_Cost_5);
            tbGoodRange6Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_6, goodView.Cumulative_Sales_Cost_6);
            tbGoodRange7Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_7, goodView.Cumulative_Sales_Cost_7);
            tbGoodRange8Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_8, goodView.Cumulative_Sales_Cost_8);
            tbGoodRange9Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_9, goodView.Cumulative_Sales_Cost_9);
            tbGoodRange10Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_10, goodView.Cumulative_Sales_Cost_10);
            tbGoodRange11Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_11, goodView.Cumulative_Sales_Cost_11);
            tbGoodRange12Real.Text = MonthRangeReal(goodView.Cumulative_Sales_Retail_Price_12, goodView.Cumulative_Sales_Cost_12);
            tbGoodRangeRangeReal.Text = MonthRangeReal(RetailPriceRange, SalesCostRange);
        }

        private string MonthRangeReal(decimal Sales_Retail_Price, decimal Sales_Cost)
        {
            if ((Sales_Retail_Price == 0) && (Sales_Cost == 0)) return GlobalViewModel.GetStringFromDecimalValue(0, DecimalType.Percent, true);
            else if ((Sales_Retail_Price == 0) && (Sales_Cost != 0)) return GlobalViewModel.GetStringFromDecimalValue(100, DecimalType.Percent, true);
            else
            {
                return GlobalViewModel.GetStringFromDecimalValue(((Sales_Retail_Price - Sales_Cost) / Sales_Retail_Price) * 100, DecimalType.Percent, true);
            }
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window
                this.Closed += GoodRanges_Closed; ;
            //  By default the manager for the Customer Data changes is active.
                DataChangedManagerActive = true;
                tbCumulativeSalesRetailPrice1.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice1.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice2.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice2.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice3.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice3.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice4.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice4.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice5.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice5.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice6.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice6.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice7.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice7.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice8.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice8.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice9.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice9.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice10.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice10.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice11.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice11.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesRetailPrice12.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesRetailPrice12.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost1.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost1.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost2.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost2.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost3.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost3.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost4.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost4.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost5.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost5.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost6.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost6.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost7.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost7.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost8.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost8.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost9.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost9.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost10.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost10.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost11.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost11.TextChanged += TBCurrencyDataChanged;
                tbCumulativeSalesCost12.PreviewTextInput += TBPreviewTextInput;
                tbCumulativeSalesCost12.TextChanged += TBCurrencyDataChanged;
            //  Button managers
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnEdit.Click += BtnEdit_Click;
        }

        #region TextBox

        /// <summary>
        /// Select all text in sender TextBox control.
        /// </summary>
        /// <param name="sender">TextBox control that has produced the event.</param>
        /// <param name="e">Parameters associateds to the event.</param>
        private void TBGotFocus(object sender, RoutedEventArgs e)
        {
            GlobalViewModel.Instance.SelectAllTextInGotFocusEvent(sender, e);
        }

        /// <summary>
        /// Checking the Input Char in function of the data type that has associated
        /// </summary>
        private void TBPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if ((sender == tbCumulativeSalesRetailPrice1) || (sender == tbCumulativeSalesRetailPrice2) ||
                (sender == tbCumulativeSalesRetailPrice3) || (sender == tbCumulativeSalesRetailPrice4) ||
                (sender == tbCumulativeSalesRetailPrice5) || (sender == tbCumulativeSalesRetailPrice6) ||
                (sender == tbCumulativeSalesRetailPrice7) || (sender == tbCumulativeSalesRetailPrice8) ||
                (sender == tbCumulativeSalesRetailPrice9) || (sender == tbCumulativeSalesRetailPrice10) ||
                (sender == tbCumulativeSalesRetailPrice11) || (sender == tbCumulativeSalesRetailPrice12) ||
                (sender == tbCumulativeSalesCost1) || (sender == tbCumulativeSalesCost2) ||
                (sender == tbCumulativeSalesCost3) || (sender == tbCumulativeSalesCost4) ||
                (sender == tbCumulativeSalesCost5) || (sender == tbCumulativeSalesCost6) ||
                (sender == tbCumulativeSalesCost7) || (sender == tbCumulativeSalesCost8) ||
                (sender == tbCumulativeSalesCost9) || (sender == tbCumulativeSalesCost10) ||
                (sender == tbCumulativeSalesCost11) || (sender == tbCumulativeSalesCost12))
            {
                e.Handled = ! GlobalViewModel.IsValidCurrencyChar(e.Text);
            }
        }

        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBCurrencyDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Currency);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbCumulativeSalesRetailPrice1) EditedGood.Cumulative_Sales_Retail_Price_1 = value;
                    else if (sender == tbCumulativeSalesRetailPrice2) EditedGood.Cumulative_Sales_Retail_Price_2 = value;
                    else if (sender == tbCumulativeSalesRetailPrice3) EditedGood.Cumulative_Sales_Retail_Price_3 = value;
                    else if (sender == tbCumulativeSalesRetailPrice4) EditedGood.Cumulative_Sales_Retail_Price_4 = value;
                    else if (sender == tbCumulativeSalesRetailPrice5) EditedGood.Cumulative_Sales_Retail_Price_5 = value;
                    else if (sender == tbCumulativeSalesRetailPrice6) EditedGood.Cumulative_Sales_Retail_Price_6 = value;
                    else if (sender == tbCumulativeSalesRetailPrice7) EditedGood.Cumulative_Sales_Retail_Price_7 = value;
                    else if (sender == tbCumulativeSalesRetailPrice8) EditedGood.Cumulative_Sales_Retail_Price_8 = value;
                    else if (sender == tbCumulativeSalesRetailPrice9) EditedGood.Cumulative_Sales_Retail_Price_9 = value;
                    else if (sender == tbCumulativeSalesRetailPrice10) EditedGood.Cumulative_Sales_Retail_Price_10 = value;
                    else if (sender == tbCumulativeSalesRetailPrice11) EditedGood.Cumulative_Sales_Retail_Price_11 = value;
                    else if (sender == tbCumulativeSalesRetailPrice12) EditedGood.Cumulative_Sales_Retail_Price_12 = value;
                    else if (sender == tbCumulativeSalesCost1) EditedGood.Cumulative_Sales_Cost_1 = value;
                    else if (sender == tbCumulativeSalesCost2) EditedGood.Cumulative_Sales_Cost_2 = value;
                    else if (sender == tbCumulativeSalesCost3) EditedGood.Cumulative_Sales_Cost_3 = value;
                    else if (sender == tbCumulativeSalesCost4) EditedGood.Cumulative_Sales_Cost_4 = value;
                    else if (sender == tbCumulativeSalesCost5) EditedGood.Cumulative_Sales_Cost_5 = value;
                    else if (sender == tbCumulativeSalesCost6) EditedGood.Cumulative_Sales_Cost_6 = value;
                    else if (sender == tbCumulativeSalesCost7) EditedGood.Cumulative_Sales_Cost_7 = value;
                    else if (sender == tbCumulativeSalesCost8) EditedGood.Cumulative_Sales_Cost_8 = value;
                    else if (sender == tbCumulativeSalesCost9) EditedGood.Cumulative_Sales_Cost_9 = value;
                    else if (sender == tbCumulativeSalesCost10) EditedGood.Cumulative_Sales_Cost_10 = value;
                    else if (sender == tbCumulativeSalesCost11) EditedGood.Cumulative_Sales_Cost_11 = value;
                    else if (sender == tbCumulativeSalesCost12) EditedGood.Cumulative_Sales_Cost_12 = value;
                    CalculateRanges(EditedGood);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInWindow(EditedGood);
                }
                AreDataChanged = (EditedGood != Data);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Editing Good

        /// <summary>
        /// Manage the button for .
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActualizeGoodFromDb();
                if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(Data, out string ErrMsg))
                {
                    CtrlOperation = Operation.Edit;
                    gbAcumSalesCosts.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                }
                else MsgManager.ShowMessage(ErrMsg);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, a l'iniciar l'edició dels Acumulats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the button for accept the edit operation.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            GoodsAttributes ErrorField = GoodsAttributes.None;
            try
            {
                EditedGood.Validate(out ErrorField);
                GlobalViewModel.Instance.HispaniaViewModel.UpdateGoodAcums(EditedGood);
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Data, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
                Data = new GoodsView(EditedGood);
                gbAcumSalesCosts.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                EvAcceptGoodRangeChanged?.Invoke(new GoodsView(EditedGood));
            }
            catch (FormatException fex)
            {

                MsgManager.ShowMessage(string.Format("Error, al guardar les dades dels Acumulats que s'han editat.\r\nDetalls: {0}", MsgManager.ExcepMsg(fex)));
                EditedGood.RestoreSourceValue(Data, ErrorField);
                LoadDataInWindow(EditedGood);
                AreDataChanged = (EditedGood != Data);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al guardar les dades dels Acumulats que s'han editat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                EditedGood.RestoreSourceValue(Data, ErrorField);
                LoadDataInWindow(EditedGood);
                AreDataChanged = (EditedGood != Data);
            }
        }

        /// <summary>
        /// Manage the button for cancel the edit operation.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Data, out string ErrMsg))
            {
                MsgManager.ShowMessage(ErrMsg);
            }
            EditedGood = null;
            CtrlOperation = Operation.Show;
            gbAcumSalesCosts.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
        }

        #endregion

        #region Window

        private void GoodRanges_Closed(object sender, EventArgs e)
        {
            if (EditedGood != null)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Data, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion
                
        #region Database Operations

        private void ActualizeGoodFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                Data = GlobalViewModel.Instance.HispaniaViewModel.GetGoodFromDb(Data);
                EditedGood = new GoodsView(Data);
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion
    }
}
