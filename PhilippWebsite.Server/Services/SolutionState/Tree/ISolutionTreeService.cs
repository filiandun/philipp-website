using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionState.Tree
{
    public interface ISolutionTreeService
    {
        public Task<SolutionTreeItem> GetTreeAsync();
    }
}
