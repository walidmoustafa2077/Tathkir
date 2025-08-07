using System.Windows.Controls;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.ViewModels
{
    public class SettingsPageViewModel
    {
        public string Name { get; }
        public UserControl Content { get; }
        public ISettingsPageService? ViewModel => Content.DataContext as ISettingsPageService;

        public SettingsPageViewModel(string name, UserControl content)
        {
            Name = name;
            Content = content;
        }
    }

}
