using System.Windows.Controls;

namespace Tathkīr_WPF.Views.SettingsViews
{
    /// <summary>
    /// Interaction logic for AthkarSettingsControl.xaml
    /// </summary>
    public partial class AthkarSettingsControl : UserControl
    {
        public AthkarSettingsControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.Settings.AthkarSettingsViewModel();
        }
    }
}
