using System.Net.Http.Json;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Clients.SolutionState
{
    public class SolutionStateClient : ISolutionStateClient
    {
        private readonly ILogger<SolutionStateClient> _logger;

        private readonly HttpClient _httpClient;


        public SolutionStateClient(ILogger<SolutionStateClient> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
        }


        public async Task<SolutionStateModel?> GetModel()
        {
            string url = "SolutionState/GetModel/";

            try
            {
                var response = await this._httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading Solution State '{Url}'.", url);

                    return await response.Content.ReadFromJsonAsync<SolutionStateModel>();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading Solution State '{Url}'.", response.StatusCode, url);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading Solution State '{Url}'.", url);
            }

            return null;
        }
    }
}
