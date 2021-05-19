#region Libraries used by the class

using MBCode.Framework.Managers.Logs;
using MBCode.Framework.Managers.Messages;
using System;

#endregion

namespace HispaniaCommon.ViewModel
{
    public class LogViewModel
    {
        #region Attributes

        /// <summary>
        /// Store the Path File Log
        /// </summary>
        private string m_PathFileLog = string.Format("{0}HispaniaComptabilitat_{1}.log", System.AppDomain.CurrentDomain.BaseDirectory, Environment.MachineName.ToUpper());

        /// <summary>
        /// Store the Trace Level to use.
        /// </summary>
        private TraceLevel m_TraceLevelToUse = TraceLevel.TRACE_LEVEL_ERRORS;

        #endregion

        #region Properties

        /// <summary>
        /// Get the Path File Log
        /// </summary>
        private string PathFileLog
        {
            get
            {
                return m_PathFileLog;
            }
        }

        public TraceLevel TraceLevelToUse
        {
            get
            {
                return m_TraceLevelToUse;
            }
            set
            {
                m_TraceLevelToUse = value;
            }
        }

        #endregion

        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static LogViewModel instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static LogViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogViewModel();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private LogViewModel()
        {
        }

        #endregion

        #region Functions

        public void WriteLog(string sInfo)
        {
            Manager_Log.WriteLog(PathFileLog, sInfo, TraceLevelToUse);
        }

        public void WriteLog(string sInfo, object AdditionalInfo_1)
        {
            Manager_Log.WriteLog(PathFileLog, string.Format(sInfo, AdditionalInfo_1), TraceLevelToUse);
        }

        public void WriteLog(string sInfo, object AdditionalInfo_1, object AdditionalInfo_2)
        {
            Manager_Log.WriteLog(PathFileLog, string.Format(sInfo, AdditionalInfo_1, AdditionalInfo_2), TraceLevelToUse);
        }

        public void WriteLog(string sInfo, object AdditionalInfo_1, object AdditionalInfo_2, object AdditionalInfo_3)
        {
            Manager_Log.WriteLog(PathFileLog, string.Format(sInfo, AdditionalInfo_1, AdditionalInfo_2, AdditionalInfo_3), TraceLevelToUse);
        }

        public void WriteLog(string sInfo, object AdditionalInfo_1, object AdditionalInfo_2, object AdditionalInfo_3, object AdditionalInfo_4)
        {
            Manager_Log.WriteLog(PathFileLog, string.Format(sInfo, AdditionalInfo_1, AdditionalInfo_2, AdditionalInfo_3, AdditionalInfo_4), TraceLevelToUse);
        }

        #endregion
    }
}