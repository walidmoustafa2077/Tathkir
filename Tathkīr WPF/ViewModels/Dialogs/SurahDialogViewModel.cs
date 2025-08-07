using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Tathkīr_WPF.Commands;
using Tathkīr_WPF.Helpers;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.Services.ModulesService.QuranService;
using Tathkīr_WPF.Services.ModulesService.QuranService.Interfaces;

namespace Tathkīr_WPF.ViewModels.Dialogs
{
    public class SurahDialogViewModel : BaseDialogViewModel
    {
        private readonly IAudioPlaybackService _audioPlayer;
        private readonly IAudioCacheService _cacheService;
        private readonly IAudioRepository _audioRepo;
        private readonly MediaPlayer _mediaPlayer;
        private readonly AppSettings _settings = SettingsService.AppSettings;
        private readonly DispatcherTimer _timer;

        public Surah Surah { get; set; } = null!;

        public SearchableList<string> ReadersList { get; } = new();

        private string _loopStatus = Strings.Looping_Off;
        public string LoopStatus { get => _loopStatus; set { _loopStatus = value; OnPropertyChanged(); } }

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

        public SurahDialogViewModel(AudioPlaybackService audioPlayer, AudioRepository audioRepo, AudioCacheService cacheService)
        {
            _audioPlayer = audioPlayer;
            _audioRepo = audioRepo;
            _cacheService = cacheService;
            _mediaPlayer = _audioPlayer.MediaPlayer;

            surahAudio = _audioRepo.LoadAudioData();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

            InitializeCommands();
            InitializeReaders();
            InitializeTimer();
            HookMediaPlayerEvents();

            RestoreLastReader();
            SetInitialPlaybackState();
        }

        private void InitializeCommands()
        {
            PlaySurahCommand = new CommandBase(async _ => await PlaySurahAsync());
            StopSurahCommand = new CommandBase(_ =>
            {
                _audioPlayer.Stop();
                IsPlay = true;
                IsPause = false;
                _currentTrackUrl = string.Empty;
            });

            LoopSurahCommand = new CommandBase(_ =>
            {
                _isLoopEnabled = !_isLoopEnabled;
                LoopStatus = _isLoopEnabled ? Strings.Looping_On : Strings.Looping_Off;
            });

            CloseDialog = new CommandBase(_ =>
            {
                MainWindowViewModel.Instance.DialogControl = null;
            });
        }

        private void InitializeReaders()
        {
            var readers = surahAudio?.Audio?
                .Select(a => a.Reciter.Ar)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            if (readers == null) return;

            foreach (var reader in readers)
                ReadersList.Items.Add(reader);

            // Handle selection changes
            ReadersList.PropertyChanged += ReadersList_PropertyChanged;
        }

        private void ReadersList_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReadersList.SelectedItem))
            {
                var selected = ReadersList.SelectedItem;

                if (!string.IsNullOrWhiteSpace(selected))
                {
                    IsPause = false;
                    IsPlay = true;
                    _currentTrackUrl = string.Empty;

                    _settings.LastSelectedReader = selected;
                }
            }
        }

        private void InitializeTimer()
        {
            _timer.Tick += (s, e) =>
            {
                if (!_isSeeking && _mediaPlayer.Source != null)
                    MediaPosition = _mediaPlayer.Position.TotalSeconds;
            };
            _timer.Start();
        }

        private void HookMediaPlayerEvents()
        {
            _mediaPlayer.MediaOpened += (_, _) =>
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                    MediaDuration = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            };

            _mediaPlayer.MediaEnded += async (_, _) =>
            {
                if (_isLoopEnabled && !string.IsNullOrEmpty(_currentTrackUrl) && !string.IsNullOrEmpty(ReadersList.SelectedItem))
                {
                    var localPath = await _cacheService.GetOrDownloadAsync(_currentTrackUrl, ReadersList.SelectedItem);
                    _audioPlayer.Play(localPath);
                }
                else
                {
                    IsPlay = true;
                    IsPause = false;
                    _currentTrackUrl = string.Empty;
                }
            };
        }

        private void RestoreLastReader()
        {
            if (!string.IsNullOrEmpty(_settings.LastSelectedReader) &&
                ReadersList.Items.Contains(_settings.LastSelectedReader))
            {
                ReadersList.SelectedItem = _settings.LastSelectedReader;
            }
            else
            {
                ReadersList.SelectedItem = ReadersList.Items.FirstOrDefault() ?? string.Empty;
            }
        }

        private void SetInitialPlaybackState()
        {
            if (!_audioPlayer.IsPlaying)
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

                IsPlay = false;
                IsPause = true;
                _currentTrackUrl = _mediaPlayer.Source?.LocalPath ?? string.Empty;
            }
        }

        public async Task PlaySurahAsync()
        {
            if (Surah == null || string.IsNullOrEmpty(ReadersList.SelectedItem)) return;

            var audio = surahAudio?.Audio?
                .FirstOrDefault(a => a.Reciter.Ar == ReadersList.SelectedItem);

            if (audio == null) return;

            var id = Surah.Number.ToString("D3");
            var url = $"{audio.Server.TrimEnd('/')}/{id}.mp3";
            var localPath = await _cacheService.GetOrDownloadAsync(url, ReadersList.SelectedItem);

            if (_audioPlayer.IsEnded || _currentTrackUrl != url || _cachedReader != ReadersList.SelectedItem)
            {
                _currentTrackUrl = url;
                _cachedReader = ReadersList.SelectedItem;
                _audioPlayer.Play(localPath);
            }

            TogglePlayPause();
        }

        public void StartSeeking()
        {
            _isSeeking = true;
        }

        public void StopSeeking()
        {
            _isSeeking = false;
            _mediaPlayer.Position = TimeSpan.FromSeconds(MediaPosition);
        }

        private void TogglePlayPause()
        {
            if (_audioPlayer.IsPlaying)
            {
                _audioPlayer.Pause();
                IsPlay = true;
                IsPause = false;
            }
            else
            {
                _audioPlayer.Resume();
                IsPlay = false;
                IsPause = true;
            }
        }
    }
}
