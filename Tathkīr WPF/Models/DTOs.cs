namespace Tathkīr_WPF.Models
{
    public record PrayerTimesResponse
    {
        public PrayerData Data { get; set; } = new();
    }

    public record PrayerData
    {
        public PrayerTimings Timings { get; set; } = new();
        public PrayerDate Date { get; set; } = new();
        public PrayerMeta Meta { get; set; } = new();
    }

    public record Address
    {
        public string Country { get; set; } = string.Empty;
        public string CountryLocalized { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string CityLocalized { get; set; } = string.Empty;
    }

    public record Country
    {
        public string Name { get; set; } = string.Empty;
        public string NameLocalized { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public record City
    {
        public string Name { get; set; } = string.Empty;
        public string NameLocalized { get; set; } = string.Empty;
    }

    public record PrayerTimings
    {
        public string? Fajr { get; set; }
        public string? Sunrise { get; set; }
        public string? Dhuhr { get; set; }
        public string? Asr { get; set; }
        public string? Sunset { get; set; }
        public string? Maghrib { get; set; }
        public string? Isha { get; set; }
        public string? Midnight { get; set; }
        public string? Lastthird { get; set; }
    }

    public record PrayerDate
    {
        public string Readable { get; set; } = string.Empty;
        public HijriDate Hijri { get; set; } = new();
    }

    public record HijriDate
    {
        public string Date { get; set; } = string.Empty;
        public HijriWeekday Weekday { get; set; } = new();
        public HijriMonth Month { get; set; } = new();
    }

    public record HijriWeekday
    {
        public string En { get; set; } = string.Empty;
        public string Ar { get; set; } = string.Empty;
    }

    public record HijriMonth
    {
        public string En { get; set; } = string.Empty;
        public string Ar { get; set; } = string.Empty;
    }

    public record PrayerMeta
    {
        public string Timezone { get; set; } = string.Empty;
    }

}
