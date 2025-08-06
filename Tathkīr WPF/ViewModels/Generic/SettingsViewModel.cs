using System.Collections.ObjectModel;
using System.Windows.Input;
using Tathkīr_WPF.Commands;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Dialogs;
using Tathkīr_WPF.ViewModels.Settings;
using Tathkīr_WPF.Views.SettingsViews;

namespace Tathkīr_WPF.ViewModels.Generic
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
                new SettingsPageViewModel(Strings.General, new GeneralSettingsControl()),
                new SettingsPageViewModel(Strings.Prayer_Times, new AthanSettingsControl()),
                new SettingsPageViewModel(Strings.Athkar_Reminder, new AthkarSettingsControl()),
                new SettingsPageViewModel(Strings.Widgets, new WidgetsSettingsControl()),
                new SettingsPageViewModel(Strings.Location, new LocationSettingsControl()),
            };

            SelectedPage = Pages[0];

            Save = new CommandBase(SaveSettings);
            Reset = new CommandBase(ResetSettings);
        }

        private void SaveSettings(object? obj)
        {
            var page = SelectedPage.Content;
            Action? action = null;

            switch (page)
            {
                case GeneralSettingsControl generalSettings:
                    var generalSettingsViewModel = generalSettings.DataContext as GeneralSettingsViewModel;
                    if (generalSettingsViewModel is null)
                    {
                        DialogService.Instance.ShowError(Strings.Error, "Strings.Settings_Error_Message");
                        return;
                    }

                    // Save general settings
                    action = () => generalSettingsViewModel.SaveSettings();
                    break;
                case AthanSettingsControl athanSettings:
                    var athanSettingsViewModel = athanSettings.DataContext as AthanSettingsViewModel;
                    if (athanSettingsViewModel is null)
                    {
                        DialogService.Instance.ShowError(Strings.Error, "Strings.Settings_Error_Message");
                        return;
                    }

                    // Save athan settings
                    action = () => athanSettingsViewModel.SaveSettings();
                    break;
                case AthkarSettingsControl athkarSettings:
                    var athkarSettingsViewModel = athkarSettings.DataContext as AthkarSettingsViewModel;
                    if (athkarSettingsViewModel is null)
                    {
                        DialogService.Instance.ShowError(Strings.Error, "Strings.Settings_Error_Message");
                        return;
                    }

                    // Save athkar settings
                    action = () => athkarSettingsViewModel.SaveSettings();
                    break;
                case WidgetsSettingsControl widgetsSettings:
                    break;
                case LocationSettingsControl locationSettings:
                    var locationSettingsViewModel = locationSettings.DataContext as LocationSettingsViewModel;

                    if (locationSettingsViewModel is null)
                    {
                        DialogService.Instance.ShowError(Strings.Error, "Strings.Settings_Error_Message");
                        return;
                    }
                    // Save location settings
                    action = () => locationSettingsViewModel.SaveSettings();
                    break;
            }

            if (action is null)
            {
                DialogService.Instance.ShowError(Strings.Error, "No Action Set");
                return;
            }
            
            var vm = new BaseDialogViewModel
            {
                Title = Strings.Save,
                Message = Strings.OnSave,
                PrimaryButton = Strings.OK,
                SecondaryButton = Strings.Cancel,
                PrimaryButtonAction = () =>
                {
                    action.Invoke();
                    DialogService.Instance.CloseDialog();
                },
                SecondaryButtonAction = () => DialogService.Instance.CloseDialog(),
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
