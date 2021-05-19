#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

#endregion

namespace MBCode.Framework.DataBase.Utils
{
    ///<summary>
    ///Autor: Alejandro Moltó Bou
    ///Fecha ultima modificación: 27/04/2009
    ///Descripción: clase que define un conjunto de métodos que extienden las funcionalidades de Bases
    ///             de Datos nativas de .Net. Y que definen un conjunto de operaciones sobre los tipos 
    ///             básicos de  datos:
    /// 
    ///                        - Tipo :- BOOLEAN    -> Recognized
    ///                        - Tipo :- BYTE       -> Recognized
    ///                        - Tipo :- CHAR       -> Recognized
    ///                        - Tipo :- DATETIME   -> Recognized
    ///                        - Tipo :- DECIMAL    -> Recognized
    ///                        - Tipo :- DOUBLE     -> Recognized
    ///                        - Tipo :- SINGLE     -> Recognized
    ///                        - Tipo :- GUID       -> Recognized
    ///                        - Tipo :- INT32      -> Recognized
    ///                        - Tipo :- INTPTR     -> Recognized
    ///                        - Tipo :- LONG       -> Recognized
    ///                        - Tipo :- OBJECT     -> Recognized
    ///                        - Tipo :- STRING     -> Recognized
    ///                        - Tipo :- SBYTE      -> Recognized
    ///                        - Tipo :- SHORT      -> Recognized
    ///                        - Tipo :- UINT       -> Recognized
    ///                        - Tipo :- UINTPTR    -> Recognized
    ///                        - Tipo :- ULONG      -> Recognized
    ///                        - Tipo :- USHORT     -> Recognized
    /// 
    ///</summary>
    public static class BDUtils
    {
        #region Enumerados

        /// <summary>
        /// Define el tipo de formato que se desea usar para mostrar la fecha.
        /// </summary>
        public enum FormatDateTime
        { 
            /// <summary>
            /// Fecha.
            /// </summary>
            Date,

            /// <summary>
            /// Hora.
            /// </summary>
            Time,

            /// <summary>
            /// Fecha y Hora.
            /// </summary>
            DateAndTime,

            /// <summary>
            /// Hora con milisegundos
            /// </summary>
            DateAndTimeWithMiliseconds
        }

        #endregion

        #region Constantes

        #region Valores de Inicialización para los tipos de la Base de Datos

        #region Boolean

        /// <summary>
        /// Valor por defecto para los elementos de tipo boolean.
        /// </summary>
        public readonly static bool BOOLEAN_DB_INIT_VALUE = false;

        #endregion       

        #region Byte

        /// <summary>
        /// Valor por defecto para los elementos de tipo byte.
        /// </summary>
        public readonly static byte BYTE_DB_INIT_VALUE = byte.MinValue + 1;

        #endregion

        #region Byte[]

        /// <summary>
        /// Valor por defecto para los elementos de tipo byte.
        /// </summary>
        public readonly static byte[] BYTEARRAY_DB_INIT_VALUE = new byte[1];

        #endregion

        #region Char

        /// <summary>
        /// Valor por defecto para los elementos de tipo char.
        /// </summary>
        public readonly static char CHAR_DB_INIT_VALUE =  (char) (((int) (char.MinValue)) + 1);

        #endregion

        #region DateTime

        /// <summary>
        /// Valor por defecto para los elementos de tipo DateTime.
        /// </summary>
        public readonly static DateTime DATETIME_DB_INIT_VALUE = new DateTime(1900, 1, 1, 0, 0, 0);

        /// <summary>
        /// Valor por defecto para los elementos de tipo DateTime para SQL Server.
        /// </summary>
        public readonly static DateTime DATETIME_DB_INIT_VALUE_SQL_SERVER = new DateTime(1753, 1, 1, 12, 0, 0);

        #endregion

        #region Decimal

        /// <summary>
        /// Valor por defecto para los elementos de tipo decimal.
        /// </summary>
        public readonly static decimal DECIMAL_DB_INIT_VALUE = Decimal.MinValue + 1;

        #endregion

        #region Double

        /// <summary>
        /// Valor por defecto para los elementos de tipo double.
        /// </summary>
        public readonly static double DOUBLE_DB_INIT_VALUE = double.MinValue + 1;

        #endregion

        #region Single (Float)

        /// <summary>
        /// Valor por defecto para los elementos de tipo Single (float).
        /// </summary>
        public readonly static Single SINGLE_DB_INIT_VALUE = Single.MinValue + 1;

        #endregion

        #region Guid
        
        /// <summary>
        /// Valor por defecto para los elementos de tipo guid.
        ///         { 0x00000000, 0x0000, 0x0000, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } };
        /// </summary>
        public readonly static Guid GUID_DB_INIT_VALUE = Guid.Empty; 

        #endregion

        #region Int16 (Short)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 16 bits.
        /// </summary>
        public readonly static short SHORT_DB_INIT_VALUE = short.MinValue + 1;

        #endregion

        #region Int32

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 32 bits.
        /// </summary>
        public readonly static int INT32_DB_INIT_VALUE = int.MinValue + 1;

        #endregion

        #region Int64 (Long)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 64 bits.
        /// </summary>
        public readonly static long LONG_DB_INIT_VALUE = long.MinValue + 1;

        #endregion

        #region IntPtr

        /// <summary>
        /// Valor por defecto para los elementos de tipo puntero IntPtr.
        /// </summary>
        public readonly static IntPtr INTPTR_DB_INIT_VALUE = new IntPtr(1);

        #endregion

        #region Object

