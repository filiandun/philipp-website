using Microsoft.Extensions.Caching.Memory;

using PhilippWebsite.Server.Models;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.ProjectTree.GitHub
{
    public class GitHubTreeProvider : ITreeProvider
    {
        private const string FOLDER_TYPE = "tree";
        private const string FILE_TYPE = "blob";

 
        private readonly ILogger<GitHubTreeProvider> _logger;

        private readonly HttpClient _httpClient;

        private readonly IMemoryCache _memoryCache;


        public GitHubTreeProvider(ILogger<GitHubTreeProvider> logger, HttpClient httpClient, IMemoryCache memoryCache)
        {
            this._logger = logger;

            this._httpClient = httpClient;

            this._memoryCache = memoryCache;
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
                Name = GitHubTreeConfig.SolutionName,
                Path = string.Empty,

                Type = SolutionTreeItemType.Solution,
            };

            foreach (var repo in GitHubTreeConfig.RepoList)
            {
                string repoUrl = GitHubTreeConfig.GetApiUrl(repo);

                this._logger.LogDebug("Http request to '{RepoUrl}'.", repoUrl);

                GitHubTreeResponse? githubTree = await this._httpClient.GetFromJsonAsync<GitHubTreeResponse>(repoUrl);

                if (githubTree is null) throw new HttpRequestException("GitHubProjectTreeResponse is null");

                SolutionTreeItem solutionItem = this.MapToProjectTreeItem(githubTree, repo);
                solutionTree.Children.Add(solutionItem);
            }

            this._logger.LogDebug("Root items count {ItemsCount}", solutionTree.Children.Count);

            return solutionTree;
        }


        private SolutionTreeItem MapToProjectTreeItem(GitHubTreeResponse githubTree, string repo)
        {
            Dictionary<string, SolutionTreeItem> treeItemDictionary = new();

            foreach (GitHubTreeItem githubTreeItem in githubTree.LinearTree)
            {
                SolutionTreeItem treeItem = new()
                {
                    Name = githubTreeItem.Path.Split('/').Last(), // TODO maybe exception
                    Path = GitHubTreeConfig.GetGitUrl(repo, githubTreeItem.Path),

                    Type = githubTreeItem.Type == FOLDER_TYPE ? SolutionTreeItemType.Folder : SolutionTreeItemType.File,
                };

                treeItemDictionary[githubTreeItem.Path] = treeItem;
            }


            SolutionTreeItem treeRoot = new()
            {
                Name = repo,
                Path = string.Empty,

                Type = SolutionTreeItemType.Project,
            };

            foreach (GitHubTreeItem githubTreeItem in githubTree.LinearTree)
            {
                SolutionTreeItem treeItem = treeItemDictionary[githubTreeItem.Path];

                int lastSlashIndex = githubTreeItem.Path.LastIndexOf('/');

                if (lastSlashIndex == -1)
                {
                    treeRoot.Children.Add(treeItem);
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

            this.SortTree(treeRoot);

            return treeRoot;
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
