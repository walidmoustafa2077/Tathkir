using System.Windows.Media;

namespace Tathkīr_WPF.Managers
{
    public class AudioManager
    {
        private static AudioManager? _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new AudioManager();
                return _instance;
            }
        }

        public MediaPlayer MediaPlayer = new MediaPlayer();

        public bool IsPlaying { get; private set; } = false;
        public bool IsEnded { get; private set; } = false;

        private AudioManager()
        {
            // Initialize any resources needed for audio management
            IsPlaying = false;
            MediaPlayer.MediaEnded += (s, e) =>
            {
                IsPlaying = false;
                IsEnded = true;
            };
        }

        public void Play(string audioFilePath)
        {
            MediaPlayer.Open(new Uri(audioFilePath));
            MediaPlayer.Play();
            IsPlaying = true;
            IsEnded = false;
        }

        public void Resume()
        {
            if (MediaPlayer.Source != null)
            {
                MediaPlayer.Play();
                IsPlaying = true;
            }
        }

        public void Pause()
        {
            if (MediaPlayer.Source != null)
            {
                MediaPlayer.Pause();
                IsPlaying = false;
            }
        }

        public void Stop()
        {
            if (IsPlaying)
            {
                MediaPlayer.Stop();
                IsPlaying = false;
            }
        }

        public void SetVolume(double volume)
        {
            if (volume < 0 || volume > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 1.");
            }
            MediaPlayer.Volume = volume;
        }

        public string GetCurrentTrack()
        {
            return MediaPlayer.Source?.LocalPath ?? string.Empty;
        }
    }
}
