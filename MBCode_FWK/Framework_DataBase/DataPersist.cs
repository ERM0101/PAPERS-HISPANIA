#region Librerias de la clase

#endregion

namespace MBCode.Framework.DataBase
{
    #region Enumerados

    public enum EnumEstadosCambios : short
    {
        SinCambios = 0,
        Agregado = 1,
        Modificado = 2,
        Eliminado = 3
    }

    #endregion

    /// <summary>
    ///  Clase base para los objetos que deben reflejar un estado de persistencia en bd. Es decir, se guarda uno de los 
    ///  posbiles 3 estados:
    ///                         - Sin cambios: no debe hacerse nada en bd con el objeto.
    ///                         - Agregado:  el objeto debe agregarse (insert) como registro en bd.
    ///                         - Modificado: el objeto debe actualizarse (update) en base de datos.
    ///                         - Eliminado: el objeto debe eliminarse (delete) en base de datos.
    ///                         
    ///  Se opera en colecciones de objetos, y cuando se ha finalizado se recorren los objetos y según su estado se 
    ///  opera en bd.
    /// </summary>
    public class DataPersist
    {
        protected EnumEstadosCambios miEstadoCambios;

        public DataPersist()
        {
            miEstadoCambios = EnumEstadosCambios.SinCambios;
        }

        public EnumEstadosCambios EstadoCambios
        {
            get
            {
                return miEstadoCambios;
            }
            set
            {
                if (miEstadoCambios == EnumEstadosCambios.Agregado)
                {
                    if ((value == EnumEstadosCambios.Eliminado) || (value == EnumEstadosCambios.SinCambios)) 
                        miEstadoCambios = value;
                }
                if (miEstadoCambios == EnumEstadosCambios.Modificado)
                {
                    if ((value == EnumEstadosCambios.Eliminado) || (value==EnumEstadosCambios.SinCambios))
                        miEstadoCambios = value;
                }

                if (miEstadoCambios == EnumEstadosCambios.SinCambios)
                {
                    if ((value == EnumEstadosCambios.Modificado) || (value == EnumEstadosCambios.Eliminado) ||
                        value == EnumEstadosCambios.Agregado)
                        miEstadoCambios = value;
                }
                if (miEstadoCambios == EnumEstadosCambios.Eliminado)  miEstadoCambios = value;
            }
        }
    }
}
