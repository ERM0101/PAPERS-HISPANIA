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
    public partial class IVATypesData : UserControl
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
        private Brush brNormalCalendarForeColor = new SolidColorBrush(Color.FromArgb(255, 5, 86, 158));

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
        /// Types of IVA are correct.
        /// </summary>
        /// <param name="NewOrEditedIVATypesView">New or Edited City and Postal Code.</param>
        public delegate void dlgAccept(IVATypesView NewOrEditedIVATypesView);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Types of IVA are correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the IVA Types data to manage.
        /// </summary>
        private IVATypesView m_IVATypes = null;

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
        /// Get or Set the IVA Type data to manage.
        /// </summary>
        public IVATypesView IVATypeData
        {
            get
            {
                return (m_IVATypes);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_IVATypes = value;
                    EditedIVATypes = new IVATypesView(m_IVATypes);
                    LoadDataInControls(m_IVATypes);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades del Tipus d'IVA a carregar."); 
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
                         if (IVATypeData == null) throw new InvalidOperationException("Error, impossible visualitzar un Tipus d'IVA sense dades.");
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         IVATypeData = new IVATypesView();
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (IVATypeData == null) throw new InvalidOperationException("Error, impossible editar un Tipus d'IVA sense dades.");
                         tbCancel.Text = "Cancel·lar";
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
                    else if (control is Calendar)
                    {
                        control.IsHitTestVisible = (m_CtrlOperation != Operation.Show);
                        ((Calendar)control).BorderBrush = ((m_CtrlOperation == Operation.Show) ? BrAppColor : brNormalCalendarForeColor);
                    }
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
            }
        }

        /// <summary>
        /// Get or Set the Edited Type IVA information.
        /// </summary>
        private IVATypesView EditedIVATypes
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set if the data of the Type IVA has changed.
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
        public IVATypesData()
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
            NonEditableControls = new List<Control>();
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblIVATypesIVAPercent,
                tbIVATypesIVAPercent,
                cldrIVATypes_IVAInitValidityData,
                cldrIVATypes_IVAEndValidityData,
                lblIVATypesSurchargePercent,
                tbIVATypesSurchargePercent,
                lblIVATypesIVAType,
                tbIVATypesIVAType,
                gbIniIVA,
                gbEndIVA,
                btnClearEndDateIVA,
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
        private void LoadDataInControls(IVATypesView ivaTypesView)
        {
            DataChangedManagerActive = false;
            tbIVATypesIVAType.Text = ivaTypesView.Type;
            tbIVATypesIVAPercent.Text = GlobalViewModel.GetStringFromDecimalValue(ivaTypesView.IVAPercent, DecimalType.Percent);
            DateTime InitValidityPeriod = (DateTime)ivaTypesView.IVAInitValidityData;
            cldrIVATypes_IVAInitValidityData.SelectedDate = cldrIVATypes_IVAInitValidityData.DisplayDate = InitValidityPeriod;
            cldrIVATypes_IVAEndValidityData.SelectedDate = ivaTypesView.IVAEndValidityData;
            cldrIVATypes_IVAEndValidityData.DisplayDate = ivaTypesView.IVAEndValidityData ?? DateTime.Now;
            cldrIVATypes_IVAEndValidityData.DisplayDateStart = InitValidityPeriod;
            tbIVATypesSurchargePercent.Text = GlobalViewModel.GetStringFromDecimalValue(ivaTypesView.SurchargePercent, DecimalType.Percent);
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
            //  TextBox
                tbIVATypesIVAType.PreviewTextInput += TBPreviewTextInput;
                tbIVATypesIVAType.TextChanged += TBDataChanged;
                tbIVATypesIVAPercent.PreviewTextInput += TBPreviewTextInput;
                tbIVATypesIVAPercent.TextChanged += TBPrecentDataChanged;
                tbIVATypesSurchargePercent.PreviewTextInput += TBPreviewTextInput;
                tbIVATypesSurchargePercent.TextChanged += TBPrecentDataChanged;
            //  Calendar
                cldrIVATypes_IVAInitValidityData.SelectedDatesChanged += SelectedDateChanged;
                cldrIVATypes_IVAEndValidityData.SelectedDatesChanged += SelectedDateChanged;
            //  Buttons
                btnClearEndDateIVA.Click += BtnClearEndDateIVA_Click;
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
            if (sender == tbIVATypesIVAType) e.Handled = ! GlobalViewModel.IsValidIVATypeChar(e.Text);
            else if (sender == tbIVATypesIVAPercent) e.Handled = ! GlobalViewModel.IsValidPercentChar(e.Text, true);
            else if (sender == tbIVATypesSurchargePercent) e.Handled = ! GlobalViewModel.IsValidPercentChar(e.Text, true);
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
                    if (sender == tbIVATypesIVAType) EditedIVATypes.Type = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedIVATypes);
                }
                AreDataChanged = (EditedIVATypes != IVATypeData);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBPrecentDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Percent);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbIVATypesIVAPercent) EditedIVATypes.IVAPercent = value;
                    else if (sender == tbIVATypesSurchargePercent) EditedIVATypes.SurchargePercent = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedIVATypes);
                }
                AreDataChanged = (EditedIVATypes != IVATypeData);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button

        #region Accept

        /// <summary>
        /// Accept the edition or creatin of the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            IVATypesAttributes ErrorField = IVATypesAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedIVATypes.Validate(out ErrorField);
                    EvAccept?.Invoke(new IVATypesView(EditedIVATypes));
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al validar les dades introduïdes.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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

        #region Clear IVA End Controls

        /// <summary>
        /// Clear the end Date IVA value 
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnClearEndDateIVA_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                cldrIVATypes_IVAEndValidityData.DisplayDate = DateTime.Now;
                cldrIVATypes_IVAEndValidityData.DisplayDateStart = cldrIVATypes_IVAInitValidityData.SelectedDate;
                cldrIVATypes_IVAEndValidityData.InvalidateVisual();
                cldrIVATypes_IVAEndValidityData.SelectedDate = null;
                cldrIVATypes_IVAEndValidityData.InvalidateVisual();
                EditedIVATypes.IVAEndValidityData = null;
                AreDataChanged = (EditedIVATypes != IVATypeData);
                DataChangedManagerActive = true;
            }
        }

        private void ClearOperationsUI()
        {
        }

        #endregion

        #endregion

        #region Calendar

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (sender == cldrIVATypes_IVAInitValidityData)
                {
                    DateTime InitValidityPeriod = (DateTime)cldrIVATypes_IVAInitValidityData.SelectedDate;
                    DateTime? EndValidityPeriod = cldrIVATypes_IVAEndValidityData.SelectedDate;
                    if ((EndValidityPeriod != null) && (InitValidityPeriod > (DateTime)EndValidityPeriod))
                    {
                        MsgManager.ShowMessage("Error, la data d'inici del període de vigència de l'IVA no pot ser major que la de final.");
                        cldrIVATypes_IVAInitValidityData.SelectedDate = EditedIVATypes.IVAInitValidityData;
                    }
                    else
                    {
                        cldrIVATypes_IVAEndValidityData.DisplayDate = EndValidityPeriod ?? InitValidityPeriod; // DateTime.Now;
                        cldrIVATypes_IVAEndValidityData.DisplayDateStart = InitValidityPeriod;
                        EditedIVATypes.IVAInitValidityData = (DateTime)cldrIVATypes_IVAInitValidityData.SelectedDate;
                    }
                    cldrIVATypes_IVAInitValidityData.DisplayDate = EditedIVATypes.IVAInitValidityData;
                }
                else if (sender == cldrIVATypes_IVAEndValidityData) 
                {
                    EditedIVATypes.IVAEndValidityData = cldrIVATypes_IVAEndValidityData.SelectedDate;
                }
                AreDataChanged = (EditedIVATypes != IVATypeData);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedIVATypes.RestoreSourceValues(IVATypeData);
            LoadDataInControls(EditedIVATypes);
            AreDataChanged = (EditedIVATypes != IVATypeData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(IVATypesAttributes ErrorField)
        {
            EditedIVATypes.RestoreSourceValue(IVATypeData, ErrorField);
            LoadDataInControls(EditedIVATypes);
            AreDataChanged = (EditedIVATypes != IVATypeData);
        }

        #endregion
    }
}
