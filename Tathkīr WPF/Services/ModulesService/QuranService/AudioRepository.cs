using Newtonsoft.Json;
using System.IO;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services.ModulesService.QuranService.Interfaces;

namespace Tathkīr_WPF.Services.ModulesService.QuranService
{
    public class AudioRepository : IAudioRepository
    {
        private readonly string _audioFilePath;

        public AudioRepository()
        {
            _audioFilePath = Path.Combine(App.baseDir, "QuranAudios.json");
        }

        public SurahAudio LoadAudioData()
        {
            if (!File.Exists(_audioFilePath))
                return new SurahAudio();

            var json = File.ReadAllText(_audioFilePath);
            return JsonConvert.DeserializeObject<SurahAudio>(json)
                   ?? new SurahAudio();
        }
    }
}
