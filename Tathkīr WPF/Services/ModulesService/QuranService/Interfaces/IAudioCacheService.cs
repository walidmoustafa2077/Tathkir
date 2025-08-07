namespace Tathkīr_WPF.Services.ModulesService.QuranService.Interfaces
{
    public interface IAudioCacheService
    {
        Task<string> GetOrDownloadAsync(string url, string reader);
    }
}
