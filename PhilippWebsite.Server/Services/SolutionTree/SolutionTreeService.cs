using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Options;
using PhilippWebsite.Server.Services.SolutionTree.TreeProvider;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionTree
{
    public class SolutionTreeService : ISolutionTreeService
    {
        private readonly ILogger<SolutionTreeService> _logger;

        private readonly IEnumerable<ITreeProvider> _treeProviders;

        private readonly SolutionStateOptions _options;

        private Task<SolutionTreeItem>? _rootInitTask;


        public SolutionTreeService(ILogger<SolutionTreeService> logger, IEnumerable<ITreeProvider> treeProviders, IOptions<SolutionStateOptions> options)
        {
            this._logger = logger;
            
            this._treeProviders = treeProviders;

            this._options = options.Value;
        }


        public Task PreloadAsync()
        {
            this._logger.LogDebug("Preload Project Tree Root");

            return this.GetOrStartInitTask();
        }


        public Task<SolutionTreeItem> GetTreeAsync()
        {
            this._logger.LogDebug("Get Project Tree Root.");

            return this.GetOrStartInitTask();
        }


        public async Task<SolutionTreeItem> BuildTreeAsync()
        {
            SolutionTreeItem solutionTree = new SolutionTreeItem()
            {
                Name = this._options.SolutionName,
                TreePath = this._options.SolutionName,

                Type = SolutionTreeItemType.Solution,
            };

            foreach (ITreeProvider treeProvider in this._treeProviders)
            {
                SolutionTreeItem treeItem = await treeProvider.GetTreeAsync();

                this._logger.LogDebug("Tree Provider '{TreeProvider}' return tree.", treeProvider.GetType().Name);

                solutionTree.Children.Add(treeItem);
            }

            return solutionTree;
        }

        private Task<SolutionTreeItem> GetOrStartInitTask() => this._rootInitTask ??= this.BuildTreeAsync();
    }
}
