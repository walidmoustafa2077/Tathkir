using System.Windows.Input;
using Tathkīr_WPF.Commands;

namespace Tathkīr_WPF.ViewModels.Dialogs
{
    public class ThikrDialogItem : ViewModelBase
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;

                if (!_isInitialized)
                {
                    _cacheCount = value;
                    _isInitialized = true;
                }

                OnPropertyChanged();
            }
        }

        private bool _resetDateVisibility;
        public bool ResetDateVisibility
        {
            get => _resetDateVisibility;
            set { _resetDateVisibility = value; OnPropertyChanged(); }
        }


        public ICommand DecrementCountCommand { get; }
        public ICommand ResetDateCommand { get; }

        private int _cacheCount;
        private bool _isInitialized = false;

        // Constructor
        public ThikrDialogItem()
        {
            CommandBase decrementCommand = null!;

            decrementCommand = new CommandBase(
                _ =>
                {
                    if (Count > 0)
                        Count--;

                    if (!ResetDateVisibility)
                        ResetDateVisibility = true;

                    decrementCommand.RaiseCanExecuteChanged();
                },
                _ => Count > 0
            );

            DecrementCountCommand = decrementCommand;

            ResetDateCommand = new CommandBase(_ =>
            {
                Count = _cacheCount;
                ResetDateVisibility = false; // Hide reset date button after resetting
            });
        }
    }
}
