using System.Windows.Media;

namespace Tathkīr_WPF.Services
{
    public interface IAudioPlaybackService
    {
        void Play(string path);
        void Pause();
        void Resume();
        void Stop();
        bool IsPlaying { get; }
        bool IsEnded { get; }
        MediaPlayer MediaPlayer { get; }
    }

}
