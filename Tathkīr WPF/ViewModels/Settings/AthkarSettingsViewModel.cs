using System.Collections.ObjectModel;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class AthkarSettingsViewModel : ViewModelBase
    {
        public ObservableCollection<AthkarItem> AthkarList { get; set; }

        public AthkarSettingsViewModel()
        {
            if (SettingsService.AppSettings.AppConfig.AthkarConfigSettings == null)
            {
                SettingsService.AppSettings.AppConfig.AthkarConfigSettings = SettingsService.AppSettings.AppConfig.SetAthkarConfigSettings();

                AthkarList = new ObservableCollection<AthkarItem>
                {
                    new AthkarItem { Name = Strings.Subhan_allah_alhamd_lilhi_allah_akbar_la_alah_ala_allah, SelectedRepeatOption = Strings.None },
                    new AthkarItem { Name = Strings.Alllahumm_slla_ealaa_sayidina_muhammad, SelectedRepeatOption = Strings.None },
                    new AthkarItem { Name = Strings.Subhan_allah_wabihamdihi_subhan_allh_aleazimi, SelectedRepeatOption = Strings.None },
                    new AthkarItem { Name = Strings.La_hawl_wala_quwwat_alla_biallahi, SelectedRepeatOption = Strings.None },
                    new AthkarItem { Name = Strings.La_allah_ala_ant_subhanak_anna_kunt_min_alzaalimina, SelectedRepeatOption = Strings.None }
                };

                SettingsService.AppSettings.AppConfig.AthkarConfigSettings.AthkarList = AthkarList;
                foreach (var item in SettingsService.AppSettings.AppConfig.AthkarConfigSettings.AthkarList)
                    item.Name = item.Name.ToInvariantLanguage();

                SettingsService.Save(SettingsService.AppSettings);
            }
            else
            {
                AthkarList = SettingsService.AppSettings.AppConfig.AthkarConfigSettings.AthkarList;
                
                foreach (var item in AthkarList)
                    item.Name = item.Name.ToLocalizedLanguage();
            }
        }

        public void SaveSettings()
        {
            if (SettingsService.AppSettings.AppConfig.AthkarConfigSettings == null)
                SettingsService.AppSettings.AppConfig.AthkarConfigSettings = SettingsService.AppSettings.AppConfig.SetAthkarConfigSettings();

            SettingsService.AppSettings.AppConfig.AthkarConfigSettings.AthkarList = AthkarList;
            SettingsService.Save(SettingsService.AppSettings);
        }
    }
}
