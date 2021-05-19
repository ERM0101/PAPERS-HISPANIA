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
    public enum DeliveryNotesAttributes
    {
        Year,
        Date,
        FileNamePDF,
        None,
    }

    /// <summary>
    /// Class that Store the information of a DeliveryNote.
    /// </summary>
    public class DeliveryNotesView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the DeliveryNote class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the DeliveryNote class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Factura", "DeliveryNote_Id" },
                        { "Any", "Year" },
                        { "Data", "Date" },
                        { "Fitxer", "FileNamePDF" }
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
                return string.Format("{0}{1}", 
                                     GlobalViewModel.GetStringFromIntIdValue(DeliveryNote_Id),
                                     GlobalViewModel.GetStringFromYearValue(Year));
            }
        }

        #endregion

        #region Main Fields

        public int DeliveryNote_Id { get; set; }
        public decimal Year { get; set; }
        public DateTime Date { get; set; }
        public string FileNamePDF { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public DeliveryNotesView()
        {
            DeliveryNote_Id = -1;
            Date = DateTime.Now;
            Year = Date.Year;
            FileNamePDF = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal DeliveryNotesView(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            DeliveryNote_Id = DeliveryNote.DeliveryNote_Id;
            Year = DeliveryNote.Year;
            Date = GlobalViewModel.GetDateTimeValue(DeliveryNote.Date);
            FileNamePDF = DeliveryNote.FileNamePDF;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public DeliveryNotesView(DeliveryNotesView DeliveryNote)
        {
            DeliveryNote_Id = DeliveryNote.DeliveryNote_Id;
            Year = DeliveryNote.Year;
            Date = DeliveryNote.Date;
            FileNamePDF = DeliveryNote.FileNamePDF;
        }

        #endregion

        #region GetDeliveryNote

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.DeliveryNote GetDeliveryNote()
        {
            HispaniaCompData.DeliveryNote DeliveryNote = new HispaniaCompData.DeliveryNote()
            {
                DeliveryNote_Id = DeliveryNote_Id,
                FileNamePDF = FileNamePDF,
                Year = Year,
                Date = Date,
            };
            return (DeliveryNote);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out DeliveryNotesAttributes ErrorField)
        {
            ErrorField = DeliveryNotesAttributes.None;
            #region Main Fields
            if (!GlobalViewModel.IsYear(Year, "ANY DE LA FACTURA", out string ErrMsg))
            {
                ErrorField = DeliveryNotesAttributes.Year;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsDateTime(Date, "DATA DE LA FACTURA", out ErrMsg))
            {
                ErrorField = DeliveryNotesAttributes.Date;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsName(FileNamePDF))
            {
                ErrorField = DeliveryNotesAttributes.FileNamePDF;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            #endregion
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(DeliveryNotesView Data)
        {
            Year = Data.Year;
            Date = Data.Date;
            FileNamePDF = Data.FileNamePDF;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(DeliveryNotesView Data, DeliveryNotesAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case DeliveryNotesAttributes.Year:
                     Year = Data.Year;
                     break;
                case DeliveryNotesAttributes.Date:
                     Date = Data.Date;
                     break;
                case DeliveryNotesAttributes.FileNamePDF:
                     FileNamePDF = Data.FileNamePDF;
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
                DeliveryNotesView DeliveryNote = obj as DeliveryNotesView;
                if ((Object)DeliveryNote == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (DeliveryNote_Id == DeliveryNote.DeliveryNote_Id) && (Year == DeliveryNote.Year) && 
                       (Date.Date == DeliveryNote.Date.Date) && (FileNamePDF == DeliveryNote.FileNamePDF);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(DeliveryNotesView DeliveryNote)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)DeliveryNote == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (DeliveryNote_Id == DeliveryNote.DeliveryNote_Id) && (Year == DeliveryNote.Year) && 
                       (Date.Date == DeliveryNote.Date.Date) && (FileNamePDF == DeliveryNote.FileNamePDF);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="DeliveryNote_1">Primera instáncia a comparar.</param>
        /// <param name="DeliveryNote_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(DeliveryNotesView DeliveryNote_1, DeliveryNotesView DeliveryNote_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(DeliveryNote_1, DeliveryNote_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)DeliveryNote_1 == null) || ((object)DeliveryNote_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (DeliveryNote_1.DeliveryNote_Id == DeliveryNote_2.DeliveryNote_Id) && 
                       (DeliveryNote_1.Year == DeliveryNote_2.Year) &&
                       (DeliveryNote_1.Date.Date == DeliveryNote_2.Date.Date) && 
                       (DeliveryNote_1.FileNamePDF == DeliveryNote_2.FileNamePDF);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="DeliveryNote_1">Primera instáncia a comparar.</param>
        /// <param name="DeliveryNote_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(DeliveryNotesView DeliveryNote_1, DeliveryNotesView DeliveryNote_2)
        {
            return !(DeliveryNote_1 == DeliveryNote_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(DeliveryNote_Id, Year).GetHashCode());
        }

        #endregion
    }
}
