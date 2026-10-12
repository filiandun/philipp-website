using System.Net.Http.Json;

using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Client.Clients.ProjectTree
{
    public class ProjectTreeClient : IProjectTreeClient
    {
        private readonly ILogger<ProjectTreeClient> _logger;

        private readonly HttpClient _httpClient;


        public ProjectTreeClient(ILogger<ProjectTreeClient> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
        }


        public async Task<ProjectTreeRoot?> GetTreeAsync()
        {
            string url = "ProjectTree/GetTree/";

            try
            {
                var response = await this._httpClient.GetAsync(url);

                if (response is null)
                {
                    this._logger.LogWarning($"Request return null by '{url}'");

                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading project tree '{Url}'.", url);

                    return await response.Content.ReadFromJsonAsync<ProjectTreeRoot>();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading project tree '{Url}'.", response.StatusCode, url);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading project tree '{Url}'.", url);
            }

            return null;
        }
    }
}
