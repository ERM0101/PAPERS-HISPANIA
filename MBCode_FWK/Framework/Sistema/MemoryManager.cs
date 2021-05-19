#region Librerias usadas por la clase

using System;
using System.Runtime.InteropServices;

#endregion

namespace MBCode.Framework.Managers.Sistema
{
    #region  Enumerados
    
    /// <summary>
    /// Almacena los valores de los diferentes tipos.
    /// </summary>
    public enum MemoryInfoType
    {
        /// <summary>
        /// Indicara que la información se prosentará en bytes.
        /// </summary>
        b = 0,

        /// <summary>
        /// Indicara que la información se prosentará en kilobytes.
        /// </summary>
        Kb = 10,

        /// <summary>
        /// Indicara que la información se prosentará en megabytes.
        /// </summary>
        Mb = 20,

        /// <summary>
        /// Indicara que la información se prosentará en gigabytes.
        /// </summary>
        Gb = 30,

        /// <summary>
        /// Indicara que la información se prosentará en terabytes.
        /// </summary>
        Tb = 40,
    }

    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha Última Modificación: 03/03/2010.
    /// Descripción: clase que muestra información sobre el estado de la memória.
    /// </summary>
    public static class MemoryManager
    {
        #region API -> Elementos necesarios para obtener la información

        /// <summary>
        /// Declaración del método que obtiene la información sobre el status de la memória.
        /// </summary>
        /// <param name="lpBuffer">Estructura en la que se almacena la información obtenida.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        /// <summary>
        /// Clase en la que se almacena la información sobre el estado de la memória.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        #endregion

        #region Estructuras Externas

        /// <summary>
        /// Estructura que almacena la información obtenida sobre el estado de la memória.
        /// </summary>
        public struct MemoryStatus
        {
            #region Atributos

            /// <summary>
            /// Almacena el Porcentaje de la memória física que está en uso.
            /// </summary>
            private uint uiPercentMemoryLoad;

            /// <summary>
            /// Almacena el total de memória física instalada en bytes.
            /// </summary>
            private ulong ulTotalPhysicalMemory;

            /// <summary>
            /// Almacena la memória física disponible en bytes.
            /// </summary>
            private ulong ulAvailablePhysicalMemory;

            /// <summary>
            /// Almacena el tamaño del fichero de paginación en bytes.
            /// </summary>
            private ulong ulPageFileSize;

            /// <summary>
            /// Almacena la memória de paginación disponible en bytes.
            /// </summary>
            private ulong ulAvailableMemoryInPageFile;

            /// <summary>
            /// Almacena el total de memória virtual en bytes.
            /// </summary>
            private ulong ulTotalAmountVirtualMemory;

            /// <summary>
            /// Almacena la memória virtual disponible en bytes.
            /// </summary>
            private ulong ulAvailableVirtualMemory;

            #endregion

            #region Propiedades

            /// <summary>
            /// Obtiene el Porcentaje de la memória física que está en uso.
            /// </summary>
            public uint PercentMemoryLoad
            {
                get
                {
                    return (uiPercentMemoryLoad);
                }
            }

            #region Physical Memory

            /// <summary>
            /// Obtiene el total de memória física instalada en bytes.
            /// </summary>
            public ulong TotalPhysicalMemory
            {
                get
                {
                    return (ulTotalPhysicalMemory);
                }
            }

            /// <summary>
            /// Obtiene la memória física disponible en bytes.
            /// </summary>
            public ulong AvailablePhysicalMemory
            {
                get
                {
                    return (AvailablePhysicalMemory);
                }
            }

            /// <summary>
            /// Obtiene la memória física disponible en bytes.
            /// </summary>
            public ulong LoadedPhysicalMemory
            {
                get
                {
                    return (ulTotalPhysicalMemory - ulAvailablePhysicalMemory);
                }
            }

            #endregion

            #region PageFile

            /// <summary>
            /// Obtiene el tamaño del fichero de paginación en bytes.
            /// </summary>
            public ulong PageFileSize
            {
                get
                {
                    return (ulPageFileSize);
                }
            }

            /// <summary>
            /// Obtiene la memória de paginación disponible en bytes.
            /// </summary>
            public ulong AvailableMemoryInPageFile
            {
                get
                {
                    return (ulAvailableMemoryInPageFile);
                }
            }

            /// <summary>
            /// Obtiene la memória de paginación disponible en bytes.
            /// </summary>
            public ulong LoadedMemoryInPageFile
            {
                get
                {
                    return (ulPageFileSize - ulAvailableMemoryInPageFile);
                }
            }

            #endregion

            #region Virtual Memory

            /// <summary>
            /// Obtiene el total de memória virtual en bytes.
            /// </summary>
            public ulong TotalAmountVirtualMemory
            {
                get
                {
                    return (ulTotalAmountVirtualMemory);
                }
            }

            /// <summary>
            /// Obtiene la memória virtual disponible en bytes.
            /// </summary>
            public ulong AvailableVirtualMemory
            {
                get
                {
                    return (ulAvailableVirtualMemory);
                }
            }

