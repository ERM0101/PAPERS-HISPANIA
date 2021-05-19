#region Libraries used by the class

using System.Windows;
using System.Windows.Input;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows.Common
{    
    /// <summary>
    /// Class that allow the user to pass between two controls with the Enter key.
    /// </summary>
    public static class FocusAdvancement
    {
        #region Attributes

        public static readonly DependencyProperty AdvancesByEnterKeyProperty =
                                                          DependencyProperty.RegisterAttached("AdvancesByEnterKey", typeof(bool), typeof(FocusAdvancement),
                                                                                              new UIPropertyMetadata(OnAdvancesByEnterKeyPropertyChanged));

        #endregion

        #region Public Methods

        public static bool GetAdvancesByEnterKey(DependencyObject obj)
        {
            return (bool)obj.GetValue(AdvancesByEnterKeyProperty);
        }

        public static void SetAdvancesByEnterKey(DependencyObject obj, bool value)
        {
            obj.SetValue(AdvancesByEnterKeyProperty, value);
        }

        #endregion

        #region Private Methods

        static void OnAdvancesByEnterKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element == null) return;

            if ((bool)e.NewValue) element.KeyDown += Keydown;
            else element.KeyDown -= Keydown;
        }

        static void Keydown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter)) return;

            var element = sender as UIElement;
            if (element != null) element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        #endregion
    }
}
