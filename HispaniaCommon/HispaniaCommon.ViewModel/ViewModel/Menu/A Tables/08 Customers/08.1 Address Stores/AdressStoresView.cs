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
    public enum AddressStoresAttributes
    {
        ContactPerson,
        Timetable,
        Phone,
        FAX,
        Address,
        PostalCode,
        Remarks,
        None,
    }

    /// <summary>
    /// Class that Store the information of a AddressStore.
    /// </summary>
    public class AddressStoresView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the AddressStore class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the AddressStore class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Nº Magatzem", "AddressStore_Id" },
                        { "Persona de Contacte", "ContactPerson" },
                        { "Horari", "Timetable" },
                        { "Telèfon", "Phone" },
                        { "Fax", "Fax" },
                        { "Adreça", "Address" },
                        { "CP", "PostalCode_Str" }
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
                return GlobalViewModel.GetStringFromIntIdValue(AddressStore_Id);
            }
        }

        #endregion

        #region Main Fields

        public int AddressStore_Id { get; set; }
        public string ContactPerson { get; set; }
        public string Timetable { get; set; }
        public string Phone { get; set; }
        public string FAX { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }

        #endregion

        #region ForeignKey Properties

        #region Customer

        public int Customer_Id
        {
            get;
            private set;
        }

        #endregion

        #region PostalCode

        private int? _PostalCode_Id { get; set; }

        private PostalCodesView _PostalCode;

        public PostalCodesView PostalCode
        {
            get
            {
                if ((_PostalCode == null) && (_PostalCode_Id != GlobalViewModel.IntIdInitValue) && 
                    (_PostalCode_Id != null))
                {
                    _PostalCode = new PostalCodesView(GlobalViewModel.Instance.HispaniaViewModel.GetPostalCode((int)_PostalCode_Id));
                }
                return (_PostalCode);
            }
            set
            {
                if (value != null)
                {
                    _PostalCode = new PostalCodesView(value);
                    if (_PostalCode == null) _PostalCode_Id = GlobalViewModel.IntIdInitValue;
                    else _PostalCode_Id = _PostalCode.PostalCode_Id;
                }
                else
                {
                    _PostalCode = null;
                    _PostalCode_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string PostalCode_Str
        {
            get
            {
                return (PostalCode == null) ? string.Empty : PostalCode.Postal_Code;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public AddressStoresView(int customer_Id)
        {
            AddressStore_Id = -1;
            Customer_Id = customer_Id;
            ContactPerson = string.Empty;
            Timetable = string.Empty;
            Phone = string.Empty;
            FAX = string.Empty;
            Address = string.Empty;
            Remarks= string.Empty;
            PostalCode = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal AddressStoresView(HispaniaCompData.AddressStore AddressStore)
        {
            AddressStore_Id = AddressStore.AddressStore_Id;
            Customer_Id = AddressStore.Customer_Id;
            ContactPerson = AddressStore.ContactPerson;
            Timetable = AddressStore.Timetable;
            Phone = AddressStore.Phone;
            FAX = AddressStore.FAX;
            Address = AddressStore.Address;
            Remarks = AddressStore.Remarks;
            _PostalCode_Id = AddressStore.PostalCode_Id;
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public AddressStoresView(AddressStoresView AddressStore)
        {
            AddressStore_Id = AddressStore.AddressStore_Id;
            Customer_Id = AddressStore.Customer_Id;
            ContactPerson = AddressStore.ContactPerson;
            Timetable = AddressStore.Timetable;
            Phone = AddressStore.Phone;
            FAX = AddressStore.FAX;
            Address = AddressStore.Address;
            Remarks = AddressStore.Remarks;
            _PostalCode_Id = AddressStore._PostalCode_Id;
        }

        #endregion

        #region GetAddressStore

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.AddressStore GetAddressStore()
        {
            HispaniaCompData.AddressStore AddressStore = new HispaniaCompData.AddressStore()
            {
                AddressStore_Id = AddressStore_Id,
                Customer_Id = Customer_Id,
                ContactPerson = ContactPerson,
                Timetable = Timetable,
                Phone = Phone,
                FAX = FAX,
                Address = Address,
                Remarks = Remarks,
                PostalCode_Id = _PostalCode_Id
            };
            return (AddressStore);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out AddressStoresAttributes ErrorField)
        {
            ErrorField = AddressStoresAttributes.None;
            #region main Fields
            if (!GlobalViewModel.IsEmptyOrName(ContactPerson))
            {
                ErrorField = AddressStoresAttributes.ContactPerson;
                throw new FormatException("Error, format incorrecte de la Persona de Contacte");
            }
            if (!GlobalViewModel.IsEmptyOrTimeTable(Timetable))
            {
                ErrorField = AddressStoresAttributes.Timetable;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrTimeTableError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Phone))
            {
                ErrorField = AddressStoresAttributes.Phone;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(FAX))
            {
                ErrorField = AddressStoresAttributes.FAX;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrAddress(Address))
            {
                ErrorField = AddressStoresAttributes.Address;
                throw new FormatException("Error, format incorrecte de l'Aadreça del Magatzem");
            }
            if (!GlobalViewModel.IsEmptyOrComment(Remarks))
            {
                ErrorField = AddressStoresAttributes.Remarks;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            #endregion
            if (PostalCode == null)
            {
                ErrorField = AddressStoresAttributes.PostalCode;
                throw new FormatException("Error, manca seleccionar el Codi Postal.");
            }
        }
        
        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValues(AddressStoresView Data)
        {
            ContactPerson = Data.ContactPerson;
            Timetable = Data.Timetable;
            Phone = Data.Phone;
            FAX = Data.FAX;
            Address = Data.Address;
            Remarks = Data.Remarks;
            PostalCode = Data.PostalCode;
        }
        
        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(AddressStoresView Data, AddressStoresAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case AddressStoresAttributes.ContactPerson:
                     ContactPerson = Data.ContactPerson;
                     break;
                case AddressStoresAttributes.Timetable:
                     Timetable = Data.Timetable;
                     break;
                case AddressStoresAttributes.Phone:
                     Phone = Data.Phone;
                     break;
                case AddressStoresAttributes.FAX:
                     FAX = Data.FAX;
                     break;
                case AddressStoresAttributes.Address:
                     Address = Data.Address;
                     break;
                case AddressStoresAttributes.Remarks:
                     Remarks = Data.Remarks;
                     break;
                case AddressStoresAttributes.PostalCode:
                     PostalCode = Data.PostalCode;
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
                AddressStoresView AddressStore = obj as AddressStoresView;
                if ((Object)AddressStore == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (AddressStore_Id == AddressStore.AddressStore_Id) && (Customer_Id == AddressStore.Customer_Id) &&
                       (ContactPerson == AddressStore.ContactPerson) && (Timetable == AddressStore.Timetable) &&
                       (Phone == AddressStore.Phone) && (FAX == AddressStore.FAX) &&
                       (Address == AddressStore.Address) && (Remarks == AddressStore.Remarks) &&
                       (PostalCode == AddressStore.PostalCode);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(AddressStoresView AddressStore)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)AddressStore == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (AddressStore_Id == AddressStore.AddressStore_Id) && (Customer_Id == AddressStore.Customer_Id) &&
                       (ContactPerson == AddressStore.ContactPerson) && (Timetable == AddressStore.Timetable) &&
                       (Phone == AddressStore.Phone) && (FAX == AddressStore.FAX) &&
                       (Address == AddressStore.Address) && (Remarks == AddressStore.Remarks) &&
                       (PostalCode == AddressStore.PostalCode);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="AddressStore_1">Primera instáncia a comparar.</param>
        /// <param name="AddressStore_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(AddressStoresView AddressStore_1, AddressStoresView AddressStore_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(AddressStore_1, AddressStore_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)AddressStore_1 == null) || ((object)AddressStore_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (AddressStore_1.AddressStore_Id == AddressStore_2.AddressStore_Id) && (AddressStore_1.Customer_Id == AddressStore_2.Customer_Id) &&
                       (AddressStore_1.ContactPerson == AddressStore_2.ContactPerson) && (AddressStore_1.Timetable == AddressStore_2.Timetable) &&
                       (AddressStore_1.Phone == AddressStore_2.Phone) && (AddressStore_1.FAX == AddressStore_2.FAX) &&
                       (AddressStore_1.Address == AddressStore_2.Address) && (AddressStore_1.Remarks == AddressStore_2.Remarks) &&
                       (AddressStore_1.PostalCode == AddressStore_2.PostalCode);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(AddressStoresView customer_1, AddressStoresView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(AddressStore_Id, Customer_Id).GetHashCode());
        }

        #endregion
    }
}
