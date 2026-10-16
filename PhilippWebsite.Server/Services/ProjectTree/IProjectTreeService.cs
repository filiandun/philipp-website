using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.ProjectTree
{
    public interface IProjectTreeService
    {
        public Task<SolutionTreeRoot> GetTreeAsync();
    }
}
