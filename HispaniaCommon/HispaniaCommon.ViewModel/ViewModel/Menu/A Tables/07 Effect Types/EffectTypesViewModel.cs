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

        public void CreateEffectType(EffectTypesView EffectTypesView)
        {
            HispaniaCompData.EffectType effectTypeToCreate = EffectTypesView.GetEffectType();
            CreateEffectTypeInDb(effectTypeToCreate);
            EffectTypesView.EffectType_Id = effectTypeToCreate.EffectType_Id;
            RefreshEffectTypes();
        }

        public void RefreshEffectTypes()
        {
            try
            {
                EffectTypesInDb = HispaniaDataAccess.Instance.ReadEffectTypes();
                _EffectTypes = new ObservableCollection<EffectTypesView>();
                _EffectTypesInDictionary = new Dictionary<string, EffectTypesView>();
                foreach (HispaniaCompData.EffectType effectType in EffectTypesInDb)
                {
                    EffectTypesView NewEffectTypesView = new EffectTypesView(effectType);
                    _EffectTypes.Add(NewEffectTypesView);
                    _EffectTypesInDictionary.Add(GetKeyEffectTypeView(effectType), NewEffectTypesView);
                }
            }
            catch (Exception ex)
            {
                _EffectTypes = null;
                throw ex;
            }
        }

        private ObservableCollection<EffectTypesView> _EffectTypes = null;

        public ObservableCollection<EffectTypesView> EffectTypes
        {
            get
            {
                return _EffectTypes;
            }
        }

        private Dictionary<string, EffectTypesView> _EffectTypesInDictionary = null;

        public Dictionary<string, EffectTypesView> EffectTypesDict
        {
            get
            {
                return _EffectTypesInDictionary;
            }
        }

        public string GetKeyEffectTypeView(EffectTypesView effectTypesView)
        {
            return GetKeyEffectTypeView(effectTypesView.Code, effectTypesView.Description);
        }

        private string GetKeyEffectTypeView(HispaniaCompData.EffectType effectType)
        {
            return GetKeyEffectTypeView(effectType.Code, effectType.Description);
        }

        private string GetKeyEffectTypeView(string EffectType_Code, string EffectType_Description)
        {
            return string.Format("Tipus {0} :- Efecte {1}", EffectType_Code, EffectType_Description);
        }

        public void UpdateEffectType(EffectTypesView effectTypesView)
        {
            if ((effectTypesView != null) && (effectTypesView.EffectType_Id < 10))
            {
                throw new Exception(string.Format("No es pot modificar el Tipus d''Enviament '{0}' ja que és de Sistema.",
                                                  effectTypesView.Code));
            }
            UpdateEffectTypeInDb(effectTypesView.GetEffectType());
            RefreshEffectTypes();
        }
        public void DeleteEffectType(EffectTypesView effectTypesView)
        {
            if ((effectTypesView != null) && (effectTypesView.EffectType_Id < 10))
            {
                throw new Exception(string.Format("No es pot esborrar el Tipus d''Enviament '{0}' ja que és de Sistema.",
                                                  effectTypesView.Code));
            }
            DeleteEffectTypeInDb(effectTypesView.GetEffectType());
            RefreshEffectTypes();
        }

        public EffectTypesView GetEffectTypeFromDb(EffectTypesView effectTypesView)
        {
            return new EffectTypesView(GetEffectTypeInDb(effectTypesView.EffectType_Id));
        }

        public HispaniaCompData.EffectType GetEffectType(int EffectTypes_Id)
        {
            return GetEffectTypeInDb(EffectTypes_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateEffectTypeInDb(HispaniaCompData.EffectType effectType)
        {
            HispaniaDataAccess.Instance.CreateEffectType(effectType);
        }

        private List<HispaniaCompData.EffectType> _EffectTypesInDb;

        private List<HispaniaCompData.EffectType> EffectTypesInDb
        {
            get
            {
                return (this._EffectTypesInDb);
            }
            set
            {
                this._EffectTypesInDb = value;
            }
        }

        private void UpdateEffectTypeInDb(HispaniaCompData.EffectType effectType)
        {
            HispaniaDataAccess.Instance.UpdateEffectType(effectType);
        }

        private void DeleteEffectTypeInDb(HispaniaCompData.EffectType effectType)
        {
            HispaniaDataAccess.Instance.DeleteEffectType(effectType);
        }

        private HispaniaCompData.EffectType GetEffectTypeInDb(int EffectTypes_Id)
        {
            return HispaniaDataAccess.Instance.GetEffectType(EffectTypes_Id);
        }

        #endregion
    }
}