        /// <summary>
        /// Valor por defecto para los elementos de tipo object.
        /// </summary>
        public readonly static object OBJECT_DB_INIT_VALUE = new object();

        #endregion

        #region SByte

        /// <summary>
        /// Valor por defecto para los elementos de tipo sbyte.
        /// </summary>
        public readonly static sbyte SBYTE_DB_INIT_VALUE = sbyte.MinValue + 1;

        #endregion

        #region String

        /// <summary>
        /// Valor por defecto para los elementos de tipo cadena de carácteres.
        /// </summary>
        public readonly static string STRING_DB_INIT_VALUE = string.Empty;

        #endregion

        #region UInt16 (UShort)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 16 bits sin signo.
        /// </summary>
        public readonly static ushort USHORT_DB_INIT_VALUE = ushort.MinValue + 1;

        #endregion

        #region UInt32

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 32 bits sin signo.
        /// </summary>
        public readonly static uint UINT32_DB_INIT_VALUE = uint.MinValue + 1;

        #endregion

        #region UInt64 (ULong)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 64 bits sin signo.
        /// </summary>
        public readonly static ulong ULONG_DB_INIT_VALUE = ulong.MinValue + 1;

        #endregion

        #region UIntPtr

        /// <summary>
        /// Valor por defecto para los elementos de tipo puntero sin signo.
        /// </summary>
        public readonly static UIntPtr UINTPTR_DB_INIT_VALUE = new UIntPtr(1);

        #endregion

        #endregion

        #region Valores de Error para los tipos de la Base de Datos

        #region Boolean

        /// <summary>
        /// Valor por defecto para los elementos de tipo boolean.
        /// </summary>
        public readonly static bool BOOLEAN_DB_ERROR_VALUE = false;

        #endregion

        #region Byte

        /// <summary>
        /// Valor por defecto para los elementos de tipo byte.
        /// </summary>
        public readonly static byte BYTE_DB_ERROR_VALUE = byte.MinValue;

        #endregion

        #region Byte[]

        /// <summary>
        /// Valor por defecto para los elementos de tipo byte.
        /// </summary>
        public readonly static byte[] BYTEARRAY_DB_ERROR_VALUE = new byte[0];

        #endregion

        #region Char

        /// <summary>
        /// Valor por defecto para los elementos de tipo char.
        /// </summary>
        public readonly static char CHAR_DB_ERROR_VALUE = char.MinValue;

        #endregion

        #region DateTime

        /// <summary>
        /// Valor por defecto para los elementos de tipo DateTime.
        /// </summary>
        public readonly static DateTime DATETIME_DB_ERROR_VALUE = DateTime.MinValue;

        #endregion

        #region Decimal

        /// <summary>
        /// Valor por defecto para los elementos de tipo decimal.
        /// </summary>
        public readonly static decimal DECIMAL_DB_ERROR_VALUE = decimal.MinValue;

        #endregion

        #region Double

        /// <summary>
        /// Valor por defecto para los elementos de tipo double.
        /// </summary>
        public readonly static double DOUBLE_DB_ERROR_VALUE = double.MinValue;

        #endregion

        #region Single (Float)

        /// <summary>
        /// Valor por defecto para los elementos de tipo Single (float).
        /// </summary>
        public readonly static Single SINGLE_DB_ERROR_VALUE = Single.MinValue;

        #endregion

        #region Guid

        /// <summary>
        /// Valor por defecto para los elementos de tipo Guid.
        ///         
        /// </summary>
        public readonly static Guid GUID_DB_ERROR_VALUE = new Guid("{ 0x00000000, 0x0000, 0x0000, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 } }");

        #endregion

        #region Int16 (Short)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 16 bits.
        /// </summary>
        public readonly static short SHORT_DB_ERROR_VALUE = short.MinValue;

        #endregion

        #region Int32

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 32 bits.
        /// </summary>
        public readonly static int INT32_DB_ERROR_VALUE = int.MinValue;

        #endregion

        #region Int64 (Long)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 64 bits.
        /// </summary>
        public readonly static long LONG_DB_ERROR_VALUE = long.MinValue;

        #endregion

        #region IntPtr

        /// <summary>
        /// Valor por defecto para los elementos de tipo puntero.
        /// </summary>
        public readonly static IntPtr INTPTR_DB_ERROR_VALUE = IntPtr.Zero;

        #endregion

        #region Object

        /// <summary>
        /// Valor por defecto para los elementos de tipo object.
        /// </summary>
        public readonly static object OBJECT_DB_ERROR_VALUE = null;

        #endregion

        #region SByte

        /// <summary>
        /// Valor por defecto para los elementos de tipo byte.
        /// </summary>
        public readonly static sbyte SBYTE_DB_ERROR_VALUE = sbyte.MinValue;

        #endregion

        #region String

        /// <summary>
        /// Valor por defecto para los elementos de tipo cadena de carácteres.
        /// </summary>
        public readonly static string STRING_DB_ERROR_VALUE = null;

        #endregion

        #region UInt16 (UShort)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 16 bits sin signo.
        /// </summary>
        public readonly static ushort USHORT_DB_ERROR_VALUE = ushort.MinValue;

        #endregion

        #region UInt32

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 32 bits sin signo.
        /// </summary>
        public readonly static uint UINT32_DB_ERROR_VALUE = uint.MinValue;

        #endregion

        #region UInt64 (ULong)

        /// <summary>
        /// Valor por defecto para los elementos de tipo entero de 64 bits sin signo.
        /// </summary>
        public readonly static ulong ULONG_DB_ERROR_VALUE = ulong.MinValue;

        #endregion

        #region UintPtr

