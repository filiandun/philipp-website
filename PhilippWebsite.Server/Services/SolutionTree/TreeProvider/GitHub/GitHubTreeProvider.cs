using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Models;
using PhilippWebsite.Server.Options;
using PhilippWebsite.Server.Services.SolutionTree.TreeProvider;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionTree.TreeProvider.GitHub
{
    public class GitHubTreeProvider : ITreeProvider
    {
        private const string FOLDER_TYPE = "tree";
        private const string FILE_TYPE = "blob";

 
        private readonly ILogger<GitHubTreeProvider> _logger;

        private readonly HttpClient _httpClient;

        private readonly IMemoryCache _memoryCache;

        private readonly GitHubContentOptions _options;


        public GitHubTreeProvider(ILogger<GitHubTreeProvider> logger, HttpClient httpClient, IMemoryCache memoryCache, IOptions<GitHubContentOptions> options)
        {
            this._logger = logger;

            this._httpClient = httpClient;

            this._memoryCache = memoryCache;

            this._options = options.Value;
        }


        public async Task<SolutionTreeItem> GetTreeAsync()
        {
            if (this._memoryCache.TryGetValue(CacheKeys.GITHUB_PROJECT_TREE, out SolutionTreeItem? cachedTree) && cachedTree is not null)
            {
                this._logger.LogInformation("GitHub tree loaded from cache.");

                return cachedTree;
            }

            this._logger.LogInformation("Cache miss. Fetching GitHub tree from API.");

            SolutionTreeItem solutionTree = await this.BuildTree();

            this._memoryCache.Set(CacheKeys.GITHUB_PROJECT_TREE, solutionTree, TimeSpan.FromHours(1));
            this._logger.LogInformation("GitHub tree successfully fetched and cached.");
 
            return solutionTree;
        }


        private async Task<SolutionTreeItem> BuildTree()
        {
            SolutionTreeItem solutionTree = new()
            {
                Name = this._options.SectionName,
                TreePath = this._options.SectionName,

                Type = this._options.SectionType,
            };

            foreach (var repo in this._options.Repositories)
            {
                string repoUrl = this._options.GetApiUrl(repo.Key);

                this._logger.LogDebug("Http request to '{RepoUrl}'.", repoUrl);

                HttpResponseMessage response = await this._httpClient.GetAsync(repoUrl);

                if (!response.IsSuccessStatusCode)
                {
                    this._logger.LogWarning("Skip repo '{Repo}'. GitHub returned {StatusCode}.", repo.Key, response.StatusCode);
                    continue;
                }

                GitHubTreeResponse? githubTree = await response.Content.ReadFromJsonAsync<GitHubTreeResponse>();

                if (githubTree is null)
                {
                    this._logger.LogWarning("Skip repo '{Repo}'. GitHub tree is null.", repo.Key);
                    continue;
                }

                SolutionTreeItem solutionItem = this.MapToSolutionTreeItem(githubTree, repo);
                solutionTree.Children.Add(solutionItem);
            }

            this._logger.LogDebug("Solution items count {ItemsCount}", solutionTree.Children.Count);

            return solutionTree;
        }


        private SolutionTreeItem MapToSolutionTreeItem(GitHubTreeResponse githubTree, KeyValuePair<string, SolutionTreeItemType> repo)
        {
            Dictionary<string, SolutionTreeItem> treeItemDictionary = new();

            foreach (GitHubTreeItem githubTreeItem in githubTree.LinearTree)
            {
                string itemName = githubTreeItem.Path.Split('/').Last(); // TODO maybe exception

                SolutionTreeItem treeItem = new()
                {
                    Name = itemName, 
                    TreePath = PathHelper.Combine(this._options.SectionName, itemName), // githubTreeItem.Path
                    ContentPath = this._options.GetWebUrl(repo.Key, githubTreeItem.Path),

                    Type = githubTreeItem.Type == FOLDER_TYPE ? SolutionTreeItemType.Folder : SolutionTreeItemType.File,
                };

                treeItemDictionary[githubTreeItem.Path] = treeItem;
            }


            SolutionTreeItem solutionTree = new()
            {
                Name = repo.Key,
                TreePath = PathHelper.Combine(this._options.SectionName, repo.Key),

                Type = repo.Value,
            };

            foreach (GitHubTreeItem githubTreeItem in githubTree.LinearTree)
            {
                SolutionTreeItem treeItem = treeItemDictionary[githubTreeItem.Path];

                int lastSlashIndex = githubTreeItem.Path.LastIndexOf('/');

                if (lastSlashIndex == -1)
                {
                    solutionTree.Children.Add(treeItem);
                }
                else
                {
                    string parentPath = githubTreeItem.Path.Substring(0, lastSlashIndex);

                    if (treeItemDictionary.TryGetValue(parentPath, out var parentItem))
                    {
                        parentItem.Children.Add(treeItem);
                    }
                }
            }

            this.SortTree(solutionTree);

            return solutionTree;
        }

        private void SortTree(SolutionTreeItem treeItem)
        {
            treeItem.Children.Sort((a, b) =>
            {
                if (a.Type != b.Type) return a.Type.CompareTo(b.Type);

                return a.Name.CompareTo(b.Name);
            });

            foreach (SolutionTreeItem item in treeItem.Children.Where(n => n.Children.Any()))
            {
                this.SortTree(item);
            }
        }
    }
}
