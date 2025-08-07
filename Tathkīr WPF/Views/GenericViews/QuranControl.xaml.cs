using System.Windows.Controls;
using System.Windows.Input;

namespace Tathkīr_WPF.Views.GenericViews
{
    /// <summary>
    /// Interaction logic for QuranControl.xaml
    /// </summary>
    public partial class QuranControl : UserControl
    {
        public QuranControl()
        {
            InitializeComponent();
            var vm = new ViewModels.Generic.QuranViewModel();
            DataContext = vm;

            // Load data async after UI is ready
            Loaded += async (s, e) =>
            {
                await vm.LoadDataAsync();
            };
        }

        private void AthkarListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = (ListView)sender;

            listView.SelectedItem = null;
        }
    }
}
