using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionTree
{
    public interface ISolutionTreeService
    {
        public Task<SolutionTreeItem> GetTreeAsync();
    }
}
