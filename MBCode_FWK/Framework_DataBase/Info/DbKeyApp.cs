#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;  

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 14/10/2008
    /// Descripción: clase que define los datos con los que se identifica a un cliente de la clase de
    ///              acceso a Base de Datos dentro del sistema.
    /// </summary>
    public class DbKeyApp
    {
        #region Constantes

        /// <summary>
        /// Almacena un valor que indica el número de intentos que realiza el programa como máximo
        /// para conseguir un Guid válido que asociar a la nueva conexión.
        /// </summary>
        private const int NUM_INTENTOS_OBTENER_GUID = 5;

        #endregion

        #region Atributos

        /// <summary>
        /// Almacena el identificador de la aplicación.
        /// </summary>
        private Guid? guidApplicationID;

        /// <summary>
        /// Almacena el identificador distintivo de la clave.
        /// </summary>
        private Guid? guidDataKeyID;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene/Establece el identificador de la conexión a la que se refiere el cambió de estado de la conexión.
        /// </summary>
        public Guid? ApplicationID
        {
            get
            {
                return (guidApplicationID);
            }
        }

        /// <summary>
        /// Obtiene/Establece el identificador distintivo de la clave.
        /// </summary>
        internal Guid? DataKeyID
        {
            get
            {
                return (guidDataKeyID);
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase
        /// </summary>
        /// <param name="guidApplicationID">Identificador de la aplicación.</param>
        /// <param name="bResult">true, operación correcta, false, error.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        public DbKeyApp(Guid? guidApplicationID, out bool bResult, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Almacena el identificador de la aplicación a la que se concede la clave.
                    this.guidApplicationID = guidApplicationID;
                //  Obtiene un identificador válido.
                    this.guidDataKeyID = Guid.NewGuid();
                    bResult = true;
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                bResult = false;
            }
        }

        #endregion

        #region Métodos

        #region Métodos que redefinen la comparación de instáncias de esta clase

        /// <summary>
        /// Método encargado de determinar si dos claves son iguales.
        /// </summary>
        /// <param name="dbkaKeyOne">Primera clave a comparar.</param>
        /// <param name="dbkaKeyTwo">Segunda clave a comparar.</param>
        /// <returns>true, si las dos claves son iguales, false, si son diferentes</returns>
        public static bool operator ==(DbKeyApp dbkaKeyOne, DbKeyApp dbkaKeyTwo)
        {
            //  Si los dos valores son nulo, o los dos objetos corresponden a la misma instáncia, retorna cierto.
                if (Object.ReferenceEquals(dbkaKeyOne, dbkaKeyTwo)) return (true);
            //  Si solo uno de los dos es nulo, retorna falso.
                if (((object)dbkaKeyOne == null) || ((object)dbkaKeyTwo == null)) return (false);
            //  En cualquier otro caso compara el contenido mismo de los dos ojetos.
                return ((dbkaKeyOne.guidApplicationID == dbkaKeyTwo.guidApplicationID) &&
                        (dbkaKeyOne.guidDataKeyID == dbkaKeyTwo.guidDataKeyID));
        }

        /// <summary>
        /// Método encargado de determinar si dos claves son diferentes.
        /// </summary>
        /// <param name="dbkaKeyOne">Primera clave a comparar.</param>
        /// <param name="dbkaKeyTwo">Segunda clave a comparar.</param>
        /// <returns>true, si las dos claves son diferentes, false, si son iguales</returns>
        public static bool operator !=(DbKeyApp dbkaKeyOne, DbKeyApp dbkaKeyTwo)
        {
            return (!(dbkaKeyOne == dbkaKeyTwo));
        }

        /// <summary>
        /// Método que compara dos instáncias de esta clase.
        /// </summary>
        /// <param name="obj">Objeto a comparar</param>
        /// <returns>true, si son iguales, false, sino.</returns>
        public override bool Equals(Object obj)
        {
            //  Primero comprobamos que el parámetro de entrada sea distinto de nulo.
                if (obj == null) return false;
            //  Si el parámetro no se puede castear a DbKeyApp retorna falso.
                DbKeyApp dbkaKeyIn = obj as DbKeyApp;
                if ((Object)dbkaKeyIn == null) return (false);
            //  En cualquier otro caso compara el contenido mismo de los dos ojetos.
                return ((this.guidApplicationID == dbkaKeyIn.guidApplicationID) &&
                        (this.guidDataKeyID == dbkaKeyIn.guidDataKeyID));
        }

        /// <summary>
        /// Método que compara dos instáncias de esta clase.
        /// </summary>
        /// <param name="dbkaKeyIn">Objeto a comparar</param>
        /// <returns>true, si son iguales, false, sino.</returns>
        public bool Equals(DbKeyApp dbkaKeyIn)
        {
            //  Primero comprobamos que el parámetro de entrada sea distinto de nulo.
                if ((object)dbkaKeyIn == null) return (false);
            //  En cualquier otro caso compara el contenido mismo de los dos ojetos.
                return ((this.guidApplicationID == dbkaKeyIn.guidApplicationID) &&
                        (this.guidDataKeyID == dbkaKeyIn.guidDataKeyID));
        }

        /// <summary>
        /// Método que obtiene el Hash.
        /// </summary>
        /// <returns>Código de Hash</returns>
        public override int GetHashCode()
        {
            return (base.GetHashCode());
        }

        #endregion

        #region Otros métodos

        /// <summary>
        /// Método encargado de escribir el contenido de la instáncia de la clave.
        /// </summary>
        /// <returns>cadena de carácteres asociada a la clave, si todo correcto, null, si error.</returns>
        public override string ToString()
        {
            return (" · APPLICATION ID :- { " + guidApplicationID.ToString() + " }  · KEY ID :- { " + guidDataKeyID.ToString() + " }");
        }

        #endregion

        #endregion
    }
}

