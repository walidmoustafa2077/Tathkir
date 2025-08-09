using System.Windows;
using System.Windows.Input;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Dialogs;
using Tathkīr_WPF.Views.Dialogs;

namespace Tathkīr_WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = ViewModels.MainWindowViewModel.Instance;

    }

    // Allow window dragging when clicking anywhere
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

    // Close button click
    private void Close_Click(object sender, RoutedEventArgs e)
    {
        // show confirmation dialog to close the application or close the window
        var vm = new BaseDialogViewModel
        {
            Title = Strings.Close_Application,
            Message = Strings.Are_You_Sure_You_Want_To_Close_The_Application,
            PrimaryButton = Strings.Yes,
            SecondaryButton = Strings.Close_Main_Window,
            PrimaryButtonAction = Application.Current.Shutdown,
            SecondaryButtonAction = () =>
            {
                Close();
                DialogService.Instance.CloseDialog();
            },
        };

        var dialog = new BaseDialogControl
        {
            DataContext = vm
        };

        DialogService.Instance.ShowDialogControl(dialog);
    }
}