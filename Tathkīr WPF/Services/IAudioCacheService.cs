namespace Tathkīr_WPF.Services
{
    public interface IAudioCacheService
    {
        Task<string> GetOrDownloadAsync(string url, string reader);
    }
}
