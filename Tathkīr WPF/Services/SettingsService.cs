using System.IO;
using System.Text.Json;
using Tathkīr_WPF.Helpers;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.Services
{
    public static class SettingsService
    {
        public static AppSettings AppSettings { get; private set; } = new();

        private static readonly string SettingsPath =
            Path.Combine(AppContext.BaseDirectory, "appsettings.dat");

        public static void PreLoad()
        {
            if (!File.Exists(SettingsPath))
            {
                AppSettings = new AppSettings();
                return;
            }

            var encryptedBytes = File.ReadAllBytes(SettingsPath);
            var json = EncryptionHelper.Decrypt(encryptedBytes);

            AppSettings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }

        public static void Load()
        {
            AppSettings.AppConfig.Onload();
        }

        public static void Save(AppSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            settings.AppConfig.OnSave();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(settings, options);
            var encryptedBytes = EncryptionHelper.Encrypt(json);
            File.WriteAllBytes(SettingsPath, encryptedBytes);
        }

    }
}
