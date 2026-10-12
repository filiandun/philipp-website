
namespace PhilippWebsite.Server.Services.FileContent
{
    public class FileContentService : IFileContentService
    {
        private readonly ILogger<FileContentService> _logger;

        private readonly IWebHostEnvironment _environment;
        private readonly HttpClient _httpClient;


        public FileContentService(ILogger<FileContentService> logger, IWebHostEnvironment environment, HttpClient httpClient)
        {
            this._logger = logger;

            this._environment = environment;
            this._httpClient = httpClient;  
        }


        public async Task<string> GetContentAsync(string path)
        {
            if (path.StartsWith("http"))
            {
                return await this.GetGitHubContentAsync(path);
            }
            else
            {
                return await this.GetLocalContentAsync(path);
            }
        }


        private async Task<string> GetGitHubContentAsync(string url)
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


        private async Task<string> GetLocalContentAsync(string path)
        {
            string safePath = Path.GetFullPath(Path.Combine(this._environment.ContentRootPath, "LocalData"));
            if (!safePath.EndsWith(Path.DirectorySeparatorChar)) safePath += Path.DirectorySeparatorChar;
            string targetPath = Path.GetFullPath(Path.Combine(safePath, path));

            if (!targetPath.StartsWith(safePath))
            {
                this._logger.LogWarning("Path traversal attempt detected '{Path}'", targetPath);

                return $"Access Denied '{targetPath}'.";
            }

            if (!File.Exists(targetPath))
            {
                this._logger.LogWarning("Local file not found '{Path}'.", targetPath);

                return $"File not found '{targetPath}'.";
            }

            try
            {
                string fileContent = await File.ReadAllTextAsync(targetPath);

                this._logger.LogInformation("Success loading local file content '{Path}'.", targetPath);

                return fileContent;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading local file content '{Path}'.", targetPath);
            }

            return $"Error loading local file content '{targetPath}'.";
        }

    }
}
