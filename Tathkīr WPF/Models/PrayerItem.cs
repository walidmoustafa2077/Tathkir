using Tathkīr_WPF.Enums;

namespace Tathkīr_WPF.Models
{
    public class PrayerItem
    {
        public PrayerType Type { get; set; }
        public string Name
        {
            get
            {
                return Strings.ResourceManager.GetString(Type.ToString()) ?? Type.ToString();
            }
        }

        public string Time { get; set; } = string.Empty;
        public string Icon => GetIconFromType(Type);
        public bool IsNextPrayer { get; set; } = false;

        private string GetIconFromType(PrayerType type)
        {
            return type switch
            {
                PrayerType.Fajr => "WeatherSunsetUp",
                PrayerType.Sunrise => "WeatherSunny",
                PrayerType.Dhuhr => "SunTime",
                PrayerType.Jumua => "SunTime",
                PrayerType.Asr => "WeatherPartlyCloudy",
                PrayerType.Maghrib => "WeatherSunsetDown",
                PrayerType.Isha => "WeatherNight",
                _ => "ClockOutline"
            };
        }
    }
}
