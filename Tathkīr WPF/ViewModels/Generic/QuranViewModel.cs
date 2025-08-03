using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Dialogs;

namespace Tathkīr_WPF.ViewModels.Generic
{
    public class QuranViewModel : ViewModelBase
    {
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
                        fullSurah.Name.DisplayName = isRtl ? fullSurah.Name.Ar : fullSurah.Name.En;
                        fullSurah.Verses?.ForEach(v =>
                        {
                            v.Text.Text = isRtl ? v.Text.Ar : v.Text.En;
                        });

                        var vm = new SurahDialogViewModel { Surah = fullSurah };

                        DialogService.Instance.ShowDialogGeneric(vm);
                    }
                }
            }
        }

        public ICommand PlayAudioCommand { get; set; } = null!;
        public ICommand PreviousAyahCommand { get; set; } = null!;
        public ICommand NextAyahCommand { get; set; } = null!;

        private bool isRtl;

        public QuranViewModel()
        {
            // Commands
            isRtl = Application.Current.MainWindow.FlowDirection == FlowDirection.RightToLeft;
        }

        public async Task LoadDataAsync()
        {
            var tempList = new List<SurahMeta>();

            await Task.Run(async () =>
            {
                var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                var jsonPath = Path.Combine(baseDir, "Quran.json");
                var surahFolder = Path.Combine(baseDir, "Surahs");

                // Create folder if it doesn't exist
                if (!Directory.Exists(surahFolder))
                    await BuildSurahsFolder(jsonPath, surahFolder);
                else
                {
                    // If folder exists, load metadata from existing files
                    foreach (var surahFile in Directory.GetFiles(surahFolder, "*.json"))
                    {
                        var fullSurah = JsonConvert.DeserializeObject<SurahMeta>(File.ReadAllText(surahFile));
                        if (fullSurah?.Name == null) continue;

                        var meta = new SurahMeta
                        {
                            Number = fullSurah.Number,
                            Name = fullSurah.Name,
                            DisplayName = isRtl ? fullSurah.Name.Ar : fullSurah.Name.En
                        };

                        tempList.Add(meta);
                    }
                }
            });

            // Update ObservableCollection on UI thread
            Surahs.Clear();
            foreach (var surah in tempList.OrderBy(s => s.Number))
            {
                Surahs.Add(surah);
            }

        }

        private Surah? LoadFullSurah(int surahNumber)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Surahs", $"{surahNumber}.json");
            if (!File.Exists(filePath)) return null;

            return JsonConvert.DeserializeObject<Surah>(File.ReadAllText(filePath));
        }

        private async Task BuildSurahsFolder(string jsonPath, string surahFolder)
        {
            var tempList = new List<Surah>();

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

                        if (fullSurah.Verses == null || fullSurah.Verses.Count == 0)
                            continue;

                        // Create metadata for the main list
                        var meta = new Surah
                        {
                            Number = fullSurah.Number,
                            Name = fullSurah.Name,
                            Verses_Count = fullSurah.Verses.Count,
                        };

                        tempList.Add(meta);
                    }
                }
            }

            await LoadDataAsync();
        }

        public void BuildSurahsAudios()
        {
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            var jsonPath = Path.Combine(baseDir, "Quran.json");

            var audioFile = Path.Combine(baseDir, "QuranAudios.json");

            using (var fs = File.OpenRead(jsonPath))
            using (var reader = new JsonTextReader(new StreamReader(fs)))
            {
                var serializer = new JsonSerializer();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        // Deserialize audio info for this Surah
                        var surahAudio = serializer.Deserialize<SurahAudio>(reader);

                        if (surahAudio == null) continue;

                        // Filter to only include "حفص عن عاصم"
                        surahAudio.Audio = surahAudio.Audio?
                            .Where(a => a.Rewaya.Ar.Trim() == "حفص عن عاصم")
                            .ToList() ?? new List<Audio>();

                        // Skip if no matching audio
                        if (surahAudio.Audio.Count == 0) continue;

                        // Add to combined list
                        File.WriteAllText(
                            audioFile,
                            JsonConvert.SerializeObject(surahAudio, Formatting.Indented)
                        );
                        return;
                    }
                }
            }

          
        }

    }
}
