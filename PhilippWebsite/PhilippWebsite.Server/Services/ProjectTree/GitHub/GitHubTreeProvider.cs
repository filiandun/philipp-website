using Microsoft.Extensions.Caching.Memory;

using PhilippWebsite.Server.Models;
using PhilippWebsite.Shared.ProjectTree;


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


        public async Task<ProjectTreeItem?> GetTreeAsync()
        {
            if (this._memoryCache.TryGetValue(CacheKeys.GITHUB_PROJECT_TREE, out ProjectTreeItem? cachedTree) && cachedTree is not null)
            {
                this._logger.LogInformation("GitHub tree loaded from cache.");

                return cachedTree;
            }

            this._logger.LogInformation("Cache miss. Fetching GitHub tree from API.");

            ProjectTreeItem? projectTree = await this.BuildTree();

            if (projectTree is not null)
            {
                this._memoryCache.Set(CacheKeys.GITHUB_PROJECT_TREE, projectTree, TimeSpan.FromHours(1));
                this._logger.LogInformation("GitHub tree successfully fetched and cached.");
            }
            else
            {
                this._logger.LogInformation("BuildTree returned null. GitHub tree was not cached.");
            }

            return projectTree;
        }


        private async Task<ProjectTreeItem?> BuildTree()
        {
            try
            {
                ProjectTreeItem treeSolution = new()
                {
                    Name = GitHubTreeConfig.SolutionName,
                    Path = string.Empty,

                    Type = ProjectTreeItemType.Solution,
                };

                foreach (var repo in GitHubTreeConfig.RepoList)
                {
                    string repoUrl = GitHubTreeConfig.GetApiUrl(repo);

                    this._logger.LogDebug("Http request to '{RepoUrl}'.", repoUrl);

                    GitHubTreeResponse? githubTree = await this._httpClient.GetFromJsonAsync<GitHubTreeResponse>(repoUrl);

                    if (githubTree is null) throw new HttpRequestException("GitHubProjectTreeResponse is null");

                    ProjectTreeItem explorerItem = this.MapToProjectTreeItem(githubTree, repo);
                    treeSolution.Children.Add(explorerItem);
                }

                this._logger.LogDebug("Root items count {ItemsCount}", treeSolution.Children.Count);

                return treeSolution;
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "Error loading tree from GitHub");
                throw;
            }
        }


        private ProjectTreeItem MapToProjectTreeItem(GitHubTreeResponse githubTree, string repo)
        {
            Dictionary<string, ProjectTreeItem> treeItemDictionary = new();

            foreach (GitHubTreeItem githubTreeItem in githubTree.LinearTree)
            {
                ProjectTreeItem treeItem = new()
                {
                    Name = githubTreeItem.Path.Split('/').Last(), // TODO maybe exception
                    Path = GitHubTreeConfig.GetGitUrl(repo, githubTreeItem.Path),

                    Type = githubTreeItem.Type == FOLDER_TYPE ? ProjectTreeItemType.Folder : ProjectTreeItemType.File,
                };

                treeItemDictionary[githubTreeItem.Path] = treeItem;
            }


            ProjectTreeItem treeRoot = new()
            {
                Name = repo,
                Path = string.Empty,

                Type = ProjectTreeItemType.Project,
            };

            foreach (GitHubTreeItem githubTreeItem in githubTree.LinearTree)
            {
                ProjectTreeItem treeItem = treeItemDictionary[githubTreeItem.Path];

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

        private void SortTree(ProjectTreeItem treeItem)
        {
            treeItem.Children.Sort((a, b) =>
            {
                if (a.Type != b.Type) return a.Type.CompareTo(b.Type);

                return a.Name.CompareTo(b.Name);
            });

            foreach (ProjectTreeItem item in treeItem.Children.Where(n => n.Children.Any()))
            {
                this.SortTree(item);
            }
        }
    }
}
