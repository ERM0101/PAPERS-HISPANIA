#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Trace;
using System;
using System.Collections.Generic;

#endregion

namespace HispaniaCommon.ViewModel
{
    public enum WindowToRefresh
    {
        #region Menu Tables
        ParametersWindow,               
        BillingSeriesWindow,   
        PostalCodesWindow,
        IVATypesWindow,
        SendTypesWindow,
        AgentsWindow,
        EffectTypesWindow,
        CustomersWindow,
        CustomersPrintWindow,
        ProvidersWindow,
        ProvidersPrintWindow,
        #endregion
        #region Menu Goods
        UnitsWindow,
        GoodsWindow,
        StocksWindow,
        #endregion
        #region Menu Orders
        CustomerOrdersWindow,
        DeliveryNotesWindow,
        DeliveryNotesPrintWindow,
        ProviderOrdersWindow,
        #endregion
        #region Menu Bills
        BillsWindow,
        BillsPrintWindow,
        MismatchesReceiptsWindow,
        BadDebtsWindow,
        SettlementsWindow,
        #endregion
        #region Menu Queries
        QueriesWindow,
        #endregion
        #region Menu Warehouse Movements
        WarehouseMovementsAddWindow,
        WarehouseMovementsWindow,
        #endregion
        #region Menu Revisions
        BillRevisionsWindow,
        MismatchesAvailableWindow,
        MismatchesStocksWindow,
        GeneralAvailableWindow,
        GeneralStocksWindow,
        #endregion
        None,
    }

    public class RefreshDataViewModel
    {
        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static RefreshDataViewModel instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static RefreshDataViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RefreshDataViewModel();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private RefreshDataViewModel()
        {
        }

        #endregion

        #region Main Method

