#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Windows;

#endregion

namespace MBCode.Framework.Managers.Theme
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 23/02/2012.
    /// Descripción: clase que dota al Framework de las funcionalidades de control de los temas de una aplicación.
    /// </summary>
    public static class ThemeManager
    {
        #region Delegados y Eventos

        /// <summary>
        /// Delegado que define la firma del evento que se encarga de notificar a los elementos de la aplicación un cambio
        /// de tema.
        /// </summary>
        public delegate void dlgChangeTheme();

        /// <summary>
        /// Evento que se encarga de notificar a los elementos de la aplicación un cambio de tema.
        /// </summary>
        public static event dlgChangeTheme evChangeTheme;

        #endregion

        #region Atributos

        /// <summary>
        /// Tema seleccionado para la aplicación indicada.
        /// </summary>
        private static ThemeInfo tiActualTheme = new ThemeInfo(Themes.ExpressionDark);

        /// <summary>
        /// Almacena el diccionario activo en función del tema seleccionado.
        /// </summary>
        private static ResourceDictionary rdActiveResourceTheme = GetResourceDictionary(tiActualTheme);

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el diccionario activo en función de la cultura seleccionada.
        /// </summary>
        public static ResourceDictionary ActiveResourceTheme
        {
            get
            {
                if (rdActiveResourceTheme == null) rdActiveResourceTheme = GetResourceDictionary(tiActualTheme);
                return (rdActiveResourceTheme);
            }
        }

        /// <summary>
        /// Lenguaje en el que se presentará la aplicación (CultureInfo).
        /// </summary>
        public static ThemeInfo ActualTheme
        {
            get
            {
                return (tiActualTheme);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_ThemeManager_001", "null"));
                if (value != tiActualTheme) ActualizeTheme(value);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que obtiene el diccionario de recursos asociado al Tema a aplicar.
        /// </summary>
        /// <param name="tiNewTheme">Nuevo tema.</param>
        /// <returns>Diccionario asociado al tema a aplicar.</returns>
        public static ResourceDictionary GetResourceDictionary()
        {
            return (rdActiveResourceTheme);
        }

        /// <summary>
        /// Actualiza el tema indicado por el usuario y lo notifca a todos los elementos que se hayan suscrito a estos
        /// cambios.
        /// </summary>
        /// <param name="tiNewTheme">Nuevo tema.</param>
        private static void ActualizeTheme(ThemeInfo tiNewTheme)
        {
            tiActualTheme = tiNewTheme;
            rdActiveResourceTheme = GetResourceDictionary(tiNewTheme);
            if (evChangeTheme != null) evChangeTheme();
        }

        /// <summary>
        /// Método que obtiene el diccionario de recursos asociado al Tema a aplicar.
        /// </summary>
        /// <param name="tiNewTheme">Nuevo tema.</param>
        /// <returns>Diccionario asociado al tema a aplicar.</returns>
        private static ResourceDictionary GetResourceDictionary(ThemeInfo tiNewTheme)
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source =
                 new Uri("pack://application:,,,/" + 
                         string.Format("{0};{1}{2}.xaml", tiNewTheme.AssemblyName, tiNewTheme.PathFileTheme, tiNewTheme.FileNameTheme));
            return (dict);
        }

        #endregion
    }
}
