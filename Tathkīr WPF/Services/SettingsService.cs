using System.IO;
using System.Text.Json;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.Services
{
    public static class SettingsService
    {
        public static AppSettings AppSettings { get; private set; } = new();

        private static readonly string SettingsPath =
            Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        public static void Load()
        {
            if (File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
                AppSettings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }            
        }

        public static void Save(AppSettings settings)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(SettingsPath, json);
        }

    }
}
