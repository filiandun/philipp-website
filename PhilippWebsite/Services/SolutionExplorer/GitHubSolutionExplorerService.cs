using System.Net.Http.Json;
using Microsoft.Extensions.Options;

using PhilippWebsite.Models.SolutionExplorer;


namespace PhilippWebsite.Services.SolutionExplorer
{
    public class GitHubSolutionExplorerService : ISolutionExplorerService
    {
        private const string FOLDER_TYPE = "tree";
        private const string FILE_TYPE = "blob";

        private readonly ILogger<GitHubSolutionExplorerService> _logger;

        private readonly GitHubApiConfig _config;

        private readonly HttpClient _httpClient;



        public GitHubSolutionExplorerService(ILogger<GitHubSolutionExplorerService> logger, IOptions<GitHubApiConfig> options, HttpClient httpClient)
        {
            this._logger = logger;

            this._config = options.Value;

            this._httpClient = httpClient;

            if (!this._httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                this._httpClient.DefaultRequestHeaders.Add("User-Agent", "PhilippPortfolioApp");
            }
        }


        public async Task<SolutionExplorerRoot?> GetProjectTreeAsync()
        {
            try
            {
                SolutionExplorerRoot explorerRoot = new SolutionExplorerRoot();

                foreach (var repo in this._config.RepoList)
                {
                    string repoUrl = this._config.BaseUrl.Replace("{repo}", repo);

                    this._logger.LogDebug("Http request to '{RepoUrl}'.", repoUrl);

                    GitHubTreeResponse? githubTree = await this._httpClient.GetFromJsonAsync<GitHubTreeResponse>(repoUrl);

                    //GitHubProjectTreeResponse? githubProjectTree = await this._httpClient.GetFromJsonAsync<GitHubProjectTreeResponse>("content/project.json");

                    if (githubTree is null) throw new HttpRequestException("GitHubProjectTreeResponse is null");

                    SolutionExplorerItem explorerItem = this.BuildTree(githubTree, repo);
                    explorerRoot.Items.Add(explorerItem);
                }

                return explorerRoot;
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "Error loading project from GitHub");

                return null;
            }
        }


        private SolutionExplorerItem BuildTree(GitHubTreeResponse githubTree, string repo)
        {
            Dictionary<string, SolutionExplorerItem> explorerItemDictionary = new();

            foreach (GitHubTreeItem item in githubTree.LinearTree)
            {
                SolutionExplorerItem treeItem = new()
                {
                    Name = item.Path.Split('/').Last(), // TODO maybe exception
                    Path = item.Path,
                    Repo = repo,
                    Type = item.Type == FOLDER_TYPE ? SolutionExplorerItemType.Folder : SolutionExplorerItemType.File
                };

                explorerItemDictionary[item.Path] = treeItem;
            }


            SolutionExplorerItem treeRoot = new SolutionExplorerItem()
            {
                Name = repo,
                Path = "/",
                Repo = repo,
                Type = SolutionExplorerItemType.Project
            };

            foreach (var item in githubTree.LinearTree)
            {
                SolutionExplorerItem treeItem = explorerItemDictionary[item.Path];

                int lastSlashIndex = item.Path.LastIndexOf('/');
                if (lastSlashIndex == -1)
                {
                    treeRoot.Items.Add(treeItem);
                }
                else
                {
                    string parentPath = item.Path.Substring(0, lastSlashIndex);

                    if (explorerItemDictionary.TryGetValue(parentPath, out var parentItem))
                    {
                        parentItem.Items.Add(treeItem);
                    }
                }
            }

            this.SortTree(treeRoot);

            return treeRoot;
        }

        private void SortTree(SolutionExplorerItem treeItem)
        {
            treeItem.Items.Sort((a, b) =>
            {
                if (a.Type != b.Type) return a.Type.CompareTo(b.Type);

                return a.Name.CompareTo(b.Name);
            });

            foreach (var item in treeItem.Items.Where(n => n.Items.Any()))
            {
                this.SortTree(item);
            }
        }
    }
}
