using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum PostalCodeAttributes
    {
        Postal_Code,
        City,
        Province,
        None,
    }

    /// <summary>
    /// Class that Store the information of a City and its Postal Code.
    /// </summary>
    public class PostalCodesView : IMenuView
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
                        { "Codi Postal", "Postal_Code" },
                        { "Població", "City" },
                        { "Provincia", "Province" }
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
                return GlobalViewModel.GetStringFromIntIdValue(PostalCode_Id);
            }
        }

        #endregion

        #region Main Fields

        public string Postal_Code { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public int PostalCode_Id { get; set; }

        #endregion

        #region For Consult

        public string Postal_Code_Info
        {
            get
            {
                return string.Format("{0}, {1}", Postal_Code, City);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public PostalCodesView()
        {
            PostalCode_Id = -1;
            Postal_Code = string.Empty;
            City = string.Empty;
            Province = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal PostalCodesView(HispaniaCompData.PostalCode postalCode)
        {
            PostalCode_Id = postalCode.PostalCode_Id;
            Postal_Code = postalCode.Postal_Code;
            City = postalCode.City;
            Province = postalCode.Province;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public PostalCodesView(PostalCodesView postalCode)
        {
            PostalCode_Id = postalCode.PostalCode_Id;
            Postal_Code = postalCode.Postal_Code;
            City = postalCode.City;
            Province = postalCode.Province;
        }

        #endregion

        #region GetPostalCode

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.PostalCode GetPostalCode()
        {
            HispaniaCompData.PostalCode postalCode = new HispaniaCompData.PostalCode()
            {
                PostalCode_Id = PostalCode_Id,
                Postal_Code = Postal_Code,
                City = City,
                Province = Province
            };
            return postalCode;
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out PostalCodeAttributes ErrorField)
        {
            ErrorField = PostalCodeAttributes.None;
            if (!GlobalViewModel.IsPostalCode(Postal_Code))
            {
                ErrorField = PostalCodeAttributes.Postal_Code;
                throw new FormatException(GlobalViewModel.ValidationPostalCodeError); 
            }
            if (!GlobalViewModel.IsEmptyOrName(City))
            {
                ErrorField = PostalCodeAttributes.City;
                throw new FormatException("Error, en les dades de la Població.");
            }
            if (!GlobalViewModel.IsEmptyOrName(Province))
            {
                ErrorField = PostalCodeAttributes.Province;
                throw new FormatException("Error, en les dades de la Província.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(PostalCodesView Data)
        {
            Postal_Code = Data.Postal_Code;
            City = Data.City;
            Province = Data.Province;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(PostalCodesView Data, PostalCodeAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case PostalCodeAttributes.Postal_Code:
                     Postal_Code = Data.Postal_Code;
                     break;
                case PostalCodeAttributes.City:
                     City = Data.City;
                     break;
                case PostalCodeAttributes.Province:
                     Province = Data.Province;
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
                PostalCodesView postalCode = obj as PostalCodesView;
                if ((Object)postalCode == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (PostalCode_Id == postalCode.PostalCode_Id) && 
                       (Postal_Code == postalCode.Postal_Code) && 
                       (City == postalCode.City) &&
                       (Province == postalCode.Province);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(PostalCodesView postalCode)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)postalCode == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (PostalCode_Id == postalCode.PostalCode_Id) &&
                       (Postal_Code == postalCode.Postal_Code) && 
                       (City == postalCode.City) &&
                       (Province == postalCode.Province);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="postalCode_1">Primera instáncia a comparar.</param>
        /// <param name="postalCode_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(PostalCodesView postalCode_1, PostalCodesView postalCode_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(postalCode_1, postalCode_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)postalCode_1 == null) || ((object)postalCode_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (postalCode_1.PostalCode_Id == postalCode_2.PostalCode_Id) &&
                       (postalCode_1.Postal_Code == postalCode_2.Postal_Code) &&
                       (postalCode_1.City == postalCode_2.City) &&
                       (postalCode_1.Province == postalCode_2.Province);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="postalCode_1">Primera instáncia a comparar.</param>
        /// <param name="postalCode_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(PostalCodesView postalCode_1, PostalCodesView postalCode_2)
        {
            return !(postalCode_1 == postalCode_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(PostalCode_Id, Postal_Code, City).GetHashCode());
        }

        #endregion
    }
}
