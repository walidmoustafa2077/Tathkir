namespace Tathkīr_API.Services.Interfaces
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string targetLanguage);
        Task<string[]> TranslateAsync(IEnumerable<string> texts, string targetLanguage);
    }

}
