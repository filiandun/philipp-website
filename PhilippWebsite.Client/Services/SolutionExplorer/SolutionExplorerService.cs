using PhilippWebsite.Client.State;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public class SolutionExplorerService : ISolutionExplorerService
    {
        private readonly ILogger<SolutionExplorerService> _logger;

        private readonly SolutionStateContainer _solutionStateContainer;


        public SolutionExplorerService(ILogger<SolutionExplorerService> logger, SolutionStateContainer solutionStateContainer)
        {
            this._logger = logger;

            this._solutionStateContainer = solutionStateContainer;
        }


        public async Task<SolutionTreeRoot?> GetTreeAsync()
        {
            SolutionTreeRoot? solutionTreeRoot = await this._solutionStateContainer.GetTreeAsync();

            if (solutionTreeRoot is null)
            {
                this._logger.LogWarning("Solution State Container return null Solution Tree Root.");
            }

            return solutionTreeRoot;
        }
    }
}
