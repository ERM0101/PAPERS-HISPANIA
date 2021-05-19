#region Librerias usadas por la clase

using System;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using MBCode.Framework.Managers.Messages;

#endregion

namespace MBCode.Framework.Credentials
{
    public class Credential
    {
        #region Constantes

        /// <summary>
        /// Define el número mínimo de carácteres de una contraseña para que se considere válida.
        /// </summary>
        private const int NUM_MIN_PASSWORD_CHARACTERS = 8;

        /// <summary>
        /// Indica el número mínimo de mayúsculas que debe contener la contraseña para ser valida.
        /// </summary>
        private const int MIN_NUM_UPPER = 1;

        /// <summary>
        /// Indica el número mínimo de minúsculas que debe contener la contraseña para ser valida.
        /// </summary>
        private const int MIN_NUM_LOWER = 1;

        /// <summary>
        /// Indica el número mínimo de números que debe contener la contraseña para ser valida.
        /// </summary>
        private const int MIN_NUM_NUMBER = 1;

        /// <summary>
        /// Indica el número mínimo de carácteres especiales que debe contener la contraseña para ser valida.
        /// </summary>
        private const int MIN_NUM_SPECIAL_CHARS = 1;

        /// <summary>
        /// Almacena el Indentificador de credencial que se asociará a un SuperUsuario (Indra).
        /// </summary>
        private static Guid INDRA_SUPER_USER_ID = new Guid("C1E1D75F-D21D-4B3B-B367-639EE8677812");

        #endregion

        #region Atributos

        /// <summary>
        /// Expresión regular que permite reconocer si un texto que contiene mayúsculas.
        /// </summary>
        private static Regex MatchUpper = new Regex("[A-Z]");

        /// <summary>
        /// Expresión regular que permite reconocer si un texto que contiene minúsculas.
        /// </summary>
        private static Regex MatchLower = new Regex("[a-z]");

        /// <summary>
        /// Expresión regular que permite reconocer si un texto que contiene números.
        /// </summary>
        private static Regex MatchNumber = new Regex("[0-9]");

        /// <summary>
        /// Expresión regular que permite reconocer si un texto que contiene carácteres especiales.
        /// </summary>
        private static Regex MatchSpecialCharacter = new Regex("[^a-zA-Z0-9]");

        /// <summary>
        /// Almacena el identificador del Grupo de Administradores.
        /// </summary>
        public static Guid ADMINISTRATOR_GROUP_ID = new Guid("F98E403D-D324-4D16-8EF3-C70726F645A3");

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene / Establece el Identificador de Base de Datos de la Credencial de Usuario.
        /// </summary>
        public Guid Credential_Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtiene / Establece el Identificador de Base de Datos de la Credencial de Usuario.
        /// </summary>
        public Guid Group_Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtiene / Establece el Nombre de usuario de la Credencial.
        /// </summary>
        public string UserName
        {
            get;
            private set;
        }

