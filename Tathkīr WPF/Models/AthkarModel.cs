namespace Tathkīr_WPF.Models
{
    public class AthkarCategory
    {
        public string Name { get; set; } = string.Empty;
        public List<AthkarEntry> Entries { get; set; } = new();
    }

    public class AthkarEntry
    {
        public string Content { get; set; } = string.Empty;
        public string Count { get; set; } = "1";
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }

}
