using System.Text.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

using PhilippWebsite.Models;


namespace PhilippWebsite.Services
{
    internal sealed class ResumeContentService
    {
        private readonly ILogger<ResumeContentService> _logger;

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ResumeContent? ResumeContent { get; private set; }

        public bool IsLoaded => this.ResumeContent != null;

        
        public ResumeContentService(ILogger<ResumeContentService> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
            this._serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            this.ResumeContent = null;
        }


        public async Task InitializeAsync() // TODO обработка нормальная нужна
        {
            if (this.IsLoaded) return;

            this.ResumeContent = await this._httpClient.GetFromJsonAsync<ResumeContent>("content/resume.json", this._serializerOptions);
        }
    }
}
