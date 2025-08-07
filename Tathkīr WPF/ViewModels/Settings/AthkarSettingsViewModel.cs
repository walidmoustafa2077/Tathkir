using System.Collections.ObjectModel;
using Tathkīr_WPF.Enums;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class AthkarSettingsViewModel : ViewModelBase, ISettingsPageService
    {
        private ObservableCollection<AthkarItem> _athkarList = new ObservableCollection<AthkarItem>();
        public ObservableCollection<AthkarItem> AthkarList
        {
            get => _athkarList;
            set
            {
                if (_athkarList != value)
                {
                    _athkarList = value;
                    OnPropertyChanged();
                }
            }
        }

        public AthkarSettingsViewModel()
        {
            if (SettingsService.AppSettings.AppConfig.AthkarConfigSettings == null)
                Initialize();
            else
                AthkarList = SettingsService.AppSettings.AppConfig.AthkarConfigSettings.AthkarList;
        }

        private void Initialize()
        {
            AthkarList.Clear();

            AthkarList = new ObservableCollection<AthkarItem>
            {
                new AthkarItem { Name = Strings.Subhan_allah_alhamd_lilhi_allah_akbar_la_alah_ala_allah, SelectedRepeatOption = new RepeatOption { Type = RepeatType.None } },
                new AthkarItem { Name = Strings.Alllahumm_slla_ealaa_sayidina_muhammad, SelectedRepeatOption = new RepeatOption { Type = RepeatType.None }  },
                new AthkarItem { Name = Strings.Subhan_allah_wabihamdihi_subhan_allh_aleazimi, SelectedRepeatOption = new RepeatOption { Type = RepeatType.None }  },
                new AthkarItem { Name = Strings.La_hawl_wala_quwwat_alla_biallahi, SelectedRepeatOption = new RepeatOption { Type = RepeatType.None }  },
                new AthkarItem { Name = Strings.La_allah_ala_ant_subhanak_anna_kunt_min_alzaalimina, SelectedRepeatOption = new RepeatOption { Type = RepeatType.None }  }
            };

            SaveSettings();
        }

        public void ResetSettings() => Initialize();


        public void SaveSettings()
        {
            if (SettingsService.AppSettings.AppConfig.AthkarConfigSettings == null)
                SettingsService.AppSettings.AppConfig.AthkarConfigSettings = SettingsService.AppSettings.AppConfig.SetAthkarConfigSettings();

            SettingsService.AppSettings.AppConfig.AthkarConfigSettings.AthkarList =
                new ObservableCollection<AthkarItem>(AthkarList.Select(item => item.Clone()));

            SettingsService.Save(SettingsService.AppSettings);
        }
    }
}
