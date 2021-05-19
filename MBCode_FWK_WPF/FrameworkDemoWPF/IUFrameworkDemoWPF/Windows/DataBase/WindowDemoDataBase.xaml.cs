#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MBCode.FrameworkDemoWFP.InterfazUsuario
{
    #region Enumerados Públicos

    public enum QueriesGroups
    {
        //[InfoValue("Entity queries", typeof(EntityQueries))]
        //Entity,
    }

    public enum EntityQueries
    {
        //[InfoValue("Insertar tipo de Entidad", null)]
        //InsertTipoOfEntity,
    }

    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha del último cambio: 16/03/2012.
    /// Descripción: ventana de test de las consultas a Base de Datos.
    /// </summary>
    public partial class WindowDemoDataBase : Window
    {
        #region Enumerados

        private enum WindowInternalState
        {
            Undefined,
            Initialized,
            ConfiguringConnection,
            Connected,
            Closed,
        }

        #endregion

        #region Eventos

        public delegate void dlgTryToConnect(string sTypeBD, string sAuthenticationType, string sServer, string sDataBase, 
                                             string sUser, string sPassword);

        public event dlgTryToConnect evTryToConnect;

        public delegate void dlgExecuteQuery(QueriesGroups eGroupSelected, Enum eQuerySelected, Dictionary<string, object> oParams = null);

        public event dlgExecuteQuery evExecuteQuery;

        #endregion

        #region Atributos

        private WindowInternalState eState = WindowInternalState.Undefined;

        private Dictionary<Enum, Dictionary<string, Enum>> dcItemsGroups;

        #endregion

        #region Atributos

        private string sBDType;

        private string sAuthenticationType;

        #endregion

        #region Propiedades

        private WindowInternalState State
        {
            get { return eState; }
            set { ActualizeState(value); }
        }

        #endregion

        #region Constructores

        public WindowDemoDataBase()
        {
            InitializeComponent();
            State = WindowInternalState.Initialized;
        }

        #endregion

        #region Eventos de la ventana

        private void OnWindowDemoDataBaseLoaded(object sender, RoutedEventArgs e)
        {
            State = WindowInternalState.ConfiguringConnection;
        }

        private void OnWindowDemoDataBaseClosed(object sender, System.EventArgs e)
        {
            State = WindowInternalState.Closed;
        }

        #endregion

        #region Eventos de los controles de la ventana

        #region Open Connection

        private void OnbtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (evTryToConnect != null)
                {
                    evTryToConnect(sBDType, sAuthenticationType, tbServer.Text, tbDataBase.Text, tbUser.Text, tbPassword.Text);
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
            }
        }

        public void Connection_Accept()
        {
            State = WindowInternalState.Connected;
        }

        public void Connection_Fail(string sMessage)
        {
            MsgManager.ShowMessage(sMessage);
        }

        #endregion

        #region Close Conection
        
        private void OnbtnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                State = WindowInternalState.ConfiguringConnection;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
            }
        }

        private void OnDataBaseType_Checked(object sender, RoutedEventArgs e)
        {
            State = WindowInternalState.ConfiguringConnection;
        }

        #endregion

        #region Selection Groups of Queries

        private void OncbGroupQueries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizeListOfQueries();
        }
        
        private void ActualizeListOfQueries()
        {
            lvQueries.ItemsSource = GetTextFromEnumValues(GetTypeValue(((QueriesGroups)cbGroupQueries.SelectedValue)));
        }

        #endregion

        #region Execute Queries

        private void OnQueryExcecute(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvQueries.SelectedItem != null)
            {
                if (evExecuteQuery != null)
                {
                    QueriesGroups eGroupSelected = (QueriesGroups)cbGroupQueries.SelectedValue;
                    Enum eQuerySelected = dcItemsGroups[eGroupSelected][(string)lvQueries.SelectedValue];
                    evExecuteQuery(eGroupSelected, eQuerySelected);
                }
            }
        }

        public void QueryExecuted_OK(object oResult)
        {
            MsgManager.ShowMessage(MsgManager.LiteralMsg("Ejecución finalizada correctamente.", MsgType.Information), MsgType.Information);
            // State = WindowInternalState.QueryExecuted;
            // Definir en el enumerado y activar su estado.
        }

        public void QueryExecuted_Failed(string sMessage)
        {
            MsgManager.ShowMessage(sMessage);
        }

        #endregion

        #endregion

        #region Actualizador de Estado

        private void ActualizeState(WindowInternalState eNewState)
        {
            switch (eNewState)
            {
                case WindowInternalState.Initialized:
                     rbSQLServer.IsChecked = true;
                     rbSQLServerNative.IsChecked = true;
                     DefineEventManagers();
                     LoadDataConnection();
                     LoadDataInCombo();
                     LoadDictionaries();                     
                     break;
                case WindowInternalState.ConfiguringConnection:
                     ConnectionStateIU(false);
                     break;
                case WindowInternalState.Connected:
                     ConnectionStateIU(true);
                     break;
                case WindowInternalState.Closed:
                     UndefineEventManagers();
                     break;
                default:
                     throw new Exception("Error, estado no reconocido."); 
            }
            eState = eNewState;
        }

        #region Interfície Gráfica

        private void ConnectionStateIU(bool bConnected) // true, si conectado, false, si no conectado.
        {
            btnConnect.IsEnabled = !bConnected;
            btnDisconnect.IsEnabled = bConnected;
            gbDataBaseConnectionType.IsEnabled = !bConnected;
            gbDataBaseSecurityType.IsEnabled = !bConnected;
            gbDataBaseParameters.IsEnabled = !bConnected;
            gbQueries.Visibility = (bConnected? Visibility.Visible : Visibility.Hidden);
            //if (bConnected) cbGroupQueries.Text = GetTextValue(QueriesGroups.Entity); 
        }

        private void LoadDataConnection()
        {
            if (rbSQLServer.IsChecked == true)
            {
                //  Grafico
                    Visibility eVisibility = Visibility.Hidden;
                    tbServer.Text = @"SERVERNT2\SQLSRV_HISPANIA"; 
                    tbDataBase.Text = "OldHispaniaBD";
                    if (rbSQLServerNative.IsChecked == true)
                    {
                        tbUser.Text = "HispaniaBD";
                        tbPassword.Text = "Phispania2";
                        eVisibility = Visibility.Visible;
                    }
                    tbUser.Visibility = tbPassword.Visibility = lblUser.Visibility = lblPassowrd.Visibility = eVisibility;
                //  Lógico
                    if (rbSQLServerNative.IsChecked == true) sAuthenticationType = "SQL_SERVER_AUTHENTICATE";
                    else sAuthenticationType = "WINDOWS_AUTHENTICATE"; // rbWindowsIntegrate.IsChecked == true
                    sBDType = "SQLSERVER";
            }
            else if (rbORACLE.IsChecked == true) throw new Exception("Error, opción aún NO implementada.");
            else throw new Exception("Error, opción aún NO implementada."); // rbPROGRESS.IsChecked == true
        }

        private void LoadDataInCombo()
        {
            cbGroupQueries.ItemsSource = GetDictionaryFromEnum(typeof(QueriesGroups));
            cbGroupQueries.SelectedValuePath = "Value";
            cbGroupQueries.DisplayMemberPath = "Key";
        }

        #endregion

        #region Managers

        private void DefineEventManagers()
        {
            Loaded += new RoutedEventHandler(OnWindowDemoDataBaseLoaded);
            Closed += new System.EventHandler(OnWindowDemoDataBaseClosed);
            rbSQLServer.Checked += new RoutedEventHandler(OnDataBaseType_Checked);
            rbORACLE.Checked += new RoutedEventHandler(OnDataBaseType_Checked);
            rbPROGRESS.Checked += new RoutedEventHandler(OnDataBaseType_Checked);
            rbSQLServerNative.Checked += new RoutedEventHandler(OnDataBaseType_Checked);
            rbWindowsIntegrate.Checked += new RoutedEventHandler(OnDataBaseType_Checked);
            btnConnect.Click += new RoutedEventHandler(OnbtnConnect_Click);
            btnDisconnect.Click += new RoutedEventHandler(OnbtnDisconnect_Click);
            cbGroupQueries.SelectionChanged += new SelectionChangedEventHandler(OncbGroupQueries_SelectionChanged);
            lvQueries.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(OnQueryExcecute);
        }

        private void UndefineEventManagers()
        {
            Loaded -= new RoutedEventHandler(OnWindowDemoDataBaseLoaded);
            Closed -= new System.EventHandler(OnWindowDemoDataBaseClosed);
            rbSQLServer.Checked -= new RoutedEventHandler(OnDataBaseType_Checked);
            rbORACLE.Checked -= new RoutedEventHandler(OnDataBaseType_Checked);
            rbPROGRESS.Checked -= new RoutedEventHandler(OnDataBaseType_Checked);
            rbSQLServerNative.Checked -= new RoutedEventHandler(OnDataBaseType_Checked);
            rbWindowsIntegrate.Checked -= new RoutedEventHandler(OnDataBaseType_Checked);
            btnConnect.Click -= new RoutedEventHandler(OnbtnConnect_Click);
            btnDisconnect.Click -= new RoutedEventHandler(OnbtnDisconnect_Click);
            cbGroupQueries.SelectionChanged -= new SelectionChangedEventHandler(OncbGroupQueries_SelectionChanged);
            lvQueries.MouseDoubleClick -= new System.Windows.Input.MouseButtonEventHandler(OnQueryExcecute);
        }

        #endregion

        #region Dictionaries

        private void LoadDictionaries()
        {
            dcItemsGroups = new Dictionary<Enum, Dictionary<string, Enum>>();
            foreach (Enum eItem in Enum.GetValues(typeof(QueriesGroups)))
            {
                dcItemsGroups.Add(eItem, GetDictionaryFromEnum(GetTypeValue(eItem)));
            }
        }

        #endregion

        #endregion

        #region Auxiliares

        private string GetTextValue(Enum value)
        {
            Type type = value.GetType();
            return ((type.GetField(value.ToString()).GetCustomAttributes(typeof(InfoValue), false)[0] as InfoValue).Value);
        }

        private Type GetTypeValue(Enum value)
        {
            Type type = value.GetType();
            return ((type.GetField(value.ToString()).GetCustomAttributes(typeof(InfoValue), false)[0] as InfoValue).TypeOfEnum);
        }

        private Dictionary<string, Enum> GetDictionaryFromEnum(Type tEnumType)
        {
            Dictionary<string, Enum> dcGroups = new Dictionary<string, Enum>();
            foreach (Enum e in Enum.GetValues(tEnumType))
            {
                dcGroups.Add(GetTextValue(e), e);
            }
            return (dcGroups);
        }

        private List<string> GetTextFromEnumValues(Type tEnumType)
        {
            List<string> lstQueries = new List<string>(); 
            foreach (Enum e in Enum.GetValues(tEnumType))
            {
                lstQueries.Add(GetTextValue(e));
            }
            return (lstQueries);
        }

        #endregion
    }
}
