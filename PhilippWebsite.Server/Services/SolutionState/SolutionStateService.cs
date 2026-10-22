using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Extensions;
using PhilippWebsite.Server.Options;
using PhilippWebsite.Server.Services.SolutionTree;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionState
{
    public class SolutionStateService : ISolutionStateService
    {
        private readonly ILogger<SolutionStateService> _logger;

        private readonly ISolutionTreeService _projectTreeService;

        private readonly SolutionStateOptions _options;


        public SolutionStateService(ILogger<SolutionStateService> logger, ISolutionTreeService projectTreeService, IOptions<SolutionStateOptions> options)
        {
            this._logger = logger;

            this._projectTreeService = projectTreeService;

            this._options = options.Value;
        }


        public async Task<SolutionStateModel> GetModel()
        {
            SolutionTreeItem solutionTree = await this._projectTreeService.GetTreeAsync();

            List<SolutionTreeItem> treeFlat = solutionTree.Flatten().ToList();

            this.MakeExpanded(treeFlat);
            List<SolutionTreeItem> initialOpenItems = this.GetOpenedItems(treeFlat);
            SolutionTreeItem? initialActiveItem = treeFlat.FirstOrDefault(i => i.TreePath == this._options.InitialActivePath);

            SolutionStateModel solutionModel = new()
            {
                Name = this._options.SolutionName,
                Tree = solutionTree,
                InitialOpenItems = initialOpenItems,
                InitialActiveItem = initialActiveItem
            };

            return solutionModel;
        }


        private void MakeExpanded(List<SolutionTreeItem> treeFlat)
        {
            foreach (string path in this._options.InitialExpandedPathes)
            {
                SolutionTreeItem? expandedItem = treeFlat.FirstOrDefault(i => i.TreePath == path);
                if (expandedItem is not null)
                {
                    expandedItem.IsExpanded = true;

                    this._logger.LogInformation("Item '{Item}' from Solution Tree was expand.", path);
                }
                else
                {
                    this._logger.LogWarning("Expanded item '{Item}' from configurations not found in Solution Tree.", path);
                }
            }
        }

        private List<SolutionTreeItem> GetOpenedItems(List<SolutionTreeItem> treeFlat)
        {
            List<SolutionTreeItem> openedItems = new();

            foreach (string path in this._options.InitialOpenPathes)
            {
                SolutionTreeItem? openedItem = treeFlat.FirstOrDefault(i => i.TreePath == path);
                if (openedItem is not null)
                {
                    openedItems.Add(openedItem);

                    this._logger.LogInformation("Item '{Item}' from Solution Tree was opened.", path);
                }
                else
                {
                    this._logger.LogWarning("Opened item '{Item}' from configurations not found in Solution Tree.", path);
                }
            }

            return openedItems;
        }
    }
}
