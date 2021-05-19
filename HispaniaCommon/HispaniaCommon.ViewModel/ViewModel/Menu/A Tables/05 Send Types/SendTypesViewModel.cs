using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void CreateSendType(SendTypesView SendTypesView)
        {
            HispaniaCompData.SendType sendTypeToCreate = SendTypesView.GetSendType();
            CreateSendTypeInDb(sendTypeToCreate);
            SendTypesView.SendType_Id = sendTypeToCreate.SendType_Id;
            RefreshSendTypes();
        }

        public void RefreshSendTypes()
        {
            try
            {
                SendTypesInDb = HispaniaDataAccess.Instance.ReadSendTypes();
                _SendTypes = new ObservableCollection<SendTypesView>();
                _SendTypesInDictionary = new Dictionary<string, SendTypesView>();
                foreach (HispaniaCompData.SendType sendType in SendTypesInDb)
                {
                    SendTypesView NewSendTypesView = new SendTypesView(sendType);
                    _SendTypes.Add(NewSendTypesView);
                    _SendTypesInDictionary.Add(GetKeySendTypeView(sendType), NewSendTypesView);
                }
            }
            catch (Exception ex)
            {
                _SendTypes = null;
                throw ex;
            }
        }

        private ObservableCollection<SendTypesView> _SendTypes = null;

        public ObservableCollection<SendTypesView> SendTypes
        {
            get
            {
                return _SendTypes;
            }
        }

        private Dictionary<string, SendTypesView> _SendTypesInDictionary = null;

        public Dictionary<string, SendTypesView> SendTypesDict
        {
            get
            {
                return _SendTypesInDictionary;
            }
        }

        public string GetKeySendTypeView(SendTypesView sendTypesView)
        {
            return GetKeySendTypeView(sendTypesView.Code, sendTypesView.Description);
        }

        private string GetKeySendTypeView(HispaniaCompData.SendType sendType)
        {
            return GetKeySendTypeView(sendType.Code, sendType.Description);
        }

        private string GetKeySendTypeView(string SendType_Code, string SendType_Description)
        {
            return string.Format("Tipus {0} :- Descripció {1}", SendType_Code, SendType_Description);
        }

        public void UpdateSendType(SendTypesView SendTypesView)
        {
            if ((SendTypesView != null) && (SendTypesView.SendType_Id < 5))
            {
                throw new Exception(string.Format("No es pot modificar el Tipus d''Enviament '{0}' ja que és de Sistema.",
                                                  SendTypesView.Code));
            }
            UpdateSendTypeInDb(SendTypesView.GetSendType());
            RefreshSendTypes();
        }
        public void DeleteSendType(SendTypesView SendTypesView)
        {
            if ((SendTypesView != null) && (SendTypesView.SendType_Id < 5))
            {
                throw new Exception(string.Format("No es pot esborrar el Tipus d''Enviament '{0}' ja que és de Sistema.",
                                                  SendTypesView.Code));
            }
            DeleteSendTypeInDb(SendTypesView.GetSendType());
            RefreshSendTypes();
        }

        public SendTypesView GetSendTypeFromDb(SendTypesView sendTypesView)
        {
            return new SendTypesView(GetSendTypeInDb(sendTypesView.SendType_Id));
        }

        public HispaniaCompData.SendType GetSendType(int SendTypes_Id)
        {
            return GetSendTypeInDb(SendTypes_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateSendTypeInDb(HispaniaCompData.SendType sendType)
        {
            HispaniaDataAccess.Instance.CreateSendType(sendType);
        }

        private List<HispaniaCompData.SendType> _SendTypesInDb;

        private List<HispaniaCompData.SendType> SendTypesInDb
        {
            get
            {
                return (this._SendTypesInDb);
            }
            set
            {
                this._SendTypesInDb = value;
            }
        }

        private void UpdateSendTypeInDb(HispaniaCompData.SendType sendType)
        {
            HispaniaDataAccess.Instance.UpdateSendType(sendType);
        }

        private void DeleteSendTypeInDb(HispaniaCompData.SendType sendType)
        {
            HispaniaDataAccess.Instance.DeleteSendType(sendType);
        }

        private HispaniaCompData.SendType GetSendTypeInDb(int SendTypes_Id)
        {
            return HispaniaDataAccess.Instance.GetSendType(SendTypes_Id);
        }

        #endregion
    }
}
