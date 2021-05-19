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

        public void CreateUnit(UnitsView unitsView)
        {
            HispaniaCompData.Unit unitToCreate = unitsView.GetUnit();
            CreateUnitInDb(unitToCreate);
            unitsView.Unit_Id = unitToCreate.Unit_Id;
            RefreshUnits();
        }

        public void RefreshUnits()
        {
            try
            {
                UnitsInDb = HispaniaDataAccess.Instance.ReadUnits();
                _Units = new ObservableCollection<UnitsView>();
                _UnitsInDictionary = new Dictionary<string, UnitsView>();
                foreach (HispaniaCompData.Unit unit in UnitsInDb)
                {
                    UnitsView NewUnitsView = new UnitsView(unit);
                    _Units.Add(NewUnitsView);
                    _UnitsInDictionary.Add(GetKeyUnitView(unit), NewUnitsView);
                }
            }
            catch (Exception ex)
            {
                _Units = null;
                throw ex;
            }
        }

        private ObservableCollection<UnitsView> _Units = null;

        public ObservableCollection<UnitsView> Units
        {
            get
            {
                return _Units;
            }
        }

        private Dictionary<string, UnitsView> _UnitsInDictionary = null;

        public Dictionary<string, UnitsView> UnitsDict
        {
            get
            {
                return _UnitsInDictionary;
            }
        }

        public string GetKeyUnitView(UnitsView unitsView)
        {
            return GetKeyUnitView(unitsView.Code, unitsView.Shipping, unitsView.Billing);
        }

        private string GetKeyUnitView(HispaniaCompData.Unit unit)
        {
            return GetKeyUnitView(unit.Code, unit.Shipping, unit.Billing);
        }

        private string GetKeyUnitView(string Code, string Shipping, string Billing)
        {
            return string.Format("{0} ( {1} | {2} )", Code, (String.IsNullOrEmpty(Shipping) ? "-" : Shipping), 
                                 (String.IsNullOrEmpty(Billing) ? "-" : Billing));
        }


        public void UpdateUnit(UnitsView unitView)
        {
            UpdateUnitInDb(unitView.GetUnit());
            RefreshUnits();
        }
        public void DeleteUnit(UnitsView unitView)
        {
            if ((unitView != null) && (!String.IsNullOrEmpty(unitView.Code)) && (int.Parse(unitView.Code) < 24))
            {
                throw new Exception(string.Format("No es pot esborrar la Unitat '{0}' ja que és de Sistema.",
                                                  unitView.Code));
            }
            DeleteUnitInDb(unitView.GetUnit());
            RefreshUnits();
        }
        
        public UnitsView GetUnitFromDb(UnitsView unitsView)
        {
            return new UnitsView(GetUnitInDb(unitsView.Unit_Id));
        }

        public HispaniaCompData.Unit GetUnit(int Unit_Id)
        {
            return GetUnitInDb(Unit_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateUnitInDb(HispaniaCompData.Unit unit)
        {
            HispaniaDataAccess.Instance.CreateUnit(unit);
        }

        private List<HispaniaCompData.Unit> _UnitsInDb;

        private List<HispaniaCompData.Unit> UnitsInDb
        {
            get
            {
                return (this._UnitsInDb);
            }
            set
            {
                this._UnitsInDb = value;
            }
        }

        private void UpdateUnitInDb(HispaniaCompData.Unit unit)
        {
            HispaniaDataAccess.Instance.UpdateUnit(unit);
        }

        private void DeleteUnitInDb(HispaniaCompData.Unit unit)
        {
            HispaniaDataAccess.Instance.DeleteUnit(unit);
        }

        private HispaniaCompData.Unit GetUnitInDb(int Unit_Id)
        {
            return HispaniaDataAccess.Instance.GetUnit(Unit_Id);
        }

        #endregion
    }
}
