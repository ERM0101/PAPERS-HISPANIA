#region Librerias usadas por la clase

using HispaniaCompData = HispaniaComptabilitat.Data;
using HispaniaCommon.DataAccess;
using System;

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

        public void RefreshParameters()
        {
            try
            {
                ParametersInDb = HispaniaDataAccess.Instance.ReadParameters();
                _Parameters = new ParametersView(ParametersInDb);
            }
            catch (Exception ex)
            {
                _Parameters = null;
                throw ex;
            }
        }

        private ParametersView _Parameters = null;

        public ParametersView Parameters
        {
            get
            {
                return _Parameters;
            }
        }

        public void UpdateParameters(ParametersView ParameterView)
        {
            UpdateParametersInDb(ParameterView.GetParameter());
            _Parameters = ParameterView;
        }

        #endregion

        #region DataBase

        private HispaniaCompData.Parameter _ParametersInDb;

        private HispaniaCompData.Parameter ParametersInDb
        {
            get
            {
                return (this._ParametersInDb);
            }
            set
            {
                this._ParametersInDb = value;
            }
        }

        private void UpdateParametersInDb(HispaniaCompData.Parameter Parameter)
        {
            HispaniaDataAccess.Instance.UpdateParameters(Parameter);
        }

        #endregion
    }
}
