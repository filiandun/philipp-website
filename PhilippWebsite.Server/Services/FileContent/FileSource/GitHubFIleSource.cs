
namespace PhilippWebsite.Server.Services.FileContent.FileSource
{
    public class GitHubFileSource : IFileSource
    {
        private readonly ILogger<GitHubFileSource> _logger;

        private readonly HttpClient _httpClient;


        public GitHubFileSource(ILogger<GitHubFileSource> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
        }


        public bool CanHandle(string url) => url.StartsWith("https://github.com/filiandun/");


        public async Task<string> GetRawContentAsync(string url)
        {
            if (!url.StartsWith("https://github.com/filiandun/"))
            {
                this._logger.LogWarning("SSRF attempt detected '{Url}'", url);

                throw new UnauthorizedAccessException("Invalid GitHub URL scope.");
            }

            string rawUrl = url.Replace("https://github.com/", "https://raw.githubusercontent.com/").Replace("/blob/", "/");

            var response = await this._httpClient.GetAsync(rawUrl);

            if (response.IsSuccessStatusCode)
            {
                this._logger.LogInformation("Success loading GitHub file content '{Url}'.", rawUrl);

                return await response.Content.ReadAsStringAsync();
            }

            this._logger.LogWarning("GitHub returned {StatusCode} for '{Url}'.", response.StatusCode, rawUrl);

            throw new HttpRequestException($"GitHub returned error {response.StatusCode}");
        }
    }
}
