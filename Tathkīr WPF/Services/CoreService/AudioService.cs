using System.IO;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Services.CoreService
{
    public class AudioService : IAudioService
    {
        private Audios? _audios;

        private void EnsureLoaded()
        {
            if (_audios != null) return;

            var path = Path.Combine(App.baseDir, "Audios.json");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _audios = System.Text.Json.JsonSerializer.Deserialize<Audios>(json);
            }
        }

        public string? GetPrayerAudioPath(string prayerName, string timing)
        {
            EnsureLoaded();

            var entry = _audios?.PrayAudios
                .FirstOrDefault(a => a.Name.Equals(prayerName, StringComparison.OrdinalIgnoreCase));

            var prop = entry?.GetType().GetProperty(timing);
            var file = prop?.GetValue(entry) as string;
            return GetFullPath(file);
        }

        public string? GetThikrAudioPath(string thikrName)
        {
            EnsureLoaded();

            var entry = _audios?.ThikrAudios
                .FirstOrDefault(a => a.Name.Equals(thikrName.ToInvariantLanguage(), StringComparison.OrdinalIgnoreCase));

            return GetFullPath(entry?.Path);
        }

        private string? GetFullPath(string? file) =>
            !string.IsNullOrWhiteSpace(file)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Audios", file)
                : null;
    }

}
