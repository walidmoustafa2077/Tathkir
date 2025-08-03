using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Tathkīr_WPF.Commands;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.ViewModels.Dialogs
{
    public class SurahDialogViewModel : BaseDialogViewModel
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly AppSettings _settings;

        public Surah Surah { get; set; } = null!;

        public ObservableCollection<string> Readers { get; set; } = new ObservableCollection<string>();

        private ICollectionView _readersView = null!;
        public ICollectionView ReadersView
        {
            get => _readersView;
            set { _readersView = value; OnPropertyChanged(); }
        }

        private string _selectedReader = string.Empty;
        public string SelectedReader
        {
            get => _selectedReader;
            set
            {
                _selectedReader = value;

                SearchText = value; 

                IsPause = false;
                IsPlay = true;
                _currentTrackUrl = string.Empty;

                _settings.LastSelectedReader = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPlay));
                OnPropertyChanged(nameof(IsPause));
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();

                    // Refresh the filter when the search text changes
                    ReadersView?.Refresh();
                }
            }
        }

        private string _loopStatus = Strings.Looping_Off;
        public string LoopStatus
        {
            get => _loopStatus;
            set
            {
                _loopStatus = value;
                OnPropertyChanged();
            }
        }

        private bool _isPlay = true;
        public bool IsPlay { get => _isPlay; set { _isPlay = value; OnPropertyChanged(); } }

        private bool _isPause = false;
        public bool IsPause { get => _isPause; set { _isPause = value; OnPropertyChanged(); } }

        private double _mediaDuration;
        public double MediaDuration { get => _mediaDuration; set { _mediaDuration = value; OnPropertyChanged(); } }

        private double _mediaPosition;
        public double MediaPosition
        {
            get => _mediaPosition;
            set
            {
                if (_mediaPosition != value)
                {
                    _mediaPosition = value;
                    OnPropertyChanged();

                    // Seek if user is dragging
                    if (_isSeeking)
                        _mediaPlayer.Position = TimeSpan.FromSeconds(value);
                }
            }
        }

        public ICommand LoopSurahCommand { get; set; } = null!;
        public ICommand StopSurahCommand { get; set; } = null!;
        public ICommand PlaySurahCommand { get; set; } = null!;
        public ICommand BookmarkSurahCommand { get; set; } = null!;
        public ICommand CloseDialog { get; set; } = null!;

        private SurahAudio? surahAudio;
        
        private string _currentTrackUrl = string.Empty;
        private string _cachedReader = string.Empty;
        
        private bool _isLoopEnabled = false;
        private bool _isSeeking;

        private string _cacheDirectory = Path.Combine(AppContext.BaseDirectory, "Cache");

        public SurahDialogViewModel()
        {
            _settings = SettingsService.AppSettings;

            _mediaPlayer = AudioManager.Instance.MediaPlayer;

            surahAudio = LoadAllAudios();

            List<string>? readers = surahAudio?.Audio?
                .Select(a => a.Reciter.Ar)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            readers?.ForEach(r => Readers.Add(r));

            ReadersView = CollectionViewSource.GetDefaultView(Readers);
            ReadersView.Filter = FilterReaders;

            PlaySurahCommand = new CommandBase(async (o) =>
            {
                await PlaySurahAsync();
            });

            StopSurahCommand = new CommandBase((o) =>
            {
                AudioManager.Instance.Stop();
                IsPlay = true;
                IsPause = false;
                _currentTrackUrl = string.Empty;
            });

            LoopSurahCommand = new CommandBase((o) =>
            {
                _isLoopEnabled = !_isLoopEnabled;

                LoopStatus = _isLoopEnabled ? Strings.Looping_On : Strings.Looping_Off;

            });

            CloseDialog = new CommandBase((o) =>
            {
                MainWindowViewModel.Instance.DialogControl = null;
            });

            // Subscribe to MediaPlayer events
            _mediaPlayer.MediaEnded += async (s, e) =>
            {
                if (_isLoopEnabled && !string.IsNullOrEmpty(_currentTrackUrl))
                {
                    await PlayAudioWithCacheAsync(_currentTrackUrl, SelectedReader);
                }
                else
                {
                    IsPlay = true;
                    IsPause = false;
                    _currentTrackUrl = string.Empty;
                }
            };

            _mediaPlayer.MediaOpened += (s, e) =>
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    MediaDuration = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                }
            };

            // Update MediaPosition every second
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (s, e) =>
            {
                if (!_isSeeking && _mediaPlayer.Source != null)
                    MediaPosition = _mediaPlayer.Position.TotalSeconds;
            };

            timer.Start();

            // Restore last selected reader
            if (!string.IsNullOrEmpty(_settings.LastSelectedReader) && Readers.Contains(_settings.LastSelectedReader))
                SelectedReader = _settings.LastSelectedReader;
            else
                SelectedReader = Readers.FirstOrDefault() ?? string.Empty;

            if (!AudioManager.Instance.IsPlaying)
            {
                MediaDuration = 100;
                MediaPosition = 0;
            }
            else
            {
                MediaDuration = _mediaPlayer.NaturalDuration.HasTimeSpan
                    ? _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds
                    : 100;
                MediaPosition = _mediaPlayer.Position.TotalSeconds;
            }

            if (AudioManager.Instance.IsPlaying)
            {
                IsPlay = false;
                IsPause = true;
                _currentTrackUrl = _mediaPlayer.Source.LocalPath;
            }
        }

        public async Task PlaySurahAsync()
        {
            if (Surah == null || string.IsNullOrEmpty(SelectedReader)) return;

            var audio = surahAudio?.Audio?
                .FirstOrDefault(a => a.Reciter.Ar == SelectedReader);

            if (audio != null)
            {
                // Format Surah number as 3 digits (001, 002, ..., 114)
                var id = Surah.Number.ToString("D3");
                var url = $"{audio.Server.TrimEnd('/')}/{id}.mp3";

                string fileName = Path.GetFileName(url);
                string sourceName = Path.GetFileName(AudioManager.Instance.GetCurrentTrack());

                if (fileName != sourceName || _cachedReader != SelectedReader || fileName == sourceName && AudioManager.Instance.IsEnded)
                {
                    _currentTrackUrl = url;
                    _cachedReader = SelectedReader;
                    await PlayAudioWithCacheAsync(url, SelectedReader);
                }
            }

            IsPlay = !IsPlay;
            IsPause = !IsPause;

            if (IsPause && !AudioManager.Instance.IsPlaying)
            {
                AudioManager.Instance.Resume();
            }
            else if (IsPlay && AudioManager.Instance.IsPlaying)
            {
                AudioManager.Instance.Pause();
            }
        }

        public void StartSeeking() => _isSeeking = true;
        public void StopSeeking()
        {
            _isSeeking = false;
            _mediaPlayer.Position = TimeSpan.FromSeconds(MediaPosition);
        }

        private SurahAudio? LoadAllAudios()
        {
            var audioFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "QuranAudios.json");
            return File.Exists(audioFile)
                ? JsonConvert.DeserializeObject<SurahAudio>(File.ReadAllText(audioFile))
                : new SurahAudio();
        }

        private async Task PlayAudioWithCacheAsync(string url, string reader)
        {
            Directory.CreateDirectory(_cacheDirectory);

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(reader))
            {
                MessageBox.Show("Invalid audio URL or reader selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string fileName = Path.GetFileName(url);
            string folderName = Path.Combine(_cacheDirectory, reader);
            string localPath = Path.Combine(folderName, fileName);

            if (!File.Exists(localPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localPath)!);
                // Play directly from URL (starts immediately)
                AudioManager.Instance.Play(url);

                using var client = new HttpClient();
                using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fs);
                }
                return;
            }

            // Play from local file
            AudioManager.Instance.Play(localPath);
        }

        private bool FilterReaders(object item)
        {
            if (string.IsNullOrEmpty(SearchText))
                return true;

            if (item == null)
                return false;

            return item.ToString()?
                       .IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

    }
}
