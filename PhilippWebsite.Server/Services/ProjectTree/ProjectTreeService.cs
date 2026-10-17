using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.ProjectTree
{
    public class ProjectTreeService : IProjectTreeService
    {
        private readonly ILogger<ProjectTreeService> _logger;

        private readonly IEnumerable<ITreeProvider> _treeProviders;

        private Task<SolutionTreeRoot>? _rootInitTask;


        public ProjectTreeService(ILogger<ProjectTreeService> logger, IEnumerable<ITreeProvider> treeProviders)
        {
            this._logger = logger;
            
            this._treeProviders = treeProviders;
        }


        public Task PreloadAsync()
        {
            this._logger.LogDebug("Preload Project Tree Root");

            return this.GetOrStartInitTask();
        }


        public Task<SolutionTreeRoot> GetTreeAsync()
        {
            this._logger.LogDebug("Get Project Tree Root.");

            return this.GetOrStartInitTask();
        }


        public async Task<SolutionTreeRoot> BuildTreeAsync()
        {
            SolutionTreeRoot treeRoot = new SolutionTreeRoot();

            foreach (ITreeProvider treeProvider in this._treeProviders)
            {
                SolutionTreeItem treeItem = await treeProvider.GetTreeAsync();

                this._logger.LogDebug("Tree Provider '{TreeProvider}' return tree.", treeProvider.GetType().Name);

                treeRoot.Items.Add(treeItem);
            }

            return treeRoot;
        }

        private Task<SolutionTreeRoot> GetOrStartInitTask() => this._rootInitTask ??= this.BuildTreeAsync();
    }
}
