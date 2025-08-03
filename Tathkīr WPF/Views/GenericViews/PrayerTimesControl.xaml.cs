using System.Windows.Controls;

namespace Tathkīr_WPF.Views.GenericViews
{
    /// <summary>
    /// Interaction logic for PrayerTimesControl.xaml
    /// </summary>
    public partial class PrayerTimesControl : UserControl
    {
        public PrayerTimesControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.Generic.PrayerTimesViewModel();
        }
    }
}
