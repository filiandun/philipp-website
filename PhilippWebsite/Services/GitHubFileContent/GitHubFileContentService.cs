

namespace PhilippWebsite.Services.GitHubFileContent
{
    public class GitHubFileContentService
    {
        private const string URL = "https://raw.githubusercontent.com/filiandun/philipp-website/main";

        private readonly ILogger<GitHubFileContentService> _logger;

        private readonly HttpClient _httpClient;


        public GitHubFileContentService(ILogger<GitHubFileContentService> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;  
        }


        public async Task<string> GetFileContentAsync(string filePath)
        {
            string url = $"{URL}/{filePath}";

            try
            {
                var response = await this._httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading file content '{FilePath}'.", filePath);

                    return await response.Content.ReadAsStringAsync();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading file content '{Url}'.", response.StatusCode, url);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading file content '{Url}'.", url);

            }

            return $"Error loading file content '{filePath}'";
        }
    }
}
