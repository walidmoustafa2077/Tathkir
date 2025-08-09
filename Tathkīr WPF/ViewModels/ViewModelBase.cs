using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tathkīr_WPF.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Mark the event as nullable (?)
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            // Use the null-conditional operator to raise the event safely
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
