using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tathkīr_WPF.Managers
{
    public sealed class GitHubReleaseManager
    {
        private static readonly HttpClient _http = CreateClient();

        private readonly string _owner;
        private readonly string _repo;

        public GitHubReleaseManager(string owner, string repo)
        {
            _owner = owner;
            _repo = repo;
        }

        public async Task<UpdateCheckResult> CheckForUpdateAsync(Version currentVersion, CancellationToken ct = default)
        {
            var url = $"https://api.github.com/repos/{_owner}/{_repo}/releases/latest";

            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            // Optional: ETag caching to reduce calls. Persist this string somewhere (e.g., user settings).
            // var etag = LoadEtag();
            // if (!string.IsNullOrWhiteSpace(etag)) req.Headers.TryAddWithoutValidation("If-None-Match", etag);

            using var res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);

            if ((int)res.StatusCode == 304) // Not Modified (if you used ETag)
                return new UpdateCheckResult(false, currentVersion, null, null, null);

            res.EnsureSuccessStatusCode();

            // var newEtag = res.Headers.ETag?.Tag; SaveEtag(newEtag);

            await using var stream = await res.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            var latest = await JsonSerializer.DeserializeAsync<GitHubRelease>(stream, GitHubRelease.JsonOpts, ct).ConfigureAwait(false);
            if (latest is null || string.IsNullOrWhiteSpace(latest.TagName))
                return new UpdateCheckResult(false, currentVersion, null, null, null);

            // Tags are typically like "v1.0.0". Trim the leading 'v' if present.
            var latestVersion = ParseVersion(latest.TagName);
            var isNewer = latestVersion > currentVersion;

            return new UpdateCheckResult(
                isNewer,
                latestVersion,
                latest.HtmlUrl ?? $"https://github.com/{_owner}/{_repo}/releases",
                latest.TagName,
                latest.Name
            );
        }

        public static Version GetCurrentAppVersion()
        {
            // Prefer InformationalVersion (supports SemVer); fall back to AssemblyName.Version
            var inf = Assembly.GetExecutingAssembly()
                              .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var v = ParseVersion(inf ?? "");
            if (v == new Version(0, 0)) // fallback
                v = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0);
            return v;
        }

        private static Version ParseVersion(string tagOrVersion)
        {
            if (string.IsNullOrWhiteSpace(tagOrVersion))
                return new Version(0, 0);

            var s = tagOrVersion.Trim();
            if (s.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                s = s[1..];

            // Strip any +metadata or -prerelease for System.Version
            var plus = s.IndexOf('+'); if (plus >= 0) s = s[..plus];
            var dash = s.IndexOf('-'); if (dash >= 0) s = s[..dash];

            return Version.TryParse(s, out var v) ? v : new Version(0, 0);
        }

        private static HttpClient CreateClient()
        {
            var c = new HttpClient();
            c.DefaultRequestHeaders.UserAgent.ParseAdd("TathkirWPF/1.0 (+https://github.com/walidmoustafa2077/Tathkir)");
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            c.DefaultRequestHeaders.TryAddWithoutValidation("X-GitHub-Api-Version", "2022-11-28");
            // Do NOT embed a PAT in a desktop app. If you must increase rate limits,
            // call your own backend which adds auth server-side.
            return c;
        }

        private sealed class GitHubRelease
        {
            [JsonPropertyName("tag_name")] public string? TagName { get; init; }
            [JsonPropertyName("name")] public string? Name { get; init; }
            [JsonPropertyName("html_url")] public string? HtmlUrl { get; init; }

            public static readonly JsonSerializerOptions JsonOpts = new()
            {
                PropertyNameCaseInsensitive = true
            };
        }


        public sealed record UpdateCheckResult(
            bool IsUpdateAvailable,
            Version CurrentLatest,
            string? ReleaseUrl,
            string? Tag,
            string? Title
        );
    }


}
