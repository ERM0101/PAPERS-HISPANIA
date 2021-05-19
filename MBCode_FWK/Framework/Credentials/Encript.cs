#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace MBCode.Framework.Credentials
{
    #region Enumerados

    /// <summary>
    /// Posibles resultados a la hora de comparar el Hash del texto pasado como parámetro con el 
    /// Hash del Checksum de la información que se desea validar.
    /// </summary>
    public enum ComparaHash
    {
        /// <summary>
        /// Comparación correcta, el hash de la información coincide con el del Checksum.
        /// </summary>
        Correcto,

        /// <summary>
        /// Comparación incorrecta, el hash de la información  no  coincide  con  el  del 
        /// Checksum.
        /// </summary>
        Incorrecto,

        /// <summary>
        /// Error al llevar a cabo la comparación.
        /// </summary>
        ErrorCalculo,
    }

    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha ultima modificación: 25/04/2013
    /// Descripción: Contiene los métodos cifrado y descifrado con las que se controla el acceso de los usuarios a la aplicación.
    /// </summary>
    public static class Encript
    {
        #region Métodos

        /// <summary>
        /// Devuelve el código hash que representa un string
        /// </summary>
        /// <param name="sPasswordIn">String de entrada del que se quiere obtener el código hash</param>
        /// <param name="sMsgError">Mensaje de error si se produce.</param>
        /// <returns>Código hash en formato string hexadecimal, null en caso de error</returns>
        internal static string CodificarPassword(string sPasswordIn, out string sMsgError)
        {
            sMsgError = string.Empty;
            try
            {
                //  Crea una nueva instancia del objeto MD5CryptoServiceProvider.
                    MD5CryptoServiceProvider CodificadorMD5 = new MD5CryptoServiceProvider();
                //  Convertir el string de entrada en un array de bytes y calcular el hash.
                    byte[] data = CodificadorMD5.ComputeHash(Encoding.Default.GetBytes(sPasswordIn));
                //  Crear un nuevo stringbuilder que contenga el array de bytes y crear el string.
                    StringBuilder sbPasswordEncoded = new StringBuilder();
                //  Iterar el array de bytes y convertirlos a strings en formato hexadecimal.
                    for (int i = 0; i < data.Length; i++)
                    {
                        sbPasswordEncoded.Append(data[i].ToString("x2"));
                    }
                // Devolver el string.
                    return (sbPasswordEncoded.ToString());
            }
            catch (Exception ex)
            {
                sMsgError += String.Format("Ejecución incorrecta del método: CalcularMd5Hash.{0}Detalles:{1}{0}" + 
                                          Environment.NewLine, MsgManager.ExcepMsg(ex));
                return (null);
            }
        }

        /// <summary>
        /// Valida un password en función a la contraseña pasada cómo parámetro.
        /// </summary>
        /// <param name="sPasswordIn">Texto a comparar con un código hash</param>
        /// <param name="sPasswordEncoded">Codigo hash a comparar con un texto</param>
        /// <param name="sMsgError">Mensaje de error si se produce.</param>
        /// <returns>Enumerado con el tipo de resultado</returns>
        public static ComparaHash ValidarPassword(string sPasswordIn, string sPasswordEncoded, out string sMsgError)
        {
            sMsgError = string.Empty;
            try
            {
                //  Codificar la entrada.
                    string hashOfInput;

                //  Codifica el elemento pasado como parámetro para realizar la comparanción.
                    if ((hashOfInput = CodificarPassword(sPasswordIn, out sMsgError)) != null)                        
                    {
                        //  Crear un StringComparer y comparar los dos códigos hash..
                            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                            if (0 == comparer.Compare(hashOfInput, sPasswordEncoded)) return (ComparaHash.Correcto);
                            else return (ComparaHash.Incorrecto);
                    }
                    else return (ComparaHash.ErrorCalculo);
            }
            catch (Exception ex)
            {
                sMsgError += String.Format("Ejecución incorrecta del método: ValidarPassword.{0}Detalles:{1}{0}" +
                                           Environment.NewLine, MsgManager.ExcepMsg(ex));
                return (ComparaHash.ErrorCalculo);
            }
        }

        #endregion
    }
}
