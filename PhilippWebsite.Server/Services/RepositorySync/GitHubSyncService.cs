using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Infrastructure.Storage;

using PhilippWebsite.Server.Models;
using PhilippWebsite.Server.Options;
using PhilippWebsite.Server.Services.SolutionTree.TreeProvider.GitHub;


namespace PhilippWebsite.Server.Services.RepositorySync
{
    public class GitHubSyncService : IRepositorySyncService
    {
        private readonly ILogger<GitHubSyncService> _logger;

        private readonly HttpClient _httpClient;
        private readonly GitHubContentOptions _options;

        private readonly IRepositoryWriter _repositoryWriter;

        private readonly GitHubContentService _gitHubContentService;


        public GitHubSyncService(ILogger<GitHubSyncService> logger, HttpClient httpClient, IOptions<GitHubContentOptions> options, IRepositoryWriter repositoryWriter, GitHubContentService gitHubContentService)
        {
            this._logger = logger;

            this._httpClient = httpClient;
            this._options = options.Value;

            this._repositoryWriter = repositoryWriter;

            this._gitHubContentService = gitHubContentService;
        }


        public async Task<Repository> SyncRepoAsync(string repo)
        {
            this._logger.LogInformation("Get GitHub tree by repo '{Repo}'.", repo);
            GitHubTreeResponse gitHubTree = await this.GetGitHubTreeAsync(repo);

            this._logger.LogInformation("Get GitHub content by repo '{Repo}' sha '{Sha}'.", repo, gitHubTree.Sha);
            Dictionary<string, string> githubContent = await this._gitHubContentService.GetContentAsync(repo, gitHubTree.Sha);

            this._logger.LogInformation("Map GitHub tree repo '{Repo}' sha '{Sha}' to database model.", repo, gitHubTree.Sha);
            Repository repository = this.MapGitHubTreeToRepository(repo, gitHubTree, githubContent);

            this._logger.LogInformation("Update databse for repo '{Repo}' sha '{Sha}'", repository.Name, repository.LastCommitSha);
            await this._repositoryWriter.UpdateRepositoryAsync(repository);

            return repository;
        }


        private async Task<GitHubTreeResponse> GetGitHubTreeAsync(string repo)
        {
            string repoUrl = this._options.GetApiUrl(repo);

            this._logger.LogDebug("Http request to '{RepoUrl}'.", repoUrl);

            HttpResponseMessage response = await this._httpClient.GetAsync(repoUrl);

            if (!response.IsSuccessStatusCode)
            {
                this._logger.LogWarning("GitHub returned {StatusCode} for repo '{Repo}'.", response.StatusCode, repo);

                throw new HttpRequestException($"GitHub returned {response.StatusCode} for repo '{repo}'.");
            }

            GitHubTreeResponse? gitHubTree = await response.Content.ReadFromJsonAsync<GitHubTreeResponse>();

            if (gitHubTree is null)
            {
                this._logger.LogWarning("Repo '{Repo}' tree is null.", repo);

                throw new HttpRequestException($"Repo '{repo}' tree is null.");
            }

            return gitHubTree;
        }


        private Repository MapGitHubTreeToRepository(string repo, GitHubTreeResponse gitHubTree, Dictionary<string, string> gitHubContent)
        {
            this._logger.LogInformation("Start map GitHub Tree by repo '{Repo}' to Repository.", repo);

            Repository repository = new()
            {
                Id = Guid.NewGuid(),

                Name = repo,
                LastCommitSha = gitHubTree.Sha,
            };


            Dictionary<string, FileNode> repositoryLinearDictionary = new();

            foreach (GitHubTreeItem githubTreeItem in gitHubTree.LinearTree)
            {
                string itemName = Path.GetFileName(githubTreeItem.Path);

                this._logger.LogTrace("Map GitHub Tree Item '{ItemName}'.", itemName);

                FileNode fileNode = new()
                {
                    Id = Guid.NewGuid(),

                    RepositoryId = repository.Id,
                    ParentId = null,

                    Name = itemName,
                    Path = githubTreeItem.Path,
                    IsDirectory = githubTreeItem.Type == "tree",

                    Repository = repository,
                    Parent = null,
                    Content = null
                };

                if (gitHubContent.TryGetValue(githubTreeItem.Path, out string? itemContent))
                {
                    this._logger.LogTrace("GitHub Tree Item '{ItemName}' has content with length '{ContentLength}'.", itemName, itemContent.Length);

                    fileNode.Content = new Models.FileContent
                    {
                        NodeId = fileNode.Id,
                        Text = itemContent,
                        Size = itemContent.Length,
                        Node = fileNode
                    };
                }
                else
                {
                    this._logger.LogTrace("GitHub Tree Item '{ItemName}' has not content.", itemName);
                }

                repositoryLinearDictionary[githubTreeItem.Path] = fileNode;
            }


            foreach (GitHubTreeItem githubTreeItem in gitHubTree.LinearTree)
            {
                FileNode fileNode = repositoryLinearDictionary[githubTreeItem.Path];

                int lastSlashIndex = githubTreeItem.Path.LastIndexOf('/');

                if (lastSlashIndex == -1)
                {
                    repository.Nodes.Add(fileNode);
                }
                else
                {
                    string parentPath = githubTreeItem.Path.Substring(0, lastSlashIndex);

                    if (repositoryLinearDictionary.TryGetValue(parentPath, out var parentItem))
                    {
                        fileNode.ParentId = parentItem.Id;
                        fileNode.Parent = parentItem;

                        parentItem.Children.Add(fileNode);
                    }
                }
            }

            return repository;
        }
    }
}