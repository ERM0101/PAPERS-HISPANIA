#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.Managers;
using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindowOp1.xaml
    /// </summary>
    public partial class PriceRanges : Window
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
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewItemPannel = new GridLength(150);

        /// <summary>
        /// Show the Serach Pannel.
        /// </summary>
        private GridLength ViewSearchPannel = new GridLength(30.0);

        /// <summary>
        /// Show the Operation Pannel.
        /// </summary>
        private GridLength ViewOperationPannel = new GridLength(30.0);

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<PriceRangesView> m_DataList = new ObservableCollection<PriceRangesView>();

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private GoodsView m_Data = null;

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
            get;
            private set;
        }

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        public ObservableCollection<PriceRangesView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (value != null)
                {
                    m_DataList = value;
                    ListItems.ItemsSource = m_DataList;
                    ListItems.DataContext = this;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                }
                else m_DataList = new ObservableCollection<PriceRangesView>();
                if (m_DataList.Count > 0) rdSearchPannel.Height = ViewSearchPannel;
                else rdSearchPannel.Height = HideComponent;
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
                    PriceRangesDataControl.Data = value;
                }
                else throw new ArgumentNullException("Error, no s'ha trobat l'Article a carregar.");
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

        #region Builders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public PriceRanges(ApplicationType AppType)
        {
            InitializeComponent();
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
        private void ActualizeWindowComponents(ApplicationType AppType)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
                PriceRangesDataControl.AppType = AppType;
            //  Initialize state of Window components.
                rdItemPannel.Height = HideComponent;
                rdSearchPannel.Height = HideComponent;
                gsSplitter.IsEnabled = false;
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Hidden;
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
                cbFieldItemToSearch.ItemsSource = PriceRangesView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (PriceRangesView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
            //  Deactivate managers
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
                if ((cbFieldItemToSearch.SelectedIndex == -1) || (String.IsNullOrEmpty(tbItemToSearch.Text))) return (true);
            //  Get Acces to the object and the property name To Filter.
                PriceRangesView ItemData = (PriceRangesView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter
                object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                if (valueToTest == null) return (false);
                else return ((valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper()));
        }
        
        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window
                Closed += PriceRanges_Closed;
                Loaded += PriceRanges_Loaded;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button Search Clients
                btnAdd.Click += BtnAdd_Click;
                btnEdit.Click += BtnEdit_Click;
                btnDelete.Click += BtnDelete_Click;
                btnViewData.Click += BtnViewData_Click;
                btnRefresh.Click += BtnRefresh_Click;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;   
            //  Define CustomerDataControl events to manage.
                PriceRangesDataControl.EvAccept += PriceRangesDataControl_evAccept;
                PriceRangesDataControl.EvCancel += PriceRangesDataControl_evCancel;
        }

        #region Filter

        /// <summary>
        /// Manage the search of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TbItemToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
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
									
        #region Refresh

        /// <summary>
        /// Manage the button for Refresh.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActualizePriceRangesFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Marges dels Preus.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Editing Price Ranges

        /// <summary>
        /// Manage the Button Mouse Click in one of the items of the List to show its data in User Control.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnViewData_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizePriceRangeFromDb();
                    PriceRangesDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'intentar veure les dades del Marge seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the button for add Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            PriceRangesDataControl.CtrlOperation = Operation.Add;
            gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
            ShowItemPannel();
        }

        /// <summary>
        /// Manage the button for edit an Item of the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizePriceRangeFromDb();
                    if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                    {
                        PriceRangesDataControl.CtrlOperation = Operation.Edit;
                        gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                        ShowItemPannel();
                    }
                    else MsgManager.ShowMessage(ErrMsg);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'iniciar l'edició del Marge seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the button for edit an Item of the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                PriceRangesView PriceRangesToDelete = (PriceRangesView)ListItems.SelectedItem;
                string Question = string.Format("Està segur que vol esborrar el Marge de Preus '{0}' ?", PriceRangesToDelete.Sequence);
                if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                {
                    string ErrMsg;
                    try
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(PriceRangesToDelete, out ErrMsg))
                        {
                            GlobalViewModel.Instance.HispaniaViewModel.DeletePriceRange(DataList[DataList.IndexOf(PriceRangesToDelete)]);
                            GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(PriceRangesToDelete, out ErrMsg);
                            DataChangedManagerActive = false;
                            if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                            DataList = GlobalViewModel.Instance.HispaniaViewModel.GetPriceRanges(Data.Good_Id);
                            DataChangedManagerActive = true;
                            ListItems.UpdateLayout();
                            HideItemPannel();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrMsg = string.Format("Error, al esborrar esborrar el Marge de Preus '{0}'.\r\nDetalls: {1}", 
                                               PriceRangesToDelete.Sequence, MsgManager.ExcepMsg(ex));
                    }
                    if (!string.IsNullOrEmpty(ErrMsg)) MsgManager.ShowMessage(ErrMsg);
                }
                else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the IVATypesDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void PriceRangesDataControl_evAccept(PriceRangesView NewOrEditedPriceRanges)
        {
            try
            {
                PriceRangesView NewPriceRange = new PriceRangesView(NewOrEditedPriceRanges);
                switch (PriceRangesDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         ValidatePriceRangeInList(NewPriceRange);
                         GlobalViewModel.Instance.HispaniaViewModel.CreatePriceRange(NewPriceRange);
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.GetPriceRanges(Data.Good_Id);
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewPriceRange;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                    case Operation.Edit:
                         ValidatePriceRangeInList(NewPriceRange);
                         GlobalViewModel.Instance.HispaniaViewModel.UpdatePriceRange(NewPriceRange);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(PriceRangesDataControl.PriceRangeData, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.GetPriceRanges(Data.Good_Id);
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewPriceRange;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                string OperationInfo = ". Operació no reconeguda.";
                switch (PriceRangesDataControl.CtrlOperation)
                {
                    case Operation.Add:
                        OperationInfo = " que s'intenta afegir.";
                        break;
                    case Operation.Edit:
                        OperationInfo = " que s'està editant.";
                        break;
                }
                MsgManager.ShowMessage(string.Format("Error, al guardar les dades del Marge{0}\r\nDetalls: {1}", OperationInfo, MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the IVATypesDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void PriceRangesDataControl_evCancel()
        {
            switch (PriceRangesDataControl.CtrlOperation)
            {
                case Operation.Add:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     break;
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(PriceRangesDataControl.PriceRangeData, out string ErrMsg))
                     {
                         MsgManager.ShowMessage(ErrMsg);
                     }
                     break;
                case Operation.Show:
                     break;
                default:
                     break;
            }
            HideItemPannel();
        }

        #endregion

        #endregion

        #region List View

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
                ActualizeButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Window

        /// <summary>
        /// Manage the window load and free the register lock.
        /// </summary>
        /// <param name="sender">Object that throw the event</param>
        /// <param name="e">Parameters which the event has been throwed</param>
        private void PriceRanges_Loaded(object sender, RoutedEventArgs e)
        {
            tbItemToSearch.Focus();
        }

        /// <summary>
        /// Manage the window close and free the register lock.
        /// </summary>
        /// <param name="sender">Object that throw the event</param>
        /// <param name="e">Parameters which the event has been throwed</param>
        private void PriceRanges_Closed(object sender, EventArgs e)
        {
            if (PriceRangesDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(PriceRangesDataControl.PriceRangeData, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion

        #region Validation

        /// <summary>
        /// Method verifies that the New Item or the Edited Item not exist yet.
        /// </summary>
        /// <param name="NewPriceRange">Item to validate.</param>
        private void ValidatePriceRangeInList(PriceRangesView NewPriceRange)
        {
            string ErrMsg = "El Marge de Preus '{0}' ja està definit.";
            switch (PriceRangesDataControl.CtrlOperation)
            {
                case Operation.Add:
                     foreach (PriceRangesView PriceRange in DataList)
                     {
                         if ((PriceRange.Sequence == NewPriceRange.Sequence) &&
                             (PriceRange.Good_Code == NewPriceRange.Good_Code))
                         {
                            throw new Exception(
                                          Manager_IU.GetText("ErrorPriceRanges_01", string.Format(ErrMsg, PriceRange.Sequence)));
                         }
                     }
                     break;
                case Operation.Edit:
                     foreach (PriceRangesView PriceRange in DataList)
                     {
                         if (((PriceRange.Sequence == NewPriceRange.Sequence) && 
                              (PriceRange.PriceRange_Id != NewPriceRange.PriceRange_Id)) &&
                              (PriceRange.Good_Code == NewPriceRange.Good_Code))
                         {
                             throw new Exception(
                                           Manager_IU.GetText("ErrorPriceRanges_01", string.Format(ErrMsg, PriceRange.Sequence)));
                         }
                     }
                     break;
            }
        }

        #endregion
                
        #region Database Operations
		        
        private void ActualizePriceRangesFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                DataList = GlobalViewModel.Instance.HispaniaViewModel.GetPriceRanges(Data.Good_Id);
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizePriceRangeFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                PriceRangesView SelectedItem = (PriceRangesView)ListItems.SelectedItem;
                PriceRangesView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetPriceRangeFromDb(SelectedItem);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                PriceRangesDataControl.PriceRangeData = ItemInDb;
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update UI

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void ShowItemPannel()
        {
            gsSplitter.IsEnabled = true;
            rdItemPannel.Height = ViewItemPannel;
            rdOperationPannel.Height = HideComponent;
            btnAdd.Visibility = btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Hidden;
            GridList.IsEnabled = false;
        }

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void HideItemPannel()
        {
            rdItemPannel.Height = HideComponent;
            rdOperationPannel.Height = ViewOperationPannel;
            gsSplitter.IsEnabled = false;
            btnAdd.Visibility = Visibility.Visible;
            if (DataList.Count > 0)
            {
                rdSearchPannel.Height = ViewSearchPannel;
                ActualizeButtonBar();
            }
            else rdSearchPannel.Height = HideComponent;
            GridList.IsEnabled = true;
        }

        /// <summary>
        /// Method that actualize the Button Bar look
        /// </summary>
        private void ActualizeButtonBar()
        {
            if (ListItems.SelectedItem != null)
            {
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Visible;
            }
            else btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
