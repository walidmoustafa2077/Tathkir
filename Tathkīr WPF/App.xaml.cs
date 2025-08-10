using System.Diagnostics;
using System.IO;
using System.Windows;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.Services.StartupServices;
using Tathkīr_WPF.Services.StartupServices.Interfaces;
using Tathkīr_WPF.ViewModels.Dialogs;

namespace Tathkīr_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data");

        private readonly IAppInitializer _initializer = new AppInitializer();

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _initializer.InitializeAsync();

            base.OnStartup(e);

            _ = CheckForUpdatesSilently(); 
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _initializer?.Shutdown();
            base.OnExit(e);
        }

        private async Task CheckForUpdatesSilently()
        {
            try
            {
                var checker = new GitHubReleaseManager("walidmoustafa2077", "Tathkir");
                var current = GitHubReleaseManager.GetCurrentAppVersion();
                var result = await checker.CheckForUpdateAsync(current);

                if (!result.IsUpdateAvailable)
                    return;


                var vm = new BaseDialogViewModel
                {
                    Title = "Update Available",
                    Message = $"    {Strings.A_New_Version_Is_Available}: {result.Tag} ({Strings.Current}: {current}).   \n     {Strings.Open_The_Releases_Page}    ",
                    PrimaryButton = Strings.Open_The_Releases_Page,
                    SecondaryButton = Strings.Later,
                    PrimaryButtonAction = () =>
                    {
                        if (!string.IsNullOrWhiteSpace(result.ReleaseUrl))
                            Process.Start(new ProcessStartInfo(result.ReleaseUrl) { UseShellExecute = true });
                        AppLifecycleManager.RemoveFromStartup();
                        DialogService.Instance.CloseDialog();
                    },
                    SecondaryButtonAction = DialogService.Instance.CloseDialog,
                };

                var view = new Views.Dialogs.BaseDialogControl
                {
                    DataContext = vm,
                };

                DialogService.Instance.ShowDialogControl(view);

            }
            catch
            {
                // Swallow errors – you don’t want update checks to break startup.
            }
        }
    }
}
