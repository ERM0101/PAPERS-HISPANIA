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

        public void CreateIVAType(IVATypesView IVATypesView)
        {
            HispaniaCompData.IVAType ivaTypeToCreate = IVATypesView.GetIVAType();
            CreateIVATypeInDb(ivaTypeToCreate);
            IVATypesView.IVAType_Id = ivaTypeToCreate.IVAType_Id;
            RefreshIVATypes();
        }

        public void RefreshIVATypes()
        {
            try
            {
                IVATypesInDb = HispaniaDataAccess.Instance.ReadIVATypes();
                _IVATypes = new ObservableCollection<IVATypesView>();
                _IVATypesInDictionary = new Dictionary<string, IVATypesView>();
                foreach (HispaniaCompData.IVAType ivaType in IVATypesInDb)
                {
                    //  Create the View object associated at 
                        IVATypesView NewIVATypesView = new IVATypesView(ivaType);
                        _IVATypes.Add(NewIVATypesView);
                    //  Insert only IVA Type in validity period
                        if ((ivaType.IVAInitValidityData <= DateTime.Now) &&
                            ((ivaType.IVAEndValidityData == null) || (DateTime.Now <= ivaType.IVAEndValidityData)))
                        {
                            _IVATypesInDictionary.Add(GetKeyIVATypeView(ivaType), NewIVATypesView);
                        }
                }
            }
            catch (Exception ex)
            {
                _IVATypes = null;
                throw ex;
            }
        }

        private ObservableCollection<IVATypesView> _IVATypes = null;

        public ObservableCollection<IVATypesView> IVATypes
        {
            get
            {
                return _IVATypes;
            }
        }

        private Dictionary<string, IVATypesView> _IVATypesInDictionary = null;

        public Dictionary<string, IVATypesView> IVATypesDict
        {
            get
            {
                return _IVATypesInDictionary;
            }
        }

        public string GetKeyIVATypeView(IVATypesView ivaTypesView)
        {
            return GetKeyIVATypeView(ivaTypesView.IVAType_Id, ivaTypesView.Type, ivaTypesView.IVAPercent, ivaTypesView.SurchargePercent);
        }

        private string GetKeyIVATypeView(HispaniaCompData.IVAType ivaType)
        {
            return GetKeyIVATypeView(ivaType.IVAType_Id, ivaType.Type, ivaType.IVAPercent, ivaType.SurchargePercent);
        }

        private string GetKeyIVATypeView(int IVATypes_Id, string IVATypes_Type, decimal IVATypes_IVAPercent, decimal IVATypes_SurchargePercent)
        {
            return string.Format("({0}) -> Tipus {1} :- IVA {2}% - Recàrrec {3}%", IVATypes_Id, IVATypes_Type, IVATypes_IVAPercent, IVATypes_SurchargePercent);
        }

        public void UpdateIVAType(IVATypesView IVATypesView)
        {
            UpdateIVATypeInDb(IVATypesView.GetIVAType());
            RefreshIVATypes();
        }

        public void DeleteIVAType(IVATypesView IVATypesView)
        {
            if (IVATypesView.IVAEndValidityData is null)
            {
                DeleteIVATypeInDb(IVATypesView.GetIVAType());
                RefreshIVATypes();
            }
            else
            {
                throw new Exception("No es pot esborrar un Tipus d'IVA que ja està tancat (data de fi de validessa definit).");
            }
        }

        public IVATypesView GetIVATypeFromDb(IVATypesView IVATypesView)
        {
            return new IVATypesView(GetIVATypeInDb(IVATypesView.IVAType_Id));
        }

        public HispaniaCompData.IVAType GetIVAType(int IVATypes_Id)
        {
            return GetIVATypeInDb(IVATypes_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateIVATypeInDb(HispaniaCompData.IVAType ivaType)
        {
            HispaniaDataAccess.Instance.CreateIVAType(ivaType);
        }

        private List<HispaniaCompData.IVAType> _IVATypesInDb;

        private List<HispaniaCompData.IVAType> IVATypesInDb
        {
            get
            {
                return (this._IVATypesInDb);
            }
            set
            {
                this._IVATypesInDb = value;
            }
        }

        private void UpdateIVATypeInDb(HispaniaCompData.IVAType ivaType)
        {
            HispaniaDataAccess.Instance.UpdateIVAType(ivaType);
        }

        private void DeleteIVATypeInDb(HispaniaCompData.IVAType ivaType)
        {
            HispaniaDataAccess.Instance.DeleteIVAType(ivaType);
        }

        private HispaniaCompData.IVAType GetIVATypeInDb(int IVATypes_Id)
        {
            return HispaniaDataAccess.Instance.GetIVAType(IVATypes_Id);
        }

        #endregion
    }
}
