#region Libraries used for this control

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
    /// Interaction logic for BillingSeries.xaml
    /// </summary>
    public partial class StocksData : UserControl
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
        private GridLength ViewAcceptButton = new GridLength(120.0);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength ViewMiddleColumn = new GridLength(2.0, GridUnitType.Star);

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
        /// Stock are correct.
        /// </summary>
        /// <param name="NewOrEditedStocksView">New or Edited Stock.</param>
        public delegate void dlgAccept(StocksView EditedStocksView);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Type of Effect are correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Effect Types data to manage.
        /// </summary>
        private StocksView m_Stocks = null;

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Store the Operation of the UserControl.
        /// </summary>
        private Operation m_CtrlOperation = Operation.Show;
        
        /// <summary>
        /// Stotre if the data of the Customer has changed.
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

        #region GUI

        /// <summary>
        /// Store the background color for the search text box.
        /// </summary>
        private Brush m_AppColor = null;

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
        /// Get or Set the Effect Type data to manage.
        /// </summary>
        public StocksView StockData
        {
            get
            {
                return (m_Stocks);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Stocks = value;
                    EditedStocks = new StocksView(m_Stocks);
                    LoadDataInControls(m_Stocks);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades de l'Estoc a carregar."); 
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
                         if (StockData == null) throw new InvalidOperationException("Error, impossible visualitzar un Estoc sense dades.");
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Edit:
                         if (StockData == null) throw new InvalidOperationException("Error, impossible editar un Estoc sense dades.");
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
                tbInitial.Focus();
            }
        }

        /// <summary>
        /// Get or Set the Edited Type IVA information.
        /// </summary>
        private StocksView EditedStocks
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set if the data of the Stock has changed.
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
        /// Get or Set if the manager of the data change for the Type IVA has active.
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

        #region Builders

        /// <summary>
        /// Builder by default of this control.
        /// </summary>
        public StocksData()
        {
            //  Initialization of controls of the UserControl
                InitializeComponent();
            //  Initialize GUI.
                InitEditableControls();
                InitNonEditableControls();
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
                lblStocksCode,
                tbStocksCode,
                lblStocksDescription,
                tbStocksDescription
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblInitial,
                tbInitial,
                lblInitial_Fact,
                tbInitial_Fact
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
        private void LoadDataInControls(StocksView StocksView)
        {
            DataChangedManagerActive = false;
            tbStocksCode.Text = StocksView.Good_Code;
            tbStocksDescription.Text = StocksView.Good_Description;
            tbInitial.Text = GlobalViewModel.GetStringFromDecimalValue(StocksView.Initial, DecimalType.Stock);
            tbInitial_Fact.Text = GlobalViewModel.GetStringFromDecimalValue(StocksView.Initial_Fact, DecimalType.Stock);
            DataChangedManagerActive = true;
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the UserControl
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Customer Data changes is active.
                DataChangedManagerActive = true;
            //  Define controls managers
                tbInitial.PreviewTextInput += TBPreviewTextInput;
                tbInitial.TextChanged += TBStockDataChanged;
                tbInitial_Fact.PreviewTextInput += TBPreviewTextInput;
                tbInitial_Fact.TextChanged += TBStockDataChanged;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
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
            if ((sender == tbInitial) || (sender == tbInitial_Fact))
            {
                e.Handled = !GlobalViewModel.IsValidStockChar(e.Text);
            }
        }
        
        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBStockDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Stock);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbInitial) EditedStocks.Initial = value;
                    else if (sender == tbInitial_Fact) EditedStocks.Initial_Fact = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedStocks);
                }
                AreDataChanged = (EditedStocks != StockData);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Buttton

        #region Accept

        /// Accept the edition or creatin of the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            StocksAttributes ErrorField = StocksAttributes.None;
            try
            {
                if (CtrlOperation == Operation.Edit)
                {
                    EditedStocks.Validate(out ErrorField);
                    EvAccept?.Invoke(new StocksView(EditedStocks));
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
        /// Cancel the edition or creatin of the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //  Send the event that indicates at the observer that the operation is cancelled.
                EvCancel?.Invoke();
        }

        #endregion

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedStocks.RestoreSourceValues(StockData);
            LoadDataInControls(EditedStocks);
            AreDataChanged = (EditedStocks != StockData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(StocksAttributes ErrorField)
        {
            EditedStocks.RestoreSourceValue(StockData, ErrorField);
            LoadDataInControls(EditedStocks);
            AreDataChanged = (EditedStocks != StockData);
        }

        #endregion
    }
}