        /// <summary>
        ///  Obtiene / Establece la Contraseña del usuario al que pertenece la Credencial.
        /// </summary>
        public string Password
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        /// <param name="Credential_Id">Identificador de la Credencial que se creará.</param>
        /// <param name="Group_Id">Grupo de Credenciales al que pertenece la Credencial.</param>
        /// <param name="UserName">Nombre de Usuario del usuario para el que se define la credencial.</param>
        /// <param name="Password">Contraseña del usuario para el que se define la credencial.</param>
        public Credential(Guid Credential_Id, Guid Group_Id, string UserName, string Password)
        { 
            string sMsgError;
            this.Credential_Id = Credential_Id;
            this.Group_Id = Group_Id;
            if (!SetUserName(UserName, out sMsgError)) throw new ArgumentException(sMsgError);
            if (!SetPassword(Password, out sMsgError, false)) throw new ArgumentException(sMsgError);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que determina si los parámetros corresponden a un SuperUsuario y si es así lo crea.
        /// </summary>
        /// <param name="sUser">UsuerName.</param>
        /// <param name="sPassword">Password.</param>
        /// <param name="SUCredential">Credencial de SuperUsuario si todo correcto, null, si error.</param>
        /// <returns>true, si es SuperUsuario, y las credenciales se han creado de manera correcta, false, sinó.</returns>
        public static bool IsSuperUser(string sUser, string sPassword, out Credential SUCredential)
        {
            SUCredential = null;
            try
            {
                if ((sUser == "SuperUser") && (sPassword == "#Indra&2013#"))
                {
                    SUCredential = new Credential(INDRA_SUPER_USER_ID, Guid.Empty, sUser, sPassword);
                    return (true);
                }
                else return (false);
            }
            catch 
            { 
                return (false); 
            }
        }

        /// <summary>
        /// Método que determina si la Credencial que invoca a este método corresponde a un SuperUsuario.
        /// </summary>
        /// <returns>true, si es SuperUsuario, false, sinó.</returns>
        public bool IsSuperUser()
        {
            try
            {
                return ((Credential_Id == INDRA_SUPER_USER_ID) && (UserName.ToUpper() == "SUPERUSER"));
            }
            catch
            {
                return (false);
            }
        }

        /// <summary>
        /// Método que actualiza el nombre de usuario almacenado en la Credencial.
        /// </summary>
        /// <param name="NewUserName">Nuevo nombre de usuario.</param>
        /// <param name="sMsgError">Mensaje de error, si se produce uno durante el proceso de actualización.</param>
        /// <returns>true, si actualización correcta, false, sinó.</returns>
        public bool SetUserName(string NewUserName, out string sMsgError)
        {
            try
            {
                //  Valida que el nombre de usuario no sea nulo ni vacío.
                    if ((String.IsNullOrEmpty(NewUserName)) || (String.IsNullOrWhiteSpace(NewUserName)))
                    {
                        sMsgError = "Error, el nombre de usuario no puede ser una cadena vacía.";
                        return (false);
                    }
                //  Actualiza el nombre de usuario.
                    UserName = NewUserName;
                    sMsgError = null;
                    return (true);
            }
            catch (Exception ex)
            {
                sMsgError = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        /// <summary>
        /// Método que actualiza la contraseña almacenada en la Credencial.
        /// </summary>
        /// <param name="NewPassword">Nueva contraseña de usuario.</param>
        /// <param name="sMsgError">Mensaje de error, si se produce uno durante el proceso de actualización.</param>
        /// <returns>true, si actualización correcta, false, sinó.</returns>
        public bool SetPassword(string NewPassword, out string sMsgError, bool EncriptPassword = true)
        {
            try
            {
                //  Variables.
                    string hashOfInput;

                //  Valida que la constrasña no sea nula ni vacía.
                    if (ValidatePassword(NewPassword, out sMsgError))
                    {
                        //  Actualiza la contraseña.
                            if (EncriptPassword)
                            {
                                if ((hashOfInput = Encript.CodificarPassword(NewPassword, out sMsgError)) != null)
                                {
                                    Password = hashOfInput;
                                }
                                else return (false);
                            }
                            else Password = NewPassword;
                            sMsgError = null;
                            return (true);
                    }
                    else return (false);
            }
            catch (Exception ex)
            {
                sMsgError = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        /// <summary>
        /// Método que permite modificar el Identificador de la Credencial.
        /// </summary>
        /// <param name="NewCredential_Id">Nuevo Identificador.</param>
        /// <param name="sMsgError">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        public bool SetCredentialId(Guid NewCredential_Id, out string sMsgError)
        {
            sMsgError = null;
            try
            {
                //  Valida el Nuevo Udentificador.
                    if (NewCredential_Id == Guid.Empty)
                    {
                        sMsgError = string.Format("Error, valor incorrecto '{0}' de Identifcador de Credencial", NewCredential_Id);
                        return (false);
                    }
                //  Si pasa la validación asignamos el nuevo Identificador de Credencial.
                    Credential_Id = NewCredential_Id;
                    return (true);

            }
            catch (Exception ex)
            {
                sMsgError = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        /// <summary>
        /// Método que valida la complejidad de la contraseña.
        /// </summary>
        /// <param name="Password">Nueva contraseña de usuario.</param>
        /// <param name="sMsgError">Mensaje de error, si se produce uno durante el proceso de actualización.</param>
        /// <param name="ValidateComplexity">Indica si se debe validar o no la complejidad de la contraseña.</param>
        /// <returns>true, si actualización correcta, false, sinó.</returns>
        public static bool ValidatePassword(string Password, out string sMsgError, bool ValidateComplexity = false)
        {
            try
            {
                //  Valida que la constrasña no sea nula ni vacía.
                    if ((String.IsNullOrEmpty(Password)) || (String.IsNullOrWhiteSpace(Password)))
                    {
                        sMsgError = "Error, la contraseña no puede ser una cadena vacía.";
                        return (false);
                    }
                //  Valida que la contraseña tenga cómo mínimo 6 carácteres.
                    if ((ValidateComplexity) && (Password.Length < NUM_MIN_PASSWORD_CHARACTERS))
                    {
                        sMsgError = 
                            MsgManager.LiteralMsg(
                               new string[] { "Error, la contraseña debe tener una longitud mínima de '{0}' carácteres.",
                                              NUM_MIN_PASSWORD_CHARACTERS.ToString() });
                        return (false);
                    }
                //  Valida la complejidad de la contraseña
                    if ((ValidateComplexity) && (MatchUpper.Matches(Password).Count < MIN_NUM_UPPER))
                    {
                        sMsgError = 
                            MsgManager.LiteralMsg(
                               new string[] { "Error, la contraseña debe contener como mínimo '{0}' carácteres en mayúscula.",
                                              MIN_NUM_UPPER.ToString() });
                        return (false);
                    }
                    if ((ValidateComplexity) && (MatchLower.Matches(Password).Count < MIN_NUM_LOWER))
                    {
                        sMsgError = 
                            MsgManager.LiteralMsg(
                               new string[] { "Error, la contraseña debe contener como mínimo '{0}' carácteres en minúscula.",
                                              MIN_NUM_LOWER.ToString() });
                        return (false);
                    }
                    if ((ValidateComplexity) && (MatchNumber.Matches(Password).Count < MIN_NUM_NUMBER))
                    {
                        sMsgError = 
                            MsgManager.LiteralMsg(
                               new string[] { "Error, la contraseña debe contener como mínimo '{0}' números.",
                                              MIN_NUM_NUMBER.ToString() });
                        return (false);
                    }
                    if ((ValidateComplexity) && (MatchSpecialCharacter.Matches(Password).Count < MIN_NUM_SPECIAL_CHARS))
                    {
                        sMsgError = 
                            MsgManager.LiteralMsg(
                               new string[] { "Error, la contraseña debe contener como mínimo '{0}' carácteres especiales.",
                                              MIN_NUM_SPECIAL_CHARS.ToString() });
                        return (false);
                    }
                //  Indica que no hay errores.
                    sMsgError = null;
                    return (true);
            }
            catch (Exception ex)
            {
                sMsgError = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }
        /// <summary>
        /// Método que sobreescribe el método ToString de la clase base.
        /// </summary>
        /// <returns>Representación en una cadena de carácteres de la instáncia de la clase.</returns>
        public override string ToString()
        {
            //  Crea la cadena de texto que contiene la información de la instáncia de la clase y lo devuelve.
                return (string.Format("Usuario: {0}", UserName));
        }

        #endregion
    }
}
