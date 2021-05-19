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
    public enum BillingSeriesAttributes
    {
        Name,
        Alias,
        None,
    }
    
    /// <summary>
    /// Class that Store the information of a Billing serie.
    /// </summary>
    public class BillingSeriesView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Customer class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Customer class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Sèrie", "Name" },
                        { "Descripció", "Alias" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Serie_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Serie_Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BillingSeriesView()
        {
            Serie_Id = -1;
            Name = string.Empty;
            Alias = string.Empty;
        }
        internal BillingSeriesView(HispaniaCompData.BillingSerie billingSerie)
        {
            Serie_Id = billingSerie.Serie_Id;
            Name = billingSerie.Name;
            Alias = billingSerie.Alias;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BillingSeriesView(BillingSeriesView billingSerie)
        {
            Serie_Id = billingSerie.Serie_Id;
            Name = billingSerie.Name;
            Alias = billingSerie.Alias;
        }

        #endregion

        #region GetBillingSerie

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.BillingSerie GetBillingSerie()
        {
            HispaniaCompData.BillingSerie billingSerie = new HispaniaCompData.BillingSerie()
            {
                Serie_Id = Serie_Id,
                Name = Name,
                Alias = Alias
            };
            return billingSerie;
        }

        #endregion
        
        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out BillingSeriesAttributes ErrorField)
        {
            ErrorField = BillingSeriesAttributes.None;
            if (!GlobalViewModel.IsBillingSerieName(Name, out string msgError))
            {
                ErrorField = BillingSeriesAttributes.Name;
                throw new FormatException(msgError);
            }
            if (!GlobalViewModel.IsEmptyOrDescription(Alias))
            {
                ErrorField = BillingSeriesAttributes.Alias;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValues(BillingSeriesView Data)
        {
            Name = Data.Name;
            Alias = Data.Alias;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(BillingSeriesView Data, BillingSeriesAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case BillingSeriesAttributes.Name:
                     Name = Data.Name;
                     break;
                case BillingSeriesAttributes.Alias:
                     Alias = Data.Alias;
                     break;
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
                BillingSeriesView serie = obj as BillingSeriesView;
                if ((Object)serie == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Serie_Id == serie.Serie_Id) && (Name == serie.Name) && (Alias == serie.Alias);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(BillingSeriesView serie)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)serie == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Serie_Id == serie.Serie_Id) && (Name == serie.Name) && (Alias == serie.Alias);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="serie_1">Primera instáncia a comparar.</param>
        /// <param name="serie_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(BillingSeriesView serie_1, BillingSeriesView serie_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(serie_1, serie_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)serie_1 == null) || ((object)serie_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (serie_1.Serie_Id == serie_2.Serie_Id) && 
                       (serie_1.Name == serie_2.Name) && (serie_1.Alias == serie_2.Alias);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="serie_1">Primera instáncia a comparar.</param>
        /// <param name="serie_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(BillingSeriesView serie_1, BillingSeriesView serie_2)
        {
            return !(serie_1 == serie_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Serie_Id, Name, Alias).GetHashCode());
        }

        #endregion
    }
}
