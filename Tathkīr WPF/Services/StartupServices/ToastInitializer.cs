using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class ToastInitializer : IToastInitializer
    {
        public void Initialize() => ToastMaganer.Initialize();
    }

}
