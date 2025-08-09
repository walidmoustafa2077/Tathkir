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
    }
}
