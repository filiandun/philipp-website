using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Server.Services.ProjectTree
{
    public class ProjectTreeService : IProjectTreeService
    {
        private readonly ILogger<ProjectTreeService> _logger;

        private readonly IEnumerable<ITreeProvider> _treeProviders;

        private Task<ProjectTreeRoot>? _rootInitTask;


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


        public Task<ProjectTreeRoot> GetTreeAsync()
        {
            this._logger.LogDebug("Get Project Tree Root.");

            return this.GetOrStartInitTask();
        }

        // TODO тут надо подумать над возвращаемым значением
        public async Task<ProjectTreeRoot> BuildTreeAsync()
        {
            try
            {
                ProjectTreeRoot treeRoot = new ProjectTreeRoot();

                foreach (ITreeProvider treeProvider in this._treeProviders)
                {
                    ProjectTreeItem? treeItem = await treeProvider.GetTreeAsync();

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

        private Task<ProjectTreeRoot> GetOrStartInitTask() => this._rootInitTask ??= this.BuildTreeAsync();
    }
}
