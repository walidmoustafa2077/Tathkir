using Newtonsoft.Json;

namespace Tathkīr_WPF.Models
{
    public record Verse
    {
        public int Number { get; set; }
        public VerseText Text { get; set; } = new VerseText();
    }

    public record VerseText
    {
        [JsonIgnore]
        public string Text { get; set; } = string.Empty;
        public string Ar { get; set; } = string.Empty;
        public string En { get; set; } = string.Empty;
    }

    public record SurahName
    {
        [JsonIgnore]
        public string DisplayName { get; set; } = string.Empty;
        public string Ar { get; set; } = string.Empty;
        public string En { get; set; } = string.Empty;
        public string Transliteration { get; set; } = string.Empty;
    }

    public record Surah
    {
        public int Number { get; set; }
        public SurahName? Name { get; set; }
        public int Verses_Count { get; set; }
        public List<Verse>? Verses { get; set; } = new List<Verse>();
    }

    public class SurahMeta
    {
        public int Number { get; set; }
        public SurahName? Name { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public record SurahAudio
    {
        public int Number { get; set; }
        public List<Audio>? Audio { get; set; } = new();
    }

    public record Audio
    {
        public Reciter Reciter { get; set; } = new();
        public Rewaya Rewaya { get; set; } = new();
        public string Server { get; set; } = string.Empty;
    }

    public record Reciter
    {
        public string Ar { get; set; } = string.Empty;
    }

    public record Rewaya
    {
        public string Ar { get; set; } = string.Empty;
    }

}
