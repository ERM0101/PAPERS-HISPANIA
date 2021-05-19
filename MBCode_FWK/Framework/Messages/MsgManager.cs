#region Librerias usadas por la clase

using MBCode.Framework.Managers.Culture;
using System;
using System.Text;
using System.Windows;

#endregion

namespace MBCode.Framework.Managers.Messages
{
    #region Enumerados

    /// <summary>
    /// Enumerado que define los posibles tipos de mensaje que se pueden crear,
    /// </summary>
    public enum MsgType
    {
        Error,
        Warning,
        Information,
        Notification,
    }

    /// <summary>
    /// Define los posibles niveles de log de la aplicación.
    /// </summary>
    public enum TraceLevel
    {
        /// <summary>
        /// No se escribe nada en el Log.
        /// </summary>
        TRACE_LEVEL_NONE_LOG = 0,

        /// <summary>
        /// Log solo con los Errores graves (HORRORES).
        /// </summary>
        TRACE_LEVEL_CRITICAL_ERRORS = 1,

        /// <summary>
        /// Log solo con los Errores graves y el resto de Errores (ERRORES).
        /// </summary>
        TRACE_LEVEL_ERRORS = 2,

        /// <summary>
        /// Log con Errores graves, el resto de Errores y Avisos importantes (WARNING).
        /// </summary>
        TRACE_LEVEL_WARNINGS = 3,

        /// <summary>
        /// Log con Errores graves, el resto de Errores, Avisos importantes e Información Varia (INFO).
        /// </summary>
        TRACE_LEVEL_INFO = 4,

        /// <summary>
        /// Log con Errores graves, el resto de Errores, Avisos importantes, Información Varia e Información Adicional (VERBOSE).
        /// </summary>
        TRACE_LEVEL_VERBOSE = 5,

        /// <summary>
        /// Log con Errores graves, el resto de Errores, Avisos importantes, Información Varia e Información Adicional (VVERBOSE).
        /// </summary>
        TRACE_LEVEL_VVERBOSE = 6,

        /// <summary>
        /// Log con Errores graves, el resto de Errores, Avisos importantes, Información Varia e Información Adicional (VVVERBOSE).
        /// </summary>
        TRACE_LEVEL_VVVERBOSE = 7,

        /// <summary>
        /// Se muestra toda la información disponible.
        /// </summary>
        TRACE_LEVEL_ALL_INFO = 256,
    }

    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 27/02/2012.
    /// Descripción: clase que controla la creación de los mensajes a partir de los recursos del Framework.
    /// </summary>
    public static class MsgManager
    {
        #region Atributos

        /// <summary>
        /// Almacena el nivel de log de error que se desea usar.
        /// </summary>
        private static TraceLevel m_LevelTraceLog = TraceLevel.TRACE_LEVEL_INFO;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene / Establece el nivel de log de error que se desea usar.
        /// </summary>
        public static TraceLevel LevelTraceLog
        {
            get
            {
                return (m_LevelTraceLog);
            }
            set
            {
                m_LevelTraceLog = value;
            }
        }

        #endregion

        #region Literal Methods

        /// <summary>
        /// Crea un menaje asociado al literal pasado cómo parámetro.
        /// </summary>
        /// <param name="sLiteral">Mensaje literal a obtener.</param>
        /// <param name="eMsgType">Tipo de mensaje</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>String.Empty, en caso de error, literal formateado en función del tipo, si todo correcto.</returns>
        public static string LiteralMsg(string sLiteral, MsgType eMsgType = MsgType.Error, TraceLevel? traceLevel = null)
        {
            switch (eMsgType)
            {
                case MsgType.Error:
                case MsgType.Warning:
                case MsgType.Information:
                     return (sLiteral);
                default:
                     return (string.Empty);
            }
        }

        /// <summary>
        /// Crea un menaje asociado al literal pasado cómo parámetro
        /// </summary>
        /// <param name="eMsgType">Tipo de mensaje</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>String.Empty, en caso de error, literal formateado en función del tipo, si todo correcto.</returns>
        public static string LiteralMsg(string[] asParams, MsgType eMsgType = MsgType.Error, TraceLevel? traceLevel = null)
        {
            return (MakeMessageWithDetails(asParams, traceLevel));
        }

