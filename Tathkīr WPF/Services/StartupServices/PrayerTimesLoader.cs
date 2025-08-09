using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class PrayerTimesLoader : IPrayerTimesLoader
    {
        public Task LoadAsync() => PrayerTimesManager.Instance.LoadPrayerTimesAsync();
    }

}
