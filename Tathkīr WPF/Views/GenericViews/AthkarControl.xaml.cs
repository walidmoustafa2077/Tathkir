using System.Windows.Controls;
using System.Windows.Input;

namespace Tathkīr_WPF.Views.GenericViews
{
    /// <summary>
    /// Interaction logic for AthanControl.xaml
    /// </summary>
    public partial class AthkarControl : UserControl
    {
        public AthkarControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.Generic.AthkarViewModel();
        }

        private void AthkarListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = (ListView)sender;

            listView.SelectedItem = null;

        }

    }
}
