using System.Windows;
using System.Windows.Input;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF
{
    /// <summary>
    /// Interaction logic for TathkirWidget.xaml
    /// </summary>
    public partial class TathkirWidget : Window
    {
        public TathkirWidget()
        {
            InitializeComponent();
            DataContext = new ViewModels.TathkirWidgetViewModel();

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Notify the user that the widget is ready
            ToastMaganer.ShowToast("Tathkir Widget", "Prayer times widget is now active.");

            // Set the window to be topmost
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Topmost = false;
            ShowInTaskbar = false;

            //Visibility = Visibility.Hidden;

            // Position at top-right corner with 20px margin
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            Left = screenWidth - Width - 20;
            Top = 20;

            // Hook into the Show Desktop event
            ShowOnDesktopService.AddHook(this);
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            ShowOnDesktopService.RemoveHook();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
