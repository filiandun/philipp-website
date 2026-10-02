using Microsoft.Extensions.Options;

using PhilippWebsite.Models;


namespace PhilippWebsite.Services.FileContent
{
    public class FileContentService : IFileContentService
    {
        private readonly ILogger<FileContentService> _logger;

        private readonly FileContentConfig _config;

        private readonly HttpClient _httpClient;


        public FileContentService(ILogger<FileContentService> logger, IOptions<FileContentConfig> options, HttpClient httpClient)
        {
            this._logger = logger;

            this._config = options.Value;

            this._httpClient = httpClient;  
        }


        public async Task<string> GetContentAsync(string repo, string path, FileSource source)
        {
            string fileUrl = source switch
            {
                FileSource.GitHub => string.Format(this._config.GitHubUrl, repo, path),
                FileSource.Local => string.Format(this._config.LocalUrl, path),

                _ => throw new ArgumentOutOfRangeException(nameof(source))
            };

            return await this.HttpRequestAsync(fileUrl);
        }


        private async Task<string> HttpRequestAsync(string url)
        {
            try
            {
                var response = await this._httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading file content '{FileUrl}'.", url);

                    return await response.Content.ReadAsStringAsync();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading file content '{FileUrl}'.", response.StatusCode, url);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading file content '{FileUrl}'.", url);

            }

            return $"Error loading file content '{url}'.";
        } 
    }
}
