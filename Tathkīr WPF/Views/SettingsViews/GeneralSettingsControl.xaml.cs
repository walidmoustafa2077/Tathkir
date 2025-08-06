using System.Windows.Controls;

namespace Tathkīr_WPF.Views.SettingsViews
{
    /// <summary>
    /// Interaction logic for GeneralSettingsControl.xaml
    /// </summary>
    public partial class GeneralSettingsControl : UserControl
    {
        public GeneralSettingsControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.Settings.GeneralSettingsViewModel();
        }
    }
}
