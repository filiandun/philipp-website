using System.Net.Http.Json;


namespace PhilippWebsite.Client.Clients.EditorContent
{
    public class EditorContentClient : IEditorContentClient
    {
        private readonly ILogger<EditorContentClient> _logger;

        private readonly HttpClient _httpClient;


        public EditorContentClient(ILogger<EditorContentClient> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
        }


        public async Task<string[]> GetCodeAsync(string path)
        {
            string url = $"EditorContent/GetContent?path={path}";

            try
            {
                var response = await this._httpClient.GetAsync(url);

                //if (response is null) // TODO null невозможен вроде бы, надо убрать
                //{
                //    this._logger.LogWarning($"Request return null by '{url}'");

                //    return Array.Empty<string>();
                //}

                if (response.IsSuccessStatusCode)
                {
                    this._logger.LogInformation("Success loading editor content '{Url}'.", url);

                    return await response.Content.ReadFromJsonAsync<string[]>() ?? Array.Empty<string>();
                }

                this._logger.LogWarning("Error '{StatusCode}' loading editor content '{Url}'.", response.StatusCode, url);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading editor content '{Url}'.", url);
            }

            return Array.Empty<string>();
        }
    }
}
