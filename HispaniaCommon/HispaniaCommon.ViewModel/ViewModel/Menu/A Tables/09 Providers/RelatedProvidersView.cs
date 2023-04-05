#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum RelatedProvidersAttributes
    {
        None,
    }

    /// <summary>
    /// Class that Store the information of a RelatedProvider.
    /// </summary>
    public class RelatedProvidersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the RelatedProvider class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the RelatedProvider class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Client", "Provider_Canceled_Id" },
                        { "Alias", "Provider_Canceled_Alias" },
                        { "Empresa", "Company_Canceled_Name" },
                    };
                }
                return (m_Fields);
            }
        }

        #endregion

        #region Properties

        #region IMenuView Interface implementation

        public string GetKey
        {
            get
            {
                return string.Format("{0}-{1}", GlobalViewModel.GetStringFromIntIdValue(Provider_Id), GlobalViewModel.GetStringFromIntIdValue(Provider_Canceled_Id));
            }
        }

        #endregion

        #region Main Fields

        public int Provider_Id { get; set; }
        public int Provider_Canceled_Id { get; set; }
        public string Remarks { get; set; }

        #endregion

        #region Calculated

        public string Provider_Canceled_Alias
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.GetProvider(Provider_Canceled_Id).Alias;
            }
        }

        public string Provider_Canceled_Name
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.GetProvider(Provider_Canceled_Id).Name;
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RelatedProvidersView()
        {
            Provider_Id = GlobalViewModel.IntIdInitValue;
            Provider_Canceled_Id = GlobalViewModel.IntIdInitValue;
            Remarks = String.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal RelatedProvidersView(HispaniaCompData.RelatedProvider relatedProvider)
        {
            Provider_Id = relatedProvider.Provider_Id;
            Provider_Canceled_Id = relatedProvider.Provider_Canceled_Id;
            Remarks = relatedProvider.Remarks;
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RelatedProvidersView(RelatedProvidersView relatedProvider)
        {
            Provider_Id = relatedProvider.Provider_Id;
            Provider_Canceled_Id = relatedProvider.Provider_Canceled_Id;
            Remarks = relatedProvider.Remarks;
        }

        #endregion

        #region GetRelatedProvider

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.RelatedProvider GetRelatedProvider()
        {
            HispaniaCompData.RelatedProvider provider = new HispaniaCompData.RelatedProvider()
            {
                Provider_Id = Provider_Id,
                Provider_Canceled_Id = Provider_Canceled_Id,
                Remarks = Remarks
            };
            return (provider);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out RelatedProvidersAttributes ErrorField)
        {
            ErrorField = RelatedProvidersAttributes.None;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(RelatedProvidersView Data)
        {
            Provider_Id = Data.Provider_Id;
            Provider_Canceled_Id = Data.Provider_Canceled_Id;
            Remarks = Data.Remarks;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(RelatedProvidersView Data, RelatedProvidersAttributes ErrorField)
        {
            switch (ErrorField)
            {
                default:
                     break;
            }
        }
        
        #endregion

        #region Equal implementation

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="obj">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public override bool Equals(Object obj)
        {
            //  Si el parámetro es nulo ya hemos acabado.
                if (obj == null) return (false);
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                RelatedProvidersView relatedProvider = obj as RelatedProvidersView;
                if ((Object)relatedProvider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Provider_Id == relatedProvider.Provider_Id) && 
                       (Provider_Canceled_Id == relatedProvider.Provider_Canceled_Id) &&
                       (Remarks == relatedProvider.Remarks);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(RelatedProvidersView relatedProvider)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)relatedProvider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Provider_Id == relatedProvider.Provider_Id) && 
                       (Provider_Canceled_Id == relatedProvider.Provider_Canceled_Id) &&
                       (Remarks == relatedProvider.Remarks);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="relatedProvider_1">Primera instáncia a comparar.</param>
        /// <param name="relatedProvider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(RelatedProvidersView relatedProvider_1, RelatedProvidersView relatedProvider_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(relatedProvider_1, relatedProvider_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)relatedProvider_1 == null) || ((object)relatedProvider_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (relatedProvider_1.Provider_Id == relatedProvider_2.Provider_Id) && 
                       (relatedProvider_1.Provider_Canceled_Id == relatedProvider_2.Provider_Canceled_Id) &&
                       (relatedProvider_1.Remarks == relatedProvider_2.Remarks);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="relatedProvider_1">Primera instáncia a comparar.</param>
        /// <param name="relatedProvider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(RelatedProvidersView relatedProvider_1, RelatedProvidersView relatedProvider_2)
        {
            return !(relatedProvider_1 == relatedProvider_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Provider_Id, Provider_Canceled_Id).GetHashCode());
        }

        #endregion
    }
}
