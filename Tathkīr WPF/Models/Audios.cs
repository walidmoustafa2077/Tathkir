namespace Tathkīr_WPF.Models
{
    public record Audios
    {
        public List<PrayAudio> PrayAudios { get; set; } = new List<PrayAudio>();
        public List<ThikrAudio> ThikrAudios { get; set; } = new List<ThikrAudio>();

    }

    public record PrayAudio
    {
        public string Name { get; set; } = string.Empty;
        public string BeforeAthanPath { get; set; } = string.Empty;
        public string OnAthanPath { get; set; } = string.Empty;
    }

    public record ThikrAudio
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}
