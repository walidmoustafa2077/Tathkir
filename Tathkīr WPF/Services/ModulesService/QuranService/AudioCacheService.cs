using System.IO;
using System.Net.Http;
using Tathkīr_WPF.Services.ModulesService.QuranService.Interfaces;

namespace Tathkīr_WPF.Services.ModulesService.QuranService
{
    public class AudioCacheService : IAudioCacheService
    {
        private readonly string _cacheRoot = Path.Combine(AppContext.BaseDirectory, "Cache");

        public async Task<string> GetOrDownloadAsync(string url, string reader)
        {
            var fileName = Path.GetFileName(url);
            var readerDir = Path.Combine(_cacheRoot, reader.Trim());
            var localPath = Path.Combine(readerDir, fileName);

            if (!File.Exists(localPath))
            {
                Directory.CreateDirectory(readerDir);

                using var client = new HttpClient();
                using var stream = await client.GetStreamAsync(url);
                using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }

            return localPath;
        }
    }

}
