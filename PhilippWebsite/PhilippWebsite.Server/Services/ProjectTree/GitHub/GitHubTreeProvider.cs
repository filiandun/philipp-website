using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Server.Services.ProjectTree.GitHub
{
    public class GitHubTreeProvider : ITreeProvider
    {
        private const string FOLDER_TYPE = "tree";
        private const string FILE_TYPE = "blob";

 
        private readonly ILogger<GitHubTreeProvider> _logger;

        private readonly HttpClient _httpClient;


        public GitHubTreeProvider(ILogger<GitHubTreeProvider> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;

            if (!this._httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                this._httpClient.DefaultRequestHeaders.Add("User-Agent", "PhilippPortfolioApp");
            }
        }


        public async Task<ProjectTreeItem?> GetTreeAsync()
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

                    ProjectTreeItem explorerItem = this.BuildTree(githubTree, repo);
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


        private ProjectTreeItem BuildTree(GitHubTreeResponse githubTree, string repo)
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
