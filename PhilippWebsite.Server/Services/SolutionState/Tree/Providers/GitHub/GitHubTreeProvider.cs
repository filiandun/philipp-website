using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Infrastructure.Storage;
using PhilippWebsite.Server.Models;
using PhilippWebsite.Server.Options;
using PhilippWebsite.Server.Services.Common;
using PhilippWebsite.Server.Services.SolutionState.Tree.Providers;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionState.Tree.Providers.GitHub
{
    public class GitHubTreeProvider : ITreeProvider
    { 
        private readonly ILogger<GitHubTreeProvider> _logger;

        private readonly IRepositoryReader _repositoryReader;

        private readonly GitHubContentOptions _options;


        public GitHubTreeProvider(ILogger<GitHubTreeProvider> logger, IRepositoryReader repositoryReader, IOptions<GitHubContentOptions> options)
        {
            this._logger = logger;

            this._repositoryReader = repositoryReader;

            this._options = options.Value;
        }


        public async Task<SolutionTreeItem> GetTreeAsync()
        {
            return await this.BuildTree();
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
                this._logger.LogDebug("Request to database for get repo '{Repo}'.", repo);

                Repository? repository = await this._repositoryReader.GetRepositoryAsync(repo.Key);

                if (repository is null)
                {
                    this._logger.LogWarning("Skip repo '{Repo}'. Database return null.", repo.Key);
                    continue;
                }

                SolutionTreeItem repoTree = new()
                {
                    Name = repo.Key,
                    TreePath = PathHelper.Combine(this._options.SectionName, repo.Key),

                    Type = repo.Value,

                    Children = this.MapRepositoryToSolutionTree(repository.Nodes.Where(n => n.ParentId is null), repo.Key)
                };

                solutionTree.Children.Add(repoTree);
            }

            this._logger.LogDebug("Solution items count {ItemsCount}", solutionTree.Children.Count);

            return solutionTree;
        }


        private List<SolutionTreeItem> MapRepositoryToSolutionTree(IEnumerable<FileNode> fileNodes, string repo)
        {
            return fileNodes.OrderByDescending(f => f.IsDirectory).ThenBy(f => f.Name).Select(fileNode => new SolutionTreeItem
                    {
                        Name = fileNode.Name,
                        TreePath = PathHelper.Combine(this._options.SectionName, fileNode.Path),

                        Type = fileNode.IsDirectory ? SolutionTreeItemType.Folder : SolutionTreeItemType.File,

                        Children = this.MapRepositoryToSolutionTree(fileNode.Children, repo)
                    }).ToList();
        }
    }
}
