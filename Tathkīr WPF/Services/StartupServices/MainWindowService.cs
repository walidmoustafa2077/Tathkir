using System.Windows;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class MainWindowService : IMainWindowService
    {
        public void Show()
        {
            var window = new MainWindow
            {
                FlowDirection = CultureService.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight
            };
            window.Show();
        }
    }
}
