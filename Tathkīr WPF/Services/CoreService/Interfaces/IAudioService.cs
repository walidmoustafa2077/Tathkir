namespace Tathkīr_WPF.Services.CoreService.Interfaces
{
    public interface IAudioService
    {
        string? GetPrayerAudioPath(string prayerName, string timing);
        string? GetThikrAudioPath(string thikrName);
    }
}
