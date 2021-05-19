#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Text.RegularExpressions;

#endregion

namespace MBCode.Framework.Managers.RegularExpressions
{
    #region Enumerados

    /// <summary>
    /// Enumerado que define si un texto corresponde a una IP válida.
    /// </summary>
    public enum ValidateIPResult
    {
        /// <summary>
        /// Se ha reconocido una dirección IP v.4.
        /// </summary>
        IP4_Recognized,

        ///// <summary>
        ///// Se ha reconocido una dirección IP v.6.
        ///// </summary>
        //IP6_Recognized,

        /// <summary>
        /// No se ha Reconocido ninguna IP.
        /// </summary>
        Unrecognized,

        /// <summary>
        /// Validación errónea.
        /// </summary>
        Error,
    }

    #endregion

    public static class RegExpManager
    {
        #region Validate IP v. 4

        /// <summary>
        /// Método encargado de comprobar que un texto tenga el formato de una dirección IP correcta.
        /// </summary>
        /// <param name="sDataInput">Texto a analizar.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar la definición del enumerado.</returns>
        public static ValidateIPResult ValidateIP4(string sDataInput, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Determina si el texto a analizar no es nulo.
                    if ((sDataInput == null) || (sDataInput.Equals(string.Empty)))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_RegExpManager_004");
                        return (ValidateIPResult.Error);
                    }
                //  Determina si la IP contiene más puntos de los deseados.
                    int iCountDots = 0;
                    for (int i = 0; i < sDataInput.Length; i++)
                    {
                        if (sDataInput[i] == '.') iCountDots++;
                    }
                    if (iCountDots != 3)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_RegExpManager_003");
                        return (ValidateIPResult.Error);
                    }
                //  Determina si el texto corresponde a una dirección IP v4 o IP v6.
                    Regex rxSegmentIP4 = new Regex(@"([0-9]+)");
                    Regex rxAddressIP4 = new Regex(string.Format("{0}.{0}.{0}.{0}", rxSegmentIP4));
                    foreach (char chr in sDataInput)
                    {
                        if ("0123456789.".IndexOf(chr) < 0)
                        {
                            sMensaje = MsgManager.ErrorMsg("MSG_RegExpManager_000", chr.ToString());
                            return (ValidateIPResult.Unrecognized);
                        }
                    }
                    if (rxAddressIP4.IsMatch(sDataInput))
                    {
                        MatchCollection matches = rxSegmentIP4.Matches(sDataInput);
                        if (matches.Count.Equals(4))
                        {
                            int iNumSegment = 1;
                            foreach (Match match in matches)
                            {
                                if (int.Parse(match.Value) > 255)
                                {
                                    sMensaje = MsgManager.ErrorMsg("MSG_RegExpManager_001", iNumSegment.ToString());
                                    return (ValidateIPResult.Unrecognized);
                                }
                                if ((iNumSegment == 1) && (int.Parse(match.Value) == 0))
                                {
                                    sMensaje = MsgManager.ErrorMsg("MSG_RegExpManager_001", iNumSegment.ToString());
                                    return (ValidateIPResult.Unrecognized);
                                }
                                iNumSegment++;
                            }
                            return (ValidateIPResult.IP4_Recognized);
                        }
                        else
                        {
                            sMensaje = MsgManager.ErrorMsg("MSG_RegExpManager_002");
                            return (ValidateIPResult.Unrecognized);
                        }
                    }
                    else
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_RegExpManager_003");
                        return (ValidateIPResult.Unrecognized);
                    }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ValidateIPResult.Error);
            }
        }

        #endregion
    }
}
