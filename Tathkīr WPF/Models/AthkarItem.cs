namespace Tathkīr_WPF.Models
{
    public class AthkarItem
    {
        public string Name { get; set; } = string.Empty;
        public string SelectedRepeatOption { get; set; } = string.Empty;
        public List<string> RepeatOptions { get; } = new List<string>
        {
            Strings.None,
            $"{Strings.Every} 1 {Strings.Hour}",
            $"{Strings.Every} 2 {Strings.Hour}",
            $"{Strings.Every} 3 {Strings.Hour}",
            $"3 {Strings.Times_Per_Day}"
        };
    }

}
