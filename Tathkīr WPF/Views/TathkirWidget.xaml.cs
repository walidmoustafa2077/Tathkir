using System.Windows;
using System.Windows.Input;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF
{
    /// <summary>
    /// Interaction logic for TathkirWidget.xaml
    /// </summary>
    public partial class TathkirWidget : Window
    {
        private static TathkirWidget? _instance;
        public static TathkirWidget Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TathkirWidget();
                return _instance;
            }

        }

        public TathkirWidget()
        {
            InitializeComponent();
            DataContext = new ViewModels.TathkirWidgetViewModel();

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeWidget();
        }

        public void InitializeWidget()
        {
            // Set the window to be topmost
            ResizeMode = ResizeMode.NoResize;
            Topmost = false;
            ShowInTaskbar = false;

            // Position at top-right corner with 20px margin
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            Left = screenWidth - Width - 20;
            Top = 20;

            // Hook into the Show Desktop event
            ShowOnDesktopService.AddHook(this);
        }

        public void MainWindow_Closed(object? sender, EventArgs e)
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
