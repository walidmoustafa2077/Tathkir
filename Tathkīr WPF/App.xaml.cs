using System.IO;
using System.Windows;
using Tathkīr_WPF.Services.StartupServices;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

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
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _initializer?.Shutdown();
            base.OnExit(e);
        }
    }
}