        /// <summary>
        /// Valor por defecto para los elementos de tipo puntero sin signo.
        /// </summary>
        public readonly static UIntPtr UINTPTR_DB_ERROR_VALUE = UIntPtr.Zero;

        #endregion

        #endregion

        #region Prefijos de los diferentes Tipos campos de Base de Datos

        public const string sPrefixBoolean = "b";         ///                 Tipo :- BOOLEAN

        public const string sPrefixByte = "bt";           ///                 Tipo :- BYTE
        
        public const string sPrefixByteArray = "btArray"; ///                 Tipo :- BYTE

        public const string sPrefixChar = "c";            ///                 Tipo :- CHAR

        public const string sPrefixDateTime = "dtm";      ///                 Tipo :- DATETIME

        public const string sPrefixDecimal = "dcm";       ///                 Tipo :- DECIMAL

        public const string sPrefixDouble = "d";          ///                 Tipo :- DOUBLE

        public const string sPrefixGuid = "guid";         ///                 Tipo :- GUID

        public const string sPrefixSingle = "f";          ///                 Tipo :- SINGLE (FLOAT)

        public const string sPrefixInt32 = "i";           ///                 Tipo :- INT32

        public const string sPrefixIntPtr = "iptr";       ///                 Tipo :- INTPTR

        public const string sPrefixInt64 = "l";           ///                 Tipo :- LONG

        public const string sPrefixObject = "o";          ///                 Tipo :- OBJECT

        public const string sPrefixSByte = "sbt";         ///                 Tipo :- SBYTE

        public const string sPrefixInt16 = "sh";          ///                 Tipo :- SHORT

        public const string sPrefixString = "s";          ///                 Tipo :- STRING

        public const string sPrefixUInt32 = "ui";         ///                 Tipo :- UINT

        public const string sPrefixUInt64 = "ul";         ///                 Tipo :- ULONG

        public const string sPrefixUInt16 = "ush";        ///                 Tipo :- USHORT

        public const string sPrefixUIntPtr = "uiptr";     ///                 Tipo :- UINTPTR

        #endregion

        #endregion

        #region Atributos

        /// <summary>
        /// Carácteres que serán cambiados al pasar a .Net porqué no se pueden usar como nombre de atributo.
        /// </summary>
        private const string sPatronCaracteres = "[- ]+";

        #endregion

        #region Propiedades

        /// <summary>
        /// Carácteres que serán cambiados al pasar a .Net porqué no se pueden usar como nombre de atributo.
        /// </summary>
        public static string PatronCaracteres
        {
            get
            {
                return (sPatronCaracteres);
            }
        }

        #endregion

        #region Métodos

        #region Lectores de Valores de la Base de Datos

        #region Boolean

        /// <summary>
        /// Método que se encarga de obtener el valor boolean .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="bValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetBooleanValue(object oValueIn, out bool bValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) bValueOut = BOOLEAN_DB_INIT_VALUE;
                else bValueOut = bool.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                bValueOut = BOOLEAN_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Byte

        /// <summary>
        /// Método que se encarga de obtener el valor byte .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="btValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetByteValue(object oValueIn, out byte btValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) btValueOut = BYTE_DB_INIT_VALUE;
                else btValueOut = byte.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                btValueOut = BYTE_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Byte[]

        /// <summary>
        /// Método que se encarga de obtener el valor byte .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="btValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetByteArrayValue(object oValueIn, out byte[] btValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) btValueOut = BYTEARRAY_DB_INIT_VALUE;
                else btValueOut = (byte[])oValueIn;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = string.Format("Error in method 'GetByteArrayValue'.\r\nDetails: {0}.", MsgManager.ExcepMsg(ex));
                btValueOut = BYTEARRAY_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Char

        /// <summary>
        /// Método que se encarga de obtener el valor char .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="cValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetCharValue(object oValueIn, out char cValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) cValueOut = CHAR_DB_INIT_VALUE;
                else cValueOut = char.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                cValueOut = CHAR_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region DateTime

        /// <summary>
        /// Método que se encarga de obtener el valor DateTime .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="dtmValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetDateTimeValue(object oValueIn, out DateTime dtmValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value || ! DateTime.TryParse(Convert.ToString(oValueIn),out dtmValueOut)) dtmValueOut = Convert.ToDateTime(DATETIME_DB_INIT_VALUE);
                else dtmValueOut = DateTime.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                dtmValueOut = DATETIME_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Decimal

        /// <summary>
        /// Método que se encarga de obtener el valor decimal .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="dcmValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetDecimalValue(object oValueIn, out decimal dcmValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) dcmValueOut = DECIMAL_DB_INIT_VALUE;
                else dcmValueOut = byte.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                dcmValueOut = DECIMAL_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Double

        /// <summary>
        /// Método que se encarga de obtener el valor double .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="dValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetDoubleValue(object oValueIn, out double dValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) dValueOut = DOUBLE_DB_INIT_VALUE;
                else dValueOut = double.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                dValueOut = DOUBLE_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Single (Float)

        /// <summary>
        /// Método que se encarga de obtener el valor Single (float) .Net asociado al valor leído de Base de 
        /// Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="fValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetSingleValue(object oValueIn, out Single fValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) fValueOut = SINGLE_DB_INIT_VALUE;
                else fValueOut = Single.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                fValueOut = SINGLE_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Guid

        /// <summary>
        /// Método que se encarga de obtener el valor Guid .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="guidValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetGuidValue(object oValueIn, out Guid guidValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) guidValueOut = GUID_DB_INIT_VALUE;
                else guidValueOut = (Guid) oValueIn;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = string.Format("Error in method 'GetGuidValue'.\r\nDetails: {0}.", MsgManager.ExcepMsg(ex));
                guidValueOut = GUID_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Int32