        /// <summary>
        /// Crea un menaje asociado al literal pasado cómo parámetro
        /// </summary>
        /// <param name="sLiteral">Mensaje literal a obtener.</param>
        /// <param name="eMsgType">Tipo de mensaje</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>String.Empty, en caso de error, literal formateado en función del tipo, si todo correcto.</returns>
        public static string LiteralMsg(string sLiteral, string[] asParams, MsgType eMsgType = MsgType.Error, 
                                        TraceLevel? traceLevel = null)
        {
            return (MakeMessgeWithParams(LiteralMsg(sLiteral, eMsgType), asParams, traceLevel));
        }

        /// <summary>
        /// Crea un menaje asociado al literal pasado cómo parámetro añadiendo la información de la excepción pasada cómo
        /// parámetro.
        /// </summary>
        /// <param name="sLiteral">Mensaje literal a obtener.</param>
        /// <param name="ex">Excepción a tratar.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>String.Empty, en caso de error, literal formateado en función del tipo, si todo correcto.</returns>
        public static string LiteralMsg(string sLiteral, Exception ex, TraceLevel? traceLevel = null)
        {
            return (MakeMessageWithDetails(LiteralMsg(sLiteral, MsgType.Error, traceLevel), ExcepMsg(ex), traceLevel));
        }

        #endregion

        #region Error Methods

