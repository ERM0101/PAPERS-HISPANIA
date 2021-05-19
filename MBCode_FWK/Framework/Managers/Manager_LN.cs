#region Librerias usadas por la clase

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

#endregion

namespace MBCode.Framework.Managers
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 14/08/2013
    /// Descripción: clase que se encarga de proporcionar funcionalidades relacionadas con la lógica de negocio.
    /// </summary>
    public static class Manager_LN
    {
        #region Inicializadores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        public static void Init()
        {
        }

        #endregion

        #region Finalizadores

        /// <summary>
        /// Método que se invoca en el cierre de la aplicación para liberar los recursos de la aplicación y parar sus Threads.
        /// </summary>
        public static void Exit()
        {
        }

        #endregion

        #region Métodos

        #region Formateo de Texto

        /// <summary>
        /// Método que se encarga de Normalizar un Texto.
        /// </summary>
        /// <param name="TextToNormalize">Texto a normalizar.</param>
        /// <returns>Texto Normalizado.</returns>
        public static string Normalize(string TextToNormalize)
        {
            if (TextToNormalize == null) return (null);
            else return (TextToNormalize.Replace("#NL#", Environment.NewLine).Replace("#BS#", " "));
        }

        #endregion

        #region Lectura de datos del fichero de configuración

        /// <summary>
        /// Accede fichero .EXE.CONFIG y devuelve el valor de la clave solicitada
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string LeerAppSetting(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return (LeerAppSetting(key, config));
        }

        /// <summary>
        /// Accede fichero .EXE.CONFIG y devuelve el valor de la clave solicitada
        /// </summary>
        /// <param name="key">Clave asociada al parámetro del fichero de configuración.</param>
        /// <param name="config">Fichero de Configuración del que se desea leer los parámetros.</param>
        /// <returns></returns>
        public static string LeerAppSetting(string key, Configuration config = null)
        {
            if (config == null) config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return (config.AppSettings.Settings[key].Value);
        }

        /// <summary>
        /// Accede fichero .EXE.CONFIG y devuelve el valor de la clave solicitada
        /// </summary>
        /// <param name="dcKeyValue">Diccionario que contiene la información de los </param>
        /// <returns>true, si operación correcta, false si error.</returns>
        public static void ReleerAppSetting(Dictionary<string, string> dcKeyValue)
        {
            //  Abre la referencia al fichero de configuración en el que se leeran los datos.
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //  Lee los parámetros del fichero de configuración que tienen  por nombre las claves de los elementos del 
            //  Diccionario.
                if (dcKeyValue != null)
                {
                    foreach (string ParamName in new ArrayList(dcKeyValue.Keys))
                    {
                        dcKeyValue[ParamName] = LeerAppSetting(ParamName, config);
                    }
                }
        }

        #endregion

        #endregion
    }
}
