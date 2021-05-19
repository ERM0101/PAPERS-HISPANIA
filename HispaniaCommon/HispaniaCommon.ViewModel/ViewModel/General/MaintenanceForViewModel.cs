#region Libraries used by the class

using MBCode.Framework.Managers.Culture;
using System.Reflection;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region Culture

        /// <summary>
        /// Almacena el nombre del ensamblador de la Dll.
        /// </summary>
        private static string sAssemblyName = Assembly.GetAssembly(typeof(MaintenanceForViewModel)).GetName().Name;

        /// <summary>
        /// Almacena el nombre del fichero de recursos del ensamblador de la Dll.
        /// </summary>
        private static string sFileResourcesName = Assembly.GetAssembly(typeof(MaintenanceForViewModel)).GetName().Name;

        /// <summary>
        /// Se define un elemento del tipo de clase de Cultura heredado que permite trabajar con el fichero de recursos.
        /// </summary>
        public static CultureAssemblyWPF MainResources = new CultureAssemblyWPF(sAssemblyName, sFileResourcesName);

        #endregion

        #region Builders

        /// <summary>
        /// Default builder.
        /// </summary>
        public MaintenanceForViewModel(ApplicationType HispaniaApplicationType)
        {
            //  Determine if the machine can execute the application.
                ValidateMachineToExecuteApplication();
        }

        #endregion
    }
}

