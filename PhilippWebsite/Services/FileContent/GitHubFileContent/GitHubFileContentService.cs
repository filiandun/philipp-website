using Microsoft.Extensions.Options;


namespace PhilippWebsite.Services.FileContent.GitHubFileContent
{
    public class GitHubFileContentService : IFileContentService
    {
        private readonly ILogger<GitHubFileContentService> _logger;

        private readonly GitHubRawConfig _config;

        private readonly HttpClient _httpClient;


        public GitHubFileContentService(ILogger<GitHubFileContentService> logger, IOptions<GitHubRawConfig> options, HttpClient httpClient)
        {
            this._logger = logger;

            this._config = options.Value;

            this._httpClient = httpClient;  
        }


        public async Task<string> GetFileContentAsync(string repo, string filePath)
        {
            string fileUrl = this._config.BaseUrl.Replace("{repo}", repo).Replace("{file}", filePath);

            try
            {
                var response = await this._httpClient.GetAsync(fileUrl);

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading file content '{FilePath}'.", filePath);

                    return await response.Content.ReadAsStringAsync();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading file content '{FileUrl}'.", response.StatusCode, fileUrl);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading file content '{FileUrl}'.", fileUrl);

            }

            return $"Error loading file content '{filePath}'";
        }
    }
}
