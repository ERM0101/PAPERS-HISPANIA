#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.IO;
using System.Text;
using System.Threading;

#endregion

namespace MBCode.Framework.Managers.Logs
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 16/07/2013
    /// Descripción: clase que se encarga de gestionar log's basicos del Framework.
    /// </summary>
    public static class Manager_Log
    {
        #region Constantes

        /// <summary>
        /// Path del fichero de Log.
        /// </summary>
        private const string PATH_FILE_LOG_FRW = @"C:\Sources\HispaniaComptabilitat\MBCode_FWK\Framework\Log\";

        /// <summary>
        /// Path completo del fichero de Log.
        /// </summary>
        public const string FILE_LOG_FRW = PATH_FILE_LOG_FRW + "Framework.log";

        #endregion

        #region Métodos

        /// <summary>
        /// Método que gestiona la creación de los directorios del Servidor.
        /// </summary>
        public static bool EnsureLogDirectoriesExist()
        {
            try
            {
                if (!Directory.Exists(PATH_FILE_LOG_FRW)) Directory.CreateDirectory(PATH_FILE_LOG_FRW);
                return (true);
            }
            catch
            {
                return (false);
            }
        }


        /// <summary>
        /// Método que escribirá la información en el Log.
        /// </summary>
        /// <param name="sInfo">Información a Escribir.</param>
        internal static void WriteLogFRW(string sInfo)
        {
            try
            {
                //  Por Seguridad/Robustez y para evitar cadenas 'vacias' 
                    if (!String.IsNullOrEmpty(FILE_LOG_FRW) && !String.IsNullOrWhiteSpace(FILE_LOG_FRW) &&
                        !String.IsNullOrEmpty(sInfo) && !String.IsNullOrWhiteSpace(sInfo))
                    { 
                        //  Se asegura que la estructura de directorios del Server está creada y si es así escribe el Log.
                            if (EnsureLogDirectoriesExist())
                            {
                                File.AppendAllText(FILE_LOG_FRW, sInfo, Encoding.UTF8);
                            }
                    }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(String.Format("Internal WriteLogFRW( {0} ) no se puede escribir en {1}. \nExcp {2}", sInfo, FILE_LOG_FRW, ex.Message));
            }
        }

        /// <summary>
        /// Método que escribirá la información en el Log.
        /// </summary>
        /// <param name="filePath">Path absoluto del fichero de Log.</param>
        /// <param name="sInfo">Información a Escribir.</param>
        /// <param name="traceLevel">Nivel de traza personalizado a usar.</param>
        public static void WriteLog(string filePath, string sInfo, TraceLevel? traceLevel = null)
        { 
            if (traceLevel != null)
            {
                if (MsgManager.LevelTraceLog >= traceLevel) WriteLog(filePath, sInfo);
                // else -> No se debe escribir nada ya que el nivel de log así lo indica.
            }
            else WriteLog(filePath, sInfo);
        }


        /// <summary>
        /// Consulta la cabecera del archivo de log indicado
        /// </summary>
        /// <param name="file">ruta completa del archivo de log</param>
        /// <returns>true / false en funcion de si ya esta en uso o no</returns>
        internal static bool IsBusyWriteLog(string file)
        {
            bool busy = false;
            if (File.Exists(file))
            {
                using (BinaryReader br = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read)))
                {
                    busy = true;
                    try
                    {
                        // Leer el primer byte en la cabecera del fichero
                        byte leido = br.ReadByte();
                        busy = (0 != leido);
                    }
                    catch (Exception ex)
                    {
                        WriteLogFRW("Error leyendo fichero con IsBusyWriteLog():\n" + MsgManager.ExcepMsg(ex));
                    }
                    finally
                    {
                        br.Close();
                    }
                }
            }

            return (busy);
        }

        /// <summary>
        /// Marca la cabecera del archivo log indicado
        /// </summary>
        /// <param name="file">ruta completa del archivo de log</param>
        internal static void SetBusyWriteLog(string file)
        {
            byte busy = 1;
            int  offSeek = 0;
            bool creado = File.Exists(file);

            using (BinaryWriter bw = new BinaryWriter(new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                try
                {
                    // Escribir el primer byte en la cabecera del fichero
                    bw.Seek(offSeek, SeekOrigin.Begin);
                    bw.Write(busy);
                    bw.Flush();
                    // MODY: Hay que asegurar el tamaño del fichero para la primera vez 
                    // (si aun no se habia creado, lo dejaremos con un tamaño 5 bytes)
                    if (!creado)
                    {
                        // Escribimos el resto de bytes (4 mas) en la cabecera del fichero
                        bw.Seek(1, SeekOrigin.Begin);
                        bw.Write(offSeek);
                        bw.Flush();
                    }

                }
                catch (Exception ex)
                {
                    WriteLogFRW("Error escribirendo fichero en SetBusyWriteLog():\n" + MsgManager.ExcepMsg(ex));
                }
                finally
                {
                    bw.Close();
                }
            }
        }


        /// <summary>
        /// Método que escribirá la información en el Log como una cola circular de tamaño maximo fijado
        /// </summary>
        /// <param name="sInfo">Cadena texto con la Información a Escribir.</param>
        private static void WriteLog(string filePath, string sInfo)
        {

            int offset_seek = 0;
            int sizeFile = 20 * 1024 * 1024; // 20 Mbytes
            string texto = string.Format("\r\n[{0}] : {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), sInfo);
            texto = texto.Replace(@"\r\n", Environment.NewLine);

            //  Por Seguridad/Robustez y para evitar cadenas 'vacias' 
            if (String.IsNullOrEmpty(filePath) || String.IsNullOrWhiteSpace(filePath) || String.IsNullOrEmpty(sInfo) || String.IsNullOrWhiteSpace(sInfo))
            {
                return; // --> Abort
            }

            ///////////////////////////////////////////////////////////////////////
            // 0.- Acceder al fichero LOG y MARK como BUSY del recurso en la cabecera...
            ////////////////////////////////////////////////////////////////////////
            try
            {
                string[] asFile = filePath.Split('\\');
                string sDirectoryName = filePath.Substring(0, filePath.Length - asFile[asFile.Length - 1].Length);
                if (!Directory.Exists(sDirectoryName)) Directory.CreateDirectory(sDirectoryName); // Crea directorio

                // MODY: Para sincronizar multiples accesos y evitar solapes de escritura en la cabecera
                if (IsBusyWriteLog(filePath)) // First attempt 
                {
                    Thread.Sleep(5); // --> Wait for 2*50ms delay
                    if (IsBusyWriteLog(filePath)) // Second attempt 
                    {
                        WriteLogFRW(texto);
                        return; // --> Abort
                    }
                }
                SetBusyWriteLog(filePath); // Mark file header as resource file ALREADY in use.
                // MODY: Para sincronizar multiples accesos y evitar solapes de escritura en la cabecera


                ///////////////////////////////////////////////////////////////////////
                // 1.- Acceder al fichero LOG y LEER el desplazamiento en la cabecera...
                ////////////////////////////////////////////////////////////////////////
                if (File.Exists(filePath) )
                {
                    using (BinaryReader br = new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
                    {
                        try
                        {
                            offset_seek = br.ReadByte(); // MODY: Descartar el primer byte de 'busy' ya tratado
                            offset_seek = br.ReadInt32(); // Obtener el actual offset de la cabecera del fichero
                        }
                        catch (EndOfStreamException ex) // Cuando no se puede leer del fichero Log
                        {
                            WriteLogFRW("Error EOS al leer en WriteLog():\n" + MsgManager.ExcepMsg(ex));
                        }
                        catch (Exception ex) // Cuando no se puede leer de fichero Log
                        {
                            WriteLogFRW("Error CLS al leer en WriteLog():\n" + MsgManager.ExcepMsg(ex));
                        }
                        finally
                        {
                            br.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFRW(String.Format("En private WriteLog( {0} ) no se puede acceder al {1}. \nExcp {2}", texto, filePath, MsgManager.ExcepMsg(ex)));
            }

            ////////////////////////////////////////////////////////////
            // 2.- Acceder al fichero LOG y ESCRIBIR en el las trazas...
            ////////////////////////////////////////////////////////////
            try
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    try
                    {
                        //  Controlar el tamaño del fichero con rango de offset[ 4...200*1024 ] Bytes
                        //  ( Nota: --> System.Runtime.InteropServices.Marshal.SizeOf( offset_seek );
                        if ((offset_seek >= sizeFile) || (offset_seek < 5 ))  
                            {
                                //offset_seek = 4; // sizeof( int offset_seek)
                                offset_seek = 5; // MODY: 4+1 = sizeof( int 32 bits ) + sizeof( byte ) */
                            }
                        //  Posicionar el fichero antes de escribir el texto
                            bw.Seek(offset_seek, SeekOrigin.Begin);
                            byte[] linea = Encoding.UTF8.GetBytes(texto);
                            bw.Write(linea);
                            bw.Write('~'); // Marca final '~' del archivo
                            bw.Flush();
                        //  Actualizando el nuevo offset escribiendolo en la cabecera...
                            offset_seek += linea.Length;
                            int offSeek = 1; // MODY: sizeof( byte 'busy')
                            bw.Seek(offSeek, SeekOrigin.Begin);
                            bw.Write(offset_seek);
                            bw.Flush();
                        // MODY: Clear 'busy' mark file header as resource file are NOT in use.
                            offSeek = 0; // Basta con escribir el primer byte con la marca a cero
                            bw.Seek(offSeek, SeekOrigin.Begin); bw.Write((byte)offSeek); bw.Flush();
                    }
                    catch (Exception ex) // Cuando no se puede escribir en el fichero Log
                    {
                        WriteLogFRW("Error tratando fichero en WriteLog():\n" + MsgManager.ExcepMsg(ex));
                    }
                    finally
                    {
                        bw.Close();
                    }
                }  
            }
            catch (Exception ex)
            {
                WriteLogFRW(String.Format("Exp WriteLog( {0} ) no se puede escribir en {1}. \nExcp {2}", texto, filePath, 
                                          MsgManager.ExcepMsg(ex)));
            }
        }


        #endregion
    }
}

