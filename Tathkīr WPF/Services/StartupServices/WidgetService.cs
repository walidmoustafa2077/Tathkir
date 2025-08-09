using System.Windows;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class WidgetService : IWidgetService
    {
        public void Show()
        {
            var window = TathkirWidget.Instance;
            TathkirWidget.Instance.FlowDirection = CultureService.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            window.Show();
        }
    }

}
