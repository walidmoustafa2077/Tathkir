using System.Windows.Controls;

namespace Tathkīr_WPF.Views.SettingsViews
{
    /// <summary>
    /// Interaction logic for AthanSettingsControl.xaml
    /// </summary>
    public partial class AthanSettingsControl : UserControl
    {
        public AthanSettingsControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.Settings.AthanSettingsViewModel();
        }
    }
}