        /// <summary>
        /// Crea el texto de error asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>Texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si error.</returns>
        public static string ErrorMsg(string ResourcesKey, TraceLevel? traceLevel = null)
        {
            try
            {
                return (CultureManager.FindText(ResourcesKey));
            }
            catch (Exception ex)
            {
                return (string.Format("Error in metod 'ErrorMsg(string ResourcesKey)'.\r\nDetails:{0}.\r\n", ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Crea el texto de error asociado a la clave pasada como parámetro con el texto pasado cómo  parámetro.  Busca  el
        /// texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="sParam1">Parámetro que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si error.</returns>
        public static string ErrorMsg(string ResourcesKey, string sParam1, TraceLevel? traceLevel = null)
        {
            return (ErrorMsg(ResourcesKey, new string[] { sParam1 }, traceLevel));
        }

        /// <summary>
        /// Crea el texto de error asociado a la clave pasada como parámetro con los textos pasados cómo parámetro. Busca el 
        /// texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="sParam1">Primer Parámetro que se desea añadir.</param>
        /// <param name="sParam2">Segundo Parámetro que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si error.</returns>
        public static string ErrorMsg(string ResourcesKey, string sParam1, string sParam2, TraceLevel? traceLevel = null)
        {
            return (ErrorMsg(ResourcesKey, new string[] { sParam1, sParam2 }, traceLevel));
        }

        /// <summary>
        /// Crea el texto de error asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="BaseMessage">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="asParams">Parámetros que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si error.</returns>
        public static string ErrorMsg(string ResourcesKey, string[] asParams, TraceLevel? traceLevel = null)
        {
            return (MakeMessgeWithParams(ErrorMsg(ResourcesKey), asParams, traceLevel));
        }

        /// <summary>
        /// Crea el texto de error asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="ex">Excepción que complementa la información a mostrar en el mensaje de error.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si error.</returns>
        public static string ErrorMsg(string ResourcesKey, Exception ex, TraceLevel? traceLevel = null)
        {
            return (MakeMessageWithDetails(ErrorMsg(ResourcesKey), ExcepMsg(ex), traceLevel));
        }

        #endregion

        #region Warning Methods

        /// <summary>
        /// Crea el texto de Aviso asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>Texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Warning.</returns>
        public static string WarningMsg(string ResourcesKey, TraceLevel? traceLevel = null)
        {
            try
            {
                return (CultureManager.FindText(ResourcesKey));
            }
            catch (Exception ex)
            {
                return (string.Format("Warning in metod 'WarningMsg(string ResourcesKey)'.\r\nDetails:{0}.\r\n", ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Crea el texto de Aviso asociado a la clave pasada como parámetro con el texto pasado cómo  parámetro.  Busca  el
        /// texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="sParam1">Parámetro que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Warning.</returns>
        public static string WarningMsg(string ResourcesKey, string sParam1, TraceLevel? traceLevel = null)
        {
            return (WarningMsg(ResourcesKey, new string[] { sParam1 }, traceLevel));
        }

        /// <summary>
        /// Crea el texto de Aviso asociado a la clave pasada como parámetro con los textos pasados cómo parámetro. Busca el 
        /// texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="sParam1">Primer Parámetro que se desea añadir.</param>
        /// <param name="sParam2">Segundo Parámetro que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Warning.</returns>
        public static string WarningMsg(string ResourcesKey, string sParam1, string sParam2, TraceLevel? traceLevel = null)
        {
            return (WarningMsg(ResourcesKey, new string[] { sParam1, sParam2 }, traceLevel));
        }

        /// <summary>
        /// Crea el texto de Aviso asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="BaseMessage">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="asParams">Parámetros que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Warning.</returns>
        public static string WarningMsg(string ResourcesKey, string[] asParams, TraceLevel? traceLevel = null)
        {
            return (MakeMessgeWithParams(WarningMsg(ResourcesKey), asParams, traceLevel));
        }

        /// <summary>
        /// Crea el texto de Aviso asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="ex">Excepción que complementa la información a mostrar en el mensaje de Warning.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Warning.</returns>
        public static string WarningMsg(string ResourcesKey, Exception ex, TraceLevel? traceLevel = null)
        {
            return (MakeMessageWithDetails(WarningMsg(ResourcesKey), ExcepMsg(ex), traceLevel));
        }

        #endregion

        #region Info Methods

        /// <summary>
        /// Crea el texto de Información asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>Texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Info.</returns>
        public static string InfoMsg(string ResourcesKey, TraceLevel? traceLevel = null)
        {
            try
            {
                return (CultureManager.FindText(ResourcesKey));
            }
            catch (Exception ex)
            {
                return (string.Format("Info in metod 'InfoMsg(string ResourcesKey)'.\r\nDetails:{0}.\r\n", ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Crea el texto de Información asociado a la clave pasada como parámetro con el texto pasado cómo  parámetro.  Busca  el
        /// texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="sParam1">Parámetro que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Info.</returns>
        public static string InfoMsg(string ResourcesKey, string sParam1, TraceLevel? traceLevel = null)
        {
            return (InfoMsg(ResourcesKey, new string[] { sParam1 }, traceLevel));
        }

        /// <summary>
        /// Crea el texto de Información asociado a la clave pasada como parámetro con los textos pasados cómo parámetro. Busca el 
        /// texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="sParam1">Primer Parámetro que se desea añadir.</param>
        /// <param name="sParam2">Segundo Parámetro que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Info.</returns>
        public static string InfoMsg(string ResourcesKey, string sParam1, string sParam2, TraceLevel? traceLevel = null)
        {
            return (InfoMsg(ResourcesKey, new string[] { sParam1, sParam2 }, traceLevel));
        }

        /// <summary>
        /// Crea el texto de Información asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="BaseMessage">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="asParams">Parámetros que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Info.</returns>
        public static string InfoMsg(string ResourcesKey, string[] asParams, TraceLevel? traceLevel = null)
        {
            return (MakeMessgeWithParams(InfoMsg(ResourcesKey), asParams, traceLevel));
        }

        /// <summary>
        /// Crea el texto de Información asociado a la clave pasada como parámetro. Busca el texto en el diccionario del Framework.
        /// </summary>
        /// <param name="ResourcesKey">Clave dentro del diccionario del texto a mostrar.</param>
        /// <param name="ex">Excepción que complementa la información a mostrar en el mensaje de Info.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si Info.</returns>
        public static string InfoMsg(string ResourcesKey, Exception ex, TraceLevel? traceLevel = null)
        {
            return (MakeMessageWithDetails(InfoMsg(ResourcesKey), ExcepMsg(ex), traceLevel));
        }

        #endregion

        #region Exception Methods

        /// <summary>
        /// Define el texto que se mostrará en caso de que se desee mostrar la información de una excepción.
        /// </summary>
        /// <param name="ex">Excpción a tratar.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>string.Empty, en caso de que la excepción sea nula, Message, si la excepción no es nula.</returns>
        public static string ExcepMsg(Exception ex, TraceLevel? traceLevel = null)
        {
            try
            {
                //  Si la excepción no es nula crea el mensaje de error a mostrar..
                    if (ex != null)
                    {
                        //  Determina el nivel de trazas que debe usar para construir el mensaje.
                            TraceLevel verbose;
                            if (traceLevel != null) verbose = (TraceLevel)traceLevel;
                            else verbose = LevelTraceLog;
                        //  Con el nivel de Traza indicado construye el mensaje a devolver.
                            switch (verbose)
                            {
                                case TraceLevel.TRACE_LEVEL_NONE_LOG:
                                     return ex.Message;
                                case TraceLevel.TRACE_LEVEL_ERRORS:
                                case TraceLevel.TRACE_LEVEL_WARNINGS:
                                case TraceLevel.TRACE_LEVEL_INFO:
                                     return String.Format("{0}\r\n{1}", 
                                                          ex.Message,
                                                          (ex.InnerException != null) ? string.Format("Details: {0}\r\n", ex.InnerException.Message) : string.Empty);
                                case TraceLevel.TRACE_LEVEL_VERBOSE:        // Increase details level of the error respect VERBOSE
                                     return String.Format("Error in App-Object:{0}\r\nMethod{1}\r\nMain Error Message: {2}\r\n", 
                                                          ex.Source,
                                                          ex.TargetSite,
                                                          ex.Message);
                                case TraceLevel.TRACE_LEVEL_VVERBOSE:        // Increase details level of the error respect VERBOSE
                                     return String.Format("Error in App-Object:{0}\r\nMethod{1}\r\nMain Error Message: {2}\r\nMore Details Error: {3}\r\n", 
                                                          ex.Source,
                                                          ex.TargetSite,
                                                          ex.Message, 
                                                          (ex.InnerException != null) ? string.Format("Details: {0}\r\n", ex.InnerException.Message) : string.Empty);
                                case TraceLevel.TRACE_LEVEL_VVVERBOSE:        // Increase details level of the error respect VVERBOSE
                                     return String.Format("Error in App-Object:{0}\r\nMethod{1}\r\nMain Error Message: {2}\r\nMore Details Error: {3}\r\n.Stack Trace: {4}\r\n.", 
                                                          ex.Source,
                                                          ex.TargetSite,
                                                          ex.Message,
                                                          (ex.InnerException != null) ? string.Format("Details: {0}\r\n", ex.InnerException.Message) : string.Empty,
                                                          ex.StackTrace);
                                case TraceLevel.TRACE_LEVEL_CRITICAL_ERRORS:  //  Bad Errors only.
                                case TraceLevel.TRACE_LEVEL_ALL_INFO:         //  All Information to provide.
                                     return String.Format("Error in App-Object:{0}\r\nMethod{1}\r\nMain Error Message: {2}\r\nMore Details Error: {3}\r\n.User Data Info: {4}\r\nStack Trace: {5}\r\n.", 
                                                          ex.Source,
                                                          ex.TargetSite,
                                                          ex.Message,
                                                          (ex.InnerException != null) ? string.Format("Details: {0}\r\n", ex.InnerException.Message) : string.Empty,
                                                          ex.StackTrace,
                                                          ex.Data);
                            }
                    }
                    return String.Empty;
            }
            catch (Exception exception)
            {
                return (string.Format("Error in metod 'ExcepMsg(Exception ex, TraceLevel? traceLevel = null)'.\r\nDetails:{0}.\r\n",
                                      ExcepMsg(exception)));
            }
        }

        #endregion

        #region Question Methods

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="sMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static MessageBoxResult ShowQuestion(object oMessage)
        {
            //  Por seguridad y robustez evitamos messages 'vacios' 
                if ((oMessage != null) && (oMessage is string))
                {
                    return ShowQuestion(oMessage.ToString());
                }
                else throw new ArgumentException("Error, el paràmetre introduït per fer la pregunta no és un text.");
        }

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="sMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static MessageBoxResult ShowQuestion(string sMessage)
        {
            if ((!String.IsNullOrEmpty(sMessage)) && (!String.IsNullOrWhiteSpace(sMessage)))
            {
                sMessage = sMessage.Replace(@"\r\n", Environment.NewLine);
            }
            return (MessageBox.Show(sMessage, "Question", MessageBoxButton.YesNo, MessageBoxImage.Question));
        }

        #endregion

        #region View Methods

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="oMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static void ShowMessage(object oMessage, MsgType eMsgType = MsgType.Error)
        {
            ShowMessage(oMessage, new string[] { }, eMsgType);
        }

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="oMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static void ShowMessage(object oMessage, string ParamValue, MsgType eMsgType = MsgType.Error)
        {
            ShowMessage(oMessage, new string[] { ParamValue }, eMsgType);
        }

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="oMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static void ShowMessage(object oMessage, string ParamValue_1, string ParamValue_2, MsgType eMsgType = MsgType.Error)
        {
            ShowMessage(oMessage, new string[] { ParamValue_1, ParamValue_2 }, eMsgType);
        }

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="oMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static void ShowMessage(object oMessage, string[] ParamValues, MsgType eMsgType = MsgType.Error)
        {
            //  Por seguridad y robustez evitamos messages 'vacios' 
                if ((oMessage != null) && (oMessage is string))
                {
                    ShowMessage(string.Format(oMessage.ToString(), ParamValues), 6, eMsgType);
                }
                else throw new ArgumentException("Error, el paràmetre introduït no és un text.");
        }

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="sMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static void ShowMessage(string sMessage, MsgType eMsgType = MsgType.Error)
        {
            //  Por seguridad y robustez evitamos messages 'vacios' 
                if (!String.IsNullOrEmpty(sMessage) && !String.IsNullOrWhiteSpace(sMessage))
                {
                    //  Pretty 'Details' show
                        sMessage = sMessage.Replace(@"\r\n", Environment.NewLine);
                        sMessage = sMessage.Replace("Details:", "Details:" + Environment.NewLine);
                        sMessage = sMessage.Replace("Détails:", "Détails:" + Environment.NewLine);
                        sMessage = sMessage.Replace("Detalles:", "Detalles:" + Environment.NewLine);
                        ShowMessage(sMessage, 6, eMsgType);
                }
        }

        /// <summary>
        /// Método que se encarga de presentar por pantalla el Mensaje pasado como parámetro.
        /// </summary>
        /// <param name="sMessage">Mensaje a Mostrar</param>
        /// <param name="eMsgType">Tipo de mensaje a mostrar.</param>
        public static void ShowMessage(string sMessage, int iSeconds, MsgType eMsgType = MsgType.Error)
        {
            //  Variables.
                //WaitDialog Wait;

            //  Por seguridad y robustez evitamos messages 'vacios' 
                if (!String.IsNullOrEmpty(sMessage) && !String.IsNullOrWhiteSpace(sMessage))
                {
                    sMessage = sMessage.Replace(@"\r\n", Environment.NewLine);
                    switch (eMsgType)
                    {
                        case MsgType.Error:
                             MessageBox.Show(sMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                             break;
                        case MsgType.Warning:
                             MessageBox.Show(sMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                             break;
                        case MsgType.Information:
                             MessageBox.Show(sMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                             break;
                        case MsgType.Notification:
                             MessageBox.Show(sMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                             break;
                    }
                    //switch (eMsgType)
                    //{
                    //    case MsgType.Error:
                    //         Wait = new WaitDialog("Error", false, iSeconds, Orientation.Horizontal, sMessage, false, MessageType.Error);
                    //         break;
                    //    case MsgType.Warning:
                    //         Wait = new WaitDialog("Warning", false, iSeconds, Orientation.Horizontal, sMessage, false, MessageType.Warning);
                    //         break;
                    //    case MsgType.Information:
                    //         Wait = new WaitDialog("Information", false, iSeconds, Orientation.Horizontal, sMessage, false, MessageType.Information);
                    //         break;
                    //    case MsgType.Notification:
                    //         Wait = new WaitDialog("Information", false, iSeconds, Orientation.Horizontal, sMessage, false, MessageType.Notification);
                    //         break;
                    //    default:
                    //         Wait = null;
                    //         break;
                    //}
                }
        }

        #endregion

        #region Auxiliars

        /// <summary>
        /// Combina los dos textos pasados como parámetro para mostrarlos como información + detalles.
        /// </summary>
        /// <param name="sMainMessage">Parte del mensaje que contiene la información.</param>
        /// <param name="sDetailsMessage">parte del mensaje que contiene los detalles.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>Mensaje formateado.</returns>
        private static string MakeMessageWithDetails(string sMainMessage, string sDetailsMessage, TraceLevel? traceLevel = null)
        {
            try
            {
                return (string.Format("{0} Details: {1}", sMainMessage, sDetailsMessage));
            }
            catch (Exception ex)
            {
                return (string.Format("Error in metod 'MakeMessageWithDetails(string sMainMessage, string sDetailsMessage, TraceLevel? traceLevel = null)'.\r\nDetails:{0}.\r\n",
                                      ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Combina los dos textos pasados como parámetro para mostrarlos como información + detalles.
        /// </summary>
        /// <param name="sMainMessage">Parte del mensaje que contiene la información.</param>
        /// <param name="asDetailsMessage">parte del mensaje que contiene los detalles.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>Mensaje formateado.</returns>
        private static string MakeMessageWithDetails(string[] asMessages, TraceLevel? traceLevel = null)
        {
            try
            {
                if (asMessages.Length == 0) return (string.Empty);
                else if (asMessages.Length == 1) return (asMessages[0]);
                else
                {
                    StringBuilder sbDetailsMessage = new StringBuilder(string.Empty);
                    for (int i = 1; i < asMessages.Length; i++)
                    {
                        sbDetailsMessage.AppendLine(asMessages[i]);
                    }
                    return (string.Format("{0} \r\nDetails: \r\n{1}", asMessages[0], sbDetailsMessage.ToString()));
                }

            }
            catch (Exception ex)
            {
                return (string.Format("Error in metod 'MakeMessageWithDetails(string[] asMessages, TraceLevel? traceLevel = null)'.\r\nDetails:{0}.\r\n",
                                      ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Combina los dos textos pasados como parámetro para mostrarlos como información + detalles.
        /// </summary>
        /// <param name="sMainMessage">Parte del mensaje que contiene la información.</param>
        /// <param name="asDetailsMessage">parte del mensaje que contiene los detalles.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>Mensaje formateado.</returns>
        private static string MakeMessageWithDetails(string BaseMessage, string[] asDetailsMessage, TraceLevel? traceLevel = null)
        {
            try
            {
                StringBuilder sbDetailsMessage = new StringBuilder(string.Empty);
                for (int i = 0; i < asDetailsMessage.Length; i++)
                {
                    sbDetailsMessage.AppendLine(asDetailsMessage[i]);
                }
                return (string.Format("{0} \r\nInfo: \r\n{1}", BaseMessage, sbDetailsMessage.ToString()));

            }
            catch (Exception ex)
            {
                return (string.Format("Error in metod 'MakeMessageWithDetails(string BaseMessage, string[] asDetailsMessage, TraceLevel? traceLevel = null)'.\r\nDetails:{0}.\r\n",
                                      ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Crea el texto asociado al mensaje base pasado cómo parámetro con los textos pasados cómo parámetro.
        /// </summary>
        /// <param name="BaseMessage">Mensaje base con el que se desea crear el texto a mostrar.</param>
        /// <param name="asParams">Parámetros que se desea añadir.</param>
        /// <param name="traceLevel">Nivel de Traza específico para el mensaje indicado.</param>
        /// <returns>texto asociado al elemento de clave oKey, si oKey es correcto, string.Empty, si error.</returns>
        private static string MakeMessgeWithParams(string BaseMessage, string[] asParams, TraceLevel? traceLevel = null)
        {
            string Result;
            try
            {
                //  Variables.
                    bool bFormatIsCorrect;

                //  En segundo lugar determina si hay parámetros a mostrar y si es así intenta formatear el mensaje con ellos.
                    if (asParams != null)
                    {
                        //  Si hat parámetros valida que el formato del texto base sea correcto para ellos.
                            bFormatIsCorrect = true;
                            for (int IndexParam = 0; IndexParam < asParams.Length; IndexParam++)
                            {
                                bFormatIsCorrect &= BaseMessage.Contains("{" + IndexParam + "}");
                                if (!bFormatIsCorrect) break;
                            }
                            if (bFormatIsCorrect) Result = string.Format(BaseMessage, asParams);
                            else Result = MakeMessageWithDetails(BaseMessage, asParams);
                    }
                    else Result = string.Format(BaseMessage, asParams);
            }
            catch (Exception ex)
            {
                Result = string.Format("Error in metod 'MakeMessgeWithParams(string BaseMessage, string[] asParams)'.\r\nDetails:{0}.\r\n",
                                      ExcepMsg(ex));
            }
            return (Result);
        }
        
        #endregion
    }
}
