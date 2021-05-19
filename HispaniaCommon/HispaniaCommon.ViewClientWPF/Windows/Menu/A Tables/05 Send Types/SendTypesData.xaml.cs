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
    public partial class SendTypesData : UserControl
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
        /// Types of Effect are correct.
        /// </summary>
        /// <param name="NewOrEditedSendTypesView">New or Edited Effect.</param>
        public delegate void dlgAccept(SendTypesView NewOrEditedSendTypesView);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Type of Send are correct.
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
        private SendTypesView m_SendTypes = null;

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
        public SendTypesView SendTypeData
        {
            get
            {
                return (m_SendTypes);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_SendTypes = value;
                    EditedSendTypes = new SendTypesView(m_SendTypes);
                    LoadDataInControls(m_SendTypes);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades del Tipus d'Enviament a carregar."); 
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
                         if (SendTypeData == null) throw new InvalidOperationException("Error, impossible visualitzar un Tipus d'Enviament sense dades.");
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         SendTypeData = new SendTypesView();
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (SendTypeData == null) throw new InvalidOperationException("Error, impossible editar un Tipus d'Enviament sense dades.");
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
            }
        }

        /// <summary>
        /// Get or Set the Edited Type IVA information.
        /// </summary>
        private SendTypesView EditedSendTypes
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
        public SendTypesData()
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
                lblSendTypesCode,
                tbSendTypesCode,
                lblSendTypesDescription,
                tbSendTypesDescription
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
        private void LoadDataInControls(SendTypesView sendTypesView)
        {
            DataChangedManagerActive = false;
            tbSendTypesCode.Text = sendTypesView.Code;
            tbSendTypesDescription.Text = sendTypesView.Description;
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
                tbSendTypesCode.PreviewTextInput += TBPreviewTextInput;
                tbSendTypesCode.TextChanged += TBDataChanged;
                tbSendTypesDescription.PreviewTextInput += TBPreviewTextInput;
                tbSendTypesDescription.TextChanged += TBDataChanged;
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
            if (sender == tbSendTypesCode) e.Handled = ! GlobalViewModel.IsValidSendTypeChar(e.Text);
            else if (sender == tbSendTypesDescription) e.Handled = ! GlobalViewModel.IsValidDescriptionChar(e.Text);
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
                    if (sender == tbSendTypesCode) EditedSendTypes.Code = value;
                    else if (sender == tbSendTypesDescription) EditedSendTypes.Description = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedSendTypes);
                }
                AreDataChanged = (EditedSendTypes != SendTypeData);
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
            SendTypesAttributes ErrorField = SendTypesAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedSendTypes.Validate(out ErrorField);
                    EvAccept?.Invoke(new SendTypesView(EditedSendTypes));
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

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValues()
        {
            EditedSendTypes.RestoreSourceValues(SendTypeData);
            LoadDataInControls(EditedSendTypes);
            AreDataChanged = (EditedSendTypes != SendTypeData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(SendTypesAttributes ErrorField)
        {
            EditedSendTypes.RestoreSourceValue(SendTypeData, ErrorField);
            LoadDataInControls(EditedSendTypes);
            AreDataChanged = (EditedSendTypes != SendTypeData);
        }

        #endregion
    }
}
