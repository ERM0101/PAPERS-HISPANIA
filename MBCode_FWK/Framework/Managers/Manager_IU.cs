#region Librerias usadas por la clase

using MBCode.Framework.Managers.Culture;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

#endregion

namespace MBCode.Framework.Managers
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 14/08/2013
    /// Descripción: clase que se encarga de proporcionar funcionalidades relacionadas con la interfaz de usuario.
    /// </summary>
    public static class Manager_IU
    {
        #region Atributos

        /// <summary>
        /// Almacena el nombre del ensamblador de la Dll.
        /// </summary>
        private static string sAssemblyName = Assembly.GetAssembly(typeof(Manager_IU)).GetName().Name;

        /// <summary>
        /// Almacena el nombre del fichero de recursos del ensamblador de la Dll.
        /// </summary>
        private static string sFileResourcesName = Assembly.GetAssembly(typeof(Manager_IU)).GetName().Name;

        /// <summary>
        /// Se define un elemento del tipo de clase de Cultura heredado que permite trabajar con el fichero de recursos.
        /// </summary>
        public static CultureAssemblyWPF MainResources = new CultureAssemblyWPF(sAssemblyName, sFileResourcesName);

        #endregion

        #region Inicializadores

        /// <summary>
        /// Inicializa los elementos del Configurador.
        /// </summary>
        public static void Init()
        {
            //  Inicializaciones por defecto.
                string CultureName = Manager_LN.LeerAppSetting("CultureName");
                CultureManager.ActualCulture = new CultureInfo(CultureName);
        }

        #endregion

        #region Métodos

        #region Carga de Información del Fichero de Recursos

        #region GetObject

        /// <summary>
        /// Obtiene el recurso asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public static object GetObject(object _objectKey)
        {
            return (MainResources.FindResource(_objectKey));
        }

        #endregion

        #region GetGuid

        /// <summary>
        /// Obtiene el Guid del Control asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public static Guid GetGuid(object _objectKey)
        {
            try
            {
                return (new Guid(GetText(_objectKey.ToString())));
            }
            catch (Exception)
            {
                return (Guid.Empty);
            }
        }

        #endregion

        #region GetImage

        /// <summary>
        /// Obtiene el Control Image asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public static Image GetImage(object _objectKey)
        {
            try
            {
                Image image = new Image();
                image.Source = GetBitmapImage(_objectKey);
                return (image);
            }
            catch (Exception)
            {
                return (null);
            }
        }

        #endregion

        #region GetBitmapImage

        /// <summary>
        /// Obtiene el BitmapImage asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public static BitmapImage GetBitmapImage(object _objectKey)
        {
            try
            {
                return (new BitmapImage(new Uri(GetObject(_objectKey).ToString())));
            }
            catch (Exception)
            {
                return (null);
            }
        }

        #endregion

        #region GetText

        /// <summary>
        /// Obtiene el texto indicado por la clave pasada cómo parámetro.
        /// </summary>
        /// <param name="sKey">Clave asociada al Texto que se desea obtener.</param>
        /// <param name="oParam1">Parámetro con el que se desea construir el texto.</param>
        public static string GetText(string sKey, object oParam1)
        {
            return (GetText(sKey, new object[] { oParam1 }));
        }

        /// <summary>
        /// Obtiene el texto indicado por la clave pasada cómo parámetro.
        /// </summary>
        /// <param name="sKey">Clave asociada al Texto que se desea obtener.</param>
        /// <param name="oParam1">Parámetro con el que se desea construir el texto.</param>
        /// <param name="oParam2">Parámetro con el que se desea construir el texto.</param>
        public static string GetText(string sKey, object oParam1, object oParam2)
        {
            return (GetText(sKey, new object[] { oParam1, oParam2 }));
        }

        /// <summary>
        /// Obtiene el texto indicado por la clave pasada cómo parámetro.
        /// </summary>
        /// <param name="sKey">Clave asociada al Texto que se desea obtener.</param>
        /// <param name="oParam1">Parámetro con el que se desea construir el texto.</param>
        /// <param name="oParam2">Parámetro con el que se desea construir el texto.</param>
        /// <param name="oParam3">Parámetro con el que se desea construir el texto.</param>
        public static string GetText(string sKey, object oParam1, object oParam2, object oParam3)
        {
            return (GetText(sKey, new object[] { oParam1, oParam2, oParam3 }));
        }

        /// <summary>
        /// Obtiene el texto indicado por la clave pasada cómo parámetro.
        /// </summary>
        /// <param name="sKey">Clave asociada al Texto que se desea obtener.</param>
        /// <param name="oParams">Parámetros con los que se desea construir el texto.</param>
        public static string GetText(string sKey, object[] oParams = null)
        {
            if (oParams == null) return (Manager_LN.Normalize(MainResources.FindTextResource(sKey)));
            else return (Manager_LN.Normalize(string.Format(MainResources.FindTextResource(sKey), oParams)));
        }

        #endregion

        #endregion

        #endregion
    }
}

