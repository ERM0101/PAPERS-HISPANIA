#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#endregion

namespace MBCode.FrameworkDemoWFP.InterfazUsuario
{
    public class ListData
    {
        public string Value_Column_1 { get; set; }
        public DateTime Value_Column_2 { get; set; }
        public string Value_Column_3 { get; set; }
        public string Value_Column_4 { get; set; }

        public ListData(string Value_Column_1, DateTime Value_Column_2, string Value_Column_3, string Value_Column_4)
        {
            this.Value_Column_1 = Value_Column_1;
            this.Value_Column_2 = Value_Column_2;
            this.Value_Column_3 = Value_Column_3;
            this.Value_Column_4 = Value_Column_4;
        }
    }

    public class ComboBoxData
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public ComboBoxData(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }

    /// <summary>
    /// Lógica de interacción para CWDemoCulture.xaml
    /// </summary>
    public partial class ChildWindowDemoCulture : Window
    {
        private ObservableCollection<ListData> m_DataList = new ObservableCollection<ListData>()
        {
            new ListData("Item 1 value 1", DateTime.Now, "Item 1 value 3", "Item 1 value 4"),
            new ListData("Item 2 value 1", DateTime.Now, "Item 2 value 3", "Item 2 value 4"),
            new ListData("Item 3 value 1", DateTime.Now, "Item 3 value 3", "Item 3 value 4")
        };

        public ObservableCollection<ListData> DataList
        {
            get
            {
                return m_DataList;
            }
        }

        private Dictionary<string, string> m_DataCombo = new Dictionary<string, string>();

        public ChildWindowDemoCulture()
        {
            InitializeComponent();
            btnChange.Click += btnChange_Click;
            btnTestProgressBar.Click += btnTestProgressBar_Click;
            chkbEnabled.Checked += chkbEnabled_Checked;
            chkbEnabled.Unchecked += chkbEnabled_Unchecked;

            ListItems.DataContext = this;
            ListBoxItems.DataContext = this;

            m_DataCombo.Add("Key 1", "Value 1");
            m_DataCombo.Add("Key 2", "Value 2");
            m_DataCombo.Add("Key 3", "Value 3");
            m_DataCombo.Add("Key 4", "Value 4");
            cbAgentPostalCode.ItemsSource = m_DataCombo;
            cbAgentPostalCode.DisplayMemberPath = "Key";
            cbAgentPostalCode.SelectedValuePath = "Value";
            cbComboToolBar.ItemsSource = m_DataCombo;
            cbComboToolBar.DisplayMemberPath = "Key";
            cbComboToolBar.SelectedValuePath = "Value";
        }

        private void btnTestProgressBar_Click(object sender, RoutedEventArgs e)
        {
            Duration duration = new Duration(TimeSpan.FromSeconds(3));
            DoubleAnimation doubleanimation = new DoubleAnimation(100.0, duration);
            doubleanimation.FillBehavior = FillBehavior.Stop;
            pbTest.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = new TreeViewItem() { Header = FindResource("WPFTN_ChildNode") + " " +
                                                    ((TreeViewItem)(tvData.Items[0])).Items.Count.ToString()};
            ((TreeViewItem)(tvData.Items[0])).Items.Add(tvi);
            tvi.IsExpanded = true;
        }

        private void chkbEnabled_Checked(object sender, RoutedEventArgs e)
        {
            this.grdData1.IsEnabled = true;
            this.grdData2.IsEnabled = true;
        }

        private void chkbEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            this.grdData1.IsEnabled = false;
            this.grdData2.IsEnabled = false;
        }
    }
}
