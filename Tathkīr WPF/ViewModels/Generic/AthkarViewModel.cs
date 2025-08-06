using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Dialogs;


namespace Tathkīr_WPF.ViewModels.Generic
{
    public class AthkarViewModel : ViewModelBase
    {
        private readonly Dictionary<string, List<AthkarEntry>> _athkarByCategory = new();

        public ObservableCollection<AthkarItem> AthkarItems { get; set; } = new ObservableCollection<AthkarItem>();
        private AthkarItem? _selectedAthkarItem = null;
        public AthkarItem? SelectedAthkarItem
        {
            get => _selectedAthkarItem;
            set
            {
                _selectedAthkarItem = value;
                OnPropertyChanged();

                HandleDialog(value?.Name);
            }
        }

        private void HandleDialog(string? categoryName)
        {
            if (string.IsNullOrEmpty(categoryName) || !_athkarByCategory.ContainsKey(categoryName))
                return;
            
            if (!_athkarByCategory.TryGetValue(categoryName, out var entries))
                return;

            var dialogItems = entries.Select(e => new ThikrDialogItem
            {
                Content = e.Content,
                Count = int.TryParse(e.Count, out int count) ? count : 1,
                Description = e.Description,
                Title = string.Empty
            }).ToList();

            var vm = new ThikrDialogViewModel
            {
                Title = categoryName,
                Items = new ObservableCollection<ThikrDialogItem>(dialogItems)
            };

            DialogService.Instance.ShowDialogGeneric(vm);
        }

        public AthkarViewModel()
        {
            var iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons");
            LoadAthkarData();
        }

        private void LoadAthkarData()
        {
            var jsonPath = Path.Combine(App.baseDir, "athkar.json");

            if (!File.Exists(jsonPath))
            {
                return;
            }

            var json = File.ReadAllText(jsonPath);
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, List<AthkarEntry>>>(json);

            if (parsed == null)
                return;

            var iconsPath = Path.Combine(App.baseDir, "Resources", "Icons");

            foreach (var kvp in parsed)
            {
                _athkarByCategory[kvp.Key] = kvp.Value;

                AthkarItems.Add(new AthkarItem
                {
                    Name = kvp.Key,
                });
            }
        }
    }

}
