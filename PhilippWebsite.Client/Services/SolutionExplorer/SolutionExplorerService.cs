using PhilippWebsite.Client.Clients.ProjectTree;
using PhilippWebsite.Client.Models.SolutionExplorer;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public class SolutionExplorerService : ISolutionExplorerService
    {
        private readonly ILogger<SolutionExplorerService> _logger;

        private readonly IProjectTreeClient _projectTreeClient;


        public SolutionExplorerService(ILogger<SolutionExplorerService> logger, IProjectTreeClient projectTreeClient)
        {
            this._logger = logger;

            this._projectTreeClient = projectTreeClient;
        }


        public async Task<SolutionExplorerRoot?> GetTreeAsync()
        {
            try
            {
                SolutionTreeRoot? treeRoot = await this._projectTreeClient.GetTreeAsync();
                if (treeRoot is null)
                {
                    this._logger.LogWarning("Server return null Project Tree Root");

                    return null;
                }

                SolutionExplorerRoot explorerRoot = new();

                foreach (SolutionTreeItem treeItem in treeRoot.Items)
                {
                    SolutionExplorerItem explorerItem = this.BuildTree(treeItem);

                    explorerRoot.Items.Add(explorerItem);
                }

                return explorerRoot;
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "Error get tree");

                return null;
            }
        }


        private SolutionExplorerItem BuildTree(SolutionTreeItem treeItem)
        {
            SolutionExplorerItem explorerItem = new()
            {
                Name = treeItem.Name,
                Path = treeItem.Path,

                Type = (SolutionExplorerItemType)treeItem.Type,
            };

            foreach (SolutionTreeItem itemChild in treeItem.Children)
            {
                SolutionExplorerItem explorerItemChild = this.BuildTree(itemChild);

                explorerItem.Items.Add(explorerItemChild);
            }

            return explorerItem;
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
