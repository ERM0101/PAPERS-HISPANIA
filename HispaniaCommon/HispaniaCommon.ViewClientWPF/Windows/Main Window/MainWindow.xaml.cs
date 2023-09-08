#region Libraries used for the class

using HispaniaCommon.ViewClientWPF.Windows.Menu.I._Queries;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        #region  GUI

        /// <summary>
        /// Background of the Active Item
        /// </summary>
        private Brush m_brBackActive = null;

        /// <summary>
        /// Background of the Active Item
        /// </summary>
        private Brush m_brBackNotImplementedActive = null;

        /// <summary>
        /// Foreground of the Active Item
        /// </summary>
        private Brush m_brForeActive = null;

        /// <summary>
        /// Store the Style for the Activate Labels.
        /// </summary>
        private Style m_LabelStyleActive = null;

        /// <summary>
        /// Store the Style for the Activate Labels.
        /// </summary>
        private Style m_LabelStyleNotImplementedActive = null;

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideHeaderPanel = new GridLength(40.0);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength ViewTancamentPanel = new GridLength(2.0, GridUnitType.Star);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength NormalEspaiPanel = new GridLength(1.25, GridUnitType.Star);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength ExpandEspaiPanel = new GridLength(3.2, GridUnitType.Star);

        /// <summary>
        /// Defines Minimum value for  ClosePannel.
        /// </summary>
        private GridLength ViewTancamentPanelMinHeight = new GridLength(430.0);

        #endregion

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Active windows in each moment.
        /// </summary>
        private List<Window> m_Active_Windows = new List<Window>();

        #region Windows

        #region MENU Tables

        /// <summary>
        /// Store the Parameters Window Type Active.
        /// </summary>
        private Parameters ParametersWindow = null;

        /// <summary>
        /// Store the BillingSeries Window Type Active.
        /// </summary>
        private BillingSeries BillingSeriesWindow = null;

        /// <summary>
        /// Store the CitiesCP Window Type Active.
        /// </summary>
        private PostalCodes PostalCodesWindow = null;
        
        /// <summary>
        /// Store the IVATypes Window Type Active.
        /// </summary>
        private IVATypes IVATypesWindow = null;

        /// <summary>
        /// Store the SendTypes Window Type Active.
        /// </summary>
        private SendTypes SendTypesWindow = null;

        /// <summary>
        /// Store the Agents Window Type Active.
        /// </summary>
        private Agents AgentsWindow = null;

        /// <summary>
        /// Store the EffectTypes Window Type Active.
        /// </summary>
        private EffectTypes EffectTypesWindow = null;

        /// <summary>
        /// Store the Customers Window Type Active.
        /// </summary>
        private Customers CustomersWindow = null;

        /// <summary>
        /// Store the Customers Window Type Active for Print.
        /// </summary>
        private Customers CustomersPrintWindow = null;

        /// <summary>
        /// Store the Providers Window Type Active.
        /// </summary>
        private Providers ProvidersWindow = null;

        /// <summary>
        /// Store the Providers Window Type Active for Print.
        /// </summary>
        private Providers ProvidersPrintWindow = null;

        #endregion

        #region MENU Goods

        /// <summary>
        /// Store the Units Window Type Active.
        /// </summary>
        private Units UnitsWindow = null;

        /// <summary>
        /// Store the Goods Window Type Active.
        /// </summary>
        private Goods GoodsWindow = null;

        /// <summary>
        /// Store the Stocks Window Type Active.
        /// </summary>
        private Stocks StocksWindow = null;

        #endregion

        #region MENU Orders ToDo : [PROVEÏDORS] ManageGestioComandesProveidors i ManageCompararComandes

        /// <summary>
        /// Store the Manger for Customer Orders Window Type Active.
        /// </summary>
        private CustomerOrders CustomerOrdersMangementWindow = null;

        /// <summary>
        /// Store the Manager for Delivery Notes Window Type Active.
        /// </summary>
        private CustomerOrders DeliveryNotesManagementWindow = null;

        /// <summary>
        /// Store the Customer Orders Window for Print Type Active.
        /// </summary>
        private CustomerOrders DeliveryNotesPrintWindow = null;

        /// <summary>
        /// Store the Issuance Supplier Orders Window Type Active.
        /// </summary>
        private ProviderOrders ProviderOrdersManagementWindow = null;

        /// <summary>
        /// Store the Issuance Supplier Orders Window Type Active.
        /// </summary>
        //private CompareCustomerOrders CompareCustomerOrdersWindow = null;

        #endregion

        #region MENU Bills

        /// <summary>
        /// Store the Bills Window Type Active.
        /// </summary>
        private Bills BillsWindow = null;

        /// <summary>
        /// Store the Bills Window Type Active.
        /// </summary>
        private Bills BillsPrintWindow = null;

        /// <summary>
        /// Store the Mismatches Window Type Active.
        /// </summary>
        private MismatchesReceipts MismatchesReceiptsWindow = null;

        /// <summary>
        /// Store the Settlement Window Type Active.
        /// </summary>
        private Settlements SettlementsWindow = null;

        /// <summary>
        /// Window instance of BadDebt.
        /// </summary>
        private BadDebts BadDebtsWindow = null;

        #endregion

        #region MENU Queries

        /// <summary>
        /// Store the Queries Window Type Active.
        /// </summary>
        private Queries QueriesWindow = null;

        #endregion

        #region MENU Warehouse

        /// <summary>
        /// Store the Warehouse Movements Window for add movements.
        /// </summary>
        private WarehouseMovementsAdd WarehouseMovementsAddWindow = null;

        /// <summary>
        /// Store the Warehouse Movements Window for manage movements.
        /// </summary>
        private WarehouseMovements WarehouseMovementsWindow = null;

        #endregion

        #region MENU Revisions

        /// <summary>
        /// Store the Good Revisions Window Type Active.
        /// </summary>
        private GoodRevisions GoodRevisionsWindow = null;

        /// <summary>
        /// Store the Mismatches Available Window Type Active.
        /// </summary>
        private Revisions MismatchesAvailableWindow = null;

        /// <summary>
        /// Store the Mismatches Stocs Window Type Active.
        /// </summary>
        private Revisions MismatchesStocksWindow = null;

        /// <summary>
        /// Store the General Available Window Type Active.
        /// </summary>
        private Revisions GeneralAvailableWindow = null;

        /// <summary>
        /// Store the General Stocs Window Type Active.
        /// </summary>
        private Revisions GeneralStocksWindow = null;

        #endregion

        #region MENU Listings

        /// <summary>
        /// Store the Stock Takings List Window Type Active.
        /// </summary>
        private StockTakingsList StockTakingsListWindow = null;

        /// <summary>
        /// Store the Ranges List Window Type Active.
        /// </summary>
        private RangesList RangesListWindow = null;

        /// <summary>
        /// Window instance of Bad Debts List.
        /// </summary>
        private BadDebts BadDebtsListWindow = null;

        /// <summary>
        /// Store the Sales Summarys List Window Type Active.
        /// </summary>
        private DiaryBandagesList DiaryBandagesListWindow = null;

        /// <summary>
        /// Store the Sales Summarys and Accounting List Window Type Active.
        /// </summary>
        private DiaryBandagesAndAccountingsList DiaryBandagesAndAccountingsListWindow = null;

        #endregion

        #region MENU Closing

        /// <summary>
        /// Store the Customers Sales List Window Type Active.
        /// </summary>
        private CustomersSalesList CustomersSalesListWindow = null;

        /// <summary>
        /// Store the Historic Acum Client Window Type Active.
        /// </summary>
        private HistoricAcumCustomers HistoricAcumCustomersWindow = null;

        /// <summary>
        /// Store the Historic Acum Good Window Type Active.
        /// </summary>
        private HistoricAcumGoods HistoricAcumGoodsWindow = null;

        /// <summary>
        /// Store the Remove Warehouse Movements Window Type Active.
        /// </summary>
        private RemoveWarehouseMovements RemoveWarehouseMovementsWindow = null;

        /// <summary>
        /// Store the Reset Bill Number Window Type Active.
        /// </summary>
        private ResetBillNumbers ResetBillNumbersWindow = null;

        /// <summary>
        /// Store the Transfer Initial Stocks Window Type Active.
        /// </summary>
        private TransferInitialStocks TransferInitialStocksWindow = null;

        /// <summary>
        /// Store the Transfer for Customer Orders Window Type Active.
        /// </summary>
        private CustomerOrders CustomerOrdersTransferWindow = null;

        /// <summary>
        /// Store the Transfer for Delivery Notes Window Type Active.
        /// </summary>
        private CustomerOrders DeliveryNotesTransferWindow = null;

        #endregion

        #region GUI

        /// <summary>
        /// Store the original Style of the Active Item
        /// </summary>
        private Style ItemActiveOriginalStyle;

        /// <summary>
        /// Store the original Foreground of the Active Item
        /// </summary>
        private Brush ItemActiveOriginalForeground;

        /// <summary>
        /// Store the original Background of the Active Item
        /// </summary>
        private Brush m_brBackActiveItemSelected = null;

        /// <summary>
        /// Store the original Background of the Not Implemented Item
        /// </summary>
        private Brush m_brBackNotImplementedItemSelected = null;

        /// <summary>
        /// Store the original HorizontalContentAlignment of the Active Item
        /// </summary>
        private HorizontalAlignment ItemActiveOriginalHorizontalContentAlignment;

        #endregion

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Store the Active windows in each moment.
        /// </summary>
        private List<Window> Active_Windows
        {
            get
            {
                return m_Active_Windows;
            }
            set
            {
                m_Active_Windows = value;
            }
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

        #region GUI
        
        /// <summary>
        /// Background of the Active Item
        /// </summary>
        private Brush BrBackActive
        {
            get
            {
                if (m_brBackActive == null)
                {
                    if (AppType == ApplicationType.Comptabilitat)
                    {
                        m_brBackActive = new RadialGradientBrush(Color.FromArgb(255, 126, 185, 237), Color.FromArgb(255, 8, 70, 124));
                    }
                    else if (AppType == ApplicationType.Help)
                    {
                        m_brBackActive = new RadialGradientBrush(Color.FromArgb(255, 200, 81, 77), Color.FromArgb(255, 135, 47, 46));
                    }
                }
                return (m_brBackActive);
            }
        }

        /// <summary>
        /// Background of the Active Item
        /// </summary>
        private Brush BrBackNotImplementedActive
        {
            get
            {
                if (m_brBackNotImplementedActive == null)
                {
                    m_brBackNotImplementedActive = new RadialGradientBrush(Color.FromArgb(255, 200, 81, 77), Color.FromArgb(255, 115, 27, 26));
                }
                return (m_brBackNotImplementedActive);
            }
        }

        /// <summary>
        /// Background of the Active Item
        /// </summary>
        private Brush BrBackActiveItemSelected
        {
            get
            {
                if (m_brBackActiveItemSelected == null)
                {
                    if (AppType == ApplicationType.Comptabilitat)
                    {
                        m_brBackActiveItemSelected = new RadialGradientBrush(Color.FromArgb(255, 126, 185, 237), Color.FromArgb(255, 0, 50, 104));
                    }
                    else if (AppType == ApplicationType.Help)
                    {
                        m_brBackActiveItemSelected = new RadialGradientBrush(Color.FromArgb(255, 200, 81, 77), Color.FromArgb(255, 115, 27, 26));
                    }
                }
                return (m_brBackActiveItemSelected);
            }
        }

        /// <summary>
        /// Background of the Active Item
        /// </summary>
        private Brush BrBackNotImplementedItemSelected
        {
            get
            {
                if (m_brBackNotImplementedItemSelected == null)
                {
                    m_brBackNotImplementedItemSelected = new RadialGradientBrush(Color.FromArgb(255, 200, 81, 77), Color.FromArgb(255, 115, 27, 26));
                }
                return (m_brBackNotImplementedItemSelected);
            }
        }

        /// <summary>
        /// Foreground of the Active Item
        /// </summary>
        private Brush BrForeActive
        {
            get
            {
                if (m_brForeActive == null) m_brForeActive = new SolidColorBrush(Colors.White);
                return (m_brForeActive);
            }
        }

        /// <summary>
        /// Store the Style for the Activate Labels.
        /// </summary>
        private Style LabelStyleActive
        {
            get
            {
                if (m_LabelStyleActive == null)
                {
                    m_LabelStyleActive = new Style(typeof(Label));
                    m_LabelStyleActive.Setters.Add(new Setter(Label.BackgroundProperty, BrBackActive));
                    m_LabelStyleActive.Setters.Add(new Setter(Label.CursorProperty, Cursors.Hand));
                }
                return (m_LabelStyleActive);
            }
        }

        /// <summary>
        /// Store the Style for the Activate Labels.
        /// </summary>
        private Style LabelStyleNotImplementedActive
        {
            get
            {
                if (m_LabelStyleNotImplementedActive == null)
                {
                    m_LabelStyleNotImplementedActive = new Style(typeof(Label));
                    m_LabelStyleNotImplementedActive.Setters.Add(new Setter(Label.BackgroundProperty, BrBackNotImplementedActive));
                    m_LabelStyleNotImplementedActive.Setters.Add(new Setter(Label.CursorProperty, Cursors.Hand));
                }
                return (m_LabelStyleNotImplementedActive);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public MainWindow(ApplicationType AppType)
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
            //  Actualize the Application Type, DataContext associated and defines the Load manager for this Window.
                this.AppType = AppType;
                this.DataContext = GlobalViewModel.Instance.HispaniaViewModel;
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
                rdTancament.Height = HideHeaderPanel;
                rdTancament.MinHeight = HideHeaderPanel.Value;
                rdEspai.Height = ExpandEspaiPanel;
            //  Define Managers for the Window controls
                LoadManagers();
        }

        #endregion
        
        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window Managers
                Closing += MainWindow_Closing;
                imgTancament.MouseDown += ImgTancament_MouseDown;
            //  Managers for Menu 'Taules'
                LoadManagersForMenuTables();
            //  Managers for Menu 'Articles'
                LoadManagersForMenuGoods();
            //  Managers for Menu 'Pedidos'
                LoadManagersForMenuOrders();
            //  Managers for Menu 'Facturas'
                LoadManagersForMenuBills();
            //  Managers for Menu 'Consultas'
                LoadManagersForMenuQueries();
            //  Managers for Menu 'Almacen'
                LoadManagersForMenuWarehouse();
            //  Managers for Menu 'Revisiones'
                LoadManagersForMenuRevisions();
            //  Managers for Menu 'Listados'
                LoadManagersForMenuListings();
            //  Managers for Menu 'Tancament'
                LoadManagersForMenuClosing();
        }

        #region Window Managers

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            foreach (Window window in new ArrayList(Active_Windows))
            {
                window.Close();
            }
        }

        #endregion

        #region Events definitions of the controls of the Windows

        #region Image Tancament Manager

        private void ImgTancament_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rdTancament.Height == HideHeaderPanel)
            {
                //  Muestra la ventana de Credenciales para hacer que el usuario se valide antes de acceder a la aplicación.
                    CredentialsWindow AccessClosingOptionsWindow = new CredentialsWindow
                    {
                        AllCredentials = GlobalViewModel.Instance.AllCredentials,
                        TransparencyActive = false
                    };
                    Nullable<bool> DialogResult = AccessClosingOptionsWindow.ShowDialog();
                //  Gestiona el resultado de la ejecución de la ventana que controla las Credenciales de Usuario.
                    if (DialogResult.HasValue)
                    {
                        if (DialogResult.Value) 
                        {
                            //  Muestra las opciones de cierre del año.
                                rdTancament.Height = ViewTancamentPanel;
                                rdTancament.MinHeight = ViewTancamentPanelMinHeight.Value;
                                rdEspai.Height = NormalEspaiPanel;
                            //  Muestra el aviso de las acciones a llevar a acbo durante el cierre del año
                                string sBillingCloseYearInfo = "Els Albarans que es vulguin passar a l'any següent s'han de passar durant l'any en curs.";
                                MsgManager.ShowMessage(sBillingCloseYearInfo, MsgType.Warning);
                        }
                        else
                        {
                            MsgManager.ShowMessage("Informació, operació cnacelada per l'usuari.", MsgType.Information);
                        }
                    }
            }
            else
            {
                rdTancament.Height = HideHeaderPanel;
                rdTancament.MinHeight = HideHeaderPanel.Value;
                rdEspai.Height = ExpandEspaiPanel;
            }
        }

        #endregion

        #region Managers for Menu Tables

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Tables in the Window
        /// </summary>
        private void LoadManagersForMenuTables()
        {
            //  Option 01 'Paràmetres Generals'
                lblParametresGenerals.MouseEnter += Manage_MouseEnter;
                lblParametresGenerals.MouseLeave += Manage_MouseLeave;
                lblParametresGenerals.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Series de Facturació'                
                lblSeriesFacturacio.MouseEnter += Manage_MouseEnter;
                lblSeriesFacturacio.MouseLeave += Manage_MouseLeave;
                lblSeriesFacturacio.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Ciutats i Còdi Postal'                
                lblPoblacions.MouseEnter += Manage_MouseEnter;
                lblPoblacions.MouseLeave += Manage_MouseLeave;
                lblPoblacions.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 04 'Tipus d'IVA i de Recàrrec'
                lblTipusIVA.MouseEnter += Manage_MouseEnter;
                lblTipusIVA.MouseLeave += Manage_MouseLeave;
                lblTipusIVA.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 05 'Tipus d'Enviament'
                lblTipusEnviament.MouseEnter += Manage_MouseEnter;
                lblTipusEnviament.MouseLeave += Manage_MouseLeave;
                lblTipusEnviament.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 06 'Representants'
                lblRepresentants.MouseEnter += Manage_MouseEnter;
                lblRepresentants.MouseLeave += Manage_MouseLeave;
                lblRepresentants.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 07 'Tipus d'Efecte'
                lblEfectes.MouseEnter += Manage_MouseEnter;
                lblEfectes.MouseLeave += Manage_MouseLeave;
                lblEfectes.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 08.1 'Gestionar Clients'
                lblGestioClients.MouseEnter += Manage_MouseEnter;
                lblGestioClients.MouseLeave += Manage_MouseLeave;
                lblGestioClients.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 08.2 'Impressió de Clients'
                lblImpressioFitxesClients.MouseEnter += Manage_MouseEnter;
                lblImpressioFitxesClients.MouseLeave += Manage_MouseLeave;
                lblImpressioFitxesClients.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 09.1 'Gestionar Proveïdors'
                lblGestioProveidors.MouseEnter += Manage_MouseEnter;
                lblGestioProveidors.MouseLeave += Manage_MouseLeave;
                lblGestioProveidors.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 09.2 'Impressió de Proveïdors'
                lblImpressioFitxesProveidors.MouseEnter += Manage_MouseEnter;
                lblImpressioFitxesProveidors.MouseLeave += Manage_MouseLeave;
                lblImpressioFitxesProveidors.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #region Managers for Menu Goods

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Goods in the Window
        /// </summary>
        private void LoadManagersForMenuGoods()
        {
            //  Option 01 'Unitats'
                lblUnitExpedFact.MouseEnter += Manage_MouseEnter;
                lblUnitExpedFact.MouseLeave += Manage_MouseLeave;
                lblUnitExpedFact.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Articles'
                lblGestioArticles.MouseEnter += Manage_MouseEnter;
                lblGestioArticles.MouseLeave += Manage_MouseLeave;
                lblGestioArticles.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Actualitzar Estocs Inicials'
                lblActualitzarEstocInicial.MouseEnter += Manage_MouseEnter;
                lblActualitzarEstocInicial.MouseLeave += Manage_MouseLeave;
                lblActualitzarEstocInicial.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #region Managers for Menu Orders

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Orders in the Window
        /// </summary>
        private void LoadManagersForMenuOrders()
        {
            //  Option 01 'Gestió Comandes Proveidors'
                lblGestioComandesProveidors.MouseEnter += Manage_MouseEnter;
                lblGestioComandesProveidors.MouseLeave += Manage_MouseLeave;
                lblGestioComandesProveidors.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Gestió Comandes Proveidors'
                lblCompararComandes.MouseEnter += Manage_MouseEnter;
                lblCompararComandes.MouseLeave += Manage_MouseLeave;
                lblCompararComandes.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Gestió Albarans'
                lblGestioAlbarans.MouseEnter += Manage_MouseEnter;
                lblGestioAlbarans.MouseLeave += Manage_MouseLeave;
                lblGestioAlbarans.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 04 'Gestió Comandes Clients'
                lblGestioComandesClients.MouseEnter += Manage_MouseEnter;
                lblGestioComandesClients.MouseLeave += Manage_MouseLeave;
                lblGestioComandesClients.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 05 'Impressió d'Albarans i Creació de Factures'
                lblImpressioComandesClients.MouseEnter += Manage_MouseEnter;
                lblImpressioComandesClients.MouseLeave += Manage_MouseLeave;
                lblImpressioComandesClients.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #region Managers for Menu Bills

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Bills in the Window
        /// </summary>
        private void LoadManagersForMenuBills()
        {
            //  Option 01 'Gestió Factures'
                lblGestioFactures.MouseEnter += Manage_MouseEnter;
                lblGestioFactures.MouseLeave += Manage_MouseLeave;
                lblGestioFactures.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Impressió d'Albarans i Creació de Factures'
                lblImpressioFactures.MouseEnter += Manage_MouseEnter;
                lblImpressioFactures.MouseLeave += Manage_MouseLeave;
                lblImpressioFactures.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Revisa Desquadres de Venciments'
                lblRevisaDesquadresVenciment.MouseEnter += Manage_MouseEnter;
                lblRevisaDesquadresVenciment.MouseLeave += Manage_MouseLeave;
                lblRevisaDesquadresVenciment.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 04 'Liquidacions'
                lblLiquidacions.MouseEnter += Manage_MouseEnter;
                lblLiquidacions.MouseLeave += Manage_MouseLeave;
                lblLiquidacions.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 05 'Impagats'
                lblImpagats.MouseEnter += Manage_MouseEnter;
                lblImpagats.MouseLeave += Manage_MouseLeave;
                lblImpagats.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #region Managers for Menu Queries

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Queries in the Window
        /// </summary>
        private void LoadManagersForMenuQueries()
        {
            //  Option 01 'Consulta Articles'
            lblConsultaArticles.MouseEnter += Manage_MouseEnter;
            lblConsultaArticles.MouseLeave += Manage_MouseLeave;
            lblConsultaArticles.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Consulta Clients'
            lblConsultaClients.MouseEnter += Manage_MouseEnter;
            lblConsultaClients.MouseLeave += Manage_MouseLeave;
            lblConsultaClients.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Consulta Comandes de Client'
            lblConsultaComandesClient.MouseEnter += Manage_MouseEnter;
            lblConsultaComandesClient.MouseLeave += Manage_MouseLeave;
            lblConsultaComandesClient.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 04 'Consulta Històric de Clients per Data'
            lblConsultaHistoCliData.MouseEnter += Manage_MouseEnter;
            lblConsultaHistoCliData.MouseLeave += Manage_MouseLeave;
            lblConsultaHistoCliData.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 05 'Consulta Històric de Clients per Data i Representant'
            lblConsultaHistoCliDataAgent.MouseEnter += Manage_MouseEnter;
            lblConsultaHistoCliDataAgent.MouseLeave += Manage_MouseLeave;
            lblConsultaHistoCliDataAgent.MouseDoubleClick += ManageLabel_MouseDoubleClick;


            lblConsultaProviders.MouseEnter += Manage_MouseEnter;
            lblConsultaProviders.MouseLeave += Manage_MouseLeave;
            lblConsultaProviders.MouseDoubleClick += LblConsultaProviders_MouseDoubleClick;

            lblQueryPaymentForecast.MouseEnter += Manage_MouseEnter;
            lblQueryPaymentForecast.MouseLeave += Manage_MouseLeave;
            lblQueryPaymentForecast.MouseDoubleClick += LblQueryPaimentForecast_MouseDoubleClick;


            //  Option 06 'Consulta Customitzada'
            lblConsultaParam.MouseEnter += Manage_MouseEnter;
            lblConsultaParam.MouseLeave += Manage_MouseLeave;
            lblConsultaParam.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LblConsultaProviders_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            Window window = FindWindow( typeof( QueryOrdersProvider ) );
            if(null == window)
            {
                // Create
                QueryOrdersProvider query_window = new QueryOrdersProvider();

                this.Active_Windows.Add( query_window );
                query_window.Closed += UniwersalOnClosedWindow;
                window = query_window;
            }
            else
            {
                // Show
            }
            window.Show();
            window.Activate();
        }

        private void LblQueryPaimentForecast_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            Window window = FindWindow( typeof( PaymentForecast ) );
            if(null == window)
            {
                // Create
                PaymentForecast query_window = new PaymentForecast();

                this.Active_Windows.Add( query_window );
                query_window.Closed += UniwersalOnClosedWindow;
                window = query_window;
            }
            else
            {
                // Show
            }
            window.Show();
            window.Activate();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniwersalOnClosedWindow( object sender, EventArgs e )
        {
            Window window = (Window)sender;
            if(sender != null)
            {
                this.Active_Windows.Remove( window );
                window.Closed -= UniwersalOnClosedWindow;                    
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeWindow"></param>
        /// <returns></returns>
        private Window FindWindow( Type typeWindow )
        {

            Window result = this.Active_Windows.Where( i => i.GetType() == typeWindow )
                                .FirstOrDefault();           

            return result;
        }

        #endregion

        #region Managers for Menu Warehouse

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Warehouse in the Window
        /// </summary>
        private void LoadManagersForMenuWarehouse()
        {
            //  Option 01 'Afegir Moviments de Magatzem'            
                lblAfegirMovimentsMagatzem.MouseEnter += Manage_MouseEnter;
                lblAfegirMovimentsMagatzem.MouseLeave += Manage_MouseLeave;
                lblAfegirMovimentsMagatzem.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Gestió Moviments de Magatzem'            
                lblGestioMovimentsMagatzem.MouseEnter += Manage_MouseEnter;
                lblGestioMovimentsMagatzem.MouseLeave += Manage_MouseLeave;
                lblGestioMovimentsMagatzem.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #region Managers for Menu Revisions

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Revisions in the Window
        /// </summary>
        private void LoadManagersForMenuRevisions()
        {
            //  Option 01 'Revisa Factures'            
                lblRevisioFactures.MouseEnter += Manage_MouseEnter;
                lblRevisioFactures.MouseLeave += Manage_MouseLeave;
                lblRevisioFactures.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Desquadres - Estocs Disponibles'
                lblEstocsDisponibles.MouseEnter += Manage_MouseEnter;
                lblEstocsDisponibles.MouseLeave += Manage_MouseLeave;
                lblEstocsDisponibles.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Desquadres - Estocs Existències'
                lblEstocsExistencies.MouseEnter += Manage_MouseEnter;
                lblEstocsExistencies.MouseLeave += Manage_MouseLeave;
                lblEstocsExistencies.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 04 'General - Disponible' 
                lblDisponibles.MouseEnter += Manage_MouseEnter;
                lblDisponibles.MouseLeave += Manage_MouseLeave;
                lblDisponibles.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 05 'General - Existències' 
                lblExistencies.MouseEnter += Manage_MouseEnter;
                lblExistencies.MouseLeave += Manage_MouseLeave;
                lblExistencies.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #region Managers for Menu Listings

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Listings in the Window
        /// </summary>
        private void LoadManagersForMenuListings()
        {
            //  Option 01 'Inventaris'            
            lblLlistatInventaris.MouseEnter += Manage_MouseEnter;
            lblLlistatInventaris.MouseLeave += Manage_MouseLeave;
            lblLlistatInventaris.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Marges'            
            lblLlistatMarges.MouseEnter += Manage_MouseEnter;
            lblLlistatMarges.MouseLeave += Manage_MouseLeave;
            lblLlistatMarges.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Impagats'
            lblLlistatImpagats.MouseEnter += Manage_MouseEnter;
            lblLlistatImpagats.MouseLeave += Manage_MouseLeave;
            lblLlistatImpagats.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 04 'Línies d'Albarà'
            lblLlistatLiniesAlbara.MouseEnter += Manage_MouseEnter;
            lblLlistatLiniesAlbara.MouseLeave += Manage_MouseLeave;
            lblLlistatLiniesAlbara.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 05 'Diari de Vendes' 
            lblLlistatDiariVendes.MouseEnter += Manage_MouseEnter;
            lblLlistatDiariVendes.MouseLeave += Manage_MouseLeave;
            lblLlistatDiariVendes.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 06 'Diari de Vendes / Comptabilitat' 
            lblLlistatDiariVendesComptabilitat.MouseEnter += Manage_MouseEnter;
            lblLlistatDiariVendesComptabilitat.MouseLeave += Manage_MouseLeave;
            lblLlistatDiariVendesComptabilitat.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #region Managers for Menu Closing

        /// <summary>
        /// Method that define the managers needed for the user operations in Menu Listings in the Window
        /// </summary>
        private void LoadManagersForMenuClosing()
        {
            //  Option 01 'Informe de Vendes Superior a...'            
            lblInformeVendesSuperiorA.MouseEnter += Manage_MouseEnter;
            lblInformeVendesSuperiorA.MouseLeave += Manage_MouseLeave;
            lblInformeVendesSuperiorA.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 02 'Històric d'acumulat de clients'            
            lblHistoricsAcumulatClient.MouseEnter += Manage_MouseEnter;
            lblHistoricsAcumulatClient.MouseLeave += Manage_MouseLeave;
            lblHistoricsAcumulatClient.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 03 'Històric d'acumulat d'articles'            
            lblHistoricsAcumulatArticles.MouseEnter += Manage_MouseEnter;
            lblHistoricsAcumulatArticles.MouseLeave += Manage_MouseLeave;
            lblHistoricsAcumulatArticles.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 04 'Esborrar Moviments de Magatzem'            
            lblEliminarMovimentsMagatzem.MouseEnter += Manage_MouseEnter;
            lblEliminarMovimentsMagatzem.MouseLeave += Manage_MouseLeave;
            lblEliminarMovimentsMagatzem.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 05 'Transferir Estocs Inicials'            
            lblTraspassarEstocsIncials.MouseEnter += Manage_MouseEnter;
            lblTraspassarEstocsIncials.MouseLeave += Manage_MouseLeave;
            lblTraspassarEstocsIncials.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 06 'Transferir Comandes de Client'  
            lblTraspassarComandesClient.MouseEnter += Manage_MouseEnter;
            lblTraspassarComandesClient.MouseLeave += Manage_MouseLeave;
            lblTraspassarComandesClient.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 07 'Transferir Albarans'  
            lblTraspassarAlbarans.MouseEnter += Manage_MouseEnter;
            lblTraspassarAlbarans.MouseLeave += Manage_MouseLeave;
            lblTraspassarAlbarans.MouseDoubleClick += ManageLabel_MouseDoubleClick;
            //  Option 08 'Resetejar el Numero de Factura'            
            lblResetejarNumeroFactura.MouseEnter += Manage_MouseEnter;
            lblResetejarNumeroFactura.MouseLeave += Manage_MouseLeave;
            lblResetejarNumeroFactura.MouseDoubleClick += ManageLabel_MouseDoubleClick;
        }

        #endregion

        #endregion

        #region Events of the controls of the Window

        #region Mouse Enter

        /// <summary>
        /// Marks actual label as the Active option of the Window
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void Manage_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((sender is Label) || (sender is Grid))
            {
                ItemActiveOriginalStyle = ((Label)sender).Style;
                ItemActiveOriginalForeground = ((Label)sender).Foreground;
                ItemActiveOriginalHorizontalContentAlignment = ((Label)sender).HorizontalContentAlignment;
                ((Label)sender).Style = LabelStyleActive;
                ((Label)sender).Foreground = BrForeActive;
                ((Label)sender).HorizontalContentAlignment = HorizontalAlignment.Center;
                #region MENU TABLES
                if (sender == lblParametresGenerals) grdParameters.Background = BrBackActiveItemSelected;
                else if (sender == lblSeriesFacturacio) grdBillingSeries.Background = BrBackActiveItemSelected;
                else if (sender == lblPoblacions) grdPostalCodes.Background = BrBackActiveItemSelected;
                else if (sender == lblTipusIVA) grdIVATypes.Background = BrBackActiveItemSelected;
                else if (sender == lblTipusEnviament) grdSendTypes.Background = BrBackActiveItemSelected;
                else if (sender == lblRepresentants) grdAgents.Background = BrBackActiveItemSelected;
                else if (sender == lblEfectes) grdEffectTypes.Background = BrBackActiveItemSelected;
                else if (sender == lblGestioClients) grdCustomers.Background = BrBackActiveItemSelected;
                else if (sender == lblImpressioFitxesClients) grdCustomersPrint.Background = BrBackActiveItemSelected;
                else if (sender == lblGestioProveidors) grdProviders.Background = BrBackActiveItemSelected;
                else if (sender == lblImpressioFitxesProveidors) grdProvidersPrint.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU GOODS
                else if (sender == lblUnitExpedFact) grdUnitExpedFact.Background = BrBackActiveItemSelected;
                else if (sender == lblGestioArticles) grdGestioArticles.Background = BrBackActiveItemSelected;
                else if (sender == lblActualitzarEstocInicial) grdActualitzarEstocInicial.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU ORDERS
                else if (sender == lblGestioComandesProveidors)
                {
                    grdGestioComandesProveidors.Background = BrBackNotImplementedItemSelected;
                    ((Label)sender).Style = LabelStyleNotImplementedActive;
                    ((Label)sender).Foreground = BrForeActive;
                    ((Label)sender).HorizontalContentAlignment = HorizontalAlignment.Center;
                }
                else if (sender == lblCompararComandes)
                {
                    grdCompararComandes.Background = BrBackNotImplementedItemSelected;
                    ((Label)sender).Style = LabelStyleNotImplementedActive;
                    ((Label)sender).Foreground = BrForeActive;
                    ((Label)sender).HorizontalContentAlignment = HorizontalAlignment.Center;
                }
                else if (sender == lblGestioComandesClients) grdGestioComandesClients.Background = BrBackActiveItemSelected;
                else if (sender == lblGestioAlbarans) grdGestioAlbarans.Background = BrBackActiveItemSelected;
                else if (sender == lblImpressioComandesClients) grdComandesClientsPrint.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU BILLS
                else if (sender == lblGestioFactures) grdGestioFactures.Background = BrBackActiveItemSelected;
                else if (sender == lblImpressioFactures) grdFacturesPrint.Background = BrBackActiveItemSelected;
                else if (sender == lblRevisaDesquadresVenciment) grdRevisaDesquadresVenciment.Background = BrBackActiveItemSelected;
                else if (sender == lblLiquidacions) grdLiquidacions.Background = BrBackActiveItemSelected;
                else if (sender == lblImpagats) grdImpagats.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU QUERIES
                else if (sender == lblConsultaArticles) grdConsultaArticles.Background = BrBackActiveItemSelected;
                else if (sender == lblConsultaClients) grdConsultaClients.Background = BrBackActiveItemSelected;
                else if (sender == lblConsultaComandesClient) grdConsultaComandesClient.Background = BrBackActiveItemSelected;
                else if (sender == lblConsultaHistoCliData) grdConsultaHistoCliData.Background = BrBackActiveItemSelected;
                else if (sender == lblConsultaHistoCliDataAgent) grdConsultaHistoCliDataAgent.Background = BrBackActiveItemSelected;
                else if(sender == lblConsultaProviders )
                    grdConsultaProviders.Background = BrBackActiveItemSelected;
                else if (sender == lblConsultaParam) grdConsultaParam.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU WAREHOUSE
                else if (sender == lblAfegirMovimentsMagatzem) grdAfegirMovimentsMagatzem.Background = BrBackActiveItemSelected;
                else if (sender == lblGestioMovimentsMagatzem) grdGestioMovimentsMagatzem.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU REVISIONS
                else if (sender == lblRevisioFactures) grdRevisioFactures.Background = BrBackActiveItemSelected;
                else if (sender == lblEstocsDisponibles) grdEstocsDisponibles.Background = BrBackActiveItemSelected;
                else if (sender == lblEstocsExistencies) grdEstocsExistencies.Background = BrBackActiveItemSelected;
                else if (sender == lblDisponibles) grdDisponibles.Background = BrBackActiveItemSelected;
                else if (sender == lblExistencies) grdExistencies.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU LISTINGS
                else if (sender == lblLlistatInventaris) grdLlistatInventaris.Background = BrBackActiveItemSelected;
                else if (sender == lblLlistatMarges) grdLlistatMarges.Background = BrBackActiveItemSelected;
                else if (sender == lblLlistatImpagats) grdLlistatImpagats.Background = BrBackActiveItemSelected;
                else if (sender == lblLlistatLiniesAlbara) grdLlistatLiniesAlbara.Background = BrBackActiveItemSelected;
                else if (sender == lblLlistatDiariVendes) grdLlistatDiariVendes.Background = BrBackActiveItemSelected;
                else if (sender == lblLlistatDiariVendesComptabilitat) grdLlistatDiariVendesComptabilitat.Background = BrBackActiveItemSelected;
                #endregion
                #region MENU CLOSING
                else if (sender == lblInformeVendesSuperiorA) grdInformeVendesSuperiorA.Background = BrBackActiveItemSelected;
                else if (sender == lblHistoricsAcumulatClient) grdHistoricsAcumulatClient.Background = BrBackActiveItemSelected;
                else if (sender == lblHistoricsAcumulatArticles) grdHistoricsAcumulatArticles.Background = BrBackActiveItemSelected;
                else if (sender == lblTraspassarEstocsIncials) grdTraspassarEstocsIncials.Background = BrBackActiveItemSelected;
                else if (sender == lblTraspassarComandesClient) grdTraspassarComandesClient.Background = BrBackActiveItemSelected;
                else if (sender == lblTraspassarAlbarans) grdTraspassarAlbarans.Background = BrBackActiveItemSelected;
                else if (sender == lblEliminarMovimentsMagatzem) grdEliminarMovimentsMagatzem.Background = BrBackActiveItemSelected;
                else if (sender == lblResetejarNumeroFactura) grdResetejarNumeroFactura.Background = BrBackActiveItemSelected;
                #endregion
            }
        }

        #endregion

        #region Mouse Leave

        /// <summary>
        /// Marks actual label as not the Active option of the Window
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void Manage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if ((sender is Label) || (sender is Grid))
            {
                ((Label)sender).Style = ItemActiveOriginalStyle;
                ((Label)sender).Foreground = ItemActiveOriginalForeground;
                ((Label)sender).HorizontalContentAlignment = ItemActiveOriginalHorizontalContentAlignment;
                #region MENU TABLES
                if(sender == lblParametresGenerals)
                    grdParameters.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblSeriesFacturacio)
                    grdBillingSeries.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblPoblacions)
                    grdPostalCodes.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblTipusIVA)
                    grdIVATypes.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblTipusEnviament)
                    grdSendTypes.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblRepresentants)
                    grdAgents.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblEfectes)
                    grdEffectTypes.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblGestioClients)
                    grdCustomers.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblImpressioFitxesClients)
                    grdCustomersPrint.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblGestioProveidors)
                    grdProviders.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblImpressioFitxesProveidors)
                    grdProvidersPrint.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region MENU GOODS
                else if(sender == lblUnitExpedFact)
                    grdUnitExpedFact.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblGestioArticles)
                    grdGestioArticles.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblActualitzarEstocInicial)
                    grdActualitzarEstocInicial.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region MENU ORDERS
                else if(sender == lblGestioComandesProveidors)
                    grdGestioComandesProveidors.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblCompararComandes)
                    grdCompararComandes.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblGestioComandesClients)
                    grdGestioComandesClients.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblGestioAlbarans)
                    grdGestioAlbarans.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblImpressioComandesClients)
                    grdComandesClientsPrint.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region MENU BILLS
                else if(sender == lblGestioFactures)
                    grdGestioFactures.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblImpressioFactures)
                    grdFacturesPrint.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblRevisaDesquadresVenciment)
                    grdRevisaDesquadresVenciment.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblLiquidacions)
                    grdLiquidacions.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblImpagats)
                    grdImpagats.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region MENU QUERIES
                else if(sender == lblConsultaArticles)
                    grdConsultaArticles.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblConsultaClients)
                    grdConsultaClients.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblConsultaComandesClient)
                    grdConsultaComandesClient.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblConsultaHistoCliData)
                    grdConsultaHistoCliData.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblConsultaHistoCliDataAgent)
                    grdConsultaHistoCliDataAgent.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblConsultaProviders)
                {
                    grdConsultaProviders.Background = new SolidColorBrush( Colors.WhiteSmoke );
                }
                else if(sender == lblConsultaParam)
                    grdConsultaParam.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region MENU WAREHOUSE
                else if(sender == lblAfegirMovimentsMagatzem)
                    grdAfegirMovimentsMagatzem.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblGestioMovimentsMagatzem)
                    grdGestioMovimentsMagatzem.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region MENU REVISIONS
                else if(sender == lblRevisioFactures)
                    grdRevisioFactures.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblEstocsDisponibles)
                    grdEstocsDisponibles.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblEstocsExistencies)
                    grdEstocsExistencies.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblDisponibles)
                    grdDisponibles.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblExistencies)
                    grdExistencies.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region LISTINGS
                else if(sender == lblLlistatInventaris)
                    grdLlistatInventaris.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblLlistatMarges)
                    grdLlistatMarges.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblLlistatImpagats)
                    grdLlistatImpagats.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblLlistatLiniesAlbara)
                    grdLlistatLiniesAlbara.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblLlistatDiariVendes)
                    grdLlistatDiariVendes.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblLlistatDiariVendesComptabilitat)
                    grdLlistatDiariVendesComptabilitat.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
                #region MENU CLOSING
                else if(sender == lblInformeVendesSuperiorA)
                    grdInformeVendesSuperiorA.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblHistoricsAcumulatClient)
                    grdHistoricsAcumulatClient.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblHistoricsAcumulatArticles)
                    grdHistoricsAcumulatArticles.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblTraspassarEstocsIncials)
                    grdTraspassarEstocsIncials.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblTraspassarComandesClient)
                    grdTraspassarComandesClient.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblTraspassarAlbarans)
                    grdTraspassarAlbarans.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblEliminarMovimentsMagatzem)
                    grdEliminarMovimentsMagatzem.Background = new SolidColorBrush( Colors.WhiteSmoke );
                else if(sender == lblResetejarNumeroFactura)
                    grdResetejarNumeroFactura.Background = new SolidColorBrush( Colors.WhiteSmoke );
                #endregion
            }
        }

        #endregion

        #region Mouse Double Click

        /// <summary>
        /// Try the correct actions to do after the user select an operation
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ManageLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                #region MENU TABLES
                if (sender == lblParametresGenerals) ManageParameters();
                else if (sender == lblSeriesFacturacio) ManageBillingSeries();
                else if (sender == lblPoblacions) ManagePostalCodes();
                else if (sender == lblTipusIVA) ManageIVATypes();
                else if (sender == lblTipusEnviament) ManageSendTypes();
                else if (sender == lblRepresentants) ManageAgents();
                else if (sender == lblEfectes) ManageEffectTypes();
                else if (sender == lblGestioClients) ManageCustomers();
                else if (sender == lblImpressioFitxesClients) ManageCustomersForPrint();
                else if (sender == lblGestioProveidors) ManageProviders();
                else if (sender == lblImpressioFitxesProveidors) ManageProvidersForPrint();
                #endregion
                #region MENU GOODS
                else if (sender == lblUnitExpedFact) ManageUnitExpedFact();
                else if (sender == lblGestioArticles) ManageGoods();
                else if (sender == lblActualitzarEstocInicial) ManageActualitzarEstocInicial();
                #endregion
                #region MENU ORDERS
                //else if ((sender == lblGestioComandesProveidors) || (sender == lblCompararComandes))
                //{
                //    MsgManager.ShowMessage("Avís, opció encara pendent de definició.", MsgType.Information);
                //}
                else if (sender == lblGestioComandesProveidors) ManageGestioComandesProveidors();
                //else if (sender == lblCompararComandes) ManageCompararComandes();
                else if (sender == lblGestioComandesClients) ManageGestioComandesClients();
                else if (sender == lblGestioAlbarans) ManageGestioAlbarans();
                else if (sender == lblImpressioComandesClients) ManageImpressioComandesClients();
                #endregion
                #region MENU BILLS
                else if (sender == lblGestioFactures) ManageGestioFactures();
                else if (sender == lblImpressioFactures) ManageImpressioFactures();
                else if (sender == lblRevisaDesquadresVenciment) ManageRevisaDesquadresVenciment();
                else if (sender == lblLiquidacions) ManageGestioLiquidacions();
                else if (sender == lblImpagats) ManageGestioImpagats();
                #endregion
                #region MENU QUERIES
                else if (sender == lblConsultaArticles) ManageGestioConsultes(QueryType.Goods);
                else if (sender == lblConsultaClients) ManageGestioConsultes(QueryType.Customers_Full);
                else if (sender == lblConsultaComandesClient) ManageGestioConsultes(QueryType.CustomerOrders);
                else if (sender == lblConsultaHistoCliData) ManageGestioConsultes(QueryType.HistoCustomerForData);
                else if (sender == lblConsultaHistoCliDataAgent) ManageGestioConsultes(QueryType.HistoCustomerForDataAndAgent);
                else if( sender == lblConsultaProviders ) ManageGestioConsultes( QueryType.Providers );
                else if (sender == lblConsultaParam) ManageGestioConsultes(QueryType.CustomQuery);
                #endregion
                #region MENU WAREHOUSE
                else if (sender == lblAfegirMovimentsMagatzem) ManageAfegirMovimentsMagatzem();
                else if (sender == lblGestioMovimentsMagatzem) ManageGestioMovimentsMagatzem();
                #endregion
                #region MENU REVISIONS
                else if (sender == lblRevisioFactures) ManageRevisioFactues();
                else if (sender == lblEstocsDisponibles) ManageDesquadresEstocsDisponibles();
                else if (sender == lblEstocsExistencies) ManageDesquadresEstocsExistencies();
                else if (sender == lblDisponibles) ManageGeneralDisponible();
                else if (sender == lblExistencies) ManageGeneralExistencies();
                #endregion
                #region LISTINGS
                else if (sender == lblLlistatInventaris) ManageLlistatInventaris();
                else if (sender == lblLlistatMarges) ManageLlistatMarges();
                else if (sender == lblLlistatImpagats) ManageLlistatImpagats();
                else if (sender == lblLlistatLiniesAlbara) ManageLlistatLiniesAlbara();
                else if (sender == lblLlistatDiariVendes) ManageLlistatDiariVendes();
                else if (sender == lblLlistatDiariVendesComptabilitat) ManageLlistatDiariVendesComptabilitat();
                #endregion
                #region MENU CLOSING
                else if (sender == lblInformeVendesSuperiorA) ManageInformeVendesSuperiorA();
                else if (sender == lblHistoricsAcumulatClient) ManageHistoricsAcumulatClient();
                else if (sender == lblHistoricsAcumulatArticles) ManageHistoricsAcumulatArticle();
                else if (sender == lblTraspassarEstocsIncials) ManageTraspassarEstocsInicials();
                else if (sender == lblTraspassarComandesClient) ManageTraspassarComandesClient();
                else if (sender == lblTraspassarAlbarans) ManageTraspassarAlbarans();
                else if (sender == lblEliminarMovimentsMagatzem) ManageEsborrarMovimentsMagatzem();
                else if (sender == lblResetejarNumeroFactura) ManageResetejarNumeroFactura();
                #endregion
                else MsgManager.ShowMessage("Error, opción no implementada.", MsgType.Error);
            }
        }

        #endregion

        #endregion

        #region Event Managers Methods

        #region Specific Managers for options of Menu Tables

        #region Management of the General Params operations

        /// <summary>
        /// Manage the option 'Paràmetres Generals' of the Application
        /// </summary>
        private void ManageParameters()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (ParametersWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ParametersWindow);
                    ParametersWindow = new Parameters(AppType)
                    {
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        Data = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        CtrlOperation = UserControls.Operation.Show
                    };
                    ParametersWindow.Closed += ParametersWindow_Closed;
                    ParametersWindow.Show();
                    Active_Windows.Add(ParametersWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Paràmetres.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else ParametersWindow.Activate();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Parameters Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ParametersWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(ParametersWindow);
            ParametersWindow.Closed -= ParametersWindow_Closed;
            ParametersWindow = null;
        }

        #endregion
        
        #region Management of the Billing Series operations

        /// <summary>
        /// Manage the option 'Series de Facturació' of the Application
        /// </summary>
        private void ManageBillingSeries()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (BillingSeriesWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BillingSeriesWindow);
                    BillingSeriesWindow = new BillingSeries(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.BillingSeries
                    };
                    BillingSeriesWindow.Closed += BillingSeriesWindow_Closed;
                    BillingSeriesWindow.Show();
                    Active_Windows.Add(BillingSeriesWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Sèries de Facturació.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (BillingSeriesWindow.WindowState == WindowState.Minimized) BillingSeriesWindow.WindowState = WindowState.Normal;
                BillingSeriesWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void BillingSeriesWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(BillingSeriesWindow);
            BillingSeriesWindow.Closed -= BillingSeriesWindow_Closed;
            BillingSeriesWindow = null;
        }

        #endregion

        #region Management of the Postal Codes operations

        /// <summary>
        /// Manage the option 'Poblacions' of the Application
        /// </summary>
        private void ManagePostalCodes()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (PostalCodesWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.PostalCodesWindow);
                    PostalCodesWindow = new PostalCodes(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.PostalCodes
                    };
                    PostalCodesWindow.Closed += CitiesCPWindow_Closed;
                    PostalCodesWindow.Show();
                    Active_Windows.Add(PostalCodesWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Codis Postals / Poblacions.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (PostalCodesWindow.WindowState == WindowState.Minimized) PostalCodesWindow.WindowState = WindowState.Normal;
                PostalCodesWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the CitiesCPWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CitiesCPWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(PostalCodesWindow);
            PostalCodesWindow.Closed -= CitiesCPWindow_Closed;
            PostalCodesWindow = null;
        }

        #endregion

        #region Management of the IVATypes operations

        /// <summary>
        /// Manage the option 'Tipus d'IVA i Recàrrec' of the Application
        /// </summary>
        private void ManageIVATypes()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (IVATypesWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.IVATypesWindow);
                    IVATypesWindow = new IVATypes(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.IVATypes
                    };
                    IVATypesWindow.Closed += IVATypesWindow_Closed;
                    IVATypesWindow.Show();
                    Active_Windows.Add(IVATypesWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Tipus d'IVA.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (IVATypesWindow.WindowState == WindowState.Minimized) IVATypesWindow.WindowState = WindowState.Normal;
                IVATypesWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the IVATypesWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void IVATypesWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(IVATypesWindow);
            IVATypesWindow.Closed -= IVATypesWindow_Closed;
            IVATypesWindow = null;
        }

        #endregion

        #region Management of the SendTypes operations

        /// <summary>
        /// Manage the option 'Tipus d'Efecte' of the Application
        /// </summary>
        private void ManageSendTypes()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SendTypesWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.SendTypesWindow);
                    SendTypesWindow = new SendTypes(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.SendTypes
                    };
                    SendTypesWindow.Closed += SendTypesWindow_Closed;
                    SendTypesWindow.Show();
                    Active_Windows.Add(SendTypesWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Tipus d'Enviament.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (SendTypesWindow.WindowState == WindowState.Minimized) SendTypesWindow.WindowState = WindowState.Normal;
                SendTypesWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the SendTypesWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void SendTypesWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(SendTypesWindow);
            SendTypesWindow.Closed -= SendTypesWindow_Closed;
            SendTypesWindow = null;
        }

        #endregion

        #region Management of the Agent operations

        /// <summary>
        /// Manage the option 'Representants' of the Application
        /// </summary>
        private void ManageAgents()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (AgentsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.AgentsWindow);
                    AgentsWindow = new Agents(AppType)
                    {
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Agents
                    };
                    AgentsWindow.Closed += AgentsWindow_Closed;
                    AgentsWindow.Show();
                    Active_Windows.Add(AgentsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Representants.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (AgentsWindow.WindowState == WindowState.Minimized) AgentsWindow.WindowState = WindowState.Normal;
                AgentsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the AgentsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void AgentsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(AgentsWindow);
            AgentsWindow.Closed -= AgentsWindow_Closed;
            AgentsWindow = null;
        }

        #endregion

        #region Management of the EffectTypes operations

        /// <summary>
        /// Manage the option 'Tipus d'Efecte' of the Application
        /// </summary>
        private void ManageEffectTypes()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (EffectTypesWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.EffectTypesWindow);
                    EffectTypesWindow = new EffectTypes(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.EffectTypes
                    };
                    EffectTypesWindow.Closed += EffectTypesWindow_Closed;
                    EffectTypesWindow.Show();
                    Active_Windows.Add(EffectTypesWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Tipus d'Efecte.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (EffectTypesWindow.WindowState == WindowState.Minimized) EffectTypesWindow.WindowState = WindowState.Normal;
                EffectTypesWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the EffectTypesWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void EffectTypesWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(EffectTypesWindow);
            EffectTypesWindow.Closed -= EffectTypesWindow_Closed;
            EffectTypesWindow = null;
        }

        #endregion

        #region Management of the Customer operations

        #region Manage Customers

        /// <summary>
        /// Manage the option 'Gestionar Clients' of the Application
        /// </summary>
        private void ManageCustomers()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (CustomersWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.CustomersWindow);
                    CustomersWindow = new Customers(AppType)
                    {
                        Print = false,
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        IVATypes = GlobalViewModel.Instance.HispaniaViewModel.IVATypesDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Customers
                    };
                    CustomersWindow.Closed += CustomersWindow_Closed;
                    CustomersWindow.Show();
                    Active_Windows.Add(CustomersWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió de Clients.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (CustomersWindow.WindowState == WindowState.Minimized) CustomersWindow.WindowState = WindowState.Normal;
                CustomersWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomersWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(CustomersWindow);
            CustomersWindow.Closed -= CustomersWindow_Closed;
            CustomersWindow = null;
        }

        #endregion

        #region Print Customers

        /// <summary>
        /// Manage the option 'Gestionar Clients' of the Application
        /// </summary>
        private void ManageCustomersForPrint()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (CustomersPrintWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.CustomersPrintWindow);
                    CustomersPrintWindow = new Customers(AppType)
                    {
                        Print = true,
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        IVATypes = GlobalViewModel.Instance.HispaniaViewModel.IVATypesDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Customers
                    };
                    CustomersPrintWindow.Closed += CustomersPrintWindow_Closed;
                    CustomersPrintWindow.Show();
                    Active_Windows.Add(CustomersPrintWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari d'Impressió de Clients.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (CustomersPrintWindow.WindowState == WindowState.Minimized) CustomersPrintWindow.WindowState = WindowState.Normal;
                CustomersPrintWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the CustomersPrintWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomersPrintWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(CustomersPrintWindow);
            CustomersPrintWindow.Closed -= CustomersPrintWindow_Closed;
            CustomersPrintWindow = null;
        }

        #endregion

        #endregion

        #region Management of the Provider operations

        #region Manage Providers

        /// <summary>
        /// Manage the option 'Proveïdors' of the Application
        /// </summary>
        private void ManageProviders()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (ProvidersWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ProvidersWindow);
                    ProvidersWindow = new Providers(AppType)
                    {
                        Print = false,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        IVATypes = GlobalViewModel.Instance.HispaniaViewModel.IVATypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Providers
                    };
                    ProvidersWindow.Closed += ProvidersWindow_Closed;
                    ProvidersWindow.Show();
                    Active_Windows.Add(ProvidersWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió de Proveïdors.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (ProvidersWindow.WindowState == WindowState.Minimized) ProvidersWindow.WindowState = WindowState.Normal;
                ProvidersWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the ProvidersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ProvidersWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(ProvidersWindow);
            ProvidersWindow.Closed -= ProvidersWindow_Closed;
            ProvidersWindow = null;
        }

        #endregion

        #region Print Providers

        /// <summary>
        /// Manage the option 'Proveïdors' of the Application
        /// </summary>
        private void ManageProvidersForPrint()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (ProvidersPrintWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ProvidersPrintWindow);
                    ProvidersPrintWindow = new Providers(AppType)
                    {
                        Print = true,
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        IVATypes = GlobalViewModel.Instance.HispaniaViewModel.IVATypesDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Providers
                    };
                    ProvidersPrintWindow.Closed += ProvidersPrintWindow_Closed;
                    ProvidersPrintWindow.Show();
                    Active_Windows.Add(ProvidersPrintWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari d'Impressió de Proveïdors.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (ProvidersPrintWindow.WindowState == WindowState.Minimized) ProvidersPrintWindow.WindowState = WindowState.Normal;
                ProvidersPrintWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the ProvidersPrintWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ProvidersPrintWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(ProvidersPrintWindow);
            ProvidersPrintWindow.Closed -= ProvidersPrintWindow_Closed;
            ProvidersPrintWindow = null;
        }

        #endregion

        #endregion

        #endregion

        #region Specific Managers for options of Menu Goods

        #region Management of the Units operations

        /// <summary>
        /// Manage the option 'Unitats' of the Application
        /// </summary>
        private void ManageUnitExpedFact()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (UnitsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.UnitsWindow);
                    UnitsWindow = new Units(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Units
                    };
                    UnitsWindow.Closed += UnitsWindow_Closed;
                    UnitsWindow.Show();
                    Active_Windows.Add(UnitsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió d'Unitats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (UnitsWindow.WindowState == WindowState.Minimized) UnitsWindow.WindowState = WindowState.Normal;
                UnitsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void UnitsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(UnitsWindow);
            UnitsWindow.Closed -= UnitsWindow_Closed;
            UnitsWindow = null;
        }

        #endregion

        #region Management of the Goods operations

        /// <summary>
        /// Manage the option 'Articles' of the Application
        /// </summary>
        private void ManageGoods()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (GoodsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.GoodsWindow);
                    GoodsWindow = new Goods(AppType)
                    {
                        Units = GlobalViewModel.Instance.HispaniaViewModel.UnitsDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Goods
                    };
                    GoodsWindow.Closed += GoodsWindow_Closed;
                    GoodsWindow.Show();
                    GoodsWindow.GoodsVM.Managemend = true;
                    Active_Windows.Add(GoodsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió d'Artícles.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (GoodsWindow.WindowState == WindowState.Minimized) GoodsWindow.WindowState = WindowState.Normal;

                GoodsWindow.GoodsVM.Managemend = true;

                GoodsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Goods Window is closed we actualize the GoodsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void GoodsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(GoodsWindow);
            GoodsWindow.Closed -= GoodsWindow_Closed;
            GoodsWindow = null;
        }

        #endregion

        #region Management of the 'Initial Stocks' operations

        /// <summary>
        /// Manage the option 'Tipus d'Efecte' of the Application
        /// </summary>
        private void ManageActualitzarEstocInicial()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (StocksWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.StocksWindow);
                    StocksWindow = new Stocks(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Stocks
                    };
                    StocksWindow.Closed += StocksWindow_Closed;
                    StocksWindow.Show();
                    Active_Windows.Add(StocksWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió d'Estocs.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (StocksWindow.WindowState == WindowState.Minimized) StocksWindow.WindowState = WindowState.Normal;
                StocksWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the StocksWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void StocksWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(StocksWindow);
            StocksWindow.Closed -= StocksWindow_Closed;
            StocksWindow = null;
        }

        #endregion

        #endregion

        #region Specific Managers for options of Menu Orders ToDo: PROVIDERS GestioComandesProveidors/CompararComandes

        #region [PROVIDERS] : Management of the Issuance Supplier Orders Gestió Comandes Proveidors

        /// <summary>
        /// Manage the option 'Manage of Providers Orders' of the Application
        /// </summary>
        private void ManageGestioComandesProveidors()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (ProviderOrdersManagementWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ProviderOrdersWindow);
                    ProviderOrdersManagementWindow = new ProviderOrders(AppType, false, true, false )
                    {
                        Providers = GlobalViewModel.Instance.HispaniaViewModel.ProvidersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders
                    };
                    ProviderOrdersManagementWindow.Closed += ProviderOrdersManagementWindow_Closed;
                    ProviderOrdersManagementWindow.Show();
                    Active_Windows.Add(ProviderOrdersManagementWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió de Comandes de Proveidors.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (ProviderOrdersManagementWindow.WindowState == WindowState.Minimized)ProviderOrdersManagementWindow.WindowState = WindowState.Normal;
                ProviderOrdersManagementWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ProviderOrdersManagementWindow_Closed(object sender, EventArgs e)
        {
            ProviderOrdersManagementWindow.Closed -= ProviderOrdersManagementWindow_Closed;
            ProviderOrdersManagementWindow = null;
        }

        #endregion

        #region [PROVIDERS] : Management of the Issuance Supplier Orders Comparar Comandes

        /// <summary>
        /// Manage the option 'Manage of Compare Customer Order' of the Application
        /// </summary>
        private void ManageCompararComandes()
        {
            //if (CompareCustomerOrdersWindow == null)
            //{
            //    CompareCustomerOrdersWindow = new CompareCustomerOrders(AppType);
            //    GlobalViewModel.Instance.HispaniaViewModel.RefreshCompareCustomerOrders();
            //    CompareCustomerOrdersWindow.DataList = GlobalViewModel.Instance.HispaniaViewModel.CompareCustomerOrders;
            //    GlobalViewModel.Instance.HispaniaViewModel.RefreshProviders();
            //    CompareCustomerOrdersWindow.Providers = GlobalViewModel.Instance.HispaniaViewModel.ProvidersDict;
            //    GlobalViewModel.Instance.HispaniaViewModel.RefreshMovementsOrdersSuppliersGestioComandesProveidors();
            //    CompareCustomerOrdersWindow.MovementsOrdersSuppliers = GlobalViewModel.Instance.HispaniaViewModel.MovementsOrdersSuppliersGestioComandesProveidors;
            //    CompareCustomerOrdersWindow.Closed += CompareCustomerOrdersWindow_Closed;
            //    CompareCustomerOrdersWindow.Show();
            //}
            //else UnitsWindow.Activate();
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CompareCustomerOrdersWindow_Closed(object sender, EventArgs e)
        {
            //CompareCustomerOrdersWindow.Closed -= CompareCustomerOrdersWindow_Closed;
            //CompareCustomerOrdersWindow = null;
        }

        #endregion

        #region Management of the Customer Orders

        /// <summary>
        /// Manage the option 'Gestionar Comandes de Clients' of the Application
        /// </summary>
        private void ManageGestioComandesClients()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (CustomerOrdersMangementWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.CustomerOrdersWindow);
                    CustomerOrdersMangementWindow = new CustomerOrders(AppType, false, true, false)
                    {
                        Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders
                    };
                    CustomerOrdersMangementWindow.Closed += CustomerOrdersWindow_Closed;
                    CustomerOrdersMangementWindow.Show();
                    Active_Windows.Add(CustomerOrdersMangementWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió de Comandes de Client.\r\nDetalls: {0}",  MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (CustomerOrdersMangementWindow.WindowState == WindowState.Minimized) CustomerOrdersMangementWindow.WindowState = WindowState.Normal;
                CustomerOrdersMangementWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomerOrdersWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(CustomerOrdersMangementWindow);
            CustomerOrdersMangementWindow.Closed -= CustomerOrdersWindow_Closed;
            CustomerOrdersMangementWindow = null;
        }

        #endregion

        #region Management of Delivery Notes

        /// <summary>
        /// Manage the option 'Gestionar Albarans' of the Application
        /// </summary>
        private void ManageGestioAlbarans()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (DeliveryNotesManagementWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesWindow);
                    DeliveryNotesManagementWindow = new CustomerOrders(AppType, false, false, true)
                    {
                        Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders
                    };
                    DeliveryNotesManagementWindow.Closed += DeliveryNotesManagementWindow_Closed;
                    DeliveryNotesManagementWindow.Show();
                    Active_Windows.Add(DeliveryNotesManagementWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió d'Albarans.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (DeliveryNotesManagementWindow.WindowState == WindowState.Minimized) DeliveryNotesManagementWindow.WindowState = WindowState.Normal;
                DeliveryNotesManagementWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the DeliveryNotesManagementWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void DeliveryNotesManagementWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(DeliveryNotesManagementWindow);
            DeliveryNotesManagementWindow.Closed -= DeliveryNotesManagementWindow_Closed;
            DeliveryNotesManagementWindow = null;
        }

        #endregion

        #region Print Customer Orders

        /// <summary>
        /// Manage the option 'Gestionar Comandes de Clients' of the Application
        /// </summary>
        private void ManageImpressioComandesClients()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (DeliveryNotesPrintWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesPrintWindow);
                    DeliveryNotesPrintWindow = new CustomerOrders(AppType, true, false, true)
                    {
                        Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders
                    };
                    DeliveryNotesPrintWindow.Closed += CustomerOrdersPrintWindow_Closed;

                    DeliveryNotesPrintWindow.Show();
                    DeliveryNotesPrintWindow.Activate();
                    
                    Active_Windows.Add(DeliveryNotesPrintWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari d'Impressió d'Albarans i Creació de Factures.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (DeliveryNotesPrintWindow.WindowState == WindowState.Minimized)
                    DeliveryNotesPrintWindow.WindowState = WindowState.Normal;
                
                DeliveryNotesPrintWindow.Show();
                DeliveryNotesPrintWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomerOrdersPrintWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(DeliveryNotesPrintWindow);
            DeliveryNotesPrintWindow.Closed -= CustomerOrdersPrintWindow_Closed;
            DeliveryNotesPrintWindow = null;
        }

        #endregion

        #endregion

        #region Specific Managers for options of Menu Bills

        #region Management of the Bill operations

        #region Bills Management

        /// <summary>
        /// Manage the option 'Gestionar Factures' of the Application
        /// </summary>
        private void ManageGestioFactures()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (BillsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BillsWindow);
                    BillsWindow = new Bills(AppType)
                    {
                        Print = false,
                        Selection = false,
                        SelectedBill = null,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        BillingSeries = GlobalViewModel.Instance.HispaniaViewModel.BillingSeriesDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsActiveDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills
                    };
                    BillsWindow.Closed += BillsWindow_Closed;
                    BillsWindow.Show();
                    Active_Windows.Add(BillsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Gestió de Factures.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (BillsWindow.WindowState == WindowState.Minimized) BillsWindow.WindowState = WindowState.Normal;
                BillsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Bill Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void BillsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(BillsWindow);
            BillsWindow.Closed -= BillsWindow_Closed;
            BillsWindow = null;
        }

        #endregion

        #region Bills Print

        /// <summary>
        /// Manage the option 'Gestionar Factures' of the Application
        /// </summary>
        private void ManageImpressioFactures()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (BillsPrintWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BillsPrintWindow);
                    BillsPrintWindow = new Bills(AppType)
                    {
                        Print = true,
                        Selection = false,
                        SelectedBill = null,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        BillingSeries = GlobalViewModel.Instance.HispaniaViewModel.BillingSeriesDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsActiveDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills
                    };
                    BillsPrintWindow.Closed += BillsPrintWindow_Closed;
                    BillsPrintWindow.Show();
                    Active_Windows.Add(BillsPrintWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari d'Impressió de Factures.\r\nDetalls: {0}",
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (BillsPrintWindow.WindowState == WindowState.Minimized) BillsPrintWindow.WindowState = WindowState.Normal;
                BillsPrintWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Bill Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void BillsPrintWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(BillsPrintWindow);
            BillsPrintWindow.Closed -= BillsPrintWindow_Closed;
            BillsPrintWindow = null;
        }

        #endregion

        #endregion

        #region Management of the Mismatches Receipts operations

        /// <summary>
        /// Manage the option 'Revisa desquadres de venciments' of the Application
        /// </summary>
        private void ManageRevisaDesquadresVenciment()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (MismatchesReceiptsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.MismatchesReceiptsWindow);
                    MismatchesReceiptsWindow = new MismatchesReceipts(AppType)
                    {
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills
                    };
                    MismatchesReceiptsWindow.Closed += MismatchesReceiptsWindow_Closed;
                    MismatchesReceiptsWindow.Show();
                    Active_Windows.Add(MismatchesReceiptsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Desquadres de Venciment.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (MismatchesReceiptsWindow.WindowState == WindowState.Minimized) MismatchesReceiptsWindow.WindowState = WindowState.Normal;
                MismatchesReceiptsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Bill Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void MismatchesReceiptsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(MismatchesReceiptsWindow);
            MismatchesReceiptsWindow.Closed -= MismatchesReceiptsWindow_Closed;
            MismatchesReceiptsWindow = null;
        }

        #endregion

        #region Management of Bad Debts

        /// <summary>
        /// Manage the option 'Manage of Unpaids' of the Application
        /// </summary>
        private void ManageGestioImpagats()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (BadDebtsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BadDebtsWindow);
                    BadDebtsWindow = new BadDebts(AppType)
                    {
                        Print = false,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.BadDebts,
                        Data = null
                    };
                    BadDebtsWindow.Closed += BadDebtsWindow_Closed;
                    BadDebtsWindow.Show();
                    Active_Windows.Add(BadDebtsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir el formulari de Gestió dels Impagats.\r\nDetalls: {0}",MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (BadDebtsWindow.WindowState == WindowState.Minimized) BadDebtsWindow.WindowState = WindowState.Normal;
                BadDebtsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BadDebtsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(BadDebtsWindow);
            BadDebtsWindow.Closed -= BadDebtsWindow_Closed;
            BadDebtsWindow = null;
        }

        #endregion

        #region Management of Settlements Gestió de Liquidacions

        /// <summary>
        /// Manage the option 'Manage of Settlements' of the Application
        /// </summary>
        private void ManageGestioLiquidacions()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SettlementsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.SettlementsWindow);
                    SettlementsWindow = new Settlements(AppType)
                    {
                        Bills = GlobalViewModel.Instance.HispaniaViewModel.BillsDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict
                    };
                    SettlementsWindow.Closed += SettlementsWindow_Closed;
                    SettlementsWindow.Show();
                    Active_Windows.Add(SettlementsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Liquidacions.\r\nDetalls: {0}",
                                      MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (SettlementsWindow.WindowState == WindowState.Minimized) SettlementsWindow.WindowState = WindowState.Normal;
                SettlementsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void SettlementsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(SettlementsWindow);
            SettlementsWindow.Closed -= SettlementsWindow_Closed;
            SettlementsWindow = null;
        }

        #endregion

        #endregion

        #region Specific Managers for options of Menu Queries

        #region Manager Queries

        /// <summary>
        /// Manage the option 'Consultes ...' of the Application
        /// </summary>
        private void ManageGestioConsultes(QueryType queryType)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (QueriesWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData( WindowToRefresh.QueriesWindow );
                    QueriesWindow = new Queries( AppType, queryType )
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Agents
                    };
                    QueriesWindow.Closed += QueriesWindow_Closed;
                    QueriesWindow.Show();
                    Active_Windows.Add( QueriesWindow );
                } catch(Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format( "Error, a l'obrir el formulari de Consultes.\r\nDetalls: {0}", MsgManager.ExcepMsg( ex ) ) );
                }
            }
            else
            {
                if (QueriesWindow.WindowState == WindowState.Minimized) QueriesWindow.WindowState = WindowState.Normal;
                QueriesWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Query Window is closed.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void QueriesWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(QueriesWindow);
            QueriesWindow.Closed -= QueriesWindow_Closed;
            QueriesWindow = null;
        }

        #endregion

        #endregion

        #region Specific Managers for options of Menu Warehouse

        #region Adding Warehouse Movements 

        /// <summary>
        /// Manage the option 'Revisa desquadres de venciments' of the Application
        /// </summary>
        private void ManageAfegirMovimentsMagatzem()
        {
            if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister("WarehouseMovementsAdd", out string ErrMsg))
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (WarehouseMovementsAddWindow == null)
                {
                    try
                    {
                        RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.WarehouseMovementsAddWindow);
                        WarehouseMovementsAddWindow = new WarehouseMovementsAdd()
                        {
                            DataGoods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                            DataProviders = GlobalViewModel.Instance.HispaniaViewModel.ProvidersActiveDict,
                        };
                        WarehouseMovementsAddWindow.Closed += WarehouseMovementsAddWindow_Closed;
                        WarehouseMovementsAddWindow.Show();
                        Active_Windows.Add(WarehouseMovementsAddWindow);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'obrir el formulari que afegix moviments de magatzem.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                        GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister("WarehouseMovementsAdd", out ErrMsg);
                    }
                }
                else
                {
                    if (WarehouseMovementsAddWindow.WindowState == WindowState.Minimized) WarehouseMovementsAddWindow.WindowState = WindowState.Normal;
                    WarehouseMovementsAddWindow.Activate();
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            else MsgManager.ShowMessage(ErrMsg);
        }

        /// <summary>
        /// When the Warehouse Movement Add Window is closed we actualize the Warehose Movement Add attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void WarehouseMovementsAddWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(WarehouseMovementsAddWindow);
            WarehouseMovementsAddWindow.Closed -= WarehouseMovementsAddWindow_Closed;
            WarehouseMovementsAddWindow = null;
            GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister("WarehouseMovementsAdd", out string ErrMsg);
        }

        #endregion

        #region Management of the Warehouse Movements operations

        /// <summary>
        /// Manage the option 'Revisa desquadres de venciments' of the Application
        /// </summary>
        private void ManageGestioMovimentsMagatzem()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (WarehouseMovementsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.WarehouseMovementsWindow);
                    WarehouseMovementsWindow = new WarehouseMovements(AppType)
                    {
                        Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                        Providers = GlobalViewModel.Instance.HispaniaViewModel.ProvidersDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.WarehouseMovements
                    };
                    WarehouseMovementsWindow.Closed += WarehouseMovementsWindow_Closed;
                    WarehouseMovementsWindow.Show();
                    Active_Windows.Add(WarehouseMovementsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                        string.Format("Error, a l'obrir el formulari que gestiona els moviments de magatzem.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (WarehouseMovementsWindow.WindowState == WindowState.Minimized) WarehouseMovementsWindow.WindowState = WindowState.Normal;
                WarehouseMovementsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Warehouse Movement Window is closed we actualize the Warehose Movement attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void WarehouseMovementsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(WarehouseMovementsWindow);
            WarehouseMovementsWindow.Closed -= WarehouseMovementsWindow_Closed;
            WarehouseMovementsWindow = null;
        }

        #endregion

        #endregion

        #region Specific Managers for options of Menu Revisions

        #region Management of Bill Revisions

        /// <summary>
        /// Manage the option 'Manage of GoodRevisions' of the Application
        /// </summary>
        private void ManageRevisioFactues()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (GoodRevisionsWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BillRevisionsWindow);
                    GoodRevisionsWindow = new GoodRevisions(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Goods
                    };
                    GoodRevisionsWindow.Closed += GoodRevisionsWindow_Closed;
                    GoodRevisionsWindow.Show();
                    Active_Windows.Add(GoodRevisionsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari que .\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (GoodRevisionsWindow.WindowState == WindowState.Minimized) GoodRevisionsWindow.WindowState = WindowState.Normal;
                GoodRevisionsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void GoodRevisionsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(GoodRevisionsWindow);
            GoodRevisionsWindow.Closed -= GoodRevisionsWindow_Closed;
            GoodRevisionsWindow = null;
        }

        #endregion

        #region Management of Available Mismatches 

        /// <summary>
        /// Manage the option 'Manage of MismatchesAvilables' of the Application
        /// </summary>
        private void ManageDesquadresEstocsDisponibles()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (MismatchesAvailableWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.MismatchesAvailableWindow);
                    MismatchesAvailableWindow = new Revisions(AppType, RevisionsType.MismatchesAvailable)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions
                    };
                    MismatchesAvailableWindow.Closed += MismatchesAvailableWindow_Closed;
                    MismatchesAvailableWindow.Show();
                    Active_Windows.Add(MismatchesAvailableWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Desquadres d'Estocs Disponibles .\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (MismatchesAvailableWindow.WindowState == WindowState.Minimized) MismatchesAvailableWindow.WindowState = WindowState.Normal;
                MismatchesAvailableWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void MismatchesAvailableWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(MismatchesAvailableWindow);
            MismatchesAvailableWindow.Closed -= MismatchesAvailableWindow_Closed;
            MismatchesAvailableWindow = null;
        }

        #endregion

        #region Management of Stocks Mismatches 

        /// <summary>
        /// Manage the option 'Manage of MismatchesStocs' of the Application
        /// </summary>
        private void ManageDesquadresEstocsExistencies()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (MismatchesStocksWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.MismatchesStocksWindow);
                    MismatchesStocksWindow = new Revisions(AppType, RevisionsType.MismatchesStocks)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions
                    };
                    MismatchesStocksWindow.Closed += MismatchesStocsWindow_Closed;
                    MismatchesStocksWindow.Show();
                    Active_Windows.Add(MismatchesStocksWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Desquadres d'Estocs Existències.\r\nDetalls: {0}",  MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (MismatchesStocksWindow.WindowState == WindowState.Minimized) MismatchesStocksWindow.WindowState = WindowState.Normal;
                MismatchesStocksWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void MismatchesStocsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(MismatchesStocksWindow);
            MismatchesStocksWindow.Closed -= MismatchesStocsWindow_Closed;
            MismatchesStocksWindow = null;
        }

        #endregion

        #region Management of General Available

        /// <summary>
        /// Manage the option 'Manage of GeneralAvailables' of the Application
        /// </summary>
        private void ManageGeneralDisponible()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (GeneralAvailableWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.GeneralAvailableWindow);
                    GeneralAvailableWindow = new Revisions(AppType, RevisionsType.GeneralAvailable)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions
                    };
                    GeneralAvailableWindow.Closed += GeneralAvailableWindow_Closed;
                    GeneralAvailableWindow.Show();
                    Active_Windows.Add(GeneralAvailableWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Revisió de Disponible.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (GeneralAvailableWindow.WindowState == WindowState.Minimized) GeneralAvailableWindow.WindowState = WindowState.Normal;
                GeneralAvailableWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void GeneralAvailableWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(GeneralAvailableWindow);
            GeneralAvailableWindow.Closed -= GeneralAvailableWindow_Closed;
            GeneralAvailableWindow = null;
        }

        #endregion

        #region Management of General Stocs

        /// <summary>
        /// Manage the option 'Manage of GeneralStocss' of the Application
        /// </summary>
        private void ManageGeneralExistencies()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (GeneralStocksWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.GeneralStocksWindow);
                    GeneralStocksWindow = new Revisions(AppType, RevisionsType.GeneralStocks)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions
                    };
                    GeneralStocksWindow.Closed += GeneralStocsWindow_Closed;
                    GeneralStocksWindow.Show();
                    Active_Windows.Add(GeneralStocksWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Revisió d'Existències.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (GeneralStocksWindow.WindowState == WindowState.Minimized) GeneralStocksWindow.WindowState = WindowState.Normal;
                GeneralStocksWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void GeneralStocsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(GeneralStocksWindow);
            GeneralStocksWindow.Closed -= GeneralStocsWindow_Closed;
            GeneralStocksWindow = null;
        }

        #endregion

        #endregion

        #region Specific Managers for options of Menu Listings

        #region Management of Stock Takings List

        /// <summary>
        /// Manage the option 'Manage of Stock Takings List' of the Application
        /// </summary>
        private void ManageLlistatInventaris()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (StockTakingsListWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
                    StockTakingsListWindow = new StockTakingsList(AppType)
                    {
                        Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict
                    };
                    StockTakingsListWindow.Closed += StockTakingsListWindow_Closed;
                    StockTakingsListWindow.Show();
                    Active_Windows.Add(StockTakingsListWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Llistat d'Inventaris.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (StockTakingsListWindow.WindowState == WindowState.Minimized) StockTakingsListWindow.WindowState = WindowState.Normal;
                StockTakingsListWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void StockTakingsListWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(StockTakingsListWindow);
            StockTakingsListWindow.Closed -= StockTakingsListWindow_Closed;
            StockTakingsListWindow = null;
        }

        #endregion

        #region Management of Ranges List

        /// <summary>
        /// Manage the option 'Manage of Ranges List' of the Application
        /// </summary>
        private void ManageLlistatMarges()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (RangesListWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshBills();
                    RangesListWindow = new RangesList(AppType)
                    {
                        Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                        Bills = GlobalViewModel.Instance.HispaniaViewModel.BillsDict,
                    };
                    RangesListWindow.Closed += RangesListWindow_Closed;
                    RangesListWindow.Show();
                    Active_Windows.Add(RangesListWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Llistat de Marges.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (RangesListWindow.WindowState == WindowState.Minimized) RangesListWindow.WindowState = WindowState.Normal;
                RangesListWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void RangesListWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(RangesListWindow);
            RangesListWindow.Closed -= RangesListWindow_Closed;
            RangesListWindow = null;
        }

        #endregion

        #region Management of BadDebts List

        /// <summary>
        /// Manage the option 'Manage of Unpaids List' of the Application
        /// </summary>
        private void ManageLlistatImpagats()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (BadDebtsListWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BadDebtsWindow);
                    BadDebtsListWindow = new BadDebts(AppType)
                    {
                        Print = true,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.BadDebts,
                        Data = null
                    };
                    BadDebtsListWindow.Closed += BadDebtsListWindow_Closed;
                    BadDebtsListWindow.Show();
                    Active_Windows.Add(BadDebtsListWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir el formulari d'Impresió dels Impagats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (BadDebtsListWindow.WindowState == WindowState.Minimized) BadDebtsListWindow.WindowState = WindowState.Normal;
                BadDebtsListWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BadDebtsListWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(BadDebtsListWindow);
            BadDebtsListWindow.Closed -= BadDebtsListWindow_Closed;
            BadDebtsListWindow = null;
        }

        #endregion

        #region Management of Customer Order Movement List

        /// <summary>
        /// Manage the option 'Manage of Customer Order Movements List' of the Application
        /// </summary>
        private void ManageLlistatLiniesAlbara()
        {
            //  The function called manage the possible exception thats produced.
                Mouse.OverrideCursor = Cursors.Wait;
                DeliveryNoteLinesReportView.CreateReport();
                Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion

        #region Management of Diary Bandages List

        /// <summary>
        /// Manage the option 'Manage of DiaryBandagesList' of the Application
        /// </summary>
        private void ManageLlistatDiariVendes()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (DiaryBandagesListWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshBills();
                    DiaryBandagesListWindow = new DiaryBandagesList(AppType)
                    {
                        Bills = GlobalViewModel.Instance.HispaniaViewModel.BillsDict
                    };
                    DiaryBandagesListWindow.Closed += DiaryBandagesListWindow_Closed;
                    DiaryBandagesListWindow.Show();
                    Active_Windows.Add(DiaryBandagesListWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Llistat de Diari de Vendes .\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (DiaryBandagesListWindow.WindowState == WindowState.Minimized) DiaryBandagesListWindow.WindowState = WindowState.Normal;
                DiaryBandagesListWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the DiaryBandages Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void DiaryBandagesListWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(DiaryBandagesListWindow);
            DiaryBandagesListWindow.Closed -= DiaryBandagesListWindow_Closed;
            DiaryBandagesListWindow = null;
        }

        #endregion

        #region Management of Diary Bandages and Accounting List

        /// <summary>
        /// Manage the option 'Manage of Diary Bandages and Accounting List' of the Application
        /// </summary>
        private void ManageLlistatDiariVendesComptabilitat()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (DiaryBandagesAndAccountingsListWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshBills();
                    DiaryBandagesAndAccountingsListWindow = new DiaryBandagesAndAccountingsList(AppType)
                    {
                        Bills = GlobalViewModel.Instance.HispaniaViewModel.BillsDict
                    };
                    DiaryBandagesAndAccountingsListWindow.Closed += DiaryBandagesAndAccountingsList_Closed;
                    DiaryBandagesAndAccountingsListWindow.Show();
                    Active_Windows.Add(DiaryBandagesAndAccountingsListWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de llistat diari de Vendes Comptabilitat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (DiaryBandagesAndAccountingsListWindow.WindowState == WindowState.Minimized) DiaryBandagesAndAccountingsListWindow.WindowState = WindowState.Normal;
                DiaryBandagesAndAccountingsListWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void DiaryBandagesAndAccountingsList_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(DiaryBandagesAndAccountingsListWindow);
            DiaryBandagesAndAccountingsListWindow.Closed -= DiaryBandagesAndAccountingsList_Closed;
            DiaryBandagesAndAccountingsListWindow = null;
        }

        #endregion

        #endregion

        #region Specific Managers for options of Menu Closing

        #region Management of Customer Sales List

        /// <summary>
        /// Manage the option 'Manage of Customer Sales List' of the Application
        /// </summary>
        private void ManageInformeVendesSuperiorA()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (CustomersSalesListWindow == null)
            {
                try
                {
                    CustomersSalesListWindow = new CustomersSalesList(AppType);
                    CustomersSalesListWindow.Closed += CustomersSalesListWindow_Closed;
                    CustomersSalesListWindow.Show();
                    Active_Windows.Add(CustomersSalesListWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de l'Informe de Vendes Superior A.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (CustomersSalesListWindow.WindowState == WindowState.Minimized) CustomersSalesListWindow.WindowState = WindowState.Normal;
                CustomersSalesListWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomersSalesListWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(CustomersSalesListWindow);
            CustomersSalesListWindow.Closed -= CustomersSalesListWindow_Closed;
            CustomersSalesListWindow = null;
        }

        #endregion

        #region Management of Historic Acum Client

        /// <summary>
        /// Manage the option 'Manage of Historic Acum Client' of the Application
        /// </summary>
        private void ManageHistoricsAcumulatClient()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (HistoricAcumCustomersWindow == null)
            {
                try
                {
                    HistoricAcumCustomersWindow = new HistoricAcumCustomers(AppType);
                    HistoricAcumCustomersWindow.Closed += HistoricAcumCustomersWindow_Closed;
                    HistoricAcumCustomersWindow.Show();
                    Active_Windows.Add(HistoricAcumCustomersWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de l'Històric Acumulat de Client .\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (HistoricAcumCustomersWindow.WindowState == WindowState.Minimized) HistoricAcumCustomersWindow.WindowState = WindowState.Normal;
                HistoricAcumCustomersWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void HistoricAcumCustomersWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(HistoricAcumCustomersWindow);
            HistoricAcumCustomersWindow.Closed -= HistoricAcumCustomersWindow_Closed;
            HistoricAcumCustomersWindow = null;
        }

        #endregion

        #region Management of Historic Acum Article

        /// <summary>
        /// Manage the option 'Manage of Historic Acum Article' of the Application
        /// </summary>
        private void ManageHistoricsAcumulatArticle()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (HistoricAcumGoodsWindow == null)
            {
                try
                {
                    HistoricAcumGoodsWindow = new HistoricAcumGoods(AppType);
                    HistoricAcumGoodsWindow.Closed += HistoricAcumGoodsWindow_Closed;
                    HistoricAcumGoodsWindow.Show();
                    Active_Windows.Add(HistoricAcumGoodsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de l'Històric Acumulat d'Artícles.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (HistoricAcumGoodsWindow.WindowState == WindowState.Minimized) HistoricAcumGoodsWindow.WindowState = WindowState.Normal;
                HistoricAcumGoodsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void HistoricAcumGoodsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(HistoricAcumGoodsWindow);
            HistoricAcumGoodsWindow.Closed -= HistoricAcumGoodsWindow_Closed;
            HistoricAcumGoodsWindow = null;
        }

        #endregion

        #region Management of Remove Warehouse Movement
        
        /// <summary>
        /// Manage the option 'Manage of Historic Acum Article' of the Application
        /// </summary>
        private void ManageEsborrarMovimentsMagatzem()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (RemoveWarehouseMovementsWindow == null)
            {
                try
                {
                    RemoveWarehouseMovementsWindow = new RemoveWarehouseMovements(AppType);
                    RemoveWarehouseMovementsWindow.Closed += RemoveWarehouseMovementsWindow_Closed;
                    RemoveWarehouseMovementsWindow.Show();
                    Active_Windows.Add(RemoveWarehouseMovementsWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari que Esborra els Moviments de Magatzem.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (RemoveWarehouseMovementsWindow.WindowState == WindowState.Minimized) RemoveWarehouseMovementsWindow.WindowState = WindowState.Normal;
                RemoveWarehouseMovementsWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void RemoveWarehouseMovementsWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(RemoveWarehouseMovementsWindow);
            RemoveWarehouseMovementsWindow.Closed -= RemoveWarehouseMovementsWindow_Closed;
            RemoveWarehouseMovementsWindow = null;
        }

        #endregion

        #region Management of Transfer Initial Stocks

        /// <summary>
        /// Manage the option 'Manage of Transfer Initial Stocks' of the Application
        /// </summary>
        private void ManageTraspassarEstocsInicials()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (TransferInitialStocksWindow == null)
            {
                try
                {
                    TransferInitialStocksWindow = new TransferInitialStocks(AppType);
                    TransferInitialStocksWindow.Closed += TransferInitialStocksWindow_Closed;
                    TransferInitialStocksWindow.Show();
                    Active_Windows.Add(TransferInitialStocksWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari que Transfereix els Estocs Inicials.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (TransferInitialStocksWindow.WindowState == WindowState.Minimized) TransferInitialStocksWindow.WindowState = WindowState.Normal;
                TransferInitialStocksWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the TransferInitialStocks Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void TransferInitialStocksWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(TransferInitialStocksWindow);
            TransferInitialStocksWindow.Closed -= TransferInitialStocksWindow_Closed;
            TransferInitialStocksWindow = null;
        }

        #endregion

        #region Management of Transfer Customer Orders

        /// <summary>
        /// Manage the option 'Traspassar Comandes de Clients' of the Application
        /// </summary>
        private void ManageTraspassarComandesClient()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (CustomerOrdersTransferWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.CustomerOrdersWindow);
                    CustomerOrdersTransferWindow = new CustomerOrders(AppType, false, true, false, true)
                    {
                        Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders
                    };
                    CustomerOrdersTransferWindow.Closed += CustomerOrdersTransferWindow_Closed;
                    CustomerOrdersTransferWindow.Show();
                    Active_Windows.Add(CustomerOrdersTransferWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Traspàs de Comandes de Client.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (CustomerOrdersTransferWindow.WindowState == WindowState.Minimized) CustomerOrdersTransferWindow.WindowState = WindowState.Normal;
                CustomerOrdersTransferWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomerOrdersTransferWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(CustomerOrdersTransferWindow);
            CustomerOrdersTransferWindow.Closed -= CustomerOrdersTransferWindow_Closed;
            CustomerOrdersTransferWindow = null;
        }

        #endregion

        #region Management of Transfer of Delivery Notes

        /// <summary>
        /// Manage the option 'Gestionar Albarans' of the Application
        /// </summary>
        private void ManageTraspassarAlbarans()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (DeliveryNotesTransferWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesWindow);
                    DeliveryNotesTransferWindow = new CustomerOrders(AppType, false, false, true, true)
                    {
                        Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders
                    };
                    DeliveryNotesTransferWindow.Closed += DeliveryNotesTransferWindow_Closed;
                    DeliveryNotesTransferWindow.Show();
                    Active_Windows.Add(DeliveryNotesTransferWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari de Traspàs d'Albarans.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (DeliveryNotesTransferWindow.WindowState == WindowState.Minimized) DeliveryNotesTransferWindow.WindowState = WindowState.Normal;
                DeliveryNotesTransferWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the DeliveryNotesTransferWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void DeliveryNotesTransferWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(DeliveryNotesTransferWindow);
            DeliveryNotesTransferWindow.Closed -= DeliveryNotesTransferWindow_Closed;
            DeliveryNotesTransferWindow = null;
        }

        #endregion

        #region Management of Reset Bill Number

        /// <summary>
        /// Manage the option 'Manage of Reset Bill Number' of the Application
        /// </summary>
        private void ManageResetejarNumeroFactura()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (ResetBillNumbersWindow == null)
            {
                try
                {
                    ResetBillNumbersWindow = new ResetBillNumbers(AppType);
                    ResetBillNumbersWindow.Closed += ResetBillNumbersWindow_Closed;
                    ResetBillNumbersWindow.Show();
                    Active_Windows.Add(ResetBillNumbersWindow);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir el formulari que Reseteja el Comptador de Factures.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else
            {
                if (ResetBillNumbersWindow.WindowState == WindowState.Minimized) ResetBillNumbersWindow.WindowState = WindowState.Normal;
                ResetBillNumbersWindow.Activate();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the BillingSeries Window is closed we actualize the UnitsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ResetBillNumbersWindow_Closed(object sender, EventArgs e)
        {
            Active_Windows.Remove(ResetBillNumbersWindow);
            ResetBillNumbersWindow.Closed -= ResetBillNumbersWindow_Closed;
            ResetBillNumbersWindow = null;
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }
}