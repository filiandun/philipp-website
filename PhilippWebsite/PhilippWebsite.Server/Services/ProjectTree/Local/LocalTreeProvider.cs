using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Server.Services.ProjectTree.Local
{
    public class LocalTreeProvider : ITreeProvider
    {
        private readonly ILogger<LocalTreeProvider> _logger;


        public LocalTreeProvider(ILogger<LocalTreeProvider> logger)
        {
            this._logger = logger;
        }


        public async Task<ProjectTreeItem?> GetTreeAsync()
        {
            try
            {
                ProjectTreeItem treeSolution = new()
                {
                    Name = LocalTreeConfig.SolutionName,
                    Path = string.Empty,

                    Type = ProjectTreeItemType.Solution,
                };

                foreach (string file in LocalTreeConfig.FileList)
                {
                    ProjectTreeItem treeItem = new()
                    {
                        Name = file,
                        Path = LocalTreeConfig.GetBaseUrl(file),

                        Type = ProjectTreeItemType.File,
                    };

                    treeSolution.Children.Add(treeItem);
                }

                this._logger.LogDebug("Root items count {ItemsCount}", treeSolution.Children.Count);

                return treeSolution;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error loading tree from local");
                throw;
            }
        }
    }
}
