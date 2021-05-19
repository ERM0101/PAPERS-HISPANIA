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

        public void CreatePostalCode(PostalCodesView postalCodeView)
        {
            HispaniaCompData.PostalCode postalCodeToCreate = postalCodeView.GetPostalCode();
            CreatePostalCodeInDb(postalCodeToCreate);
            postalCodeView.PostalCode_Id = postalCodeToCreate.PostalCode_Id;
            RefreshPostalCodes();
        }
        public void RefreshPostalCodes()
        {
            try
            {
                PostalCodesInDb = HispaniaDataAccess.Instance.ReadPostalCodes();
                _PostalCodes = new ObservableCollection<PostalCodesView>();
                _PostalCodesInDictionary = new Dictionary<string, PostalCodesView>();
                foreach (HispaniaCompData.PostalCode postalCode in PostalCodesInDb)
                {
                    PostalCodesView NewPostalCodeView = new PostalCodesView(postalCode);
                    _PostalCodes.Add(NewPostalCodeView);
                    _PostalCodesInDictionary.Add(GetKeyPostalCodeView(postalCode), NewPostalCodeView);
                }
            }
            catch (Exception ex)
            {
                _PostalCodes = null;
                throw ex;
            }
        }

        private ObservableCollection<PostalCodesView> _PostalCodes = null;

        public ObservableCollection<PostalCodesView> PostalCodes
        {
            get
            {
                return _PostalCodes;
            }
        }

        private Dictionary<string, PostalCodesView> _PostalCodesInDictionary = null;

        public Dictionary<string, PostalCodesView> PostalCodesDict
        {
            get
            {
                return _PostalCodesInDictionary;
            }
        }
        public string GetKeyPostalCodeView(PostalCodesView postalCodeView)
        {
            return GetKeyPostalCodeView(postalCodeView.PostalCode_Id, postalCodeView.Postal_Code, postalCodeView.City);
        }
        private string GetKeyPostalCodeView(HispaniaCompData.PostalCode postalCodeView)
        {
            return GetKeyPostalCodeView(postalCodeView.PostalCode_Id, postalCodeView.Postal_Code, postalCodeView.City);
        }
        private string GetKeyPostalCodeView(int PostalCode_Id, string Postal_Code, string City)
        {
            return string.Format("{0} -> Ciutat: {1}  (Id: {2})", Postal_Code, City, PostalCode_Id);
        }
        public void UpdatePostalCode(PostalCodesView postalCodeView)
        {
            UpdatePostalCodeInDb(postalCodeView.GetPostalCode());
            RefreshPostalCodes();
        }
        public void DeletePostalCode(PostalCodesView postalCodeView)
        {
            DeletePostalCodeInDb(postalCodeView.GetPostalCode());
            RefreshPostalCodes();
        }

        public PostalCodesView GetPostalCodeFromDb(PostalCodesView postalCodesView)
        {
            return new PostalCodesView(GetPostalCodeInDb(postalCodesView.PostalCode_Id));
        }

        public HispaniaCompData.PostalCode GetPostalCode(int PostalCode_PostalCode_Id)
        {
            return GetPostalCodeInDb(PostalCode_PostalCode_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreatePostalCodeInDb(HispaniaCompData.PostalCode postalCode)
        {
            HispaniaDataAccess.Instance.CreatePostalCode(postalCode);
        }

        private List<HispaniaCompData.PostalCode> _PostalCodesInDb;

        private List<HispaniaCompData.PostalCode> PostalCodesInDb
        {
            get
            {
                return (this._PostalCodesInDb);
            }
            set
            {
                this._PostalCodesInDb = value;
            }
        }

        private void UpdatePostalCodeInDb(HispaniaCompData.PostalCode postalCode)
        {
            HispaniaDataAccess.Instance.UpdatePostalCode(postalCode);
        }

        private void DeletePostalCodeInDb(HispaniaCompData.PostalCode postalCode)
        {
            HispaniaDataAccess.Instance.DeletePostalCode(postalCode);
        }

        private HispaniaCompData.PostalCode GetPostalCodeInDb(int PostalCode_PostalCode_Id)
        {
            return HispaniaDataAccess.Instance.GetPostalCode(PostalCode_PostalCode_Id);
        }

        #endregion
    }
}
