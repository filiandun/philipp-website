using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Models;
using PhilippWebsite.Server.Options;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.ProjectTree.Local
{
    public class LocalTreeProvider : ITreeProvider
    {
        private readonly ILogger<LocalTreeProvider> _logger;

        private readonly IMemoryCache _memoryCache;

        private readonly LocalContentOptions _options;


        public LocalTreeProvider(ILogger<LocalTreeProvider> logger, IMemoryCache memoryCache, IOptions<LocalContentOptions> options)
        {
            this._logger = logger;

            this._memoryCache = memoryCache;

            this._options = options.Value;
        }


        public async Task<SolutionTreeItem> GetTreeAsync()
        {
            if (this._memoryCache.TryGetValue(CacheKeys.LOCAL_PROJECT_TREE, out SolutionTreeItem? cachedTree) && cachedTree is not null)
            {
                this._logger.LogInformation("Local tree loaded from cache.");

                return cachedTree;
            }

            this._logger.LogInformation("Cache miss. Fetching Local tree from drive.");

            SolutionTreeItem? solutionTree = await this.BuildTree();

            this._memoryCache.Set(CacheKeys.LOCAL_PROJECT_TREE, solutionTree, TimeSpan.FromHours(1));
            this._logger.LogInformation("Local tree successfully fetched and cached.");

            return solutionTree;
        }

        private async Task<SolutionTreeItem> BuildTree()
        {
            SolutionTreeItem rootTree = new()
            {
                Name = this._options.SectionName,
                Path = string.Empty,

                Type = SolutionTreeItemType.Solution,
            };

            foreach (string file in this._options.Files)
            {
                SolutionTreeItem treeItem = new()
                {
                    Name = file,
                    Path = this._options.GetPath(file),

                    Type = SolutionTreeItemType.File,
                };

                rootTree.Children.Add(treeItem);
            }

            this._logger.LogDebug("Root items count {ItemsCount}", rootTree.Children.Count);

            return rootTree;
        }
    }
}
