
namespace PhilippWebsite.Client.Clients.FileContent
{
    public class FileContentClient : IFileContentClient
    {
        private readonly ILogger<FileContentClient> _logger;

        private readonly HttpClient _httpClient;


        public FileContentClient(ILogger<FileContentClient> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
        }


        public async Task<string> GetContentAsync(string path)
        {
            string url = $"FileContent/GetContent?path={path}";

            try
            {
                var response = await this._httpClient.GetAsync(url);

                if (response is null)
                {
                    this._logger.LogWarning($"Request return null by '{url}'");

                    return $"Error loading file content.";
                }

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading file content '{Url}'.", url);

                    return await response.Content.ReadAsStringAsync();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading file content '{Url}'.", response.StatusCode, url);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading file content '{Url}'.", url);
            }

            return $"Error loading file content.";
        }
    }
}
