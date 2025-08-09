namespace Tathkīr_API.Services.Interfaces
{
    public interface IKeywordProcessor
    {
        string RemoveKeywords(string input, IEnumerable<string> keywords);
    }

}
