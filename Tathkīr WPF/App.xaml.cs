using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Helpers;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data");

        private const string CultureAr = "ar-SA";
        private const string CultureEn = "en-US";

        public static bool IsRtl;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            // Initialize the application
            InitializeComponent();
        }
        /// <summary>
        /// Handles the startup event of the application.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            SettingsService.Load();

            // Set the culture based on the saved language setting or default to English
            if (string.IsNullOrEmpty(SettingsService.AppSettings.AppConfig.Language))
                SetCulture(CultureEn);
            else
                SetCulture(SettingsService.AppSettings.AppConfig.Language == "English" ? CultureEn : CultureAr);

            if (SettingsService.AppSettings.AppConfig.Theme == "Light")
                ModifyTheme(false);
            else
                ModifyTheme(true);

            SettingsService.Load();

            base.OnStartup(e);

            if (SettingsService.AppSettings.ApiConfig.LastUpdated < new DateTime(2025, 1, 1))
            {
                var language = CultureAr.Split('-')[0]; // "ar"
                var config = await HostService.Instance.GetAddress(language);

                SettingsService.AppSettings.ApiConfig.Country = config.CountryLocalized;
                SettingsService.AppSettings.ApiConfig.CountryCode = config.CountryCode;
                SettingsService.AppSettings.ApiConfig.City = config.CityLocalized;
                SettingsService.AppSettings.ApiConfig.Address = $"{config.City}, {config.CountryCode}";
                SettingsService.AppSettings.ApiConfig.LastUpdated = DateTime.Now;

                SettingsService.AppSettings.AppConfig.Language = Strings.English;
                SettingsService.AppSettings.AppConfig.Theme = Strings.Light;

                SettingsService.Save(SettingsService.AppSettings);
            }

            await PrayerTimesManager.Instance.LoadPrayerTimesAsync();

            ToastHelper.Initialize();

            // open the main window 
            var mainWindow = new MainWindow();


            // Set FlowDirection and then show
            mainWindow.FlowDirection =
                    IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            mainWindow.Show();

            //// Open TathkirWidget window 
            //var widget = new TathkirWidget();
            //widget.FlowDirection =
            //        Culture.StartsWith("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            //widget.Show();

            // if json file already exists, delete it
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Audios.json");
        
            if (!File.Exists(jsonPath))
                HandleAudios();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Save application settings on exit
            SettingsService.Save(SettingsService.AppSettings);
        }

        public static void SetCulture(string culture)
        {
            var ci = new CultureInfo(culture);

            // Force English numerals (Western digits)
            ci.NumberFormat.NativeDigits = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            ci.NumberFormat.DigitSubstitution = DigitShapes.None;
            ci.NumberFormat.NumberDecimalSeparator = ".";
            ci.NumberFormat.NumberGroupSeparator = ",";

            // Set the calendar to Gregorian
            ci.DateTimeFormat.Calendar = new GregorianCalendar();

            Strings.Culture = ci;

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;

            IsRtl = culture.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
        }

        private static void HandleAudios()
        {
            string resourcesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string audiosFolder = Path.Combine(resourcesFolder, "Audios");

            if (Directory.Exists(audiosFolder))
            {
                string[] files = Directory.GetFiles(audiosFolder, "*.wav");
                Audios audios = new Audios();

                var prayerNames = new[] { "fajr", "dhuhr", "asr", "maghrib", "isha", "jumua" };

                foreach (var name in prayerNames)
                {
                    var before = files.FirstOrDefault(f =>
                        Path.GetFileName(f).StartsWith(name, StringComparison.OrdinalIgnoreCase) &&
                        f.Contains("-before"));

                    var on = files.FirstOrDefault(f =>
                        Path.GetFileName(f).StartsWith(name, StringComparison.OrdinalIgnoreCase) &&
                        f.Contains("-on"));

                    audios.PrayAudios.Add(new PrayAudio
                    {
                        Name = name,
                        // only the file names are saved
                        BeforeAthanPath = before != null ? Path.GetFileName(before) : string.Empty,
                        OnAthanPath = on != null ? Path.GetFileName(on) : string.Empty
                    });
                }

                // Add remaining files as ThikrAudio (exclude prayer audio files)
                foreach (var file in files)
                {
                    string filename = Path.GetFileName(file);

                    bool isPrayerFile = prayerNames.Any(p =>
                        filename.StartsWith(p, StringComparison.OrdinalIgnoreCase) &&
                        (filename.Contains("-before") || filename.Contains("-on")));

                    if (!isPrayerFile)
                    {
                        audios.ThikrAudios.Add(new ThikrAudio
                        {
                            Name = Path.GetFileNameWithoutExtension(file),
                            Path = filename // store only file name
                        });
                    }
                }

                // Save JSON
                string jsonPath = Path.Combine(resourcesFolder, "Audios.json");
                string json = JsonSerializer.Serialize(audios, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, json);
            }

        }

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }
    }
}
