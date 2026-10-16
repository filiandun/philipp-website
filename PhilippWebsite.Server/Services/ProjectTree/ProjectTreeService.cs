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

        // TODO тут надо подумать над возвращаемым значением
        public async Task<SolutionTreeRoot> BuildTreeAsync()
        {
            try
            {
                SolutionTreeRoot treeRoot = new SolutionTreeRoot();

                foreach (ITreeProvider treeProvider in this._treeProviders)
                {
                    SolutionTreeItem? treeItem = await treeProvider.GetTreeAsync();

                    if (treeItem is null)
                    {
                        this._logger.LogWarning("Tree Provider '{TreeProvider}' return null.", treeProvider.GetType().Name);
                        continue;
                    }

                    treeRoot.Items.Add(treeItem);
                }

                return treeRoot;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to load Project Tree Root.");

                this._rootInitTask = null;
                throw;
            }
        }

        private Task<SolutionTreeRoot> GetOrStartInitTask() => this._rootInitTask ??= this.BuildTreeAsync();
    }
}
