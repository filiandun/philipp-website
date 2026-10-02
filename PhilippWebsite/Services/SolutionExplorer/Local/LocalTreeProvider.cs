using Microsoft.Extensions.Options;

using PhilippWebsite.Models;
using PhilippWebsite.Models.SolutionExplorer;


namespace PhilippWebsite.Services.SolutionExplorer.Local
{
    public class LocalTreeProvider : ITreeProvider
    {
        private static readonly FileSource _fileSource = FileSource.Local;


        private readonly ILogger<LocalTreeProvider> _logger;

        private readonly LocalTreeConfig _config;

        private readonly HttpClient _httpClient;


        public LocalTreeProvider(ILogger<LocalTreeProvider> logger, IOptions<LocalTreeConfig> options, HttpClient httpClient)
        {
            this._logger = logger;

            this._config = options.Value;

            this._httpClient = httpClient;
        }


        public async Task<SolutionExplorerItem?> GetTreeAsync()
        {
            try
            {
                SolutionExplorerItem explorerSolution = new SolutionExplorerItem()
                {
                    Name = this._config.SolutionName,
                    Path = string.Empty,
                    Repo = string.Empty,
                    Type = SolutionExplorerItemType.Solution,
                    Source = _fileSource
                };

                foreach (string file in this._config.FileList)
                {
                    SolutionExplorerItem explorerItem = new SolutionExplorerItem()
                    {
                        Name = file,
                        Path = file,
                        Repo = this._config.SolutionName,
                        Type = SolutionExplorerItemType.File,
                        Source = _fileSource
                    };

                    explorerSolution.Items.Add(explorerItem);
                }

                return explorerSolution;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading tree from local");
            }

            return null;
        }
    }
}
