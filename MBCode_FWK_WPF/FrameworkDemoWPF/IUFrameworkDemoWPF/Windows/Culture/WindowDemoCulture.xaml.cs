#region Librerias usada por la clase

using MBCode.Framework.Managers.Culture;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MBCode.FrameworkDemoWFP.InterfazUsuario
{
    /// <summary>
    /// Lógica de interacción para WindowDemoCulture.xaml
    /// </summary>
    public partial class WindowDemoCulture : Window
    {
        public List<ChildWindowDemoCulture> lstFormsChilds;

        public WindowDemoCulture()
        {
            InitializeComponent();            
            this.Loaded += new RoutedEventHandler(OnWindowLoaded);
            this.Closing += new CancelEventHandler(OnWindowClosing);
        }

        private void WindowDemoCulture_Closing(object sender, CancelEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            lstFormsChilds = new List<ChildWindowDemoCulture>();
            this.miEnglish.Click += new RoutedEventHandler(OnChangeCultureClick);
            this.miFrench.Click += new RoutedEventHandler(OnChangeCultureClick);
            this.miSpanish.Click += new RoutedEventHandler(OnChangeCultureClick);
            this.miCatalan.Click += new RoutedEventHandler(OnChangeCultureClick);
            this.btnOpenChild.Click += new RoutedEventHandler(OnOpenChild_Click);
            this.btnDeactivateMenu.Click += btnDeactivateMenu_Click;
            this.miBureauBlack.Click += new RoutedEventHandler(onChangeThemeClick);
            this.miBureauBlue.Click += new RoutedEventHandler(onChangeThemeClick);
            this.miExpressionDark.Click += new RoutedEventHandler(onChangeThemeClick);
            this.miExpressionLight.Click += new RoutedEventHandler(onChangeThemeClick);
            this.miShinyBlue.Click += new RoutedEventHandler(onChangeThemeClick);
            this.miShinyRed.Click += new RoutedEventHandler(onChangeThemeClick);
            this.miWhistlerBlue.Click += new RoutedEventHandler(onChangeThemeClick);
            this.miHispania_Vermell.Click += new RoutedEventHandler(onChangeAppThemeClick);
            this.miHispania_Blau.Click += new RoutedEventHandler(onChangeAppThemeClick);
            this.miMBCode.Click += new RoutedEventHandler(onChangeAppThemeClick);
            this.miTemp.Click += new RoutedEventHandler(onChangeAppThemeClick);
            CultureManager.ActualCultureName = Cultures.Catalan_ES;
            UpdateCultureCheck();
            rbTest.IsChecked = true;
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Window[] aoFrmFormChild = lstFormsChilds.ToArray();
            for (int i = 0; i < aoFrmFormChild.Length; i++)
            {
                aoFrmFormChild[i].Close();
            }
            lstFormsChilds.Clear();
            this.miEnglish.Click -= new RoutedEventHandler(OnChangeCultureClick);
            this.miFrench.Click -= new RoutedEventHandler(OnChangeCultureClick);
            this.miSpanish.Click -= new RoutedEventHandler(OnChangeCultureClick);
            this.miCatalan.Click -= new RoutedEventHandler(OnChangeCultureClick);
            this.btnOpenChild.Click -= new RoutedEventHandler(OnOpenChild_Click);
            this.btnDeactivateMenu.Click -= btnDeactivateMenu_Click;
            this.miBureauBlack.Click -= new RoutedEventHandler(onChangeThemeClick);
            this.miBureauBlue.Click -= new RoutedEventHandler(onChangeThemeClick);
            this.miExpressionDark.Click -= new RoutedEventHandler(onChangeThemeClick);
            this.miExpressionLight.Click -= new RoutedEventHandler(onChangeThemeClick);
            this.miShinyBlue.Click -= new RoutedEventHandler(onChangeThemeClick);
            this.miShinyRed.Click -= new RoutedEventHandler(onChangeThemeClick);
            this.miWhistlerBlue.Click -= new RoutedEventHandler(onChangeThemeClick);
            this.miHispania_Vermell.Click += new RoutedEventHandler(onChangeAppThemeClick);
            this.miHispania_Blau.Click += new RoutedEventHandler(onChangeAppThemeClick);
            this.miMBCode.Click -= new RoutedEventHandler(onChangeAppThemeClick);
            this.miTemp.Click -= new RoutedEventHandler(onChangeAppThemeClick);
        }

        private void OnOpenChild_Click(object sender, System.EventArgs e)
        {
            ChildWindowDemoCulture frmChild = new ChildWindowDemoCulture();
            frmChild.Show();
            frmChild.Closed += new EventHandler(OnChildWindowClosed);
            lstFormsChilds.Add(frmChild);
        }

        private void OnChildWindowClosed(object sender, EventArgs e)
        {
            lstFormsChilds.Remove((ChildWindowDemoCulture)sender);
        }

        private void btnDeactivateMenu_Click(object sender, RoutedEventArgs e)
        {
            menu1.IsEnabled = !menu1.IsEnabled;
        }

        private void OnChangeCultureClick(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem) sender).Name)
            {
                case "miEnglish":
                     CultureManager.ActualCultureName = Cultures.English_GB;
                     break;
                case "miSpanish":
                     CultureManager.ActualCultureName = Cultures.Spanish_ES;
                     break;
                case "miFrench":
                     CultureManager.ActualCultureName = Cultures.French_FR;
                     break;
                case "miCatalan":
                     CultureManager.ActualCultureName = Cultures.Catalan_ES;
                     break;
            }
            UpdateCultureCheck();
        }

        private void onChangeThemeClick(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem) sender).Name)
            {
                case "miBureauBlack":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.BureauBlack);
                     break;
                case "miBureauBlue":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.BureauBlue);
                     break;
                case "miExpressionDark":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.ExpressionDark);
                     break;
                case "miExpressionLight":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.ExpressionLight);
                     break;
                case "miShinyBlue":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.ShinyBlue);
                     break;
                case "miShinyRed":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.ShinyRed);
                     break;
                case "miWhistlerBlue":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.WhistlerBlue);
                     break;
                case "miMBCode":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.MBCode);
                     break;
            }
        }

        private void onChangeAppThemeClick(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem) sender).Name)
            {
                case "miHispania_Vermell":
                     ThemeManager.ActualTheme = new ThemeInfo(@"MBCode.Framework.Controls.WPF", 
                                                              @"component/Recursos/Themes/", "Hispania_Vermell");
                     break;
                case "miHispania_Blau":
                     ThemeManager.ActualTheme = new ThemeInfo(@"MBCode.Framework.Controls.WPF", 
                                                              @"component/Recursos/Themes/", "Hispania_Blau");
                     break;
                case "miMBCode":
                     ThemeManager.ActualTheme = new ThemeInfo(Themes.MBCode);
                     break;
                case "miTemp":
                     ThemeManager.ActualTheme = new ThemeInfo(@"MBCode.Framework.Controls.WPF",
                                                              @"component/Recursos/Themes/", "Temp");
                     break;
            }
        }

        private void UpdateCultureCheck()
        { 
            this.miEnglish.IsChecked = (CultureManager.ActualCultureName == "en");
            this.miFrench.IsChecked = (CultureManager.ActualCultureName == "fr");
            this.miSpanish.IsChecked = (CultureManager.ActualCultureName == "es");
            this.miCatalan.IsChecked = (CultureManager.ActualCultureName == "ca");
        }
    }
}
