namespace Tathkīr_API.DTOs
{
    public record IpApiResponse
    {
        public string Status { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Timezone { get; set; } = string.Empty;
    }

    public record PrayerTimingsResponse
    {
        public PrayerData Data { get; set; } = new PrayerData();
    }

    public record PrayerData
    {
        public Dictionary<string, string> Timings { get; set; } = new Dictionary<string, string>();
        public PrayerDate Date { get; set; } = new PrayerDate();
        public PrayerMeta Meta { get; set; } = new PrayerMeta();
    }

    public record PrayerDate
    {
        public string Readable { get; set; } = string.Empty;
        public HijriDate Hijri { get; set; } = new HijriDate();
    }

    public record HijriDate
    {
        public string Date { get; set; } = string.Empty;
        public Weekday Weekday { get; set; } = new Weekday();
        public HijriMonth Month { get; set; } = new HijriMonth();
    }

    public record Weekday
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
