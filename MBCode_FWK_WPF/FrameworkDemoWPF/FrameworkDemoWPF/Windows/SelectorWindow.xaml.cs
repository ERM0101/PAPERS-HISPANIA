#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using MBCode.FrameworkDemoWFP.LogicaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MBCode.Framework.Demo.WPF
{
    /// <summary>
    /// Interaction logic for SelectorWindow.xaml
    /// </summary>
    public partial class SelectorWindow : Window
    {
        #region Builders

        /// <summary>
        /// Default buider of this Window.
        /// </summary>
        public SelectorWindow()
        {
            InitializeComponent();
            Initialize();
        }
        /// <summary>
        /// Methot that initializes this Window
        /// </summary>
        private void Initialize()
        {
            //  Load the data of every windows in the List of windows.
                LoadWindowDataInListBox();
        }

        /// <summary>
        /// Methot that load the data of every windows in the List of windows.
        /// </summary>
        private void LoadWindowDataInListBox()
        {
            //  Create the Data that contains the different kind of windows.
                Dictionary<string, DemoType> dcWindows = new Dictionary<string, DemoType>();
                dcWindows.Add("DataBase", DemoType.DataBaseDemo);
                dcWindows.Add("Culture", DemoType.CultureDemo);
                dcWindows.Add("Message Manager", DemoType.MsgManagerDemo);
                dcWindows.Add("XML", DemoType.XMLDemo);
            //  Load in the list of Windows the data.
                foreach (KeyValuePair<string, DemoType> kvp in dcWindows)
                {
                    ListBoxItem lbItem = new ListBoxItem();
                    lbItem.Content = kvp.Key;
                    lbItem.Tag = kvp.Value;
                    lbWindows.Items.Add(lbItem);
                }
            //  Define the managers of the events of the component.
                lbWindows.MouseDoubleClick += LbWindows_MouseDoubleClick;
        }

        #endregion

        #region Managers

        /// <summary>
        /// Manager for the selection of once of the Windows Types
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbWindows_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lbWindows.SelectedItem != null) ManageWindowSelection((DemoType)((ListBoxItem)lbWindows.SelectedItem).Tag);
        }

        /// <summary>
        /// Method that manage the selection of once type of windows.
        /// </summary>
        private void ManageWindowSelection(DemoType WindowType)
        {
            try
            {
                //  Create parameters for the process of load of the window.
                    Hashtable Params = new Hashtable();
                //  Load the Window.
                    Manager_IU.LoadMainWindow(WindowType, Params);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, al cargar el formulario de demostración, {" + MsgManager.ExcepMsg(ex) + "}.");
                Environment.Exit(-1);
            }
        }

        #endregion
    }
}
