using System.Text.Json;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API.Services
{
    public class GoogleTranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private const string TranslateUrl = "https://translation.googleapis.com/language/translate/v2";
        private const string ApiKey = "AIzaSyA8FDUt9a83GZHoJ8Z8rq1HdkicFDkQvJY";

        public GoogleTranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TranslateAsync(string text, string targetLanguage)
        {
            var results = await TranslateAsync(new[] { text }, targetLanguage);
            return results.FirstOrDefault() ?? string.Empty;
        }

        public async Task<string[]> TranslateAsync(IEnumerable<string> texts, string targetLanguage)
        {
            const int MaxBatchSize = 128;

            var textList = texts.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            var result = new List<string>();

            for (int i = 0; i < textList.Count; i += MaxBatchSize)
            {
                var batch = textList.Skip(i).Take(MaxBatchSize).ToList();
                var translatedBatch = await TranslateBatchAsync(batch, targetLanguage);
                result.AddRange(translatedBatch);
            }

            return result.ToArray();
        }

        private async Task<string[]> TranslateBatchAsync(IEnumerable<string> texts, string targetLanguage)
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new("target", targetLanguage),
                new("format", "text"),
                new("key", ApiKey)
            };

            foreach (var text in texts)
            {
                data.Add(new("q", text));
            }

            var content = new FormUrlEncodedContent(data);

            var response = await _httpClient.PostAsync(TranslateUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Google Translate API failed: {response.StatusCode}, Body: {result}");

            using var jsonDoc = JsonDocument.Parse(result);

            return jsonDoc.RootElement
                .GetProperty("data")
                .GetProperty("translations")
                .EnumerateArray()
                .Select(t => t.GetProperty("translatedText").GetString() ?? "")
                .ToArray();
        }


    }

}