        /// <summary>
        /// Método que se encarga de obtener el valor entero de 32 bits .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="iValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetInt32Value(object oValueIn, out int iValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) iValueOut = INT32_DB_INIT_VALUE;
                else iValueOut = int.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                iValueOut = INT32_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion      

        #region IntPtr

        /// <summary>
        /// Método que se encarga de obtener el valor del puntero .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="iptrValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetIntPtrValue(object oValueIn, out IntPtr iptrValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn != DBNull.Value)
                {
                    Type oType = oValueIn.GetType();
                    if (oType == typeof(int)) iptrValueOut = new IntPtr((int) oValueIn);
                    else if (oType == typeof(long)) iptrValueOut = new IntPtr((long)oValueIn);
                    else iptrValueOut = INTPTR_DB_ERROR_VALUE;
                }
                else iptrValueOut = INTPTR_DB_INIT_VALUE;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                iptrValueOut = INTPTR_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Long (Int64)

        /// <summary>
        /// Método que se encarga de obtener el valor entero de 32 bits .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="iValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetLongValue(object oValueIn, out long lValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) lValueOut = LONG_DB_INIT_VALUE;
                else lValueOut = long.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                lValueOut = LONG_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Object

        /// <summary>
        /// Método que se encarga de obtener el valor object .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="oValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetObjectValue(object oValueIn, out object oValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) oValueOut = OBJECT_DB_INIT_VALUE;
                else oValueOut = oValueIn;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                oValueOut = OBJECT_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region SByte

        /// <summary>
        /// Método que se encarga de obtener el valor sbyte .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="sbtValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetSbyteValue(object oValueIn, out sbyte sbtValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) sbtValueOut = SBYTE_DB_INIT_VALUE;
                else sbtValueOut = sbyte.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sbtValueOut = SBYTE_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Short (Int16)

        /// <summary>
        /// Método que se encarga de obtener el valor entero de 16 bits .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="shValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetShortValue(object oValueIn, out short shValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) shValueOut = SHORT_DB_INIT_VALUE;
                else shValueOut = short.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                shValueOut = SHORT_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region String

        /// <summary>
        /// Método que se encarga de obtener la cadena de carácteres .Net asociada al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="sValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetStringValue(object oValueIn, out string sValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) sValueOut = STRING_DB_INIT_VALUE;
                else sValueOut = oValueIn.ToString().Trim();
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = string.Format("Error in method 'GetStringValue'.\r\nDetails: {0}.", MsgManager.ExcepMsg(ex));
                sValueOut = STRING_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Uint32 (Int32 sin signo)

        /// <summary>
        /// Método que se encarga de obtener el valor entero de 32 bits sin signo .Net asociado al valor leído de Base 
        /// de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="uiValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetUint32Value(object oValueIn, out uint uiValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) uiValueOut = UINT32_DB_INIT_VALUE;
                else uiValueOut = uint.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                uiValueOut = UINT32_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region UIntPtr

        /// <summary>
        /// Método que se encarga de obtener el valor del puntero .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="uiptrValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetUIntPtrValue(object oValueIn, out UIntPtr uiptrValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn != DBNull.Value)
                {
                    Type oType = oValueIn.GetType();
                    if (oType == typeof(int)) uiptrValueOut = new UIntPtr((uint)oValueIn);
                    else if (oType == typeof(long)) uiptrValueOut = new UIntPtr((ulong)oValueIn);
                    else uiptrValueOut = UINTPTR_DB_ERROR_VALUE;
                }
                else uiptrValueOut = UINTPTR_DB_INIT_VALUE;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                uiptrValueOut = UINTPTR_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Ulong (Int64 sin signo)

        /// <summary>
        /// Método que se encarga de obtener el valor entero de 32 bits sin signo .Net asociado al valor leído de Base 
        /// de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="uiValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetUlongValue(object oValueIn, out ulong ulValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) ulValueOut = ULONG_DB_INIT_VALUE;
                else ulValueOut = ulong.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                ulValueOut = ULONG_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #region Ushort (Int16 sin signo)

