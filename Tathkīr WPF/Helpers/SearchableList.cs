using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Tathkīr_WPF.Helpers
{
    public class SearchableList<T> : INotifyPropertyChanged
    {
        private string _searchText = string.Empty;
        private T? _selectedItem;

        public ObservableCollection<T> Items { get; } = new();
        public ICollectionView FilteredView { get; }


        public T? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_selectedItem, value))
                {
                    _selectedItem = value;
                    SearchText = value?.ToString() ?? string.Empty;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    FilteredView.Refresh();
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public SearchableList(IEnumerable<T>? items = null)
        {
            if (items != null)
            {
                foreach (var item in items)
                    Items.Add(item);
            }

            FilteredView = CollectionViewSource.GetDefaultView(Items);
            FilteredView.Filter = FilterPredicate;
        }

        private bool FilterPredicate(object obj)
        {
            if (obj is T item)
            {
                return string.IsNullOrWhiteSpace(SearchText)
                    || item?.ToString()?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
