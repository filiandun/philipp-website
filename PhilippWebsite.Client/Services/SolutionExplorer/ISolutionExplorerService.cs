using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public interface ISolutionExplorerService
    {
        public Task<SolutionTreeRoot?> GetTreeAsync();
    }
}
