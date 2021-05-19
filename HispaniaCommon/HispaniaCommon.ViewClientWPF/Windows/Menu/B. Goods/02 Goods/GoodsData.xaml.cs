#region Libraries used for this control

using HispaniaCommon.ViewClientWPF.Windows;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for GoodsData.xaml
    /// </summary>
    public partial class GoodsData : UserControl
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
        /// Show the button bar.
        /// </summary>
        private GridLength ViewHistoricButton = new GridLength(160.0);

        /// <summary>
        /// Hide the button bar.
        /// </summary>
        private GridLength HideHistoricButton = new GridLength(0.0);

        /// <summary>
        /// Show the button bar.
        /// </summary>
        private GridLength ViewAcceptButton = new GridLength(120.0);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength ViewMiddleColumn = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Hide the button bar.
        /// </summary>
        private GridLength HideAcceptButton = new GridLength(0.0);

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
        public delegate void dlgAccept(GoodsView NewOrEditedGood);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Good is correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Good data to manage.
        /// </summary>
        private GoodsView m_Good = null;

        /// <summary>
        /// Store the Cities and CP 
        /// </summary>
        public Dictionary<string, UnitsView> m_Units;

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Store the Operation of the UserControl.
        /// </summary>
        private Operation m_CtrlOperation = Operation.Show;
        
        /// <summary>
        /// Stotre if the data of the Good has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store the list of Editable Controls.
        /// </summary>
        private List<Control> EditableControls = null;

        /// <summary>
        /// Store the list of Non Editable Controls.
        /// </summary>
        private List<Control> NonEditableControls = null;

        /// <summary>
        /// Store the list of Editable Controls.
        /// </summary>
        private List<Control> OnlyQueryControls = null;

        #region GUI

        /// <summary>
        /// Store the background color for the search text box.
        /// </summary>
        private Brush m_AppColor = null;

        /// <summary>
        /// Window to manage the Price Ranges
        /// </summary>
        private PriceRanges PriceRangesWindow;

        /// <summary>
        /// Window to manage the Providers
        /// </summary>
        private Providers ProvidersWindow;

        /// <summary>
        /// Window to manage the Good Ranges
        /// </summary>
        private GoodRanges GoodRangesWindow;

        /// <summary>
        /// Window to manage the Histo Goods
        /// </summary>
        private HistoGoods HistoGoodsWindow;

        /// <summary>
        /// Window to manage the Inputs Outputs
        /// </summary>
        private GoodInputsOutputs GoodInputsOutputsWindow;

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
        /// Get or Set the Good data to manage.
        /// </summary>
        public GoodsView Good
        {
            get
            {
                return (m_Good);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Good = value;
                    EditedGood = new GoodsView(m_Good);
                    DataChangedManagerActive = false;
                    chkbEditGeneralTab.IsChecked = false;
                    DataChangedManagerActive = true;
                    LoadDataInControls(m_Good, true);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades de l'article a carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the Edited Good information.
        /// </summary>
        private GoodsView EditedGood
        {
            get;
            set;
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
                         if (Good == null) throw new InvalidOperationException("Error, impossible visualitzar un Article sense dades.");
                         tiConsullation.Visibility = Visibility.Visible;
                         tbCancel.Text = "Tornar";
                         lblCanceled.Visibility = Visibility.Visible;
                         chkbCanceled.Visibility = Visibility.Visible;
                         btnPrices.Visibility = btnProviders.Visibility = Visibility.Visible;
                         break;
                    case Operation.Add:
                         Good = new GoodsView();
                         tiConsullation.Visibility = Visibility.Hidden;
                         tbCancel.Text = "Cancel·lar";
                         lblCanceled.Visibility = Visibility.Hidden;
                         chkbCanceled.Visibility = Visibility.Hidden;
                         btnPrices.Visibility = btnProviders.Visibility = Visibility.Hidden;
                         break;
                    case Operation.Edit:
                         if (Good == null) throw new InvalidOperationException("Error, impossible editar un Article sense dades.");
                         tiConsullation.Visibility = Visibility.Visible;
                         tbCancel.Text = "Cancel·lar";
                         lblCanceled.Visibility = Visibility.Visible;
                         chkbCanceled.Visibility = Visibility.Visible;
                         btnPrices.Visibility = btnProviders.Visibility = Visibility.Visible;
                         break;
                }
                string properyValue;
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is GroupBox)
                    {
                        properyValue = ((m_CtrlOperation == Operation.Show) ? "NonEditableGroupBox" : "EditableGroupBox");
                        ((GroupBox)control).SetResourceReference(Control.StyleProperty, properyValue);
                    }
                    else if (control is TabItem)
                    {
                        properyValue = ((m_CtrlOperation == Operation.Show) ? "NonEditableTabItem" : "EditableTabItem");
                        ((TabItem)control).SetResourceReference(Control.StyleProperty, properyValue);
                    }
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
                foreach (Control control in OnlyQueryControls)
                {
                    if (control is Button) control.IsEnabled = (m_CtrlOperation == Operation.Show);
                    else if (control is GroupBox)
                    {
                        properyValue = ((m_CtrlOperation == Operation.Show) ? "EditableGroupBox" : "NonEditableGroupBox");
                        ((GroupBox)control).SetResourceReference(Control.StyleProperty, properyValue);
                    }
                }
                tiGeneral.IsSelected = true;
                tbGoodCode.Focus();
            }
        }

        /// <summary>
        /// Get or Set if the data of the Good has changed.
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
                    cbAcceptButton.Width = ViewAcceptButton;
                    cbMiddleColumn.Width = ViewMiddleColumn;
                }
                else
                {
                    cbAcceptButton.Width = HideAcceptButton;
                    cbMiddleColumn.Width = HideAcceptButton;
                }
            }
        }


        /// <summary>
        /// Get or Set if the manager of the data change for the Good has active.
        /// </summary>
        private bool DataChangedManagerActive
        {
            get;
            set;
        }

        #region Foreign Keys

        /// <summary>
        /// Get or Set the Units
        /// </summary>
        public Dictionary<string, UnitsView> Good_Units
        {
            get
            {
                return (m_Units);
            }
            set
            {
                m_Units = value;
                if (m_Units != null)
                {
                    cbUnitType.ItemsSource = m_Units;
                    cbUnitType.DisplayMemberPath = "Key";
                    cbUnitType.SelectedValuePath = "Value";
                }
            }
        }

        #endregion

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

        #region Builders

        /// <summary>
        /// Builder by default of this control.
        /// </summary>
        public GoodsData()
        {
            //  Initialization of controls of the UserControl
                InitializeComponent();
            //  Initialize GUI.
                InitEditableControls();
                InitNonEditableControls();
                InitOnlyQueryControls();
            //  Load the managers of the controls of the UserControl.
                LoadManagers();
        }

        #endregion

        #region Standings

        /// <summary>
        /// Initialize the list of NonEditable Controls.
        /// </summary>
        private void InitNonEditableControls()
        {
            NonEditableControls = new List<Control>
            {
                tbAvMonth,  // Valor calculat
                tbAcMonth,  // Valor calculat
                tbAvYear,   // Valor calculat
                tbAcYear,   // Valor calculat
                tbValue,    // Valor calculat
                lblValue,
                tbBillingUnitStocks,
                tbShippingUnitStocks,
                tbBillingUnitAvailable,
                tbShippingUnitAvailable,
                tbBillingUnitEntrance,
                tbShippingUnitEntrance,
                tbBillingUnitDepartures,
                tbShippingUnitDepartures,
                lblMinimum,
                tbMinimum,
                tbAverageBillingUnit,
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblGoodCode,
                tbGoodCode,
                lblGoodDescription,
                tbGoodDescription,
                lblCanceled,
                chkbCanceled,
                lblEditGeneralTab,
                chkbEditGeneralTab,
                lblUnitType,
                gbFactorConversion,
                tbFactorConversion,
                cbUnitType,
                tiGeneral,
                tiDivers,
                tiConsullation,
                gbAverageUnitBilling,
                tbAverageBillingUnit,
                gbFactorConversion,
                lblCodFam,
                tbCodFam,
                lblPriceCost,
                tbPriceCost,
                lblAveragePriceCost,
                tbAveragePriceCost,
                btnPrices,
                btnProviders
            };
        }

        /// <summary>
        /// Initialize the list of NonEditable Controls.
        /// </summary>
        private void InitOnlyQueryControls()
        {
            OnlyQueryControls = new List<Control>
            {
                btnEntriesDepartures,
                btnAcumulatedAnnual,
                btnAcumulatedHistoric,
                gbQueries
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
            //  Put the UserControl in Initial State.
                cbAcceptButton.Width = HideAcceptButton;
        }

        /// <summary>
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        /// <param name="GoodsView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadDataInControls(GoodsView GoodsView, bool ThrowException = false)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Good Controls
                tbGoodCode.Text = GoodsView.Good_Code;
                tbGoodDescription.Text = GoodsView.Good_Description;
                tbPriceCost.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Price_Cost, DecimalType.Currency);
                tbAveragePriceCost.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Average_Price_Cost, DecimalType.Currency);
                tbValue.Text = CalculateValueField(GoodsView);
                chkbCanceled.IsChecked = GoodsView.Canceled;
                tbBillingUnitStocks.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Billing_Unit_Stocks, DecimalType.Currency);
                tbShippingUnitStocks.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Shipping_Unit_Stocks, DecimalType.Currency);
                tbBillingUnitAvailable.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Billing_Unit_Available, DecimalType.Currency);
                tbShippingUnitAvailable.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Shipping_Unit_Available, DecimalType.Currency);
                tbBillingUnitDepartures.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Billing_Unit_Departure, DecimalType.Currency);
                tbShippingUnitDepartures.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Shipping_Unit_Departure, DecimalType.Currency);
                tbBillingUnitEntrance.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Billing_Unit_Entrance, DecimalType.Currency);
                tbShippingUnitEntrance.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Shipping_Unit_Entrance, DecimalType.Currency);
                tbCodFam.Text = GoodsView.Cod_Fam;
                CalculateCumulativeSalesInfo(GoodsView);
                tbAvYear.Text = CalculateAvYearField(GoodsView);
                tbAcYear.Text = CalculateAcYearField(GoodsView); 
                tbMinimum.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Minimum, DecimalType.Currency);
                tbFactorConversion.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Conversion_Factor, DecimalType.Unit);
                tbAverageBillingUnit.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Average_Billing_Unit, DecimalType.Unit);
                LoadExternalTablesInfo(GoodsView, ThrowException);
            //  Activate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="GoodsView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(GoodsView GoodsView, bool ThrowException = false)
        {
            if ((Good_Units != null) && (GoodsView.Good_Unit != null))
            {
                Dictionary<string, UnitsView> Items = (Dictionary<string, UnitsView>)cbUnitType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyUnitView(GoodsView.Good_Unit);
                if (Items.ContainsKey(Key)) cbUnitType.SelectedValue = Good_Units[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat la Unitat '{0}'.", Good_Units[Key].Code));
                    }
                }
            }
            else cbUnitType.SelectedIndex = -1;
        }

        private string CalculateValueField(GoodsView GoodsView)
        {
             return GlobalViewModel.GetStringFromDecimalValue(GoodsView.Average_Price_Cost * GoodsView.Billing_Unit_Stocks, DecimalType.Currency);
        }

        private void CalculateCumulativeSalesInfo(GoodsView GoodsView)
        {
            switch (DateTime.Now.Month)
            {
                case 1:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_1, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_1, DecimalType.Currency);
                     break;
                case 2:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_2, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_2, DecimalType.Currency);
                     break;
                case 3:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_3, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_3, DecimalType.Currency);
                     break;
                case 4:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_4, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_4, DecimalType.Currency);
                     break;
                case 5:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_5, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_5, DecimalType.Currency);
                     break;
                case 6:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_6, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_6, DecimalType.Currency);
                     break;
                case 7:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_7, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_7, DecimalType.Currency);
                     break;
                case 8:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_8, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_8, DecimalType.Currency);
                     break;
                case 9:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_9, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_9, DecimalType.Currency);
                     break;
                case 10:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_10, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_10, DecimalType.Currency);
                     break;
                case 11:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_11, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_11, DecimalType.Currency);
                     break;
                case 12:
                     tbAvMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Retail_Price_12, DecimalType.Currency);
                     tbAcMonth.Text = GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_12, DecimalType.Currency);
                     break;
            }
        }

        private string CalculateAvYearField(GoodsView GoodsView)
        {
            return GlobalViewModel.GetStringFromDecimalValue(
                                        GoodsView.Cumulative_Sales_Retail_Price_1 + GoodsView.Cumulative_Sales_Retail_Price_2 +
                                        GoodsView.Cumulative_Sales_Retail_Price_3 + GoodsView.Cumulative_Sales_Retail_Price_4 +
                                        GoodsView.Cumulative_Sales_Retail_Price_5 + GoodsView.Cumulative_Sales_Retail_Price_6 +
                                        GoodsView.Cumulative_Sales_Retail_Price_7 + GoodsView.Cumulative_Sales_Retail_Price_8 +
                                        GoodsView.Cumulative_Sales_Retail_Price_9 + GoodsView.Cumulative_Sales_Retail_Price_10 +
                                        GoodsView.Cumulative_Sales_Retail_Price_11 + GoodsView.Cumulative_Sales_Retail_Price_12, 
                                        DecimalType.Currency);

        }

        private string CalculateAcYearField(GoodsView GoodsView)
        {
            return GlobalViewModel.GetStringFromDecimalValue(GoodsView.Cumulative_Sales_Cost_1 + GoodsView.Cumulative_Sales_Cost_2 +
                                                             GoodsView.Cumulative_Sales_Cost_3 + GoodsView.Cumulative_Sales_Cost_4 +
                                                             GoodsView.Cumulative_Sales_Cost_5 + GoodsView.Cumulative_Sales_Cost_6 +
                                                             GoodsView.Cumulative_Sales_Cost_7 + GoodsView.Cumulative_Sales_Cost_8 +
                                                             GoodsView.Cumulative_Sales_Cost_9 + GoodsView.Cumulative_Sales_Cost_10 +
                                                             GoodsView.Cumulative_Sales_Cost_11 + GoodsView.Cumulative_Sales_Cost_12,
                                                             DecimalType.Currency);
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the UserControl
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Good Data changes is active.
                DataChangedManagerActive = true;
            //  TextBox
                tbGoodCode.PreviewTextInput += TBPreviewTextInput;
                tbGoodCode.TextChanged += TBDataChanged;
                tbGoodDescription.PreviewTextInput += TBPreviewTextInput;
                tbGoodDescription.TextChanged += TBDataChanged;
                tbCodFam.PreviewTextInput += TBPreviewTextInput;
                tbCodFam.TextChanged += TBDataChanged;
                tbFactorConversion.PreviewTextInput += TBPreviewTextInput;
                tbFactorConversion.TextChanged += TBDecimalDataChanged;
                tbAverageBillingUnit.PreviewTextInput += TBPreviewTextInput;
                tbAverageBillingUnit.TextChanged += TBDecimalDataChanged;
                tbPriceCost.PreviewTextInput += TBPreviewTextInput;
                tbPriceCost.TextChanged += TBCurrencyDataChanged;
                tbAveragePriceCost.PreviewTextInput += TBPreviewTextInput;
                tbAveragePriceCost.TextChanged += TBCurrencyDataChanged;
                tbBillingUnitStocks.PreviewTextInput += TBPreviewTextInput;
                tbBillingUnitStocks.TextChanged += TBDecimalDataChanged;
                tbShippingUnitStocks.PreviewTextInput += TBPreviewTextInput;
                tbShippingUnitStocks.TextChanged += TBDecimalDataChanged;
                tbBillingUnitAvailable.PreviewTextInput += TBPreviewTextInput;
                tbBillingUnitAvailable.TextChanged += TBDecimalDataChanged;
                tbShippingUnitAvailable.PreviewTextInput += TBPreviewTextInput;
                tbShippingUnitAvailable.TextChanged += TBDecimalDataChanged;
                tbBillingUnitEntrance.PreviewTextInput += TBPreviewTextInput;
                tbBillingUnitEntrance.TextChanged += TBDecimalDataChanged;
                tbShippingUnitEntrance.PreviewTextInput += TBPreviewTextInput;
                tbShippingUnitEntrance.TextChanged += TBDecimalDataChanged;
                tbBillingUnitDepartures.PreviewTextInput += TBPreviewTextInput;
                tbBillingUnitDepartures.TextChanged += TBDecimalDataChanged;
                tbShippingUnitDepartures.PreviewTextInput += TBPreviewTextInput;
                tbShippingUnitDepartures.TextChanged += TBDecimalDataChanged;
            //  CheckBox
                chkbCanceled.Checked += ChkbCanceled_Checked;
                chkbCanceled.Unchecked += ChkbCanceled_Unchecked;
                chkbEditGeneralTab.Checked += ChkbEditGeneralTab_Checked;
                chkbEditGeneralTab.Unchecked += ChkbEditGeneralTab_Unchecked;
            //  ComboBox
                cbUnitType.SelectionChanged += CbUnitType_SelectionChanged;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnPrices.Click += BtnPrices_Click;
                btnProviders.Click += BtnProviders_Click;
                btnEntriesDepartures.Click += BtnEntriesDepartures_Click;
                btnAcumulatedAnnual.Click += BtnAcumulatedAnnual_Click;
                btnAcumulatedHistoric.Click += BtnAcumulatedHistoric_Click;
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
            if ((sender == tbGoodCode) || (sender == tbCodFam)) e.Handled = !GlobalViewModel.IsValidGoodCodeChar(e.Text);
            else if (sender == tbGoodDescription) e.Handled = !GlobalViewModel.IsValidCommentChar(e.Text);
            else if ((sender == tbPriceCost) || (sender == tbAveragePriceCost) || (sender == tbBillingUnitStocks) || 
                     (sender == tbShippingUnitStocks) || (sender == tbBillingUnitAvailable) || (sender == tbBillingUnitAvailable) ||
                     (sender == tbBillingUnitEntrance) || (sender == tbBillingUnitEntrance) ||
                     (sender == tbBillingUnitDepartures) || (sender == tbBillingUnitDepartures))
            {
                e.Handled = !GlobalViewModel.IsValidCurrencyChar(e.Text);
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                string value = ((TextBox)sender).Text;
                try
                {
                    if (sender == tbGoodCode) EditedGood.Good_Code = value;
                    else if (sender == tbCodFam) EditedGood.Cod_Fam = value;
                    else if (sender == tbGoodDescription) EditedGood.Good_Description = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedGood);
                }
                AreDataChanged = (EditedGood != Good);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBDecimalDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Unit);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbFactorConversion) EditedGood.Conversion_Factor = value;
                    else if (sender == tbAverageBillingUnit) EditedGood.Average_Billing_Unit = value;
                    else if (sender == tbBillingUnitStocks) EditedGood.Billing_Unit_Stocks = value;
                    else if (sender == tbShippingUnitStocks) EditedGood.Shipping_Unit_Stocks = value;
                    else if (sender == tbBillingUnitAvailable) EditedGood.Billing_Unit_Available = value;
                    else if (sender == tbShippingUnitAvailable) EditedGood.Shipping_Unit_Available = value;
                    else if (sender == tbBillingUnitEntrance) EditedGood.Billing_Unit_Entrance = value;
                    else if (sender == tbShippingUnitEntrance) EditedGood.Shipping_Unit_Entrance = value;
                    else if (sender == tbBillingUnitDepartures) EditedGood.Billing_Unit_Departure = value;
                    else if (sender == tbShippingUnitDepartures) EditedGood.Shipping_Unit_Departure = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedGood);
                }
                AreDataChanged = (EditedGood != Good);
                DataChangedManagerActive = true;
            }
        }
                
        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBCurrencyDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    TextBox tbInput = (TextBox)sender;
                    try
                    {
                        GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Currency);
                        decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                        if (sender == tbPriceCost) EditedGood.Price_Cost = value;
                        else if (sender == tbAveragePriceCost) EditedGood.Average_Price_Cost = value;
                    }
                    catch (Exception ex)
                    {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedGood);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedGood != Good);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button

        #region Accept

        /// <summary>
        /// Accept the edition or creatin of the Good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            GoodsAttributes ErrorField = GoodsAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedGood.Validate(out ErrorField);
                    EvAccept?.Invoke(new GoodsView(EditedGood));
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al validar els canvis realitzats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                RestoreSourceValue(ErrorField);
            }
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Cancel the edition or creatin of the Good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //  Send the event that indicates at the observer that the operation is cancelled.
                EvCancel?.Invoke();
        }

        #endregion

        #region Price Ranges

        /// <summary>
        /// Defines a new Price for the Good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnPrices_Click(object sender, RoutedEventArgs e)
        {
            if (PriceRangesWindow == null)
            {
                try
                {
                    PriceRangesWindow = new PriceRanges(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetPriceRanges(EditedGood.Good_Id),
                        Data = EditedGood
                    };
                    PriceRangesWindow.Closed += PriceRangesWindow_Closed;
                    PriceRangesWindow.ShowDialog();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir la finestra de Marges de Preus.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else PriceRangesWindow.Activate();
        }

        /// <summary>
        /// When the Price Ranges Window is closed we actualize the PriceRangesWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void PriceRangesWindow_Closed(object sender, EventArgs e)
        {
            PriceRangesWindow.Closed -= PriceRangesWindow_Closed;
            PriceRangesWindow = null;
        }

        #endregion

        #region Providers

        /// <summary>
        /// Defines a new Provider.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnProviders_Click(object sender, RoutedEventArgs e)
        {
            if (ProvidersWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ProvidersWindow);
                    ProvidersWindow = new Providers(AppType)
                    {
                        Print = false,
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Providers
                    };
                    ProvidersWindow.Closed += ProvidersWindow_Closed;
                    ProvidersWindow.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir el formulari de Gestió de Proveïdors.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else ProvidersWindow.Activate();
        }

        /// <summary>
        /// When the Providers Window is closed we actualize the ProvidersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ProvidersWindow_Closed(object sender, EventArgs e)
        {
            ProvidersWindow.Closed -= ProvidersWindow_Closed;
            ProvidersWindow = null;
        }

        #endregion

        #region EntriesDepartures (Query data)

        /// <summary>
        /// Show Entries/Departures information.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnEntriesDepartures_Click(object sender, RoutedEventArgs e)
        {
            if (GoodInputsOutputsWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshInputsOutputs(EditedGood.Good_Id);
                    GoodInputsOutputsWindow = new GoodInputsOutputs(AppType)
                    {
                        Data = EditedGood,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.InputsOutputs
                    };
                    GoodInputsOutputsWindow.Closed += GoodInputsOutputsWindow_Closed;
                    GoodInputsOutputsWindow.ShowDialog();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir la finestra d'Entrades/Sortides.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else GoodInputsOutputsWindow.Activate();
        }

        /// <summary>
        /// When the Good Inputs/Outputs Window is closed we actualize the GoodInputsOutputsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void GoodInputsOutputsWindow_Closed(object sender, EventArgs e)
        {
            GoodInputsOutputsWindow.Closed -= GoodInputsOutputsWindow_Closed;
            GoodInputsOutputsWindow = null;
        }

        #endregion

        #region AcumulatedAnnual [GoodRanges] (Query data)

        /// <summary>
        /// Show Acumulated annual information.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAcumulatedAnnual_Click(object sender, RoutedEventArgs e)
        {
            if (GoodRangesWindow == null)
            {
                try
                {
                    GoodRangesWindow = new GoodRanges(AppType)
                    {
                        Data = EditedGood
                    };
                    GoodRangesWindow.EvAcceptGoodRangeChanged += GoodRangesWindow_evAcceptGoodRangeChanged;
                    GoodRangesWindow.Closed += GoodRangesWindow_Closed;
                    GoodRangesWindow.ShowDialog();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir la finestra de Marges d'Artícle.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else GoodRangesWindow.Activate();
        }

        /// <summary>
        /// Manage the event Accept Good Range Changed.
        /// </summary>
        /// <param name="EditedRangesGood">Edited Good.</param>
        private void GoodRangesWindow_evAcceptGoodRangeChanged(GoodsView EditedRangesGood)
        {
            Good.Cumulative_Sales_Retail_Price_1 = EditedGood.Cumulative_Sales_Retail_Price_1 = EditedRangesGood.Cumulative_Sales_Retail_Price_1;
            Good.Cumulative_Sales_Retail_Price_2 = EditedGood.Cumulative_Sales_Retail_Price_2 = EditedRangesGood.Cumulative_Sales_Retail_Price_2;
            Good.Cumulative_Sales_Retail_Price_3 = EditedGood.Cumulative_Sales_Retail_Price_3 = EditedRangesGood.Cumulative_Sales_Retail_Price_3;
            Good.Cumulative_Sales_Retail_Price_4 = EditedGood.Cumulative_Sales_Retail_Price_4 = EditedRangesGood.Cumulative_Sales_Retail_Price_4;
            Good.Cumulative_Sales_Retail_Price_5 = EditedGood.Cumulative_Sales_Retail_Price_5 = EditedRangesGood.Cumulative_Sales_Retail_Price_5;
            Good.Cumulative_Sales_Retail_Price_6 = EditedGood.Cumulative_Sales_Retail_Price_6 = EditedRangesGood.Cumulative_Sales_Retail_Price_6;
            Good.Cumulative_Sales_Retail_Price_7 = EditedGood.Cumulative_Sales_Retail_Price_7 = EditedRangesGood.Cumulative_Sales_Retail_Price_7;
            Good.Cumulative_Sales_Retail_Price_8 = EditedGood.Cumulative_Sales_Retail_Price_8 = EditedRangesGood.Cumulative_Sales_Retail_Price_8;
            Good.Cumulative_Sales_Retail_Price_9 = EditedGood.Cumulative_Sales_Retail_Price_9 = EditedRangesGood.Cumulative_Sales_Retail_Price_9;
            Good.Cumulative_Sales_Retail_Price_10 = EditedGood.Cumulative_Sales_Retail_Price_10 = EditedRangesGood.Cumulative_Sales_Retail_Price_10;
            Good.Cumulative_Sales_Retail_Price_11 = EditedGood.Cumulative_Sales_Retail_Price_11 = EditedRangesGood.Cumulative_Sales_Retail_Price_11;
            Good.Cumulative_Sales_Retail_Price_12 = EditedGood.Cumulative_Sales_Retail_Price_12 = EditedRangesGood.Cumulative_Sales_Retail_Price_12;
            Good.Cumulative_Sales_Cost_1 = EditedGood.Cumulative_Sales_Cost_1 = EditedRangesGood.Cumulative_Sales_Cost_1;
            Good.Cumulative_Sales_Cost_2 = EditedGood.Cumulative_Sales_Cost_2 = EditedRangesGood.Cumulative_Sales_Cost_2;
            Good.Cumulative_Sales_Cost_3 = EditedGood.Cumulative_Sales_Cost_3 = EditedRangesGood.Cumulative_Sales_Cost_3;
            Good.Cumulative_Sales_Cost_4 = EditedGood.Cumulative_Sales_Cost_4 = EditedRangesGood.Cumulative_Sales_Cost_4;
            Good.Cumulative_Sales_Cost_5 = EditedGood.Cumulative_Sales_Cost_5 = EditedRangesGood.Cumulative_Sales_Cost_5;
            Good.Cumulative_Sales_Cost_6 = EditedGood.Cumulative_Sales_Cost_6 = EditedRangesGood.Cumulative_Sales_Cost_6;
            Good.Cumulative_Sales_Cost_7 = EditedGood.Cumulative_Sales_Cost_7 = EditedRangesGood.Cumulative_Sales_Cost_7;
            Good.Cumulative_Sales_Cost_8 = EditedGood.Cumulative_Sales_Cost_8 = EditedRangesGood.Cumulative_Sales_Cost_8;
            Good.Cumulative_Sales_Cost_9 = EditedGood.Cumulative_Sales_Cost_9 = EditedRangesGood.Cumulative_Sales_Cost_9;
            Good.Cumulative_Sales_Cost_10 = EditedGood.Cumulative_Sales_Cost_10 = EditedRangesGood.Cumulative_Sales_Cost_10;
            Good.Cumulative_Sales_Cost_11 = EditedGood.Cumulative_Sales_Cost_11 = EditedRangesGood.Cumulative_Sales_Cost_11;
            Good.Cumulative_Sales_Cost_12 = EditedGood.Cumulative_Sales_Cost_12 = EditedRangesGood.Cumulative_Sales_Cost_12;
        }

        /// <summary>
        /// When the Good Ranges Window is closed we actualize the GoodRangesWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void GoodRangesWindow_Closed(object sender, EventArgs e)
        {
            GoodRangesWindow.EvAcceptGoodRangeChanged -= GoodRangesWindow_evAcceptGoodRangeChanged;
            GoodRangesWindow.Closed -= GoodRangesWindow_Closed;
            GoodRangesWindow = null;
        }

        #endregion

        #region AcumulatedHistoric (Query data)

        /// <summary>
        /// Show Acumulated Historic information.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAcumulatedHistoric_Click(object sender, RoutedEventArgs e)
        {
            if (HistoGoodsWindow == null)
            {
                try
                {
                    HistoGoodsWindow = new HistoGoods(AppType)
                    {
                        Data = EditedGood,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoGoods(EditedGood.Good_Id)
                    };
                    HistoGoodsWindow.Closed += HistoGoodsWindow_Closed;
                    HistoGoodsWindow.ShowDialog();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir la finestra de Històric Acumulat d'Artícles.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else HistoGoodsWindow.Activate();
        }

        /// <summary>
        /// When the Histo Goods Window is closed we actualize the HistoGoodsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void HistoGoodsWindow_Closed(object sender, EventArgs e)
        {
            HistoGoodsWindow.Closed -= HistoGoodsWindow_Closed;
            HistoGoodsWindow = null;
        }

        #endregion

        #endregion

        #region ComboBox

        /// <summary>
        /// Manage the change of the Data in the combobox of Units.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbUnitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbUnitType.SelectedItem != null)
                {
                    UnitsView unitSelected = ((UnitsView)cbUnitType.SelectedValue);
                    EditedGood.Good_Unit = unitSelected;
                    AreDataChanged = (EditedGood != Good);
                }
            }
        }

        #endregion

        #region CheckBox

        private void ChkbCanceled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedGood.Canceled = false;
                AreDataChanged = (EditedGood != Good);
            }
        }

        private void ChkbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedGood.Canceled = true;
                AreDataChanged = (EditedGood != Good);
            }
        }

        private void ChkbEditGeneralTab_Unchecked(object sender, RoutedEventArgs e)
        {
            tbBillingUnitStocks.IsReadOnly = tbShippingUnitStocks.IsReadOnly = tbBillingUnitAvailable.IsReadOnly = true;
            tbShippingUnitAvailable.IsReadOnly = tbBillingUnitEntrance.IsReadOnly = tbShippingUnitEntrance.IsReadOnly = true;
            tbBillingUnitDepartures.IsReadOnly = tbShippingUnitDepartures.IsReadOnly = true;
            if (DataChangedManagerActive)
            {
                MsgManager.ShowMessage("Els valors de la pestanya General ja no es poden canviar.", MsgType.Notification);
            }
        }
        private void ChkbEditGeneralTab_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                string Question = "Està segur que vol editar els valors de la pestanya General? Un cop canviats els valors originals no es poden recalcular.";
                if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                {
                    tbBillingUnitStocks.IsReadOnly = tbShippingUnitStocks.IsReadOnly = tbBillingUnitAvailable.IsReadOnly = false;
                    tbShippingUnitAvailable.IsReadOnly = tbBillingUnitEntrance.IsReadOnly = tbShippingUnitEntrance.IsReadOnly = false;
                    tbBillingUnitDepartures.IsReadOnly = tbShippingUnitDepartures.IsReadOnly = false;
                    MsgManager.ShowMessage("Ara ja es poden canviar els valors de la pestanya General.", MsgType.Notification);
                }
                else
                {
                    MsgManager.ShowMessage("La operació ha estat cancel·lada per l'usuari.", MsgType.Information);
                }
            }
        }
        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Restore all values.
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedGood.RestoreSourceValues(Good);
            LoadDataInControls(EditedGood);
            AreDataChanged = (EditedGood != Good);
        }

        /// <summary>
        /// Restore the value of the indicated field.
        /// </summary>
        /// <param name="ErrorField">Field to restore value.</param>
        public void RestoreSourceValue(GoodsAttributes ErrorField)
        {
            EditedGood.RestoreSourceValue(Good, ErrorField);
            LoadDataInControls(EditedGood);
            AreDataChanged = (EditedGood != Good);
        }

        #endregion
    }
}
