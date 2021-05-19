#region Librerias usadas por la clase

using System;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 27/02/2012.
    /// Descripción: clase que contiene los elementos basicos de las estructuras en las que se almacena la
    ///              información de un registro de una de las tablas de la Base de Datos.
    /// </summary>
    [Serializable()] 
    public abstract class CADataTable
    {
        #region Enumerados

        /// <summary>
        /// Enumerado que define los posibles estados de la instáncia de la clase.
        /// </summary>
        public enum InstanceState
        { 
            /// <summary>
            /// Indica que la instáncia de la clase no contine información.
            /// </summary>
            Empty,

            /// <summary>
            /// Indica que la instáncia de la clase contiene información.
            /// </summary>
            DataContains,
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Almacena la información del estado de la instáncia.
        /// </summary>
        private InstanceState eInstanceState;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene/Establece la información del estado de la instáncia.
        /// </summary>
        public InstanceState EstadoInstancia
        {
            get 
            { 
                return (eInstanceState); 
            }
            set 
            {
                eInstanceState = value; 
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        public CADataTable()
        {
            eInstanceState = InstanceState.Empty;
        }

        #endregion
    }
}
