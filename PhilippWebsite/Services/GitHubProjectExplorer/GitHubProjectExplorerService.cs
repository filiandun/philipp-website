using PhilippWebsite.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace PhilippWebsite.Services.GitHubProjectExplorer
{
    public class GitHubProjectExplorerService : IProjectExplorerService
    {
        private const string FOLDER_TYPE = "tree";
        private const string FILE_TYPE = "blob";

        private const string URL = $"https://api.github.com/repos/filiandun/philipp-website/git/trees/main?recursive=1";

        private readonly ILogger<GitHubProjectExplorerService> _logger;

        private readonly HttpClient _httpClient;


        public GitHubProjectExplorerService(ILogger<GitHubProjectExplorerService> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;

            if (!this._httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                this._httpClient.DefaultRequestHeaders.Add("User-Agent", "PhilippPortfolioApp");
            }
        }


        // TODO сделать промежуточную DTO, так как иначе смысла в интерфейсе нет
        public async Task<List<ProjectNode>?> GetProjectTreeAsync()
        {
            try
            {
                //GitHubProjectResponse? githubProject = await this._httpClient.GetFromJsonAsync<GitHubProjectResponse>(URL);

                GitHubProjectResponse? githubProject = await this._httpClient.GetFromJsonAsync<GitHubProjectResponse>("content/project.json");

                if (githubProject is null) throw new HttpRequestException("GitHub Project is null");

                return this.BuildTree(githubProject);
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "Error loading project from GitHub");
                return null;
            }
        }


        private List<ProjectNode> BuildTree(GitHubProjectResponse githubProject)
        {
            Dictionary<string, ProjectNode> nodeDictionary = new();

            foreach (var item in githubProject.Tree)
            {
                ProjectNode node = new()
                {
                    Name = item.Path.Split('/').Last(), // TODO maybe exception
                    Path = item.Path,
                    Type = item.Type == FOLDER_TYPE ? ProjectNodeType.Folder : ProjectNodeType.File
                };

                nodeDictionary[item.Path] = node;
            }


            List<ProjectNode> nodeList = new();

            foreach (var item in githubProject.Tree)
            {
                ProjectNode node = nodeDictionary[item.Path];

                int lastSlashIndex = item.Path.LastIndexOf('/');
                if (lastSlashIndex == -1)
                {
                    nodeList.Add(node);
                }
                else
                {
                    string parentPath = item.Path.Substring(0, lastSlashIndex);

                    if (nodeDictionary.TryGetValue(parentPath, out var parentNode))
                    {
                        parentNode.Children.Add(node);
                    }
                }
            }

            this.SortTree(nodeList);

            return nodeList;
        }

        private void SortTree(List<ProjectNode> nodeList)
        {
            nodeList.Sort((a, b) =>
            {
                if (a.Type != b.Type) return a.Type.CompareTo(b.Type);

                return a.Name.CompareTo(b.Name);
            });

            foreach (var node in nodeList.Where(n => n.Children.Any()))
            {
                this.SortTree(node.Children);
            }
        }
    }
}
