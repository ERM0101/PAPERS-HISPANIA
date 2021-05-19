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
    /// Last update Data: 04/01/2018
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void RefreshInputsOutputs(int Good_Id)
        {
            try
            {
                InputsOutputsInDb = HispaniaDataAccess.Instance.ReadInputOutputs(Good_Id);
                _InputsOutputs = new ObservableCollection<InputsOutputsView>();
                _InputsOutputsInDictionary = new Dictionary<string, InputsOutputsView>();
                foreach (HispaniaCompData.InputOutput InputOutput in InputsOutputsInDb)
                {
                    InputsOutputsView NewInputsOutputsView = new InputsOutputsView(InputOutput);
                    _InputsOutputs.Add(NewInputsOutputsView);
                    _InputsOutputsInDictionary.Add(GetKeyInputOutputView(InputOutput), NewInputsOutputsView);
                }
            }
            catch (Exception ex)
            {
                _InputsOutputs = null;
                throw ex;
            }
        }

        private ObservableCollection<InputsOutputsView> _InputsOutputs = null;

        public ObservableCollection<InputsOutputsView> InputsOutputs
        {
            get
            {
                return _InputsOutputs;
            }
        }

        private Dictionary<string, InputsOutputsView> _InputsOutputsInDictionary = null;

        public Dictionary<string, InputsOutputsView> InputsOutputsDict
        {
            get
            {
                return _InputsOutputsInDictionary;
            }
        }

        public string GetKeyInputOutputView(InputsOutputsView InputsOutputsView)
        {
            return GetKeyInputOutputView(InputsOutputsView.GetHashCode().ToString());
        }

        private string GetKeyInputOutputView(HispaniaCompData.InputOutput InputOutput)
        {
            return GetKeyInputOutputView(InputOutput.GetHashCode().ToString());
        }

        private string GetKeyInputOutputView(string InputOutputCode)
        {
            return string.Format("{0}", InputOutputCode);
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.InputOutput> ReadInputsOutputsInDb(int Good_Id)
        {
            return (HispaniaDataAccess.Instance.ReadInputOutputs(Good_Id));
        }

        private List<HispaniaCompData.InputOutput> _InputsOutputsInDb;

        private List<HispaniaCompData.InputOutput> InputsOutputsInDb
        {
            get
            {
                return (this._InputsOutputsInDb);
            }
            set
            {
                this._InputsOutputsInDb = value;
            }
        }

        #endregion
    }
}
