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
    public enum RelatedCustomersAttributes
    {
        None,
    }

    /// <summary>
    /// Class that Store the information of a RelatedCustomer.
    /// </summary>
    public class RelatedCustomersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the RelatedCustomer class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the RelatedCustomer class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Client", "Customer_Canceled_Id" },
                        { "Alias", "Customer_Canceled_Alias" },
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
                return string.Format("{0}-{1}", GlobalViewModel.GetStringFromIntIdValue(Customer_Id), GlobalViewModel.GetStringFromIntIdValue(Customer_Canceled_Id));
            }
        }

        #endregion

        #region Main Fields

        public int Customer_Id { get; set; }
        public int Customer_Canceled_Id { get; set; }
        public string Remarks { get; set; }

        #endregion

        #region Calculated

        public string Customer_Canceled_Alias
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.GetCustomer(Customer_Canceled_Id).Customer_Alias;
            }
        }

        public string Customer_Canceled_Name
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.GetCustomer(Customer_Canceled_Id).Company_Name;
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RelatedCustomersView()
        {
            Customer_Id = GlobalViewModel.IntIdInitValue;
            Customer_Canceled_Id = GlobalViewModel.IntIdInitValue;
            Remarks = String.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal RelatedCustomersView(HispaniaCompData.RelatedCustomer relatedCustomer)
        {
            Customer_Id = relatedCustomer.Customer_Id;
            Customer_Canceled_Id = relatedCustomer.Customer_Canceled_Id;
            Remarks = relatedCustomer.Remarks;
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RelatedCustomersView(RelatedCustomersView relatedCustomer)
        {
            Customer_Id = relatedCustomer.Customer_Id;
            Customer_Canceled_Id = relatedCustomer.Customer_Canceled_Id;
            Remarks = relatedCustomer.Remarks;
        }

        #endregion

        #region GetRelatedCustomer

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.RelatedCustomer GetRelatedCustomer()
        {
            HispaniaCompData.RelatedCustomer customer = new HispaniaCompData.RelatedCustomer()
            {
                Customer_Id = Customer_Id,
                Customer_Canceled_Id = Customer_Canceled_Id,
                Remarks = Remarks
            };
            return (customer);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out RelatedCustomersAttributes ErrorField)
        {
            ErrorField = RelatedCustomersAttributes.None;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(RelatedCustomersView Data)
        {
            Customer_Id = Data.Customer_Id;
            Customer_Canceled_Id = Data.Customer_Canceled_Id;
            Remarks = Data.Remarks;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(RelatedCustomersView Data, RelatedCustomersAttributes ErrorField)
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
                RelatedCustomersView relatedCustomer = obj as RelatedCustomersView;
                if ((Object)relatedCustomer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Customer_Id == relatedCustomer.Customer_Id) && 
                       (Customer_Canceled_Id == relatedCustomer.Customer_Canceled_Id) &&
                       (Remarks == relatedCustomer.Remarks);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(RelatedCustomersView relatedCustomer)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)relatedCustomer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Customer_Id == relatedCustomer.Customer_Id) && 
                       (Customer_Canceled_Id == relatedCustomer.Customer_Canceled_Id) &&
                       (Remarks == relatedCustomer.Remarks);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="relatedCustomer_1">Primera instáncia a comparar.</param>
        /// <param name="relatedCustomer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(RelatedCustomersView relatedCustomer_1, RelatedCustomersView relatedCustomer_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(relatedCustomer_1, relatedCustomer_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)relatedCustomer_1 == null) || ((object)relatedCustomer_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (relatedCustomer_1.Customer_Id == relatedCustomer_2.Customer_Id) && 
                       (relatedCustomer_1.Customer_Canceled_Id == relatedCustomer_2.Customer_Canceled_Id) &&
                       (relatedCustomer_1.Remarks == relatedCustomer_2.Remarks);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="relatedCustomer_1">Primera instáncia a comparar.</param>
        /// <param name="relatedCustomer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(RelatedCustomersView relatedCustomer_1, RelatedCustomersView relatedCustomer_2)
        {
            return !(relatedCustomer_1 == relatedCustomer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Customer_Id, Customer_Canceled_Id).GetHashCode());
        }

        #endregion
    }
}
