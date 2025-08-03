using System.Collections.ObjectModel;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class AthkarSettingsViewModel : ViewModelBase
    {
        public ObservableCollection<AthkarItem> AthkarList { get; set; }

        public AthkarSettingsViewModel()
        {
            AthkarList = new ObservableCollection<AthkarItem>
            {
                new AthkarItem { Name = Strings.Subhan_allah_alhamd_lilhi_allah_akbar_la_alah_ala_allah, SelectedRepeatOption = Strings.None },
                new AthkarItem { Name = Strings.Alllahumm_slla_ealaa_sayidina_muhammad, SelectedRepeatOption = Strings.None },
                new AthkarItem { Name = Strings.Subhan_allah_wabihamdihi_subhan_allh_aleazimi, SelectedRepeatOption = Strings.None },
                new AthkarItem { Name = Strings.La_hawl_wala_quwwat_alla_biallahi, SelectedRepeatOption = Strings.None },
                new AthkarItem { Name = Strings.La_allah_ala_ant_subhanak_anna_kunt_min_alzaalimina, SelectedRepeatOption = Strings.None }
            };
        }
    }
}