        public void RefreshData(WindowToRefresh windowToRefresh, object[] Params = null)
        {
            try
            {
                switch (windowToRefresh)
                {
                    #region Menu Tables
                    case WindowToRefresh.ParametersWindow:
                         RefreshParametersData();
                         break;
                    case WindowToRefresh.BillingSeriesWindow:
                         RefreshBillingSeriesData();
                         break;
                    case WindowToRefresh.PostalCodesWindow:
                         RefreshPostalCodesData();
                         break;
                    case WindowToRefresh.IVATypesWindow:
                         RefreshIVATypesData();
                         break;
                    case WindowToRefresh.SendTypesWindow:
                         RefreshSendTypesData();
                         break;
                    case WindowToRefresh.AgentsWindow:
                         RefreshAgentsData();
                         break;
                    case WindowToRefresh.EffectTypesWindow:
                         RefreshEffectTypesData();
                         break;
                    case WindowToRefresh.CustomersWindow:
                    case WindowToRefresh.CustomersPrintWindow:
                         RefreshCustomersData();
                         break;
                    case WindowToRefresh.ProvidersWindow:
                    case WindowToRefresh.ProvidersPrintWindow:
                         RefreshProvidersData();
                         break;
                    #endregion
                    #region Menu Goods
                    case WindowToRefresh.UnitsWindow:
                         RefreshUnitsData();
                         break;
                    case WindowToRefresh.GoodsWindow:
                         RefreshGoodsData();
                         break;
                    case WindowToRefresh.StocksWindow:
                         RefreshStocksData();
                         break;
                    #endregion
                    #region Menu Orders
                    case WindowToRefresh.CustomerOrdersWindow:
                    case WindowToRefresh.DeliveryNotesWindow:
                    case WindowToRefresh.DeliveryNotesPrintWindow:
                         RefreshCustomerOrdersData();
                         break;
                    case WindowToRefresh.ProviderOrdersWindow:
                         RefreshSupplierOrdersData();
                         break;
                    #endregion
                    #region Bills
                    case WindowToRefresh.BillsWindow:
                    case WindowToRefresh.BillsPrintWindow:
                         RefreshBillsData();
                         break;
                    case WindowToRefresh.MismatchesReceiptsWindow:
                         RefreshMismatchesReceiptsData();
                         break;
                    case WindowToRefresh.BadDebtsWindow:
                         RefreshBadDebtsData();
                         break;
                    case WindowToRefresh.SettlementsWindow:
                         RefreshSettlementsData();
                         break;
                    #endregion
                    #region Menu Queries
                    case WindowToRefresh.QueriesWindow:
                         RefreshQueriesData();
                         break;
                    #endregion
                    #region Menu Warehouse Movements
                    case WindowToRefresh.WarehouseMovementsAddWindow:
                         RefreshWarehouseMovementsAddData();
                         break;
                    case WindowToRefresh.WarehouseMovementsWindow:
                         RefreshWarehouseMovementsData();
                         break;
                    #endregion
                    #region Menu Revisions
                    case WindowToRefresh.BillRevisionsWindow:
                         RefreshBillRevisionsData();
                         break;
                    case WindowToRefresh.MismatchesAvailableWindow:
                        RefreshMismatchesAvailableData();
                        break;
                    case WindowToRefresh.MismatchesStocksWindow:
                         RefreshMismatchesStocksData();
                         break;
                    case WindowToRefresh.GeneralAvailableWindow:
                         RefreshGeneralAvailableData();
                         break;
                    case WindowToRefresh.GeneralStocksWindow:
                         RefreshGeneralStocksData();
                         break;
                    #endregion
                    case WindowToRefresh.None:
                    default:
                        throw new Exception(
                                     string.Format("Error, al refrescar les dades d'una finestra.\r\n" +
                                                   "Detalls: finestra '{0}' no reconeguda.",
                                                   GetWindowName(windowToRefresh)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error, al refrescar les dades de la finestra '{0}'.\r\nDetalls: {1}",
                                                  GetWindowName(windowToRefresh), MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Menu Tables

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Parameters Window.
        /// </summary>
        private void RefreshParametersData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Billing Series Window.
        /// </summary>
        private void RefreshBillingSeriesData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBillingSeries();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Postal Codes Window.
        /// </summary>
        private void RefreshPostalCodesData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the IVA Types Window.
        /// </summary>
        private void RefreshIVATypesData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshIVATypes();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Send Types Window.
        /// </summary>
        private void RefreshSendTypesData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshSendTypes();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Agents Window.
        /// </summary>
        private void RefreshAgentsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Effect Types Window.
        /// </summary>
        private void RefreshEffectTypesData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshEffectTypes();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Customers Window.
        /// </summary>
        private void RefreshCustomersData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshEffectTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshSendTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshIVATypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshCustomers();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Providers Window.
        /// </summary>
        private void RefreshProvidersData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshSendTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshIVATypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshEffectTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshProviders();
        }

        #endregion

        #region Menu Goods

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Units Window.
        /// </summary>
        private void RefreshUnitsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshUnits();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Goods Window.
        /// </summary>
        private void RefreshGoodsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshUnits();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Stocks Window.
        /// </summary>
        private void RefreshStocksData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshStocks();
        }

        #endregion

        #region Menu Orders

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Customer Orders Window.
        /// </summary>
        private void RefreshCustomerOrdersData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshCustomers();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshSendTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshEffectTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshCustomerOrders();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
        }

        private void RefreshSupplierOrdersData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshProviders();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshSendTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshEffectTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshProviderOrders();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
        }

        #endregion

        #region Menu Bills

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Bills Window.
        /// </summary>
        private void RefreshBillsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBillingSeries();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBills();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshCustomerOrders();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshEffectTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Mismatches Receipts Window.
        /// </summary>
        private void RefreshMismatchesReceiptsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBillingSeries();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBills(null, null, true);
            GlobalViewModel.Instance.HispaniaViewModel.RefreshCustomerOrders();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshEffectTypes();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Unpaids Window.
        /// </summary>
        private void RefreshBadDebtsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBillingSeries();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBills();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshReceipts();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBadDebts();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBadDebtMovements();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Settlements Window.
        /// </summary>
        private void RefreshSettlementsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBillingSeries();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshBills();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
        }

        #endregion

        #region Menu Queries

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Queries Window.
        /// </summary>
        private void RefreshQueriesData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
        }

        #endregion

