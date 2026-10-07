using PhilippWebsite.Models.SolutionExplorer;


namespace PhilippWebsite.Services.SolutionExplorer
{
    public class SolutionExplorerService
    {
        private readonly ILogger<SolutionExplorerService> _logger;

        private readonly IEnumerable<ITreeProvider> _treeProviders;

        private SolutionExplorerRoot? _cachedExplorerRoot;


        public SolutionExplorerService(ILogger<SolutionExplorerService> logger, IEnumerable<ITreeProvider> treeProviders)
        {
            this._logger = logger;
            
            this._treeProviders = treeProviders;
        }


        public async Task<SolutionExplorerRoot?> GetTreeAsync()
        {
            if (this._cachedExplorerRoot is not null) return this._cachedExplorerRoot;

            SolutionExplorerRoot explorerRoot = new SolutionExplorerRoot();

            foreach (ITreeProvider treeProvider in this._treeProviders)
            {
                SolutionExplorerItem? explorerItem = await treeProvider.GetTreeAsync();

                if (explorerItem is null) 
                {
                    this._logger.LogWarning("Tree Provider '{TreeProvider}' return null", treeProvider.GetType().Name);
                    continue;
                }

                explorerRoot.Items.Add(explorerItem);
            }

            this._cachedExplorerRoot = explorerRoot;

            return explorerRoot;
        }
    }
}
