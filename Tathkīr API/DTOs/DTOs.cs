using System.Text.Json.Serialization;

namespace Tathkīr_API.DTOs
{
    public record GeoCountryInfoResponse
    {
        public List<GeoCountryEntry> Geonames { get; set; } = new();
    }

    public record GeoCountryEntry
    {
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }

    public record GeoCityNamesResponse
    {
        public int TotalResultsCount { get; set; }
        public List<GeoCityNameEntry> Geonames { get; set; } = new();
    }

    public record GeoCityNameEntry
    {
        public string Name { get; set; } = string.Empty;
        public string FcodeName { get; set; } = string.Empty;
        public string AdminName1 { get; set; } = string.Empty;
        public string ToponymName { get; set; } = string.Empty;
    }

    public record IpResponse
    {
        public string Country { get; set; } = string.Empty;
        public string CountryLocalized { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string CityLocalized { get; set; } = string.Empty;
    }
    public record CountryResponse
    {
        public string Name { get; set; } = string.Empty;
        public string NameLocalized { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public record CityResponse 
    { 
        [JsonIgnore]
        public string AdminName1Original { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string NameLocalized { get; set; } = string.Empty;
        public List<StateResponse> States { get; set; } = new List<StateResponse>();
    }

    public record StateResponse
    {
        public string Name { get; set; } = string.Empty;
        public string NameLocalized { get; set; } = string.Empty;
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
