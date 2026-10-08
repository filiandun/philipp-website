using PhilippWebsite.Models.SolutionExplorer;


namespace PhilippWebsite.Services.SolutionExplorer
{
    public class SolutionExplorerService
    {
        private readonly ILogger<SolutionExplorerService> _logger;

        private readonly IEnumerable<ITreeProvider> _treeProviders;

        private Task<SolutionExplorerRoot>? _rootInitTask;


        public SolutionExplorerService(ILogger<SolutionExplorerService> logger, IEnumerable<ITreeProvider> treeProviders)
        {
            this._logger = logger;
            
            this._treeProviders = treeProviders;
        }


        public Task PreloadAsync()
        {
            this._logger.LogDebug("Preload Solution Explorer Root");

            return this.GetOrStartInitTask();
        }


        public Task<SolutionExplorerRoot> GetTreeAsync()
        {
            this._logger.LogDebug("Get Solution Explorer Root.");

            return this.GetOrStartInitTask();
        }

        public async Task<SolutionExplorerRoot> BuildTreeAsync()
        {
            try
            {
                SolutionExplorerRoot explorerRoot = new SolutionExplorerRoot();

                foreach (ITreeProvider treeProvider in this._treeProviders)
                {
                    SolutionExplorerItem? explorerItem = await treeProvider.GetTreeAsync();

                    if (explorerItem is null)
                    {
                        this._logger.LogWarning("Tree Provider '{TreeProvider}' return null.", treeProvider.GetType().Name);
                        continue;
                    }

                    explorerRoot.Items.Add(explorerItem);
                }

                return explorerRoot;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to load Solution Explorer Root.");

                this._rootInitTask = null;
                throw;
            }
        }

        private Task<SolutionExplorerRoot> GetOrStartInitTask() => this._rootInitTask ??= this.BuildTreeAsync();
    }
}
