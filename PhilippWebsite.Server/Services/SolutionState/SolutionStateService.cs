using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Extensions;
using PhilippWebsite.Server.Options;
using PhilippWebsite.Server.Services.ProjectTree;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionState
{
    public class SolutionStateService : ISolutionStateService
    {
        private readonly ILogger<SolutionStateService> _logger;

        private readonly IProjectTreeService _projectTreeService;

        private readonly SolutionStateOptions _options;


        public SolutionStateService(ILogger<SolutionStateService> logger, IProjectTreeService projectTreeService, IOptions<SolutionStateOptions> options)
        {
            this._logger = logger;

            this._projectTreeService = projectTreeService;

            this._options = options.Value;
        }


        public async Task<SolutionStateModel> GetModel()
        {
            SolutionTreeRoot treeRoot = await this._projectTreeService.GetTreeAsync();

            List<SolutionTreeItem> treeFlat = treeRoot.Items.Flatten().ToList();

            this.MakeExpanded(treeFlat);
            List<SolutionTreeItem> initialOpenItems = this.GetOpenedItems(treeFlat);
            SolutionTreeItem? initialActiveItem = treeFlat.FirstOrDefault(i => i.Name == this._options.InitialActiveItem);

            SolutionStateModel solutionModel = new()
            {
                Name = "PhilippPortfolio",
                Tree = treeRoot,
                InitialOpenItems = initialOpenItems,
                InitialActiveItem = initialActiveItem
            };

            return solutionModel;
        }


        private void MakeExpanded(List<SolutionTreeItem> treeFlat)
        {
            foreach (string item in this._options.InitialExpandedItems)
            {
                SolutionTreeItem? expandedItem = treeFlat.FirstOrDefault(i => i.Name == item);
                if (expandedItem is not null)
                {
                    expandedItem.IsExpanded = true;

                    this._logger.LogInformation("Item '{Item}' from Solution Tree was expand.", item);
                }
                else
                {
                    this._logger.LogWarning("Expanded item '{Item}' from configurations not found in Solution Tree.", item);
                }
            }
        }

        private List<SolutionTreeItem> GetOpenedItems(List<SolutionTreeItem> treeFlat)
        {
            List<SolutionTreeItem> openedItems = new();

            foreach (string item in this._options.InitialOpenItems)
            {
                SolutionTreeItem? openedItem = treeFlat.FirstOrDefault(i => i.Name == item);
                if (openedItem is not null)
                {
                    openedItems.Add(openedItem);

                    this._logger.LogInformation("Item '{Item}' from Solution Tree was opened.", item);
                }
                else
                {
                    this._logger.LogWarning("Opened item '{Item}' from configurations not found in Solution Tree.", item);
                }
            }

            return openedItems;
        }
    }
}
