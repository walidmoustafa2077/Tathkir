using System.Windows;
using System.Windows.Controls;

namespace Tathkīr_WPF.Views.SettingsViews
{
    /// <summary>
    /// Interaction logic for LocationSettingsControl.xaml
    /// </summary>
    public partial class LocationSettingsControl : UserControl
    {
        public LocationSettingsControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.Settings.LocationSettingsViewModel();
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && !comboBox.IsDropDownOpen)
            {
                comboBox.IsDropDownOpen = true;
            }
        }

    }
}
