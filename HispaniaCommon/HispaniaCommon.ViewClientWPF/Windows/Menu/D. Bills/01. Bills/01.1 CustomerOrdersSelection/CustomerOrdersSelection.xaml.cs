#region Librerias usadas por la ventana

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    #region Enumerations

    /// <summary>
    /// Define the Operation for this window.
    /// </summary>
    public enum CustomerOrderSelectionOperation
    {
        Add,

        Remove,
    }

    #endregion

    /// <summary>
    /// Interaction logic for CustomerOrdersSelection.xaml
    /// </summary>
    public partial class CustomerOrdersSelection : Window
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
        private GridLength ViewMiddleColumn = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Serach Pannel.
        /// </summary>
        private GridLength ViewSearchPannel = new GridLength(30.0);

        /// <summary>
        /// Hide the button bar.
        /// </summary>
        private GridLength HideAcceptButton = new GridLength(0.0);

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private BillsView m_Bill = new BillsView();

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<CustomerOrdersView> m_DataList = new ObservableCollection<CustomerOrdersView>();

        /// <summary>
        /// Store if the data of the Good has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store if the data of the Customer Orders Selected has changed.
        /// </summary>
        private List<CustomerOrdersView> m_CustomerOrdersSelecteds;

        #region GUI

        /// <summary>
        /// Store the background color for the search text box.
        /// </summary>
        private Brush m_AppColor = null;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the the result of the execution of this Window.
        /// </summary>
        public SelectionResult Result
        {
            get;
            set;
        }

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        public ApplicationType AppType
        {
            get;
            private set;
        }

        /// <summary>
        /// Store the Operation Type.
        /// </summary>
        public CustomerOrderSelectionOperation Operation
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<CustomerOrdersView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                //  Actualize the value
                    if (value != null) m_DataList = value;
                    else m_DataList = new ObservableCollection<CustomerOrdersView>();
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    ListItems.ItemsSource = m_DataList;
                    ListItems.UnselectAll();
                    ListItems.DataContext = this;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("CustomerOrder_Id", ListSortDirection.Descending));
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                    AreDataChanged = false;
                //  Reactivate managers
                    DataChangedManagerActive = true;
            }
        }

        public BillsView Bill
        {
            get
            {
                return m_Bill;
            }
            set
            {
                //  Actualize the value
                    if (value != null) m_Bill = value;
                    else m_Bill = new BillsView();
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    tbBillId.Text = m_Bill.Bill_Id_Str;
                    tbBillDate.Text = m_Bill.Bill_Date_Str;
                    tbBillSerieId.Text = m_Bill.BillingSerie_Str;
                //  Reactivate managers
                    DataChangedManagerActive = true;
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
        /// Get or Set Customer Orders selecteds
        /// </summary>
        public List<CustomerOrdersView> CustomerOrdersSelecteds
        {
            get
            {
                if (m_CustomerOrdersSelecteds is null) return (new List<CustomerOrdersView>());
                else return m_CustomerOrdersSelecteds;
            }
            private set
            {
                m_CustomerOrdersSelecteds = value;
            }
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
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        /// <param name="OperationIn">Operation that execute</param>
        public CustomerOrdersSelection(ApplicationType AppType, CustomerOrderSelectionOperation OperationIn)
        {
            InitializeComponent();
            Initialize(AppType, OperationIn);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        /// <param name="OperationIn">Operation that execute</param>
        private void Initialize(ApplicationType AppType, CustomerOrderSelectionOperation OperationIn)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType, OperationIn);
            //  Load Data in Window components.
                LoadDataInWindowComponents();
            //  Load the managers of the controls of the Window.
                LoadManagers();
        }

        #endregion

        #region Standings

        /// <summary>
        /// Method that actualize the Window components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        /// <param name="OperationIn">Operation that execute</param>
        private void ActualizeWindowComponents(ApplicationType AppType, CustomerOrderSelectionOperation OperationIn)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
                this.Operation = OperationIn;
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
            //  Initialize state of Window components.
                rdSearchPannel.Height = HideComponent;
        }

        /// <summary>
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set Data into the Window.
                ListItems.ItemsSource = m_DataList;
                DataContext = DataList;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                cbFieldItemToSearch.ItemsSource = CustomerOrdersView.DeliveryNoteFields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (CustomerOrdersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
            //  Activate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilter(object item)
        {
            //  Determine if is needed aplicate one filter.
                if (cbFieldItemToSearch.SelectedIndex == -1)  return (true);
            //  Get Acces to the object and the property name To Filter.
                CustomerOrdersView ItemData = (CustomerOrdersView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                {
                    object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                    if ((valueToTest is null) || 
                        (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper())))
                    {
                        return false;
                    }
                }
                return ((Operation == CustomerOrderSelectionOperation.Add && ItemData.According) || 
                        (Operation == CustomerOrderSelectionOperation.Remove));
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button Search Clients
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;   
        }

        #region Filter

        /// <summary>
        /// Manage the search of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TbItemToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                DataChangedManagerActive = true;
            }
        }

        #endregion

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
            try
            {
                CustomerOrdersSelecteds = new List<CustomerOrdersView>(ListItems.SelectedItems.Count);
                foreach (CustomerOrdersView customerOrder in ListItems.SelectedItems)
                {
                    CustomerOrdersSelecteds.Add(customerOrder);
                }
                Result = SelectionResult.Accept;
                Close();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al seleccionar l'Albarà.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
            MsgManager.ShowMessage("Avis, selecció cancel·lada per l'usuari.", MsgType.Information);
            Result = SelectionResult.Cancel;
            Close();
        }

        #endregion
        
        #endregion

        #region ListItems

        /// <summary>
        /// Manage the event produced when change the selection of the items list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                AreDataChanged = (ListItems.SelectedItems.Count > 0);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion
    }
}
