
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

                return $"Access Denied '{url}'.";
            }

            string rawUrl = url.Replace("https://github.com/", "https://raw.githubusercontent.com/").Replace("/blob/", "/");

            try
            {
                var response = await this._httpClient.GetAsync(rawUrl);

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading GitHub file content '{Url}'.", rawUrl);

                    return await response.Content.ReadAsStringAsync();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading GitHub file content '{Url}'.", response.StatusCode, rawUrl);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading GitHub file content '{Url}'.", rawUrl);
            }

            return $"Error loading GitHub file content '{rawUrl}'.";
        }
    }
}
