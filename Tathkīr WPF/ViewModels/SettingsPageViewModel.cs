namespace Tathkīr_WPF.ViewModels
{
    public class SettingsPageViewModel
    {
        public string Name { get; }
        public object Content { get; }

        public SettingsPageViewModel(string name, object content)
        {
            Name = name;
            Content = content;
        }
    }

}
