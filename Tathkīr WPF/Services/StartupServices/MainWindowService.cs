using System.Windows;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class MainWindowService : IMainWindowService
    {
        public void Show(bool isRtl)
        {
            var window = new MainWindow
            {
                FlowDirection = isRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight
            };
            window.Show();
        }
    }

}
