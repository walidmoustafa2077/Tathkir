using System.Windows.Input;
using Tathkīr_WPF.Commands;

namespace Tathkīr_WPF.ViewModels.Dialogs
{
    public class BaseDialogViewModel : ViewModelBase
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }
        
        private string _primaryButton = string.Empty;
        public string PrimaryButton
        {
            get => _primaryButton;
            set { _primaryButton = value; OnPropertyChanged(); }
        }

        private string _secondaryButton = string.Empty;
        public string SecondaryButton
        {
            get => _secondaryButton;
            set { _secondaryButton = value; OnPropertyChanged(); }
        }

        private bool _secondaryButtonVisibility = true;
        public bool SecondaryButtonVisibility
        {
            get => _secondaryButtonVisibility;
            set { _secondaryButtonVisibility = value; OnPropertyChanged(); }
        }

        public Action? PrimaryButtonAction { get; set; }
        public Action? SecondaryButtonAction { get; set; }

        public ICommand PrimaryButtonCommand { get; }
        public ICommand SecondaryButtonCommand { get; }

        private TaskCompletionSource<bool> _tcs = null!;

        public BaseDialogViewModel()
        {
            PrimaryButtonCommand = new CommandBase(_ =>
            {
                // Run delegate if set
                PrimaryButtonAction?.Invoke();

                _tcs?.TrySetResult(true);
            });

            SecondaryButtonCommand = new CommandBase(_ =>
            {
                SecondaryButtonAction?.Invoke();
                _tcs?.TrySetResult(false);
            });
        }

        /// <summary>
        /// Show the dialog and wait for the result (true = primary, false = secondary)
        /// </summary>
        public Task<bool> ShowDialogAsync()
        {
            _tcs = new TaskCompletionSource<bool>();
            return _tcs.Task;
        }
    }
}
