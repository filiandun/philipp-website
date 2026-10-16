using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Clients.ProjectTree
{
    public interface IProjectTreeClient
    {
        public Task<SolutionTreeRoot?> GetTreeAsync();
    }
}
