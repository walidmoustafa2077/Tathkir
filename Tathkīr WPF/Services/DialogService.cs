using Tathkīr_WPF.ViewModels;
using Tathkīr_WPF.ViewModels.Dialogs;
using Tathkīr_WPF.Views.Dialogs;

namespace Tathkīr_WPF.Services
{
    public class DialogService
    {
        private static DialogService? _instance;
        public static DialogService Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new DialogService();
                return _instance;
            }
        }

        private readonly MainWindowViewModel _mainWindowViewModel;
        public bool IsDialogOpen { get; private set; }

        public DialogService()
        {
            _mainWindowViewModel = MainWindowViewModel.Instance;
        }


        public void ShowDialog(BaseDialogViewModel dialogViewModel)
        {
            if (dialogViewModel is null)
                throw new ArgumentNullException(nameof(dialogViewModel));

            dialogViewModel.SecondaryButtonVisibility = !string.IsNullOrEmpty(dialogViewModel.SecondaryButton);

            var dialogControl = new BaseDialogControl { DataContext = dialogViewModel };

            _mainWindowViewModel.DialogControl = dialogControl;
            IsDialogOpen = true;
        }

        public void ShowDialogGeneric(object dialogViewModel)
        {
            if (dialogViewModel is null)
                throw new ArgumentNullException(nameof(dialogViewModel));

            object? dialogControl = null;

            // Switch to the specific type if needed
            switch (dialogViewModel)
            {
                case ThikrDialogViewModel thikrDialogViewModel:
                    dialogControl = new ThikrDialogControl { DataContext = thikrDialogViewModel };
                    break;
                case SurahDialogViewModel surahDialogViewModel:
                    dialogControl = new SurahDialogControl { DataContext = surahDialogViewModel };
                    break;
                default:
                    throw new ArgumentException("Unsupported dialog view model type.", nameof(dialogViewModel));
            }

            ShowDialogControl(dialogControl);
        }

        public void ShowDialogControl(object dialogControl)
        {
            if (dialogControl is null)
                throw new ArgumentNullException(nameof(dialogControl));

            _mainWindowViewModel.DialogControl = dialogControl;
            IsDialogOpen = true;
        }

        public void ShowError(string error, string message, bool secondaryButton = false)
        {
            var vm = new BaseDialogViewModel
            {
                Title = error,
                Message = message,
                PrimaryButton = Strings.OK,
                SecondaryButton = secondaryButton ? Strings.Cancel : string.Empty,
                PrimaryButtonAction = Instance.CloseDialog,
                SecondaryButtonAction = Instance.CloseDialog,
            };

            ShowDialog(vm);
        }

        public void CloseDialog()
        {
            _mainWindowViewModel.DialogControl = null;
            IsDialogOpen = false;
        }


    }
}
