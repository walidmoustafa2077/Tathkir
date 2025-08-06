using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API.Services
{
    public class KeywordProcessor : IKeywordProcessor
    {
        public string RemoveKeywords(string input, IEnumerable<string> keywords)
        {
            foreach (var keyword in keywords)
            {
                input = input.Replace(keyword, "", StringComparison.OrdinalIgnoreCase);
            }
            return input.Trim();
        }
    }

}
