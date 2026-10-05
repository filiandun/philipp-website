using PhilippWebsite.Models;
using PhilippWebsite.Models.SolutionExplorer;


namespace PhilippWebsite.Services.SolutionExplorer.Local
{
    public class LocalTreeProvider : ITreeProvider
    {
        private static readonly FileSource _fileSource = FileSource.Local;

        private readonly ILogger<LocalTreeProvider> _logger;


        public LocalTreeProvider(ILogger<LocalTreeProvider> logger)
        {
            this._logger = logger;
        }


        public async Task<SolutionExplorerItem?> GetTreeAsync()
        {
            try
            {
                SolutionExplorerItem explorerSolution = new SolutionExplorerItem()
                {
                    Name = LocalTreeConfig.SolutionName,
                    Path = string.Empty,
                    Repo = string.Empty,
                    Type = SolutionExplorerItemType.Solution,
                    Source = _fileSource
                };

                foreach (string file in LocalTreeConfig.FileList)
                {
                    SolutionExplorerItem explorerItem = new SolutionExplorerItem()
                    {
                        Name = file,
                        Path = file,
                        Repo = string.Empty,
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