            /// <summary>
            /// Obtiene la memória virtual disponible en bytes.
            /// </summary>
            public ulong LoadedVirtualMemory
            {
                get
                {
                    return (ulTotalAmountVirtualMemory - ulAvailableVirtualMemory);
                }
            }

            #endregion

            #endregion

            #region Constructores

            /// <summary>
            /// Constructor por defecto de la estructura.
            /// </summary>
            /// <param name="msex">Estructura original.</param>
            /// <param name="bResult">true, inicialización correcta, false, si error.</param>
            /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
            public MemoryStatus(MEMORYSTATUSEX msex, out bool bResult, out string sMensaje)
            {
                try
                {
                    if (msex != null)
                    {
                        uiPercentMemoryLoad = msex.dwMemoryLoad;
                        ulTotalPhysicalMemory = msex.ullTotalPhys;
                        ulAvailablePhysicalMemory = msex.ullAvailPhys;
                        ulPageFileSize = msex.ullTotalPageFile;
                        ulAvailableMemoryInPageFile = msex.ullAvailPageFile;
                        ulTotalAmountVirtualMemory = msex.ullTotalVirtual;
                        ulAvailableVirtualMemory = msex.ullAvailVirtual;
                        sMensaje = string.Empty;
                        bResult = true;
                    }
                    else
                    {
                        uiPercentMemoryLoad = 0;
                        ulTotalPhysicalMemory = ulAvailablePhysicalMemory = ulPageFileSize = 0;
                        ulAvailableMemoryInPageFile = ulTotalAmountVirtualMemory = ulAvailableVirtualMemory = 0;
                        sMensaje = "Error, la estructura original estaba vacía.";
                        bResult = false;
                    }
                }
                catch (Exception ex)
                {
                    uiPercentMemoryLoad = 0;
                    ulTotalPhysicalMemory = ulAvailablePhysicalMemory = ulPageFileSize = 0;
                    ulAvailableMemoryInPageFile = ulTotalAmountVirtualMemory = ulAvailableVirtualMemory = 0;
                    sMensaje = ex.Message;
                    bResult = false;
                }
            }

            #endregion

            #region Métodos

            /// <summary>
            /// Obtiene una reprsentación del estado de la memória en texto.
            /// </summary>
            /// <param name="eInfoType">Modo en el que se desea ver la información.</param>
            /// <returns>Cadena de carácteres asociada al estado de la memória.</returns>
            public string ToString(MemoryInfoType eInfoType)
            {
                ulong ulInfoType = (ulong)Math.Pow(2, (int)eInfoType);
                return (string.Format("Memória: Física (Usada {0}/Total {1}) - Virtual (Usada {2}/Total {3})",
                                      Normalize(LoadedPhysicalMemory / ulInfoType, eInfoType),
                                      Normalize(TotalPhysicalMemory / ulInfoType, eInfoType),
                                      Normalize(LoadedVirtualMemory / ulInfoType, eInfoType),
                                      Normalize(TotalAmountVirtualMemory / ulInfoType, eInfoType)));
            }

            /// <summary>
            /// Método que normaliza el valor pasado cómo parámetro.
            /// </summary>
            /// <param name="ulInput">Valor de Entrada.</param>
            /// <returns>Cadena de carácteres con el valor normalizado</returns>
            private string Normalize(ulong ulInput, MemoryInfoType eInfoType)
            {
                try
                {
                    //  Variables.
                    string sValue;

                    //  Formatea el valor.
                    sValue = String.Format("{0:N}", ulInput);
                    return (sValue.Substring(0, sValue.Length - 3) + eInfoType.ToString());
                }
                catch (Exception ex)
                {
                    return (ex.Message);
                }
            }

            #endregion
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de obtener la información del estado de la memória.
        /// </summary>
        public static bool GetMemoryInfo(out MemoryStatus? msInfoMemory, out string sMensaje)
        {
            msInfoMemory = null;
            bool bResult = false;
            try
            {
                //  Obtiene y formatea los resultados.
                    MEMORYSTATUSEX msex = new MEMORYSTATUSEX();
                    if (GlobalMemoryStatusEx(msex)) msInfoMemory = new MemoryStatus(msex, out bResult, out sMensaje);
                    else sMensaje = "Error, al obtener el estado de la memória.";
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
            }
            return (bResult);
        }

        
        /// <summary>
        /// Método que presenta la información sobre la memória.
        /// </summary>
        public static new string ToString()
        {
            string sMensaje = string.Empty;
            try
            {
                //  Variables.
                    MemoryManager.MemoryStatus? msInfoMemory;

                //  Consulta el estado de la memória
                    if (MemoryManager.GetMemoryInfo(out msInfoMemory, out sMensaje))
                    {
                        sMensaje = ((MemoryManager.MemoryStatus)msInfoMemory).ToString(MemoryInfoType.Kb);
                    }
                    else sMensaje = "Error, al obtener información sobre el estado de la memória.";
            }
            catch (Exception ex)
            {
                sMensaje = "Error, al obtener información sobre el estado de la memória." + ex.Message;
            }
            return (sMensaje);
        }

        #endregion
    }
}
