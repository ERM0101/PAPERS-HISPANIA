#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;

#endregion

namespace MBCode.Framework.Managers.Culture
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 16/02/2012.
    /// Descripción: clase que dota al Framework de las funcionalidades de control de la localización de una aplicación.
    /// </summary>
    public static class CultureManager
    {
        #region Constantes

        /// <summary>
        /// Nombre del ensamblador del Framework.
        /// </summary>
        private const string sAssembleNameFRWK = @"MBCode.Framework";

        /// <summary>
        /// Directorio donde se encuentra el fichero de recursos del Framework.
        /// </summary>
        private const string sPathFileResourcesFWK = @"component/Recursos/Resources/";

        /// <summary>
        /// Nombre del fichero de recursos del Framework.
        /// </summary>
        private const string sFileNameResourcesFWK = @"MBCode.Framework";

        #endregion

        #region Delegados y Eventos

        /// <summary>
        /// Delegado que define la firma del evento que se encarga de notificar a los elementos de la aplicación un cambio
        /// de cultura.
        /// </summary>
        public delegate void dlgChangeCulture();

        /// <summary>
        /// Evento que se encarga de notificar a los elementos de la aplicación un cambio de cultura.
        /// </summary>
        public static event dlgChangeCulture evChangeCulture;

        #endregion

        #region Atributos

        /// <summary>
        /// Lenguaje seleccionado para presentar los mensajes de la aplicación.
        /// </summary>
        private static CultureInfo ciActualCulture = Thread.CurrentThread.CurrentUICulture;

        /// <summary>
        /// Almacena el diccionario activo en función de la cultura seleccionada.
        /// </summary>
        private static ResourceDictionary rdActiveDictionary = GetResourceDictionary(sAssembleNameFRWK, sPathFileResourcesFWK, sFileNameResourcesFWK, ActualCultureName);

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el diccionario activo en función de la cultura seleccionada.
        /// </summary>
        public static ResourceDictionary ActiveDictionary
        {
            get
            {
                if (rdActiveDictionary == null) 
                {
                    rdActiveDictionary = GetResourceDictionary(sAssembleNameFRWK, sPathFileResourcesFWK, sFileNameResourcesFWK, ActualCultureName);
                }
                return (rdActiveDictionary);
            }
        }

        /// <summary>
        /// Lenguaje en el que se presentará la aplicación (CultureInfo).
        /// </summary>
        public static CultureInfo ActualCulture
        {
            get
            {
                return (ciActualCulture);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureManager_000"));
                if (!value.Equals(ciActualCulture)) ActualizeResources(value);
            }
        }

        /// <summary>
        /// Lenguaje en el que se presentará la aplicación (string).
        /// </summary>
        public static string ActualCultureName
        {
            get
            {
                return (ciActualCulture.TwoLetterISOLanguageName);
            }
            set
            {
                try
                {
                    if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureManager_001", "null"));
                    if (value.Equals(string.Empty)) throw new ArgumentException(MsgManager.ErrorMsg("MSG_CultureManager_001", string.Empty));
                    if (CultureInfo.GetCultureInfo(value) != ciActualCulture) ActualizeResources(new CultureInfo(value));
                }
                catch { }
            }
        }
        
        #endregion

        #region Métodos

        /// <summary>
        /// Actualiza la cultura indicada por el usuario y lo notifca a todos los elementos que se hayan suscrito a estos
        /// cambios.
        /// </summary>
        /// <param name="ciNewCulture">Nueva Cultura</param>
        private static void ActualizeResources(CultureInfo ciNewCulture)
        {
            ciActualCulture = ciNewCulture;
            Thread.CurrentThread.CurrentCulture = ciActualCulture;
            Thread.CurrentThread.CurrentUICulture = ciActualCulture;
            rdActiveDictionary = GetResourceDictionary(sAssembleNameFRWK, sPathFileResourcesFWK, sFileNameResourcesFWK, ActualCultureName);
            if (evChangeCulture != null) evChangeCulture();
        }

        /// <summary>
        /// Método que obtiene el diccionario de recursos asociado al Ensamblado, Fichero y a la cultura pasados cómo parámetro.
        /// Ejemplo:
        ///         GetResourceDictionary(@"MBCode.Framework", @"component/Recursos/Resources/", @"MBCode.Framework", ActualCultureName)
        /// </summary>
        /// <param name="sAssemblyName">Assembly donde se encuentra el fichero de recursos.</param>
        /// <param name="sResourcesPath">Path donde se encuentra el fichero de recursos.</param>
        /// <param name="sFileName">Nombre del Diccionario de Recursos, sin cultura ni extensión.</param>
        /// <param name="sCulture">Cultura a inicializa</param>
        public static ResourceDictionary GetResourceDictionary(string sAssemblyName, string sResourcesPath, string sFileName, 
                                                               string sCulture)
        {
            CultureInfo ci = CultureInfo.GetCultureInfo(sCulture);
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source =
                    new Uri(string.Format("pack://application:,,,/{0};{1}{2}.{3}.xaml", sAssemblyName, sResourcesPath, sFileName, sCulture));
            return (dict);
        }

        /// <summary>
        /// Método que obtiene el texto asociado a la clave pasada como parámetro dentro del diccionario de recursos del Framework.
        /// </summary>
        /// <param name="oKey">Clave asociada al texto dentro del diccionario del Framework.</param>
        /// <returns>Excpeción, sio no se encuentra la clave en el diccionario, texto asociado a la clave, si esta existe.</returns>
        public static string FindText(object oKey)
        {
            if (oKey == null) throw new ArgumentNullException("Error, 'null' key not found in Framework Dictionary.");
            if (oKey.Equals(string.Empty)) throw new ArgumentException("Error, 'empty' key not found in Framework Dictionary.");
            if (!ActiveDictionary.Contains(oKey)) throw new ArgumentException("Error, '" + oKey + "' key not found in Framework Dictionary.");
            return (ActiveDictionary[oKey].ToString());
        }

        #endregion
    }
}
