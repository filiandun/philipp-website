using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Options;


namespace PhilippWebsite.Server.Services.FileContent.FileSource
{
    public class GitHubFileSource : IFileSource
    {
        private readonly ILogger<GitHubFileSource> _logger;

        private readonly HttpClient _httpClient;

        private readonly GitHubContentOptions _options;

        public GitHubFileSource(ILogger<GitHubFileSource> logger, HttpClient httpClient, IOptions<GitHubContentOptions> options)
        {
            this._logger = logger;

            this._httpClient = httpClient;

            this._options = options.Value;
        }


        public bool CanHandle(string url) => url.StartsWith(GitHubContentOptions.WebBaseUrl);


        public async Task<string> GetRawContentAsync(string url)
        {
            if (!this.IsValidUrl(url))
            {
                this._logger.LogWarning("SSRF attempt detected '{Url}'", url);

                throw new UnauthorizedAccessException("Invalid GitHub URL scope.");
            }

            string rawUrl = url.Replace(GitHubContentOptions.WebBaseUrl, GitHubContentOptions.RawBaseUrl).Replace("/blob/", "/");
            this._logger.LogDebug(rawUrl);

            var response = await this._httpClient.GetAsync(rawUrl);

            if (response.IsSuccessStatusCode)
            {
                this._logger.LogInformation("Success loading GitHub file content '{Url}'.", rawUrl);

                return await response.Content.ReadAsStringAsync();
            }

            this._logger.LogWarning("GitHub returned {StatusCode} for '{Url}'.", response.StatusCode, rawUrl);

            throw new HttpRequestException($"GitHub returned error {response.StatusCode}");
        }


        private bool IsValidUrl(string url)
        {
            string baseUrl = PathHelper.Combine(GitHubContentOptions.WebBaseUrl, this._options.Username);

            foreach (string repo in this._options.Repositories.Keys)
            {
                if (url.StartsWith(PathHelper.Combine(baseUrl, repo))) return true;
            }

            return false;
        }
    }
}
