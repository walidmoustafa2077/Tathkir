using System.Windows;
using System.Windows.Input;

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
        Close();
    }
}