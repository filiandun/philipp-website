using System.Net.Http.Json;

using PhilippWebsite.Models;


namespace PhilippWebsite.Services
{
    public class GitHubProjectExplorerService : IProjectExplorerService
    {
        private const string URL = $"https://api.github.com/repos/filiandun/philipp-website/git/trees/main?recursive=1";

        private readonly ILogger<GitHubProjectExplorerService> _logger;

        private readonly HttpClient _httpClient;


        public GitHubProjectExplorerService(ILogger<GitHubProjectExplorerService> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;

            if (!this._httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                this._httpClient.DefaultRequestHeaders.Add("User-Agent", "PhilippPortfolioApp");
            }
        }


        // TODO сделать промежуточную DTO, так как иначе смысла в интерфейсе нет
        public async Task<GitHubTreeResponse?> GetProjectTreeContentAsync()
        {
            try
            {
                return await this._httpClient.GetFromJsonAsync<GitHubTreeResponse>(URL);
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "Error loading project from GitHub");
                return null;
            }
        }
    }
}