        /// <summary>
        /// Método que se encarga de obtener el valor entero de 16 bits si signo .Net asociado al valor leído de Base 
        /// de Datos.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="ushValueOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetUshortValue(object oValueIn, out ushort ushValueOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == DBNull.Value) ushValueOut = USHORT_DB_INIT_VALUE;
                else ushValueOut = ushort.Parse(oValueIn.ToString());
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                ushValueOut = USHORT_DB_ERROR_VALUE;
                return (false);
            }
        }

        #endregion

        #endregion

        #region Conversores de Valores de aplicación a valores de la Base de Datos

        #region Boolean

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetBooleanBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                sValueBBDDOut = "'" + oValueIn.ToString() + "'";
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Byte

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetByteBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((byte)oValueIn == BYTE_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString();
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Byte[]

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetByteArrayBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {            
            try
            {
                if ((byte[])oValueIn == BYTEARRAY_DB_INIT_VALUE || oValueIn==null) sValueBBDDOut = "Null";
                else sValueBBDDOut = Convert.ToBase64String((byte[])oValueIn);
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion
        
        #region Char

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetCharBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((char)oValueIn == CHAR_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = "'" + oValueIn.ToString() + "'";
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion
        
        #region DateTime

        /// <summary>
        /// Método que se encarga de obtener el valor DateTime .Net asociado al valor leído de Base de Datos.
        /// </summary>
        /// <param name="eDBType">Tipo de Base de Datos en el que se desea almacenar el valor.</param>
        /// <param name="oValueIn">Valor de entrada leído de la Base de Datos.</param>
        /// <param name="sValueBBDDOut">Valor .Net equivalente al leído de Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetDateTimeBBDDValue(DBType eDBType, object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            sValueBBDDOut = string.Empty;
            try
            {
                if ((DateTime)oValueIn != DATETIME_DB_INIT_VALUE)
                {
                    if (!BDUtils.GetDate(eDBType, (DateTime)oValueIn, BDUtils.FormatDateTime.DateAndTimeWithMiliseconds,
                                         out sValueBBDDOut, out sMensaje))  return (false);
                    sValueBBDDOut = "'" + sValueBBDDOut + "'";
                }
                else sValueBBDDOut = "Null";
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Decimal

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetDecimalBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((decimal)oValueIn == DECIMAL_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); 
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Double

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetDoubleBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((double)oValueIn == DOUBLE_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); 
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Single (Float)

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetSingleBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((Single)oValueIn == SINGLE_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); 
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Guid

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetGuidBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((Guid)oValueIn == GUID_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = "'" + oValueIn.ToString() + "'" ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Short (Int16)

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetShortBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((Int16)oValueIn == SHORT_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Int32

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetInt32BBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((Int32)oValueIn == INT32_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion
        
        #region Int64 (Long)

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetLongBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((Int64)oValueIn == LONG_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region IntPtr

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetIntPtrBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((IntPtr)oValueIn == INTPTR_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Object

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetObjectBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if (oValueIn == OBJECT_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region SByte

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetSByteBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((SByte)oValueIn == SBYTE_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region String

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetStringBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((string)oValueIn == STRING_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = "'" + oValueIn.ToString() + "'" ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region UInt16 (UShort)

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetUShortBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((UInt16)oValueIn == USHORT_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region Uint32 (Int32 sin signo)

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetUInt32BBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((UInt32) oValueIn == UINT32_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region UInt64 (ULong)

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetULongBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((UInt64)oValueIn == ULONG_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #region UIntPtr

        /// <summary>
        /// Método que se encarga de obtener el valor a escribir en la base de datos asociado al valor .Net pasado
        /// cómo parámetro.
        /// </summary>
        /// <param name="oValueIn">Valor de entrada leído de la clase en .Net.</param>
        /// <param name="sValueBBDDOut">Valor a escribir en la Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de Error, si se produce uno.</param>
        /// <returns>true, valor resultante correcto, false, error al obtener el valor.</returns>
        public static bool GetUIntPtrBBDDValue(object oValueIn, out string sValueBBDDOut, out string sMensaje)
        {
            try
            {
                if ((UIntPtr)oValueIn == UINTPTR_DB_INIT_VALUE) sValueBBDDOut = "Null";
                else sValueBBDDOut = oValueIn.ToString(); ;
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                sValueBBDDOut = string.Empty;
                return (false);
            }
        }

        #endregion

        #endregion

        #region Nombre de Tabla BBDD -> C#

        /// <summary>
        /// Método que obtiene el nombre que se usará para la tabla indicada dentro del código C# generado. 
        /// </summary>
        /// <param name="sTableNameIn">Nombre de la tabla en la Base de Datos.</param>
        /// <param name="sTableNameOut">Nombre de la Tabla en el código C#.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si operación correcta, false, si error</returns>
        public static bool TableNameCSharp(string sTableNameIn, out string sTableNameOut, out string sMensaje)
        {
            sTableNameOut = string.Empty;
            try
            {
                if ((sTableNameIn == null))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_001");
                    return (false);
                }
                if (Regex.IsMatch(sTableNameIn, sPatronCaracteres))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_002", sTableNameIn);
                    return (false);
                }
                if (sTableNameIn.Length == 0) sTableNameOut = sTableNameIn;
                else if (sTableNameIn.Length == 1) sTableNameOut = sTableNameIn.ToUpper();
                else sTableNameOut = sTableNameIn.Substring(0, 1).ToUpper() + 
                                     sTableNameIn.Substring(1, sTableNameIn.Length - 1).ToLower();
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Nombre de Campo BBDD -> C#

        /// <summary>
        /// Método que obtiene el nombre que se usará para el campo indicado dentro del código C# generado. 
        /// </summary>
        /// <param name="sFieldNameIn">Nombre del campo en la Base de Datos.</param>
        /// <param name="sFieldNameOut">Nombre del campo en el código C#.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si operación correcta, false, si error</returns>
        public static bool FieldNameCSharp(string sFieldNameIn, out string sFieldNameOut, out string sMensaje)
        {
            sFieldNameOut = string.Empty;
            try
            {
                if ((sFieldNameIn == null))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_003");
                    return (false);
                }
                if (Regex.IsMatch(sFieldNameIn, sPatronCaracteres))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_004", sFieldNameIn);
                    return (false);
                }
                if (sFieldNameIn.Length == 0) sFieldNameOut = sFieldNameIn;
                else if (sFieldNameIn.Length == 1) sFieldNameOut = sFieldNameIn.ToUpper();
                else sFieldNameOut = sFieldNameIn.Substring(0, 1).ToUpper() +
                                     sFieldNameIn.Substring(1, sFieldNameIn.Length - 1);
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Tipo de Campo BBDD -> C#

        /// <summary>
        /// Convierte el tipo del campo que se ha leido de la base de Datos en el tipo correspondiente dentro
        /// del lenguaje de programación C# y retorna este valor.
        /// </summary>
        /// <param name="tTypeBBDD">Tipo reconocido en la lectura de la Base de Datos</param>
        /// <param name="sTypeCSharp">Tipo C# equivalente al tipo de Base de Datos leído.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        public static bool DataTypeCSharp(Type tTypeBBDD, out string sTypeCSharp, out string sMensaje)
        {
            sTypeCSharp = string.Empty;
            try
            {
                switch (tTypeBBDD.Name.ToLower())
                {
                    case "boolean":
                         sTypeCSharp = "bool";
                         break;
                    case "byte[]":
                         sTypeCSharp = "byte[]";
                         break;
                    case "byte":
                         sTypeCSharp = "byte";
                         break;
                    case "char":
                         sTypeCSharp = "char";
                         break;
                    case "datetime":
                         sTypeCSharp = "DateTime";
                         break;
                    case "decimal":
                         sTypeCSharp = "decimal";
                         break;
                    case "double":
                         sTypeCSharp = "double";
                         break;
                    case "single":
                         sTypeCSharp = "Single";
                         break;
                    case "guid":
                         sTypeCSharp = "Guid";
                         break;
                    case "int32":
                         sTypeCSharp = "int";
                         break;
                    case "intptr":
                         sTypeCSharp = "IntPtr";
                         break;
                    case "int64":
                         sTypeCSharp = "long";
                         break;
                    case "object":
                         sTypeCSharp = "object";
                         break;
                    case "sbyte":
                         sTypeCSharp = "sbyte";
                         break;
                    case "int16":
                         sTypeCSharp = "short";
                         break;
                    case "string":
                         sTypeCSharp = "string";
                         break;
                    case "uint32":
                         sTypeCSharp = "uint";
                         break;
                    case "uintptr":
                         sTypeCSharp = "UIntPtr";
                         break;
                    case "uint64":
                         sTypeCSharp = "ulong";
                         break;
                    case "uint16":
                         sTypeCSharp = "ushort";
                         break;
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_000", tTypeBBDD.ToString());
                         return (false);
                }
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Tipo de Campo Stored Procedure in BBDD -> C#

        /// <summary>
        /// Convierte el tipo del campo que se ha leido de la base de Datos en el tipo correspondiente dentro
        /// del lenguaje de programación C# y retorna este valor.
        /// </summary>
        /// <param name="eDBType">Tipo de Base de Datos sobre el que se quiere actuar</param>
        /// <param name="tTypeBBDD">Tipo reconocido en la lectura de la Base de Datos</param>
        /// <param name="sTypeCSharp">Tipo C# equivalente al tipo de Base de Datos leído.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        public static bool DataTypeStoredProcedureCSharp(DBType eDBType, string sTypeStoredProcedureBBDD, out string sTypeCSharp, out string sMensaje)
        {
            sTypeCSharp = string.Empty;
            try
            {
                switch (eDBType)
                {
                    case DBType.SQLSERVER:
                         return (DataTypeStoredProcedureCSharpSQLServer(sTypeStoredProcedureBBDD, out sTypeCSharp, out sMensaje));
                    case DBType.ORACLE:
                         sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_005");
                         return (false);
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_006");
                         return (false);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #region SQL Server

        /// <summary>
        /// Convierte el tipo del campo que se ha leido de la base de Datos en el tipo correspondiente dentro
        /// del lenguaje de programación C# y retorna este valor.
        /// </summary>
        /// <param name="tTypeStoredProcedureBBDD">Tipo reconocido en la lectura de la Base de Datos</param>
        /// <param name="sTypeCSharp">Tipo C# equivalente al tipo de Base de Datos leído.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool DataTypeStoredProcedureCSharpSQLServer(string sTypeStoredProcedureBBDD,
                                                                   out string sTypeCSharp, out string sMensaje)
        {
            sTypeCSharp = string.Empty;
            try
            {
                switch (sTypeStoredProcedureBBDD.ToLower())
                {
                    case "text":
                    case "ntext":
                    case "varchar":
                    case "nvarchar":
                         sTypeCSharp = "System.String";
                         break;
                    case "uniqueidentifier":
                         sTypeCSharp = "System.Guid";
                         break;
                    case "date":
                    case "time":
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                         sTypeCSharp = "System.DateTime";
                         break;
                    case "tinyint":
                         sTypeCSharp = "System.Byte";
                         break;
                    case "smallint":
                         sTypeCSharp = "System.Int16";
                         break;
                    case "int":
                         sTypeCSharp = "System.Int32";
                         break;
                    case "real":
                         sTypeCSharp = "System.Double";
                         break;
                    case "float":
                         sTypeCSharp = "System.Single";
                         break;
                    case "bit":
                         sTypeCSharp = "System.Boolean";
                         break;
                    case "decimal":
                    case "numeric":
                         sTypeCSharp = "System.Decimal";
                         break;
                    case "bigint":
                         sTypeCSharp = "System.Int64";
                         break;
                    case "timestamp":
                         sTypeCSharp = "System.TimeStamp";
                         break;
                    case "char":
                    case "nchar":
                         sTypeCSharp = "System.Char";
                         break;                        
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_000", sTypeStoredProcedureBBDD);
                         return (false);
                }
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion

        #region Prefijo asociado a un tipo de variable de C#

        /// <summary>
        /// Obtiene el tipo de prefijo asociado a un tipo de variable de C#.
        /// </summary>
        /// <param name="tTypeBBDD">Tipo reconocido en la lectura de la Base de Datos</param>
        /// <param name="sPrefixCSharp">Prefijo asociado al tipo.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        public static bool PrefixDataTypeCSharp(Type tTypeBBDD, out string sPrefixType, out string sMensaje)
        {
            sPrefixType = string.Empty;
            try
            {
                switch (tTypeBBDD.Name.ToLower())
                {
                    case "boolean":
                         sPrefixType = BDUtils.sPrefixBoolean;
                         break;
                    case "byte":
                         sPrefixType = BDUtils.sPrefixByte;
                         break;
                    case "byte[]":
                         sPrefixType = BDUtils.sPrefixByteArray;
                         break;
                    case "char":
                         sPrefixType = BDUtils.sPrefixChar;
                         break;
                    case "datetime":
                         sPrefixType = BDUtils.sPrefixDateTime;
                         break;
                    case "decimal":
                         sPrefixType = BDUtils.sPrefixDecimal;
                         break;
                    case "double":
                         sPrefixType = BDUtils.sPrefixDouble;
                         break;
                    case "single":
                         sPrefixType = BDUtils.sPrefixSingle;
                         break;
                    case "guid":
                         sPrefixType = BDUtils.sPrefixGuid;
                         break;
                    case "int32":
                         sPrefixType = BDUtils.sPrefixInt32;
                         break;
                    case "intptr":
                         sPrefixType = BDUtils.sPrefixIntPtr;
                         break;
                    case "int64":
                         sPrefixType = BDUtils.sPrefixInt64;
                         break;
                    case "object":
                         sPrefixType = BDUtils.sPrefixObject;
                         break;
                    case "sbyte":
                         sPrefixType = BDUtils.sPrefixSByte;
                         break;
                    case "int16":
                         sPrefixType = BDUtils.sPrefixInt16;
                         break;
                    case "string":
                         sPrefixType = BDUtils.sPrefixString;
                         break;
                    case "uint32":
                         sPrefixType = BDUtils.sPrefixUInt32;
                         break;
                    case "uintptr":
                         sPrefixType = BDUtils.sPrefixUIntPtr;
                         break;
                    case "uint64":
                         sPrefixType = BDUtils.sPrefixUInt64;
                         break;
                    case "uint16":
                         sPrefixType = BDUtils.sPrefixUInt16;
                         break;
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_000", tTypeBBDD.ToString());
                         return (false);
                }
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Obtiene el método de análisis de un tipo de variable de Base de Datos -> C#

        /// <summary>
        /// Obtiene el nombre del método que obtiene el tipo de C# asociado a un tipo de Base de Datos.
        /// </summary>
        /// <param name="tTypeBBDD">Tipo reconocido en la lectura de la Base de Datos</param>
        /// <param name="sMethodAnalyze">Prefijo asociado al tipo.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        public static bool MethodToAnalyzeTypeBBDD(Type tTypeBBDD, out string sMethodAnalyze, out string sMensaje)
        {
            sMethodAnalyze = string.Empty;
            try
            {
                switch (tTypeBBDD.Name.ToLower())
                {
                    case "boolean":
                         sMethodAnalyze = "BDUtils.GetBooleanValue";
                         break;
                    case "byte":
                         sMethodAnalyze = "BDUtils.GetByteValue";
                         break;
                    case "byte[]":
                         sMethodAnalyze = "BDUtils.GetByteArrayValue";
                         break;
                    case "char":
                         sMethodAnalyze = "BDUtils.GetCharValue";
                         break;
                    case "datetime":
                         sMethodAnalyze = "BDUtils.GetDateTimeValue";
                         break;
                    case "decimal":
                         sMethodAnalyze = "BDUtils.GetDecimalValue";
                         break;
                    case "double":
                         sMethodAnalyze = "BDUtils.GetDoubleValue";
                         break;
                    case "single":
                         sMethodAnalyze = "BDUtils.GetSingleValue";
                         break;
                    case "guid":
                         sMethodAnalyze = "BDUtils.GetGuidValue";
                         break;
                    case "int32":
                         sMethodAnalyze = "BDUtils.GetInt32Value";
                         break;
                    case "intptr":
                         sMethodAnalyze = "BDUtils.GetIntPtrValue";
                         break;
                    case "int64":
                         sMethodAnalyze = "BDUtils.GetLongValue";
                         break;
                    case "object":
                         sMethodAnalyze = "BDUtils.GetObjectValue";
                         break;
                    case "sbyte":
                         sMethodAnalyze = "BDUtils.GetSbyteValue";
                         break;
                    case "int16":
                         sMethodAnalyze = "BDUtils.GetShortValue";
                         break;
                    case "string":
                         sMethodAnalyze = "BDUtils.GetStringValue";
                         break;
                    case "uint32":
                         sMethodAnalyze = "BDUtils.GetUint32Value";
                         break;
                    case "uintptr":
                         sMethodAnalyze = "BDUtils.GetUIntPtrValue";
                         break;
                    case "uint64":
                         sMethodAnalyze = "BDUtils.GetUlongValue";
                         break;
                    case "uint16":
                         sMethodAnalyze = "BDUtils.GetUshortValue";
                         break;
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_000", tTypeBBDD.ToString());
                         return (false);
                }
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Obtiene el método de análisis de un tipo de variable de C# -> Base de Datos

        /// <summary>
        /// Obtiene el nombre del método que obtiene el tipo de Base de Datos asociado a un tipo de C#.
        /// </summary>
        /// <param name="tTypeBBDD">Tipo reconocido en la lectura de la Base de Datos</param>
        /// <param name="sMethodAnalyze">Prefijo asociado al tipo.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        public static bool MethodToAnalyzeTypeCSharp(Type tTypeBBDD, out string sMethodAnalyze, out string sMensaje)
        {
            sMethodAnalyze = string.Empty;
            try
            {
                switch (tTypeBBDD.Name.ToLower())
                {
                    case "boolean":
                         sMethodAnalyze = "BDUtils.GetBooleanBBDDValue";
                         break;
                    case "byte":
                         sMethodAnalyze = "BDUtils.GetByteBBDDValue";
                         break;
                    case "byte[]":
                         sMethodAnalyze = "BDUtils.GetByteArrayBBDDValue";
                         break;
                    case "char":
                         sMethodAnalyze = "BDUtils.GetCharBBDDValue";
                         break;
                    case "datetime":
                         sMethodAnalyze = "BDUtils.GetDateTimeBBDDValue";
                         break;
                    case "decimal":
                         sMethodAnalyze = "BDUtils.GetDecimalBBDDValue";
                         break;
                    case "double":
                         sMethodAnalyze = "BDUtils.GetDoubleBBDDValue";
                         break;
                    case "single":
                         sMethodAnalyze = "BDUtils.GetSingleBBDDValue";
                         break;
                    case "guid":
                         sMethodAnalyze = "BDUtils.GetGuidBBDDValue";
                         break;
                    case "int32":
                         sMethodAnalyze = "BDUtils.GetInt32BBDDValue";
                         break;
                    case "intptr":
                         sMethodAnalyze = "BDUtils.GetIntPtrBBDDValue";
                         break;
                    case "int64":
                         sMethodAnalyze = "BDUtils.GetLongBBDDValue";
                         break;
                    case "object":
                         sMethodAnalyze = "BDUtils.GetObjectBBDDValue";
                         break;
                    case "sbyte":
                         sMethodAnalyze = "BDUtils.GetSbyteBBDDValue";
                         break;
                    case "int16":
                         sMethodAnalyze = "BDUtils.GetShortBBDDValue";
                         break;
                    case "string":
                         sMethodAnalyze = "BDUtils.GetStringBBDDValue";
                         break;
                    case "uint32":
                         sMethodAnalyze = "BDUtils.GetUint32BBDDValue";
                         break;
                    case "uintptr":
                         sMethodAnalyze = "BDUtils.GetUIntPtrBBDDValue";
                         break;
                    case "uint64":
                         sMethodAnalyze = "BDUtils.GetUlongBBDDValue";
                         break;
                    case "uint16":
                         sMethodAnalyze = "BDUtils.GetUshortBBDDValue";
                         break;
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_000", tTypeBBDD.ToString());
                         return (false);
                }
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Obtención del carácter delimitador del tipo en la Base de Datos

        /// <summary>
        /// Obtiene, si lo hay, el símbolo especial con el que se ha de.
        /// </summary>
        /// <param name="sTypeIn">Tipo reconocido en la lectura de la Base de Datos</param>
        /// <param name="sNameIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sMensaje"></param>
        /// <returns>Tipo asociado al leído en C#</returns>
        public static bool GetQuotaTypeCSharp(Type tTypeBBDD, out string sQuota, out string sMensaje)
        {
            sQuota = null;
            sMensaje = string.Empty;
            try
            {
                //  Obtiene el delimitador para el tipo indicado.
                    switch (tTypeBBDD.Name.ToLower())
                    {
                        case "byte":
                        case "byte[]":
                        case "sbyte":
                        case "int16":
                        case "int32":
                        case "int64":
                        case "uint16":
                        case "uint32":
                        case "uint64":
                        case "single":
                        case "double":
                        case "decimal":
                        case "intptr":
                        case "uintptr":
                        case "object":
                             sQuota = string.Empty;
                             break;
                        case "boolean":
                        case "char":
                        case "string":
                        case "datetime":
                        case "guid":
                             sQuota = "'";
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_000", tTypeBBDD.ToString());
                             return (false);
                    }
                //  Indica que la operación ha finalizado correctamente.
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region DateTime C# -> Date BBDD

        /// <summary>
        /// Método que obtiene una cadena de carácteres que representa la fecha C# de entrada en el formato indicado
        /// por el parámtro eFormato y en función del tipo de Base de Datos para el que se dese trabajar.
        /// </summary>
        /// <param name="eDBType">Tipo de Base de Datos.</param>
        /// <param name="dtmDate">Fecha a tratar.</param>
        /// <param name="eFormat">Formato a utilizar.</param>
        /// <param name="sDate">Cadena de carácteres que representa a la fecha en una consulta a Base de Datos.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, operación correcta, false, error.</returns>
        public static bool GetDate(DBType eDBType, DateTime dtmDate, FormatDateTime eFormat, out string sDate, out string sMensaje)
        {
            sDate = string.Empty;
            sMensaje = string.Empty;
            try
            {
                //  Obtiene la representación de la fecha en función del tipo de Base de Datos.
                    switch (eDBType)
                    {
                        case DBType.SQLSERVER:
                             switch (eFormat)
                             {
                                 case FormatDateTime.Date:
                                      sDate = dtmDate.Date.ToString("yyyyMMdd");
                                      return (true);
                                 case FormatDateTime.Time:
                                      sDate = dtmDate.ToString("HH:mm:ss");
                                      return (true);
                                 case FormatDateTime.DateAndTimeWithMiliseconds:
                                      sDate = dtmDate.ToString("yyyyMMdd HH:mm:ss:fff");
                                      return true;
                                 case FormatDateTime.DateAndTime:
                                 default:
                                      sDate = dtmDate.ToString("yyyyMMdd HH:mm:ss");
                                      return (true);
                             }                             
                        case DBType.ORACLE:
                             sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_005");
                             return (false);
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_BDUtils_006");
                             return (false);
                    }

            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion
 
    }
}
