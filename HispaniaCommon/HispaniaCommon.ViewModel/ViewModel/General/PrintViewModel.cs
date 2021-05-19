#region Libraries used by the class

using MBCode.Framework.Managers.Messages;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace HispaniaCommon.ViewModel
{
    public class PrintViewModel
    {
        #region Attributes

        /// <summary>
        /// Stores the value that manage the worker execution
        /// </summary>
        private bool runWorker = false;

        /// <summary>
        /// Create a new Background Worker process;
        /// </summary>
        private BackgroundWorker worker;

        #endregion

        #region Properties

        public bool IsInitialized
        {
            get
            {
                return runWorker;
            }
        }

        #endregion

        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static PrintViewModel instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static PrintViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PrintViewModel();
                    instance.InitializeWorker();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private PrintViewModel()
        {
        }

        #endregion
        
        #region Worker

        /// <summary>
        /// Initialize worker process.
        /// </summary>
        private void InitializeWorker()
        {
            runWorker = true;
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            if (worker.IsBusy != true) worker.RunWorkerAsync();
        }

        public void EndWorker()
        {
            runWorker = false;
        }

        /// <summary>
        /// Do the tasks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //  Load variables.
                BackgroundWorker worker = sender as BackgroundWorker;
                int Iteration = 1;
            //  Start the Main bucle of the worker.
                while (runWorker)
                {
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);


                        worker.ReportProgress(Iteration, string.Format("Iteración {0} finalida.", Iteration));
                        Iteration++;
                    }
                }
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Text.StringBuilder sbObservers = new System.Text.StringBuilder("Print information: " + e.UserState);
        }

        /// <summary>
        /// Update UI Here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) MsgManager.ShowMessage("Error: inizilitzant el gestor d'impressió.\r\nDetalls: {0}\r\n" + e.Error.Message);
            //else if (e.Cancelled == true) MessageBox.Show("Cancel·lat");
            //else MessageBox.Show("Acabat");
        }

        #endregion
    }
}