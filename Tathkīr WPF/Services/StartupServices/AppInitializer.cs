using Tathkīr_WPF.Helpers;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class AppInitializer : IAppInitializer
    {
        private readonly INotifyIconService _notifyIconService;
        private readonly IThemeManager _themeManager;
        private readonly ICultureService _cultureService;
        private readonly IStartupRegistrationService _startupService;
        private readonly IToastInitializer _toastInitializer;
        private readonly IPrayerTimesLoader _prayerLoader;
        private readonly IMainWindowService _mainWindowService;
        private readonly IWidgetService _widgetService;

        public AppInitializer()
        {
            _themeManager = new ThemeManager();
            _cultureService = new CultureService();
            _startupService = new StartupRegistrationService();
            _toastInitializer = new ToastInitializer();
            _prayerLoader = new PrayerTimesLoader();
            _mainWindowService = new MainWindowService();
            _widgetService = new WidgetService();
            _notifyIconService = new NotifyIconService(_mainWindowService);
        }

        public async Task InitializeAsync()
        {
            AppLifecycleManager.AlreadyRunning();

            SettingsService.PreLoad();
            DynamicStrings.LoadStrings();

            var settings = SettingsService.AppSettings;

            _cultureService.SetCulture(settings.AppConfig.Language);
            _themeManager.Apply(settings.AppConfig.Theme);

            if (settings.AppConfig.LaunchOnStartup)
                _startupService.Register();
            else
                _startupService.Unregister();

            if (settings.ApiConfig.LastUpdated < new DateTime(2025, 1, 1))
            {
                await UpdateApiConfigAsync(settings);
                SettingsService.Save(settings);
            }

            SettingsService.Load();

            _toastInitializer.Initialize();

            _notifyIconService.Initialize();

            await _prayerLoader.LoadAsync();

            if (settings.AppConfig.ShowMainWindow)
                _mainWindowService.Show();

            if (settings.AppConfig.ShowWidget)
                _widgetService.Show();
        }

        public void Shutdown()
        {
            SettingsService.Save(SettingsService.AppSettings);
        }

        private async Task UpdateApiConfigAsync(AppSettings settings)
        {
            var language = CultureService.CultureAr.Split('-')[0];
            var config = await HttpHostService.Instance.GetAddress(language);

            settings.ApiConfig.Country = DynamicStrings.AddString("Country", config.Country, config.CountryLocalized);
            settings.ApiConfig.City = DynamicStrings.AddString("City", config.City, config.CityLocalized);
            settings.ApiConfig.CountryCode = config.CountryCode;
            settings.ApiConfig.Address = $"{config.City}, {config.CountryCode}";
            settings.ApiConfig.LastUpdated = DateTime.Now;

            settings.AppConfig.Language = Strings.English;
            settings.AppConfig.Theme = Strings.Light;
        }
    }

}
