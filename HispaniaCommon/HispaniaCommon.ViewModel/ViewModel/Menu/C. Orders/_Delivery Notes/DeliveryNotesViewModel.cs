#region Libraries used by the class

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

        public void CreateDeliveryNote(DeliveryNotesView DeliveryNotesView)
        {
            HispaniaCompData.DeliveryNote DeliveryNoteToCreate = DeliveryNotesView.GetDeliveryNote();
            CreateDeliveryNoteInDb(DeliveryNoteToCreate);
            DeliveryNotesView.DeliveryNote_Id = DeliveryNoteToCreate.DeliveryNote_Id;
            RefreshDeliveryNotes();
        }

        public void RefreshDeliveryNotes()
        {
            try
            {
                DeliveryNotesInDb = HispaniaDataAccess.Instance.ReadDeliveryNotes();
                _DeliveryNotes = new ObservableCollection<DeliveryNotesView>();
                _DeliveryNotesInDictionary = new Dictionary<string, DeliveryNotesView>();
                foreach (HispaniaCompData.DeliveryNote DeliveryNote in DeliveryNotesInDb)
                {
                    DeliveryNotesView NewDeliveryNotesView = new DeliveryNotesView(DeliveryNote);
                    _DeliveryNotes.Add(NewDeliveryNotesView);
                    _DeliveryNotesInDictionary.Add(GetKeyDeliveryNoteView(DeliveryNote), NewDeliveryNotesView);
                }
            }
            catch (Exception ex)
            {
                _DeliveryNotes = null;
                throw ex;
            }
        }

        private ObservableCollection<DeliveryNotesView> _DeliveryNotes = null;

        public ObservableCollection<DeliveryNotesView> DeliveryNotes
        {
            get
            {
                return _DeliveryNotes;
            }
        }

        private Dictionary<string, DeliveryNotesView> _DeliveryNotesInDictionary = null;

        public Dictionary<string, DeliveryNotesView> DeliveryNotesDict
        {
            get
            {
                return _DeliveryNotesInDictionary;
            }
        }

        public string GetKeyDeliveryNoteView(DeliveryNotesView DeliveryNotesView)
        {
            return GetKeyDeliveryNoteView(DeliveryNotesView.DeliveryNote_Id, DeliveryNotesView.Year);
        }

        private string GetKeyDeliveryNoteView(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            return GetKeyDeliveryNoteView(DeliveryNote.DeliveryNote_Id, DeliveryNote.Year);
        }

        private string GetKeyDeliveryNoteView(int DeliveryNote_Id, decimal Year)
        {
            return string.Format("{0} | {1}", DeliveryNote_Id, Year);
        }

        public void UpdateDeliveryNote(DeliveryNotesView DeliveryNotesView)
        {
            UpdateDeliveryNoteInDb(DeliveryNotesView.GetDeliveryNote());
            RefreshDeliveryNotes();
        }
        public void DeleteDeliveryNote(DeliveryNotesView DeliveryNotesView)
        {
            DeleteDeliveryNoteInDb(DeliveryNotesView.GetDeliveryNote());
            RefreshDeliveryNotes();
        }

        public HispaniaCompData.DeliveryNote GetDeliveryNote(int DeliveryNotes_Id, decimal DeliveryNotes_Year)
        {
            return GetDeliveryNoteInDb(DeliveryNotes_Id, DeliveryNotes_Year);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateDeliveryNoteInDb(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            HispaniaDataAccess.Instance.CreateDeliveryNote(DeliveryNote);
        }

        private List<HispaniaCompData.DeliveryNote> _DeliveryNotesInDb;

        private List<HispaniaCompData.DeliveryNote> DeliveryNotesInDb
        {
            get
            {
                return (this._DeliveryNotesInDb);
            }
            set
            {
                this._DeliveryNotesInDb = value;
            }
        }

        private void UpdateDeliveryNoteInDb(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            HispaniaDataAccess.Instance.UpdateDeliveryNote(DeliveryNote);
        }

        private void DeleteDeliveryNoteInDb(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            HispaniaDataAccess.Instance.DeleteDeliveryNote(DeliveryNote);
        }

        private HispaniaCompData.DeliveryNote GetDeliveryNoteInDb(int DeliveryNotes_Id, decimal DeliveryNotes_Year)
        {
            return HispaniaDataAccess.Instance.GetDeliveryNote(DeliveryNotes_Id, DeliveryNotes_Year);
        }
        
        #endregion
    }
}
