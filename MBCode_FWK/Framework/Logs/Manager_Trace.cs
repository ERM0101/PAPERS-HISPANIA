#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace MBCode.Framework.Managers.Trace
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 15/07/2018
    /// Descripción: clase que se encarga de gestionar trazas's basicas del Framework.
    /// </summary>
    public static class Manager_Trace
    {
        #region Attributes

        private static Dictionary<Guid, SortedList<DateTime, string>> dcTraceInfo = new Dictionary<Guid, SortedList<DateTime, string>>();

        #endregion

        #region Methods

        public static Guid CreateTrace()
        {
            Guid NewTraceIdentifier = Guid.NewGuid();
            dcTraceInfo.Add(NewTraceIdentifier, new SortedList<DateTime, string>());
            return NewTraceIdentifier;
        }

        public static void RemoveTrace(Guid guidTraceIdentifier)
        {
            dcTraceInfo.Remove(guidTraceIdentifier);
        }

        public static void AddInfoToTrace(Guid guidTraceIdentifier, string sInformation)
        {
            DateTime dtTraceInfo = DateTime.Now;
            if (dcTraceInfo[guidTraceIdentifier].ContainsKey(dtTraceInfo))
            {
                dcTraceInfo[guidTraceIdentifier][dtTraceInfo] = string.Format("{0}|{1}", dcTraceInfo[guidTraceIdentifier][dtTraceInfo], sInformation);
            }
            else
            {
                dcTraceInfo[guidTraceIdentifier].Add(dtTraceInfo, sInformation);
            }
        }

        public static string GetTraceInfo(Guid guidTraceIdentifier)
        {
            StringBuilder sbTraceInfo = new StringBuilder(string.Empty);
            DateTime dtPreviousTraceDate = DateTime.MinValue;
            foreach (KeyValuePair<DateTime, string> infoTrace in dcTraceInfo[guidTraceIdentifier])
            {
                foreach (string InfoTraceSplitted in infoTrace.Value.Split('|'))
                {
                    sbTraceInfo.AppendFormat("{0} ({1}) : {2}\r\n",
                                             infoTrace.Key.ToString("hh:mm:ss.fff"), // "dd-MM-yyyy hh:mm:ss.fff"
                                             GetElapsedTimeTraceInfo(dtPreviousTraceDate, infoTrace.Key),
                                             InfoTraceSplitted);
                    dtPreviousTraceDate = infoTrace.Key;
                }
            }
            return (sbTraceInfo.ToString());
        }

        private static string GetElapsedTimeTraceInfo(DateTime dtPreviousTraceDate, DateTime dtTraceDate)
        {
            if (dtPreviousTraceDate == DateTime.MinValue) return "00";
            else
            {
                TimeSpan tsElapsedTime = dtTraceDate.Subtract(dtPreviousTraceDate);
                bool CanShowHours = tsElapsedTime.Hours > 0;
                bool CanShowMinutes = tsElapsedTime.Minutes > 0;
                string ElapsedTime = tsElapsedTime.ToString();
                if (CanShowHours) return ElapsedTime;
                else
                {
                    if (CanShowMinutes)
                    {
                        return ElapsedTime.Substring(3, ElapsedTime.Length - 3);
                    }
                    else
                    {
                        return ElapsedTime.Substring(6, ElapsedTime.Length - 6);
                    }
                }
            }
              
        }

        #endregion
    }
}

