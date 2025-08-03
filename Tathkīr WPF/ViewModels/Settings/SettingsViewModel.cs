using System.Collections.ObjectModel;
using System.Windows.Input;
using Tathkīr_WPF.Commands;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Dialogs;
using Tathkīr_WPF.Views.SettingsViews;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class SettingsViewModel : ViewModelBase
    {

        public ObservableCollection<SettingsPageViewModel> Pages { get; }

        private SettingsPageViewModel _selectedPage = null!;
        public SettingsPageViewModel SelectedPage
        {
            get => _selectedPage;
            set
            {
                if (_selectedPage != value)
                {
                    _selectedPage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand Save { get; }
        public ICommand Reset { get; }

        public SettingsViewModel()
        {
            // Load pages (each with its own view)
            Pages = new ObservableCollection<SettingsPageViewModel>
            {
                new SettingsPageViewModel(Strings.Location, new LocationSettingsControl()),
                new SettingsPageViewModel(Strings.Prayer_Times, new AthanSettingsControl()),
                new SettingsPageViewModel(Strings.Athkar_Reminder, new AthkarSettingsControl()),
                new SettingsPageViewModel(Strings.Widgets, new WidgetsSettingsControl()),
                new SettingsPageViewModel(Strings.General, new GeneralSettingsControl()),
            };
            SelectedPage = Pages[0];

            Save = new CommandBase(SaveSettings);
            Reset = new CommandBase(ResetSettings);
        }

        private void SaveSettings(object? obj)
        {
            var vm = new BaseDialogViewModel
            {
                Title = Strings.Save,
                Message = "Strings.Settings_Saved_Message",
                PrimaryButton = Strings.OK,
                PrimaryButtonAction = () => DialogService.Instance.CloseDialog()
            };

            DialogService.Instance.ShowDialog(vm);
        }

        private void ResetSettings(object? obj)
        {
            var vm = new BaseDialogViewModel
            {
                Title = Strings.Reset,
                Message = "Strings.Settings_Saved_Message",
                PrimaryButton = Strings.OK,
                SecondaryButton = Strings.Cancel,
                PrimaryButtonAction = () => DialogService.Instance.CloseDialog(),
                SecondaryButtonAction = () => DialogService.Instance.CloseDialog(),
            };

            DialogService.Instance.ShowDialog(vm);
        }
    }
}
