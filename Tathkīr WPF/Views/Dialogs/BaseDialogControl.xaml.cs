using System.Windows.Controls;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for BaseDialogControl.xaml
    /// </summary>
    public partial class BaseDialogControl : UserControl
    {
        public BaseDialogControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogService.Instance.CloseDialog();
        }
    }
}
