using PhilippWebsite.Client.Clients.SolutionState;

using PhilippWebsite.Client.Models;

using PhilippWebsite.Client.Services.SolutionExplorer;
using PhilippWebsite.Client.Services.TabsBarState;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Services.SolutionState
{
    public class SolutionStateInitializer : ISolutionStateInitializer
    {
        private readonly ILogger<SolutionStateInitializer> _logger;

        private readonly ISolutionStateClient _solutionStateClient;

        private readonly ISolutionExplorerService _solutionExplorerService;
        private readonly TabsBarStateService _tabsBarStateService;



        private SolutionStateModel? _solutionState;
        

        public SolutionStateInitializer(ILogger<SolutionStateInitializer> logger, ISolutionStateClient solutionStateClient, ISolutionExplorerService solutionExplorerService, TabsBarStateService tabsBarStateService)
        {
            this._logger = logger;

            this._solutionStateClient = solutionStateClient;

            this._solutionExplorerService = solutionExplorerService;
            this._tabsBarStateService = tabsBarStateService;
        }


        public async Task InitializeAsync()
        {
            this._logger.LogInformation("Get Solution State from Solution State Client.");

            this._solutionState = await this._solutionStateClient.GetModel();


            if (this._solutionState is null)
            {
                this._logger.LogWarning("Solution State Client return Get Solution State as null.");

                this._solutionExplorerService.SetTree(null);

                return;
            }

            this._solutionExplorerService.SetTree(this._solutionState.Tree);

            this._logger.LogInformation("Set '{OpenedItemsCount}' tree items as opened.", this._solutionState.InitialOpenItems.Count);

            foreach (SolutionTreeItem treeItem in this._solutionState.InitialOpenItems)
            {
                this._logger.LogDebug("Set '{ItemName}' tree item as opened.", treeItem.Name);
                this._tabsBarStateService.Open(new TabModel() { Name = treeItem.Name, Path = treeItem.TreePath });
            }

            if (this._solutionState.InitialActiveItem is null) return;

            this._logger.LogDebug("Set '{ItemName}' tree item as active.", this._solutionState.InitialActiveItem.Name);

            this._tabsBarStateService.OpenAsActive(new TabModel() { Name = this._solutionState.InitialActiveItem.Name, Path = this._solutionState.InitialActiveItem.TreePath });
        }
    }
}
