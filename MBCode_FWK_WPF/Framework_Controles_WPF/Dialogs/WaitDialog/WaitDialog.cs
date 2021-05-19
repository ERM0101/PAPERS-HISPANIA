#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

#endregion

namespace MBCode.Framework.Controls.WPF.Dialogs
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 13/08/2013
    /// Descripción: clase que permite al usuario interactuar con el WaitDialog Creado.
    /// </summary>
    public class WaitDialog
    {
        #region Enumerados

        /// <summary>
        /// Define las posibles Acciones de Usuario sobre la Ventana de Espera.
        /// </summary>
        private enum ActionType
        { 
            /// <summary>
            /// Selecciona el modo de actualización de la ProgressBar ( true :-> Continuo, false :-> Por Pasos ).
            /// </summary>
            SelectMode,

            /// <summary>
            /// Modificar la orientación de la animación de avance de la ProgressBar de la Ventana de Espera.
            /// </summary>
            OrientationAnimation,

            /// <summary>
            /// Si la ProgressBar está en modo 'Por Pasos' añade un paso a la ProgressBar.
            /// </summary>
            AddStep,

            /// <summary>
            /// Asigna el Texto de la Acción asociada a la posción actual de la ProgressBar a la Ventana de Espera.
            /// </summary>
            SetActionDescription,

            /// <summary>
            /// Indica que se debe Cerrar la Ventana de Espera. Esto solo funciona en modo 'Continuo'.
            /// </summary>
            CloseDialog,
        }

        #endregion

        #region Delegados

        /// <summary>
        /// Define la firma del método que gestiona una acción sobre la Ventana de Espera.
        /// </summary>
        /// <param name="Type">Tipo de Acción.</param>
        /// <param name="Data">Datos asociados a la acción a realizar.</param>
        private delegate void dlgExecuteAction(ActionType Type, Dictionary<string, object> Data);

        /// <summary>
        /// Delegado que define la firma del método que gestiona la notificación del cierre de la Ventana de Espera,  bien  
        /// sea por acción del usuario con el botón de 'Cancelar', bien sea porque se ha llegado al número de pasos máximo 
        /// en modo no Contínuo o que el usuario ha forzado el cierre en el modo Contínuo.
        /// </summary>
        public delegate void dlgWaitDialogClosed();

        /// <summary>
        /// Definición de la firma del método que se encarga de notificar que se ha producido una Excepción.
        /// </summary>
        /// <param name="sMsgError">Mensaje de Error, asociado a la Excepción que se ha producido.</param>
        public delegate void dlgThrowException(string sMsgError);

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se lanza al detectar que la Acción se ha completado  bien sea porque se ha llegado al número de  pasos 
        /// máximo en modo no Contínuo o que el usuario ha forzado el cierre en el modo Contínuo.
        /// </summary>
        public event dlgWaitDialogClosed evActionCompleted;

        /// <summary>
        /// Evento que se lanza al detectar que el usuario ha apretado el botón de 'Cancelar'.
        /// </summary>
        public event dlgWaitDialogClosed evCancelPressed;

        /// <summary>
        /// Evento que se lanza al detectar que se ha producido una Excepción.
        /// </summary>
        public event dlgThrowException evThrowException;

        #endregion

        #region Atributos

        /// <summary>
        /// Almacena un valor que indica si se ha producido un error de inicialización.
        /// </summary>
        private bool InitializationError;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene / Establece la Ventana de Espera.
        /// </summary>
        private WaitDialogWindow wndWait
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene / Establece el Thread que controla la presentación de la Ventana de Espera.
        /// </summary>
        private Thread thdWaitDialog
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene / Establece una referencia al Thread padre del WaitDialog.
        /// </summary>
        private Thread ParentThread
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene / Establece una referencia al Thread padre del WaitDialog.
        /// </summary>
        private Thread WindowThread
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene / Establece el Thread que controla la presentación de la Ventana de Espera.
        /// </summary>
        public bool IsThreadWaitDialogAlive
        {
            get
            {
                return (thdWaitDialog.IsAlive);
            }
        }

        /// <summary>
        /// Obtiene el Estado de la Ventana de Espera
        /// </summary>
        public DialogState WaitDialogState
        {
            get
            { 
                return (wndWait.State);
            }
        }

        public bool Mode
        {
            get
            {
                return (wndWait.ContinousMode);
            }
        }                

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        /// <param name="WaitWindowTitle">Título de la venatan de Espera.</param>
        /// <param name="ContinousMode">Valor que indica si la ProgressBar muestra valores reales o información sobre el progreso continuo y genérico.</param>
        /// <param name="TimeToAutomaticClose">Tiempo de Cierre automático de la Ventana de Espera.</param>
        /// <param name="OrientationAnimation">Define la orientación que seguirá la animación de avance de la ProgressBar.</param>
        /// <param name="InitialAction">Texto asociado a la acción inicial.</param>
        /// <param name="ShowCancelButton">Determina si se debe mostrar o no el botón de Cancelar.</param>
        /// <param name="TypeMessage">Indica el tipo de mensaje asociado a la información que se presenta.</param>
        public WaitDialog(string WaitWindowTitle = null, bool ContinousMode = true, int TimeToAutomaticClose = 0,
                          Orientation OrientationAnimation = Orientation.Horizontal, string InitialAction = null,
                          bool ShowCancelButton = false, MessageType TypeMessage = MessageType.Normal)
        {
            //  Realiza las inicializaciones de la clase.
                Initialize(WaitWindowTitle, ContinousMode, TimeToAutomaticClose, OrientationAnimation, InitialAction, ShowCancelButton,
                           TypeMessage);
        }

        /// <summary>
        /// Método inicializador de la clase.
        /// </summary>
        /// <param name="WaitDialogWindowTitle">Título de la venatan de Espera.</param>
        /// <param name="ContinousMode">Valor que indica si la ProgressBar muestra valores reales o información sobre el progreso continuo y genérico.</param>
        /// <param name="TimeToAutomaticClose">Tiempo de Cierre automático de la Ventana de Espera.</param>
        /// <param name="OrientationAnimation">Define la orientación que seguirá la animación de avance de la ProgressBar.</param>
        /// <param name="InitialAction">Texto asociado a la acción inicial.</param>
        /// <param name="ShowCancelButton">Determina si se debe mostrar o no el botón de Cancelar.</param>
        /// <param name="TypeMessage">Indica el tipo de mensaje asociado a la información que se presenta.</param>
        private void Initialize(string WaitDialogWindowTitle, bool ContinousMode, int TimeToAutomaticClose, 
                                Orientation OrientationAnimation, string InitialAction, bool ShowCancelButton, 
                                MessageType TypeMessage) 
        {
            //  Actualiza la referencia al Thread padre del WaitDialog.
                ParentThread = Thread.CurrentThread;
            //  Crea la Estructura de Datos con la que se pasa la información de Inicio al Thread.
                Dictionary<string, object> dcWindowDataInput = new Dictionary<string, object>();
                dcWindowDataInput.Add("WaitDialogWindowTitle", WaitDialogWindowTitle);
                dcWindowDataInput.Add("ContinousMode", ContinousMode);
                dcWindowDataInput.Add("TimeToAutomaticClose", TimeToAutomaticClose);
                dcWindowDataInput.Add("OrientationAnimation", OrientationAnimation);
                dcWindowDataInput.Add("InitialAction", InitialAction);
                dcWindowDataInput.Add("ShowCancelButton", ShowCancelButton);
                dcWindowDataInput.Add("TypeMessage", TypeMessage);
            //  Crea el Thread que se encargará de mostrar la Ventana de Espera y de gestionar las operaciones con la misma.
                thdWaitDialog = new Thread(this.ThreadStartingPoint); 
                thdWaitDialog.SetApartmentState(ApartmentState.STA);
                thdWaitDialog.IsBackground = true;
                thdWaitDialog.Start(dcWindowDataInput);
            //  Se espera hasta que el Thread ha creado la Ventana de Espera.
                InitializationError = false;
                while ((wndWait == null) && (!InitializationError))
                {
                    Thread.Sleep(250);
                }
        }

        #endregion

        #region Métodos

        #region Gestores de la Ventana de Espera y de su Thread

        /// <summary>
        /// Método que gestiona la ejecución del Thread.
        /// </summary>
        /// <param name="DataInput">Datos con los que se debe ejecutar el Thread.</param>
        private void ThreadStartingPoint(object DataInput)
        {
            try
            {
                WindowThread = Thread.CurrentThread;
                Dictionary<string, object> dcWindowDataInput = (Dictionary<string, object>)DataInput;
                wndWait = new WaitDialogWindow(     (string) dcWindowDataInput["WaitDialogWindowTitle"],
                                                      (bool) dcWindowDataInput["ContinousMode"],
                                                      (int)  dcWindowDataInput["TimeToAutomaticClose"],
                                               (Orientation) dcWindowDataInput["OrientationAnimation"],
                                                    (string) dcWindowDataInput["InitialAction"],
                                                      (bool) dcWindowDataInput["ShowCancelButton"], 
                                               (MessageType) dcWindowDataInput["TypeMessage"]);
                wndWait.Closed += new EventHandler(OnWaitDialogWindowClosed);
                wndWait.Show();
                Dispatcher.Run();
            }
            catch (Exception ex)
            {
                //  Da por finalizado el fotograma de ejecución iniciado para la Ventana de Espera.
                    Dispatcher.FromThread(WindowThread).InvokeShutdown();
                //  Notifica la Excepción que se debe lanzar al proceso padre mediante un evento.
                    //Dispatcher.FromThread(ParentThread).BeginInvoke(new Action(delegate() { ThrowThreadException(ex); }));
                    Dispatcher.FromThread(ParentThread).BeginInvoke(new Action(delegate() { ThrowThreadException(MsgManager.ExcepMsg(ex)); }));
                //  Indica que se ha producido un error de inicialización, si es el caso, y que la espera ya no es
                //  necesaria, ya que nunca se inicializará correctamente la Ventana de Espera.
                    InitializationError = true;
            }
        }

        /// <summary>
        /// Método que Controla el Cierre de la Ventana de Espera.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el Evento.</param>
        private void OnWaitDialogWindowClosed(object sender, EventArgs e)
        {
            //  Lee el estado final de la Ventana de Espera al Cerrarse.
                DialogState wndState = wndWait.State;
            //  Da por finalizado el fotograma de ejecución iniciado para la Ventana de Espera.
                Dispatcher.FromThread(WindowThread).InvokeShutdown();
            //  Usa el Dispatcher del Thread padre del WaitdDialog para lanzar el evento correspondiente al Cierre  de  la 
            //  Ventana de Espera sin que este le cree problemas de Subprocesos con los elementos visuales de dicho Thread.
                Dispatcher.FromThread(ParentThread).BeginInvoke(new Action(delegate() { ThrowEvent(wndState); }));
        }

        /// <summary>
        /// Método que en función del estado de cierre de la Ventana de Espera lanzará un evento indicandoel motivo del cierre.
        /// </summary>
        /// <param name="wndState">Estado mediante el que se determinará el evento que se ha de notificar.</param>
        private void ThrowEvent(DialogState wndState)
        {
            //  Lanza el evento que indicará la usuario que se ha cerrado la Ventana de Espera y el Porque del Cierre.
                switch (wndState)
                {
                    case DialogState.ActionCompleted:
                         if (evActionCompleted != null) evActionCompleted();
                         break;
                    case DialogState.CancelPressed:
                         if (evCancelPressed != null) evCancelPressed();
                         break;
                }
        }

        #endregion

        #region Métodos públicos expuestos por la clase para la gestión de la ventana de espera por parte del Usuario

        /// <summary>
        /// Método que permite modificar el modo de funcionamiento de la ProgressBar de la Ventana de Espera.
        /// 
        ///                tiempo de espera <= 0 :-> No tiene en cuenta el cierre retardado
        ///                tiempo de espera > 0  :-> Cierra la ventana de espera en el tiempo en segundos expresado
        ///                                          por el valor pasado cómo parámetro.
        /// 
        /// </summary>
        /// <param name="TimeToAutomaticClose">Tiempo de Cierre automático de la Ventana de Espera.</param>
        public void SelectMode(int TimeToAutomaticClose)
        {
            Dictionary<string, object> Data = new Dictionary<string, object>();
            Data.Add("TimeToAutomaticClose", TimeToAutomaticClose);
            wndWait.Dispatcher.Invoke(new dlgExecuteAction(ExecuteAction), DispatcherPriority.Normal,
                                      new object[] { ActionType.SelectMode, Data });
        }

        /// <summary>
        /// Método que permite modificar el modo de avance de la ProgressBar de la Ventana de Espera.
        /// 
        ///                              true  :-> Continuo
        ///                              false :-> Por Pasos
        /// 
        /// </summary>
        /// <param name="ContinousMode">Modo de avance.</param>
        public void SelectMode(bool ContinousMode)
        {
            Dictionary<string, object> Data = new Dictionary<string,object>();
            Data.Add("ContinousMode", ContinousMode);
            wndWait.Dispatcher.Invoke(new dlgExecuteAction(ExecuteAction), DispatcherPriority.Normal, 
                                      new object[] {  ActionType.SelectMode, Data });
        }

        /// <summary>
        /// Método que permite modificar la orientación de la animación de avance de la ProgressBar de la Ventana de Espera.
        /// 
        ///          Horizontal  :-> La animación de avance recorre la ProgressBar de izquierda a derecha.
        ///          Vertical    :-> La animación de avance recorre la ProgressBar de abajo hacia arriba.
        /// 
        /// </summary>
        /// <param name="OrientationAnimation">Tipo de orientación</param>
        public void ChangeOrientationAnimation(Orientation OrientationAnimation)
        {
            Dictionary<string, object> Data = new Dictionary<string, object>();
            Data.Add("OrientationAnimation", OrientationAnimation);
            wndWait.Dispatcher.Invoke(new dlgExecuteAction(ExecuteAction), DispatcherPriority.Normal,
                                      new object[] { ActionType.OrientationAnimation, Data });
        }

        /// <summary>
        /// Método que, si la ProgressBar está en modo 'Por Pasos', añade el número de pasos indicado por la variable pasada
        /// como parámetro a la ProgressBar.
        /// </summary>
        /// <param name="NumSteps">Número de pasos que se desea añadir a la animación de avance de la ProgressBar.</param>
        public void AddStep(int NumSteps = 1)
        {
            Dictionary<string, object> Data = new Dictionary<string, object>();
            Data.Add("NumSteps", NumSteps);
            wndWait.Dispatcher.Invoke(new dlgExecuteAction(ExecuteAction), DispatcherPriority.Normal,
                                      new object[] { ActionType.AddStep, Data });
        }

        /// <summary>
        /// Método que, si la ProgressBar está en modo 'Por Pasos', añade el número de pasos indicado por la variable pasada
        /// como parámetro a la ProgressBar.
        /// </summary>
        /// <param name="NumSteps">Número de pasos que se desea añadir a la animación de avance de la ProgressBar.</param>
        /// <param name="ActionDescription">Descripción de la Acción que se está realizando.</param>
        public void AddStep(string ActionDescription,int NumSteps = 1)
        {
            Dictionary<string, object> Data = new Dictionary<string, object>();
            Data.Add("NumSteps", NumSteps);
            Data.Add("ActionDescription", ActionDescription);
            wndWait.Dispatcher.Invoke(new dlgExecuteAction(ExecuteAction), DispatcherPriority.Normal,
                                      new object[] { ActionType.AddStep, Data });
        }

        /// <summary>
        /// Método que asigna el Texto de la Acción asociada a la posción actual de la ProgressBar a la Ventana de Espera.
        /// </summary>
        /// <param name="ActionDescription">Descripción de la Acción que se está realizando.</param>
        public void SetActionDescription(string ActionDescription)
        {
            Dictionary<string, object> Data = new Dictionary<string, object>();
            Data.Add("ActionDescription", ActionDescription);
            wndWait.Dispatcher.Invoke(new dlgExecuteAction(ExecuteAction), DispatcherPriority.Normal,
                                      new object[] { ActionType.SetActionDescription, Data });
        }

        /// <summary>
        /// Método que, si la ProgressBar está en modo 'Continuo', cierra la Ventana de Espera.
        /// </summary>
        public void CloseDialog()
        {
            wndWait.Dispatcher.Invoke(new dlgExecuteAction(ExecuteAction), DispatcherPriority.Normal,
                                      new object[] { ActionType.CloseDialog, null });
        }

        /// <summary>
        /// Define la firma del método que gestiona una acción sobre la Ventana de Espera.
        /// </summary>
        /// <param name="Type">Tipo de Acción.</param>
        /// <param name="Data">Datos asociados a la acción a realizar.</param>
        private void ExecuteAction(ActionType Type, Dictionary<string, object> Data)
        {
            try
            {
                //  Declaración de Variables.
                    string sMsgError;
                
                //  Determina la acción que se desea ejecutar y actua en consecuéncia.
                    switch (Type)
                    {
                        case ActionType.SelectMode:
                             if (Data.ContainsKey("ContinousMode")) wndWait.ContinousMode = (bool)Data["ContinousMode"];
                             if (Data.ContainsKey("TimeToAutomaticClose")) wndWait.TimeToAutomaticClose = (int)Data["TimeToAutomaticClose"];
                             break;
                        case ActionType.OrientationAnimation:
                             wndWait.OrientationAnimation = (Orientation)Data["OrientationAnimation"];
                             break;
                        case ActionType.AddStep:
                             if (wndWait.AddStep((int)Data["NumSteps"], out sMsgError))
                             {
                                 if (Data.Count == 2) wndWait.SetActionDescription = (string)Data["ActionDescription"];
                             }
                             else Dispatcher.FromThread(ParentThread).BeginInvoke(new Action(delegate() { ThrowThreadException(sMsgError); }));
                             break;
                        case ActionType.SetActionDescription:
                             wndWait.SetActionDescription = (string)Data["ActionDescription"];
                             break;
                        case ActionType.CloseDialog:
                             if (!wndWait.CloseDialog(out sMsgError))
                             {
                                 Dispatcher.FromThread(ParentThread).BeginInvoke(new Action(delegate() { ThrowThreadException(sMsgError); }));
                             }
                             break;
                    }
            }
            catch (Exception ex)
            {
                //  Notifica la Excepción que se debe lanzar al proceso padre mediante un evento.
                    Dispatcher.FromThread(ParentThread).BeginInvoke(new Action(delegate() { ThrowThreadException(MsgManager.ExcepMsg(ex)); }));
            }
        }

        /// <summary>
        /// Método que gestiona la propagación de los Mensajes de Error del Thread.
        /// </summary>
        /// <param name="sMsgError">Mensaje de Error, que se ha producido en el Thread y que se debe propagar.</param>
        private void ThrowThreadException(string sMsgError)
        {
            //  Lanza el evento que notifica la excepción que se ha producido para que la capture el proceso padre.
                if (evThrowException != null)
                {
                    evThrowException(sMsgError);
                }
        }

        #endregion

        #endregion
    }
}
