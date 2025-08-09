using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class StartupRegistrationService : IStartupRegistrationService
    {
        public void Register() => AppLifecycleManager.AddToStartup();
        public void Unregister() => AppLifecycleManager.RemoveFromStartup();
    }

}