        #region Menu Warehouse Movements

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Warehouse Movements Add Window.
        /// </summary>
        private void RefreshWarehouseMovementsAddData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshProviders();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Warehouse Movements Window.
        /// </summary>
        private void RefreshWarehouseMovementsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshProviders();
            GlobalViewModel.Instance.HispaniaViewModel.RefreshWarehouseMovements();
        }

        #endregion

        #region Menu Revisions

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Billing Revisions Window.
        /// </summary>
        private void RefreshBillRevisionsData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Mismatches Available Window.
        /// </summary>
        private void RefreshMismatchesAvailableData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshRevisions(RevisionsType.MismatchesAvailable);
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the Mismatches Stocks Window.
        /// </summary>
        private void RefreshMismatchesStocksData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshRevisions(RevisionsType.MismatchesStocks);
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the General Available Window.
        /// </summary>
        private void RefreshGeneralAvailableData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshRevisions(RevisionsType.GeneralAvailable);
        }

        /// <summary>
        /// Method that Refresh from the Database the data needed for the General Stocks Window.
        /// </summary>
        private void RefreshGeneralStocksData()
        {
            GlobalViewModel.Instance.HispaniaViewModel.RefreshRevisions(RevisionsType.GeneralStocks);
        }

        #endregion

        #region Common Functions

        private string GetWindowName(WindowToRefresh windowToRefresh)
        {
            switch (windowToRefresh)
            {
                #region Menu Tables
                case WindowToRefresh.ParametersWindow:
                     return "Paràmetres";
                case WindowToRefresh.BillingSeriesWindow:
                     return "Sèries de Facturació";
                case WindowToRefresh.PostalCodesWindow:
                     return "Codis Postals / Poblacions";
                case WindowToRefresh.IVATypesWindow:
                     return "Tipus d'IVA";
                case WindowToRefresh.SendTypesWindow:
                     return "Tipus d'enviament";
                case WindowToRefresh.AgentsWindow:
                     return "Representants";
                case WindowToRefresh.EffectTypesWindow:
                     return "Tipus d'efectes";
                case WindowToRefresh.CustomersWindow:
                     return "Gestió de Clients";
                case WindowToRefresh.CustomersPrintWindow:
                     return "Impressió de Clients";
                case WindowToRefresh.ProvidersWindow:
                     return "Gestió de Proveïdors";
                case WindowToRefresh.ProvidersPrintWindow:
                     return "Impressió de Proveïdors";
                #endregion
                #region Menu Goods
                case WindowToRefresh.UnitsWindow:
                     return "Unitats";
                case WindowToRefresh.GoodsWindow:
                     return "Articles";
                case WindowToRefresh.StocksWindow:
                     return "Estocs";
                #endregion
                #region Menu Orders
                case WindowToRefresh.CustomerOrdersWindow:
                     return "Comandes de Client";
                case WindowToRefresh.DeliveryNotesWindow:
                     return "Albarans";
                case WindowToRefresh.DeliveryNotesPrintWindow:
                     return "Impressió d'Albarans i Creació de Factures";
                #endregion
                #region Menu Bills
                case WindowToRefresh.BillsWindow:
                     return "Gestió de Factures";
                case WindowToRefresh.BillsPrintWindow:
                     return "Impressió de Factures";
                case WindowToRefresh.BadDebtsWindow:
                     return "Impagats";
                case WindowToRefresh.SettlementsWindow:
                     return "Liquidacions";
                #endregion
                #region Menu Queries
                case WindowToRefresh.QueriesWindow:
                     return "Consultes";
                #endregion
                #region Menu Warehouse Movements
                case WindowToRefresh.WarehouseMovementsAddWindow:
                    return "Afegir Moviments de Magatzem";
                case WindowToRefresh.WarehouseMovementsWindow:
                     return "Gestionar Moviments de Magatzem";
                #endregion
                #region Menu Revisions
                case WindowToRefresh.BillRevisionsWindow:
                     return "Revisió de Factures";
                case WindowToRefresh.MismatchesAvailableWindow:
                     return "Desquadres d'Estocs Disponibles";
                case WindowToRefresh.MismatchesStocksWindow:
                     return "Desquadres d'Estocs Existències";
                case WindowToRefresh.GeneralAvailableWindow:
                     return "Revisió de Disponible";
                case WindowToRefresh.GeneralStocksWindow:
                     return "Revisió d'Existències";
                #endregion
                case WindowToRefresh.None:
                default:
                    return windowToRefresh.ToString();
            }
        }

        #endregion
    }
}
