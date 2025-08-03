using System.Collections.ObjectModel;
using System.Windows.Input;
using Tathkīr_WPF.Commands;

namespace Tathkīr_WPF.ViewModels.Dialogs
{
    public class ThikrDialogViewModel : ViewModelBase
    {
        public string Title { get; set; } = "Thikr Dialog";
        public ObservableCollection<ThikrDialogItem> Items { get; set; }

        public ICommand CloseDialog { get; }

        public ThikrDialogViewModel()
        {
            Items = new ObservableCollection<ThikrDialogItem>();

            CloseDialog = new CommandBase((o) =>
            {
                MainWindowViewModel.Instance.DialogControl = null;
            });
        }
    }

}
