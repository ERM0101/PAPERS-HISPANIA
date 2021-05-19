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
    public enum CustomersSalesAttributes
    {
        None,
    }

    /// <summary>
    /// Class that Store the information of a Good.
    /// </summary>
    public class CustomersSalesView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Good class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Good class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Nº Client", "Customer_Id" },
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
                return Customer_Id.ToString();
            }
        }

        #endregion

        #region Main Fields

        public int Customer_Id { get; set; }
        public string Company_Name { get; set; }
        public string Company_Cif { get; set; }
        public string Company_Address { get; set; }
        public string Postal_Code { get; set; }
        public string City { get; set; }
        public string Company_Phone_1 { get; set; }
        public string Company_Phone_2 { get; set; }
        public string Company_EMail { get; set; }
        public decimal Customer_Sales_Acum { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomersSalesView()
        {
            Customer_Id = GlobalViewModel.IntIdInitValue;
            Company_Address = string.Empty;
            Company_Name = string.Empty;
            Company_Cif = string.Empty;
            Postal_Code = string.Empty;
            City = string.Empty;
            Company_Phone_1 = string.Empty;
            Company_Phone_2 = string.Empty;
            Company_EMail = string.Empty;
            Customer_Sales_Acum = GlobalViewModel.DecimalInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal CustomersSalesView(HispaniaCompData.CustomerSale customersSale)
        {
            Customer_Id = GlobalViewModel.GetIntValue(customersSale.Customer_Id);
            Company_Name = customersSale.Company_Name;
            Company_Cif = customersSale.Company_Cif;
            Company_Address = customersSale.Company_Address;
            Postal_Code = customersSale.Postal_Code;
            City = customersSale.City;
            Company_Phone_1 = customersSale.Company_Phone_1;
            Company_Phone_2 = customersSale.Company_Phone_2;
            Company_EMail = customersSale.Company_EMail;
            Customer_Sales_Acum = GlobalViewModel.GetDecimalValue(customersSale.Customer_Sales_Acum);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomersSalesView(CustomersSalesView customersSale)
        {
            Company_Cif = customersSale.Company_Cif;
            Company_Address = customersSale.Company_Address;
            Company_Name = customersSale.Company_Name;
            Customer_Id = customersSale.Customer_Id;
            Postal_Code = customersSale.Postal_Code;
            City = customersSale.City;
            Company_Phone_1 = customersSale.Company_Phone_1;
            Company_Phone_2 = customersSale.Company_Phone_2;
            Company_EMail = customersSale.Company_EMail;
            Customer_Sales_Acum = customersSale.Customer_Sales_Acum;
        }

        #endregion

        #region GetGood

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.CustomerSale GetCustomerSale()
        {
            HispaniaCompData.CustomerSale customersSale = new HispaniaCompData.CustomerSale()
            {
                Company_Cif = Company_Cif,
                Company_Address = Company_Address,
                Company_Name = Company_Name,
                Customer_Id = Customer_Id,
                Postal_Code = Postal_Code,
                City = City,
                Company_Phone_1 = Company_Phone_1,
                Company_Phone_2 = Company_Phone_2,
                Company_EMail = Company_EMail,
                Customer_Sales_Acum = Customer_Sales_Acum,
            };
            return (customersSale);
        }

        #endregion

        #region Validate

        public void Validate(out CustomersSalesAttributes ErrorField)
        {
            ErrorField = CustomersSalesAttributes.None;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(CustomersSalesView Data)
        {
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(CustomersSalesView Data, CustomersSalesAttributes ErrorField)
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
                CustomersSalesView customersSale = obj as CustomersSalesView;
                if ((Object)customersSale == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Company_Cif == customersSale.Company_Cif) && 
                       (Company_Address == customersSale.Company_Address) &&
                       (Company_Name == customersSale.Company_Name) &&
                       (Customer_Id == customersSale.Customer_Id) &&
                       (Postal_Code == customersSale.Postal_Code) &&
                       (City == customersSale.City) &&
                       (Company_Phone_1 == customersSale.Company_Phone_1) &&
                       (Company_Phone_2 == customersSale.Company_Phone_2) &&
                       (Company_EMail == customersSale.Company_EMail) &&
                       (Customer_Sales_Acum == customersSale.Customer_Sales_Acum);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(CustomersSalesView customersSale)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)customersSale == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Company_Cif == customersSale.Company_Cif) && 
                       (Company_Address == customersSale.Company_Address) &&
                       (Company_Name == customersSale.Company_Name) &&
                       (Customer_Id == customersSale.Customer_Id) &&
                       (Postal_Code == customersSale.Postal_Code) &&
                       (City == customersSale.City) &&
                       (Company_Phone_1 == customersSale.Company_Phone_1) &&
                       (Company_Phone_2 == customersSale.Company_Phone_2) &&
                       (Company_EMail == customersSale.Company_EMail) &&
                       (Customer_Sales_Acum == customersSale.Customer_Sales_Acum);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="customersSale_1">Primera instáncia a comparar.</param>
        /// <param name="customersSale_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(CustomersSalesView customersSale_1, CustomersSalesView customersSale_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(customersSale_1, customersSale_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)customersSale_1 == null) || ((object)customersSale_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (customersSale_1.Company_Cif == customersSale_2.Company_Cif) &&
                        (customersSale_1.Company_Address == customersSale_2.Company_Address) &&
                        (customersSale_1.Company_Name == customersSale_2.Company_Name) &&
                        (customersSale_1.Customer_Id == customersSale_2.Customer_Id) &&
                        (customersSale_1.Postal_Code == customersSale_2.Postal_Code) &&
                        (customersSale_1.City == customersSale_2.City) &&
                        (customersSale_1.Company_Phone_1 == customersSale_2.Company_Phone_1) &&
                        (customersSale_1.Company_Phone_2 == customersSale_2.Company_Phone_2) &&
                        (customersSale_1.Company_EMail == customersSale_2.Company_EMail) &&
                        (customersSale_1.Customer_Sales_Acum == customersSale_2.Customer_Sales_Acum);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(CustomersSalesView customersSale_1, CustomersSalesView customersSale_2)
        {
            return !(customersSale_1 == customersSale_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Customer_Id).GetHashCode());
        }

        #endregion
    }
}
