#region Librerias usada por la clase

using MBCode.Framework.Controls.WPF.Dialogs;
using MBCode.Framework.Managers.Messages;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MBCode.FrameworkDemoWFP.InterfazUsuario
{
    /// <summary>
    /// Lógica de interacción para WindowDemoMsgManager.xaml
    /// </summary>
    public partial class WindowDemoMsgManager : Window
    {
        private WaitDialog Wait;

        public WindowDemoMsgManager()
        {
            InitializeComponent();
            cbMode.IsChecked = true;
        }

        private void OnOpenWaitDialog(object sender, RoutedEventArgs e)
        {
            if (Wait == null)
            {
                Wait = new WaitDialog("Ventana de Espera", (cbMode.IsChecked == true), 
                                      (cbMode2.IsChecked == true) ? int.Parse(tbTimeOpen.Text) : 0, Orientation.Horizontal,
                                      "Paso 0\r\nDescripción: paso 0...");
                Wait.evCancelPressed += new WaitDialog.dlgWaitDialogClosed(OnWaitCancelPressed);
                Wait.evActionCompleted += new WaitDialog.dlgWaitDialogClosed(OnWaitActionCompleted);
                Wait.evThrowException += new WaitDialog.dlgThrowException(OnWaitThrowException);
            }
            else MsgManager.ShowMessage("Ya hay una Ventana de Espera abierta.");
        }

        private void OnWaitActionCompleted()
        {
            tbResults.Text = "Action Completed\r\n" + tbResults.Text;
            Wait.evCancelPressed -= new WaitDialog.dlgWaitDialogClosed(OnWaitCancelPressed);
            Wait.evActionCompleted -= new WaitDialog.dlgWaitDialogClosed(OnWaitActionCompleted);
            Wait.evThrowException -= new WaitDialog.dlgThrowException(OnWaitThrowException);
            Wait = null;
        }

        private void OnWaitCancelPressed()
        {
            tbResults.Text = "Cancel Pressed\r\n" + tbResults.Text;
            Wait.evCancelPressed -= new WaitDialog.dlgWaitDialogClosed(OnWaitCancelPressed);
            Wait.evActionCompleted -= new WaitDialog.dlgWaitDialogClosed(OnWaitActionCompleted);
            Wait.evThrowException -= new WaitDialog.dlgThrowException(OnWaitThrowException);
            Wait = null;
        }

        private void OnWaitThrowException(string sMsgError)
        {
            tbResults.Text = string.Format("{0}\r\n{1}.", sMsgError, tbResults.Text);
        }

        private void OnAddStep(object sender, RoutedEventArgs e)
        {
            if (Wait != null) Wait.AddStep(string.Format("Paso {0}\r\nDescripción: paso {0}...", 1));
            else MsgManager.ShowMessage("No hay ninguna Ventana de Espera abierta.");
        }

        private void OnWaitWindowState(object sender, RoutedEventArgs e)
        {
            if (Wait != null)
            {
                MsgManager.ShowMessage("El thread está vivo :-> " + Wait.IsThreadWaitDialogAlive.ToString() + "\r\n" +
                                       "Estado de la Ventana :-> " + Wait.WaitDialogState.ToString());
            }
            else MsgManager.ShowMessage("No hay ninguna Ventana de Espera abierta.");
        }

        private void OnAddAllSteps(object sender, RoutedEventArgs e)
        {
            if (Wait != null)
            {
                for (int i = 1; i <= 100; i++)
                {
                    Wait.AddStep(string.Format("Paso {0}\r\nDescripción: paso {0}...", i));
                    Thread.Sleep(10);
                }
            }
            else MsgManager.ShowMessage("No hay ninguna Ventana de Espera abierta.");
        }

        private void OnTimeAutomaticModeActivated(object sender, RoutedEventArgs e)
        {
            tbTimeOpen.Text = "5";
            if (Wait != null)
            {
                Wait.SelectMode((cbMode2.IsChecked == true) ? int.Parse(tbTimeOpen.Text) : 0);
            }
        }

        private void OnContinousModeActivated(object sender, RoutedEventArgs e)
        {
            if (Wait != null)
            {
                Wait.SelectMode((cbMode.IsChecked == true));
            }
        }

        private void OnCloseDialog(object sender, RoutedEventArgs e)
        {
            if (Wait != null)
            {
                Wait.CloseDialog();
            }
        }

        private void rbError_Checked(object sender, RoutedEventArgs e)
        {
            MsgManager.ShowMessage("Error, in method 'OnTimeAutomaticModeActivated'.\r\nDetails: ppppppp", MsgType.Error);
        }

        private void rbWarning_Checked(object sender, RoutedEventArgs e)
        {
            MsgManager.ShowMessage("Aviso, in method 'OnTimeAutomaticModeActivated'.\r\nDetails: ppppppp", MsgType.Warning);
        }

        private void rbInformation_Checked(object sender, RoutedEventArgs e)
        {
            MsgManager.ShowMessage("Información, in method 'OnTimeAutomaticModeActivated'.\r\nDetails: ppppppp", MsgType.Information);
        }

        private void rbNotification_Checked(object sender, RoutedEventArgs e)
        {
            MsgManager.ShowMessage("Notificación, in method 'OnTimeAutomaticModeActivated'.\r\nDetails: ppppppp", MsgType.Notification);
        }
    }
}
