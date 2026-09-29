using System.Net.Http.Json;

using PhilippWebsite.Models.ProjectTree;


namespace PhilippWebsite.Services.ProjectTree
{
    public class GitHubProjectTreeService : IProjectTreeService
    {
        private const string FOLDER_TYPE = "tree";
        private const string FILE_TYPE = "blob";

        private const string URL = $"https://api.github.com/repos/filiandun/philipp-website/git/trees/main?recursive=1";

        private readonly ILogger<GitHubProjectTreeService> _logger;

        private readonly GitHubApiConfig _config;

        private readonly HttpClient _httpClient;



        public GitHubProjectTreeService(ILogger<GitHubProjectTreeService> logger, IOptions<GitHubApiConfig> options, HttpClient httpClient)
        {
            this._logger = logger;

            this._config = options.Value;

            this._httpClient = httpClient;

            if (!this._httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                this._httpClient.DefaultRequestHeaders.Add("User-Agent", "PhilippPortfolioApp");
            }
        }


        public async Task<ProjectTreeRoot?> GetProjectTreeAsync()
        {
            try
            {
                //GitHubProjectResponse? githubProjectTree = await this._httpClient.GetFromJsonAsync<GitHubProjectResponse>(URL);

                GitHubProjectTreeResponse? githubProjectTree = await this._httpClient.GetFromJsonAsync<GitHubProjectTreeResponse>("content/project.json");

                if (githubProjectTree is null) throw new HttpRequestException("GitHub Project is null");

                return this.BuildTree(githubProjectTree);
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "Error loading project from GitHub");

                return null;
            }
        }


        private ProjectTreeRoot BuildTree(GitHubProjectTreeResponse githubProjectTree)
        {
            Dictionary<string, ProjectTreeItem> treeItemDictionary = new();

            foreach (var item in githubProjectTree.LinearTree)
            {
                ProjectTreeItem treeItem = new()
                {
                    Name = item.Path.Split('/').Last(), // TODO maybe exception
                    Path = item.Path,
                    Type = item.Type == FOLDER_TYPE ? ProjectTreeItemType.Folder : ProjectTreeItemType.File
                };

                treeItemDictionary[item.Path] = treeItem;
            }


            ProjectTreeRoot treeRoot = new ProjectTreeRoot("PhilippWebsite");

            foreach (var item in githubProjectTree.LinearTree)
            {
                ProjectTreeItem treeItem = treeItemDictionary[item.Path];

                int lastSlashIndex = item.Path.LastIndexOf('/');
                if (lastSlashIndex == -1)
                {
                    treeRoot.Items.Add(treeItem);
                }
                else
                {
                    string parentPath = item.Path.Substring(0, lastSlashIndex);

                    if (treeItemDictionary.TryGetValue(parentPath, out var parentItem))
                    {
                        parentItem.Children.Add(treeItem);
                    }
                }
            }

            this.SortTree(treeRoot.Items);

            return treeRoot;
        }

        private void SortTree(List<ProjectTreeItem> treeItemList)
        {
            treeItemList.Sort((a, b) =>
            {
                if (a.Type != b.Type) return a.Type.CompareTo(b.Type);

                return a.Name.CompareTo(b.Name);
            });

            foreach (var item in treeItemList.Where(n => n.Children.Any()))
            {
                this.SortTree(item.Children);
            }
        }
    }
}
