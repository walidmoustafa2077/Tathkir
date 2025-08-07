using System.Windows.Media;
using Tathkīr_WPF.Services.ModulesService.QuranService.Interfaces;

namespace Tathkīr_WPF.Services.ModulesService.QuranService
{
    public class AudioPlaybackService : IAudioPlaybackService
    {
        private readonly MediaPlayer _player = new();

        public void Play(string path)
        {
            _player.Open(new Uri(path, UriKind.RelativeOrAbsolute));
            _player.Play();
        }

        public void Pause() => _player.Pause();
        public void Resume() => _player.Play();
        public void Stop() => _player.Stop();
        public bool IsPlaying => _player.Position > TimeSpan.Zero && _player.Clock != null;
        public bool IsEnded => _player.NaturalDuration.HasTimeSpan && _player.Position >= _player.NaturalDuration.TimeSpan;

        public MediaPlayer MediaPlayer => _player;
    }

}
