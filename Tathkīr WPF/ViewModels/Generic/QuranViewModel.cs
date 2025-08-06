using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Dialogs;

namespace Tathkīr_WPF.ViewModels.Generic
{
    public class QuranViewModel : ViewModelBase
    {
        private readonly AudioPlaybackService _audioPlayer = new AudioPlaybackService();
        private readonly AudioCacheService _cacheService = new AudioCacheService();
        private readonly AudioRepository _audioRepo = new AudioRepository();

        public ObservableCollection<SurahMeta> Surahs { get; set; } = new ObservableCollection<SurahMeta>();

        private SurahMeta _selectedSurah = new SurahMeta();
        public SurahMeta SelectedSurah
        {
            get => _selectedSurah;
            set
            {
                _selectedSurah = value;
                OnPropertyChanged();

                if (_selectedSurah != null && _selectedSurah.Number > 0)
                {
                    // Load the full Surah data when a Surah is selected
                    var fullSurah = LoadFullSurah(_selectedSurah.Number);
                    if (fullSurah != null && fullSurah.Name != null)
                    {
                        fullSurah.Name.DisplayName = fullSurah.Name.Ar;
                        fullSurah.Verses?.ForEach(v =>
                        {
                            v.Text.Text = v.Text.Ar;
                        });

                        var vm = new SurahDialogViewModel(_audioPlayer, _audioRepo, _cacheService) { Surah = fullSurah };

                        DialogService.Instance.ShowDialogGeneric(vm);
                    }
                }
            }
        }

        public ICommand PlayAudioCommand { get; set; } = null!;
        public ICommand PreviousAyahCommand { get; set; } = null!;
        public ICommand NextAyahCommand { get; set; } = null!;

        public QuranViewModel()
        {
            // Commands
        }

        public async Task LoadDataAsync()
        {
            var quantJsonPath = Path.Combine(App.baseDir, "Quran.json");
            var surahFolder = Path.Combine(App.baseDir, "Surahs");

            // Create folder if it doesn't exist
            if (!Directory.Exists(surahFolder))
            {
                await BuildSurahsFolder(quantJsonPath, surahFolder);
                return;
            }

            // If folder exists, load metadata from existing files and update Surahs collection
            foreach (var surahFile in Directory.GetFiles(surahFolder, "*.json").OrderBy(f => GetSurahNumberFromFilename(f)))
            {
                var fullSurah = JsonConvert.DeserializeObject<SurahMeta>(File.ReadAllText(surahFile));
                if (fullSurah?.Name == null) continue;

                var meta = new SurahMeta
                {
                    Number = fullSurah.Number,
                    Name = fullSurah.Name,
                    DisplayName = fullSurah.Name.Ar 
                };

                Surahs.Add(meta);
            }
        }

        private int GetSurahNumberFromFilename(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            return int.TryParse(fileName, out int number) ? number : 0;
        }


        private Surah? LoadFullSurah(int surahNumber)
        {
            var filePath = Path.Combine(App.baseDir, "Surahs", $"{surahNumber}.json");
            if (!File.Exists(filePath)) return null;

            return JsonConvert.DeserializeObject<Surah>(File.ReadAllText(filePath));
        }

        private async Task BuildSurahsFolder(string jsonPath, string surahFolder)
        {

            Directory.CreateDirectory(surahFolder);

            using (var fs = File.OpenRead(jsonPath))
            using (var reader = new JsonTextReader(new StreamReader(fs)))
            {
                var serializer = new JsonSerializer();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        // Deserialize full surah so we can save to separate file
                        var fullSurah = serializer.Deserialize<Surah>(reader);

                        if (fullSurah?.Name == null)
                            continue;

                        // Save this surah into a separate JSON file
                        var surahFilePath = Path.Combine(surahFolder, $"{fullSurah.Number}.json");
                        File.WriteAllText(surahFilePath, JsonConvert.SerializeObject(fullSurah, Formatting.Indented));
                    }
                }
            }

            await LoadDataAsync();
        }
    }
}
