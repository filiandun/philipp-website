using Microsoft.Extensions.Caching.Memory;

using PhilippWebsite.Server.Models;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.ProjectTree.Local
{
    public class LocalTreeProvider : ITreeProvider
    {
        private readonly ILogger<LocalTreeProvider> _logger;

        private readonly IMemoryCache _memoryCache;


        public LocalTreeProvider(ILogger<LocalTreeProvider> logger, IMemoryCache memoryCache)
        {
            this._logger = logger;

            this._memoryCache = memoryCache;
        }


        public async Task<SolutionTreeItem?> GetTreeAsync()
        {
            if (this._memoryCache.TryGetValue(CacheKeys.LOCAL_PROJECT_TREE, out SolutionTreeItem? cachedTree) && cachedTree is not null)
            {
                this._logger.LogInformation("Local tree loaded from cache.");

                return cachedTree;
            }

            this._logger.LogInformation("Cache miss. Fetching Local tree from drive.");

            SolutionTreeItem? projectTree = await this.BuildTree();

            if (projectTree is not null)
            {
                this._memoryCache.Set(CacheKeys.LOCAL_PROJECT_TREE, projectTree, TimeSpan.FromHours(1));
                this._logger.LogInformation("Local tree successfully fetched and cached.");
            }
            else
            {
                this._logger.LogInformation("BuildTree returned null. Local tree was not cached.");
            }

            return projectTree;
        }

        private async Task<SolutionTreeItem?> BuildTree()
        {
            try
            {
                SolutionTreeItem treeSolution = new()
                {
                    Name = LocalTreeConfig.SolutionName,
                    Path = string.Empty,

                    Type = SolutionTreeItemType.Solution,
                };

                foreach (string file in LocalTreeConfig.FileList)
                {
                    SolutionTreeItem treeItem = new()
                    {
                        Name = file,
                        Path = LocalTreeConfig.GetBaseUrl(file),

                        Type = SolutionTreeItemType.File,
                    };

                    treeSolution.Children.Add(treeItem);
                }

                this._logger.LogDebug("Root items count {ItemsCount}", treeSolution.Children.Count);

                return treeSolution;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading tree from local");
                throw;
            }
        }
    }
}
