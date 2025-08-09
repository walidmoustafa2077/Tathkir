using System.Windows.Forms;

namespace Tathkīr_WPF.Services.StartupServices.Interfaces
{
    public interface INotifyIconService
    {
        void Initialize();
        void Show();
        void Hide();
        void Dispose();
    }
}
