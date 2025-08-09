using System.Text.Json;

namespace Tathkīr_API.Configs
{
    public record ConfigsSettings
    {
        public string ConfigFilePath { get; set; } = string.Empty;
    }


    public record EnvironmentConfig
    {
        public string GoogleTranslationAPI { get; set; } = string.Empty;
    }

}
