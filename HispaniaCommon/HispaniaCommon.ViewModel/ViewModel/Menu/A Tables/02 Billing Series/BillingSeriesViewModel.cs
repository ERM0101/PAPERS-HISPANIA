#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void CreateBillingSerie(BillingSeriesView billingSeriesView)
        {
            HispaniaCompData.BillingSerie billingSerieToCreate = billingSeriesView.GetBillingSerie();
            CreateBillingSerieInDb(billingSerieToCreate);
            billingSeriesView.Serie_Id = billingSerieToCreate.Serie_Id;
            RefreshBillingSeries();
        }

        public void RefreshBillingSeries()
        {
            try
            {
                BillingSeriesInDb = HispaniaDataAccess.Instance.ReadBillingSeries();
                _BillingSeries = new ObservableCollection<BillingSeriesView>();
                _BillingSeriesInDictionary = new Dictionary<string, BillingSeriesView>();
                foreach (HispaniaCompData.BillingSerie billingSerie in BillingSeriesInDb)
                {
                    BillingSeriesView NewBillingSeriesView = new BillingSeriesView(billingSerie);
                    _BillingSeries.Add(NewBillingSeriesView);
                    _BillingSeriesInDictionary.Add(GetKeyBillingSerieView(NewBillingSeriesView), NewBillingSeriesView);
                }
            }
            catch (Exception ex)
            {
                _BillingSeries = null;
                throw ex;
            }
        }

        private ObservableCollection<BillingSeriesView> _BillingSeries = null;

        public ObservableCollection<BillingSeriesView> BillingSeries
        {
            get
            {
                return _BillingSeries;
            }
        }

        private Dictionary<string, BillingSeriesView> _BillingSeriesInDictionary = null;

        public Dictionary<string, BillingSeriesView> BillingSeriesDict
        {
            get
            {
                return _BillingSeriesInDictionary;
            }
        }

        public string GetKeyBillingSerieView(BillingSeriesView billingSerieView)
        {
            return GetKeyBillingSerieView(billingSerieView.Name, billingSerieView.Alias);
        }
        private string GetKeyBillingSerieView(HispaniaCompData.BillingSerie billingSerieView)
        {
            return GetKeyBillingSerieView(billingSerieView.Name, billingSerieView.Alias);
        }
        private string GetKeyBillingSerieView(string Name, string Alias)
        {
            return string.Format("{0} - {1}", Name, Alias);
        }
        public void UpdateBillingSerie(BillingSeriesView billingSeriesView)
        {
            if ((billingSeriesView != null) && (billingSeriesView.Serie_Id < 4))
            {
                throw new Exception(string.Format("No es pot modificar la Serie de Facturació '{0}' ja que és de Sistema.",
                                                  billingSeriesView.Name));
            }
            UpdateBillingSerieInDb(billingSeriesView.GetBillingSerie());
            RefreshBillingSeries();
        }

        public void DeleteBillingSerie(BillingSeriesView billingSeriesView)
        {
            if ((billingSeriesView != null) && (billingSeriesView.Serie_Id < 4))
            {
                throw new Exception(string.Format("No es pot esborrar la Serie de Facturació '{0}' ja que és de Sistema.",
                                                  billingSeriesView.Name));
            }
            DeleteBillingSerieInDb(billingSeriesView.GetBillingSerie());
            RefreshBillingSeries();
        }

        public BillingSeriesView GetBillingSerieFromDb(BillingSeriesView billingSeriesView)
        {
            return new BillingSeriesView(GetBillingSerieInDb(billingSeriesView.Serie_Id));
        }

        public BillingSeriesView GetBillingSerieFromDb(int Billing_Serie_Id)
        {
            return new BillingSeriesView(GetBillingSerieInDb(Billing_Serie_Id));
        }

        public HispaniaCompData.BillingSerie GetBillingSerie(int BillingSeries_Id)
        {
            return GetBillingSerieInDb(BillingSeries_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateBillingSerieInDb(HispaniaCompData.BillingSerie billingSerie)
        {
            HispaniaDataAccess.Instance.CreateBillingSerie(billingSerie);
        }

        private List<HispaniaCompData.BillingSerie> _BillingSeriesInDb;

        private List<HispaniaCompData.BillingSerie> BillingSeriesInDb
        {
            get
            {
                return (this._BillingSeriesInDb);
            }
            set
            {
                this._BillingSeriesInDb = value;
            }
        }

        private void UpdateBillingSerieInDb(HispaniaCompData.BillingSerie billingSerie)
        {
            HispaniaDataAccess.Instance.UpdateBillingSerie(billingSerie);
        }

        private void DeleteBillingSerieInDb(HispaniaCompData.BillingSerie billingSerie)
        {
            HispaniaDataAccess.Instance.DeleteBillingSerie(billingSerie);
        }

        private HispaniaCompData.BillingSerie GetBillingSerieInDb(int BillingSeries_Id)
        {
            return HispaniaDataAccess.Instance.GetBillingSerie(BillingSeries_Id);
        }

        #endregion
    }
}
