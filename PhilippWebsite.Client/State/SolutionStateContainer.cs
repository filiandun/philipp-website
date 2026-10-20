using PhilippWebsite.Client.Clients.SolutionState;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.State
{
    public class SolutionStateContainer
    {
        private readonly ILogger<SolutionStateContainer> _logger;

        private readonly ISolutionStateClient _solutionStateClient;


        private SolutionStateModel? _solutionState;

        private Task<SolutionStateModel?>? _solutionStateTask;
        

        public SolutionStateContainer(ILogger<SolutionStateContainer> logger, ISolutionStateClient solutionStateClient)
        {
            this._logger = logger;

            this._solutionStateClient = solutionStateClient;
        }


        public async Task InitializeAsync()
        {
            if (this._solutionState is not null)
            {
                this._logger.LogDebug("Return cached Solution State.");
            }

            if (this._solutionStateTask is null)
            {
                this._logger.LogDebug("Get Solution State from Solution State Client.");

                this._solutionStateTask = this._solutionStateClient.GetModel();
            }

            this._solutionState = await this._solutionStateTask;
        }


        public async Task<SolutionTreeRoot?> GetTreeAsync()
        {
            await this.InitializeAsync();

            return this._solutionState?.Tree;
        }

        public async Task<IEnumerable<SolutionTreeItem>?> GetInitialOpenItems()
        {
            await this.InitializeAsync();

            return this._solutionState?.InitialOpenItems;
        }

        public async Task<SolutionTreeItem?> GetActiveItem()
        {
            await this.InitializeAsync();

            return this._solutionState?.InitialActiveItem;
        }
    }
}
