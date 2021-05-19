#region Librerias usadas por la clase

using MBCode.Framework.Managers.Culture;
using MBCode.Framework.Managers.Messages;
using System;

#endregion

namespace MBCode.Framework.Managers.DataTypes
{
    #region Enumerados

    /// <summary>
    /// Define los posibles tipos de formato de salida de fecha.
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// MM/DD/AAAA
        /// </summary>
        MMDDAAAA,

        /// <summary>
        /// DD/MM/AAAA
        /// </summary>
        DDMMAAAA,
    }

    /// <summary>
    /// Define el formato con el que se devolverá la fecha asociada a una fecha.
    /// </summary>
    public enum FormatoFecha
    {
        /// <summary>
        /// DD/MM/AAAA
        /// </summary>
        FechaCorta,

        /// <summary>
        // <dia de la semana>, DD de <mes del año> de AAAA
        /// </summary>
        FechaLarga,

        /// <summary>
        /// DD/MM/AAAA HH:MM:SS
        /// </summary>
        FechaNormal,

        /// <summary>
        /// AAAAMMDD HH:MM:SS
        /// </summary>
        FechaNormalizada,
    }

    /// <summary>
    /// Define el formato con el que se devolverá la hora asociada a una fecha.
    /// </summary>
    public enum FormatoHora
    {
        /// <summary>
        /// HH:MM
        /// </summary>
        HoraCorta,

        /// <summary>
        /// HH:MM:SS
        /// </summary>
        HoraLarga,
    }

    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha ultima modificación: 05/03/2012
    /// Descripción: clase que define un conjunto de métodos que extienden las funcinalidades que proporciona la clase 
    ///              DateTime.
    /// </summary>
    public static class DateTimeManager
    {
        #region Métodos

        #region Conversión de Fecha a String

        /// <summary>
        /// Transforma la fecha pasada como parámetro en una cadena de carácteres que representa su
        /// hora en el formato escogido por el usuario.
        /// </summary>
        /// <param name="dtmIn">Fecha a tratar</param>
        /// <param name="eFormato">Tipo de formato al que se traduce la hora</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Cadena de carácteres que contiene la hora en el formato deseado.</returns>
        public static string HoraAString(DateTime dtmIn, FormatoHora eFormato, ref string sMensaje)
        {
            try
            {
                switch (eFormato)
                {
                    case FormatoHora.HoraCorta:
                         return (FormatearHora(dtmIn.ToShortTimeString(), eFormato, ref sMensaje));
                    case FormatoHora.HoraLarga:
                         return (FormatearHora(dtmIn.ToLongTimeString(), eFormato, ref sMensaje));
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_DateTimeManager_000");
                         return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Transforma la fecha pasada como parámetro en una cadena de carácteres que representa su
        /// fecha en el formato escogido por el usuario.
        /// </summary>
        /// <param name="dtmIn">Fecha a tratar</param>
        /// <param name="sMsgError">Mensaje de error si se produce uno.</param>
        /// <returns>Cadena de carácteres que contiene la fecha en el formato deseado.</returns>
        public static string FechaAString(DateTime dtmIn, FormatoFecha eFormato, out string sMsgError)
        {
            try
            {
                //  Variables.
                    string sDataInfo;

                //  Inicialización de variables.
                    sDataInfo = null;
                    sMsgError = string.Empty;
                //  Formateo de la fecha.
                    switch (eFormato)
                    {
                        case FormatoFecha.FechaCorta:
                             return (dtmIn.ToShortDateString());
                        case FormatoFecha.FechaLarga:
                             try 
                             { 
                                 sDataInfo = dtmIn.ToString("D", CultureManager.ActualCulture); 
                             }
                             catch (Exception) 
                             {
                                 sDataInfo = dtmIn.ToString("D"); 
                             }
                             return (sDataInfo[0].ToString().ToUpper() + sDataInfo.Substring(1, sDataInfo.Length -1));
                        case FormatoFecha.FechaNormalizada:
                             try
                             {
                                 sDataInfo = dtmIn.ToString("{0:yyyyMMdd HH:mm:ss}", CultureManager.ActualCulture);
                             }
                             catch (Exception ex)
                             { 
                                 sDataInfo = null;
                                 sMsgError = MsgManager.ExcepMsg(ex);
                             }
                             return (sDataInfo);
                        case FormatoFecha.FechaNormal:
                             return (dtmIn.ToString());
                        default:
                             sMsgError = MsgManager.ErrorMsg("MSG_DateTimeManager_000");
                             return (null);
                    }
            }
            catch (Exception ex)
            {
                sMsgError = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Conversión de Segundos a TimeSpan

        /// <summary>
        /// Método encarhado de conseguir la representación en un TimeSpan de la Hora correspondiente
        /// a los segundos introducidos cómo parámetro.
        /// </summary>
        /// <param name="iSegundos">Segundos a interpretar</param>
        /// <returns>TiemSpan con la hora asociada a los segundos, si todo correcto, null, sinó.</returns>
        public static TimeSpan? SegundosAHora(int iSegundos, ref string sMensaje)
        {
            try
            {
                return (new TimeSpan(Convert.ToInt64(iSegundos * Math.Pow(10, 7.0))));
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Generación de Fechas de Archivo de Windows y de obtención de las originales

        /// <summary>
        /// Transforma la fecha pasada como parámetro en un long que representa su fecha en el formato 
        /// usado  por  Windows para identificar las fechas  de  los archivos.  ES IMPORTANTE TENER EN 
        /// CUENTA QUE ESTE NÚMERO NO CORRESPONDE A LOS TICKS DE LA FECHA EN CUESTIÓN.
        /// </summary>
        /// <param name="dtmIn">Fecha a tratar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Número con la fecha de Archivo asociada al parámetro.</returns>
        public static long? GenerarFechaArchivo(DateTime dtmIn, ref string sMensaje)
        {
            try
            {
                return (dtmIn.ToFileTime());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Recupera la fecha original del fichero de windows a partir del long pasado como parámetro en.
        /// </summary>
        /// <param name="lFechaFichero">Número con la fecha de Archivo asociada al parámetro</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Fecha asociada al número</returns>
        public static DateTime? ObtenerFechaArchivo(long lFechaFichero, ref string sMensaje)
        {
            try
            {
                return (DateTime.FromFileTime(lFechaFichero));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Serialización de una Fecha
        
        /// <summary>
        /// Genera un número de 64 bits en el que se almacena el objeto de tipo fecha pasado como parámetro
        /// para posteriormente poder recuperar directamente el objeto.  ES IMPORTANTE TENER  EN CUENTA QUE
        /// ESTE NÚMERO NO CORRESPONDE A LOS TICKS DE LA FECHA EN CUESTIÓN.
        /// </summary>
        /// <param name="dtmIn">Fecha a tratar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Número qeu almacena el objeto pasado como parámetro.</returns>
        public static long? SerializarFecha(DateTime dtmIn, ref string sMensaje)
        {
            try
            {
                return (dtmIn.ToBinary());
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Método encargado de deserializar una fecha almacenada mediante el uso de la función FechaALong
        /// con el parámetro "FechaSerializada".
        /// </summary>
        /// <param name="lFechaSerializada">Número asociado a la fecha serializada</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>DateTime asociado a la fecha, o Valor Minimo de DateTime si el número no corresponde a una fecha válida</returns>
        public static DateTime? RecuperarFecha(long lFechaSerializada, ref string sMensaje)
        {
            try
            {
                return (DateTime.FromBinary(lFechaSerializada));
            }
            catch (ArgumentException ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Formateo de Fechas y Horas

        /// <summary>
        /// Formatea una fecha segun el formato indicado en la entrada
        /// </summary>
        /// <param name="dtmIn"></param>
        /// <param name="eDataFormat"></param>
        /// <param name="sMensaje"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime dtmIn, DataFormat eDataFormat, ref string sMensaje)
        {
            try
            {
                switch (eDataFormat)
                {
                    case DataFormat.DDMMAAAA:
                        return (((dtmIn.Day < 10) ? "0" : string.Empty) + dtmIn.Day.ToString() + "/" +
                                ((dtmIn.Month < 10) ? "0" : string.Empty) + dtmIn.Month.ToString() + "/" +
                                ((dtmIn.Year < 10) ? "000" :
                                 ((dtmIn.Year < 100) ? "00" :
                                  ((dtmIn.Year < 1000) ? "0" : string.Empty))) + dtmIn.Year.ToString());
                    case DataFormat.MMDDAAAA:
                        return (((dtmIn.Month < 10) ? "0" : string.Empty) + dtmIn.Month.ToString() + "/" +
                                ((dtmIn.Day < 10) ? "0" : string.Empty) + dtmIn.Day.ToString() + "/" +
                                ((dtmIn.Year < 10) ? "000" :
                                 ((dtmIn.Year < 100) ? "00" :
                                  ((dtmIn.Year < 1000) ? "0" : string.Empty))) + dtmIn.Year.ToString());
                    default:
                        return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Método encargado de formatear la hora de manera que se pueda mostrar correctamente en la
        /// pantalla.
        /// </summary>
        /// <param name="sHora">Hora a formatear</param>
        /// <param name="eFormato">Formato de hora a reconocer.</param>
        /// <param name="sMensaje">Error, si se produce</param>
        /// <returns>null, si error, hora formateada sinó</returns>
        public static string FormatearHora(string sHora, FormatoHora eFormato, ref string sMensaje)
        {
            try
            {
                string sHoraForm;
                int iTemp;
                string[] asHora = sHora.Split(':');
                if ((asHora.Length == 3) && (eFormato == FormatoHora.HoraLarga)) 
                {
                    sHoraForm = (((iTemp = int.Parse(asHora[0])) < 10) ? "0" + iTemp.ToString() : iTemp.ToString()) + ":" +
                                (((iTemp = int.Parse(asHora[1])) < 10) ? "0" + iTemp.ToString() : iTemp.ToString()) + ":" +
                                (((iTemp = int.Parse(asHora[2].Substring(0, 2))) < 10) ? "0" + iTemp.ToString() : iTemp.ToString());
                }
                else if ((asHora.Length == 2) && (eFormato == FormatoHora.HoraCorta))
                {
                    sHoraForm = (((iTemp = int.Parse(asHora[0])) < 10) ? "0" + iTemp.ToString() : iTemp.ToString()) + ":" +
                                (((iTemp = int.Parse(asHora[1].Substring(0, 2))) < 10) ? "0" + iTemp.ToString() : iTemp.ToString());
                }
                else
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_DateTimeManager_000");
                    return (null);
                }
                return (sHoraForm);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Validar Datos

        /// <summary>
        /// Valida que el string pasado cómo parámetro corresponda a una hora correcta expresada en HH:MM:SS.
        /// </summary>
        /// <param name="sHora">String para analizar</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno</param>
        /// <returns>Hora expresada en segundos, si validación correcta, -1 si error</returns>
        public static int ValidarHora(string sHora, ref string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                int iHoraEnSegundos = 0;
                if (sHora == null) 
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_DateTimeManager_001");
                    return (-1);
                }
                string[] asDatosHora = sHora.Split(':');
                if (!((asDatosHora.GetLength(0) == 3) && (int.Parse(asDatosHora[0]) < 24)))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_DateTimeManager_002");
                    return (-1);
                }
                else iHoraEnSegundos += int.Parse(asDatosHora[0]) * 3600;
                if (int.Parse(asDatosHora[1]) > 59)
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_DateTimeManager_003");
                    return (-1);
                }
                else iHoraEnSegundos += int.Parse(asDatosHora[1]) * 60;
                if (int.Parse(asDatosHora[2]) > 59)
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_DateTimeManager_004");
                    return (-1);
                }
                else iHoraEnSegundos += int.Parse(asDatosHora[2]);
                return (iHoraEnSegundos);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (-1);
            }
        }

        #endregion

        #endregion
    }
}
