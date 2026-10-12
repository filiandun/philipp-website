using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Client.Clients.ProjectTree
{
    public interface IProjectTreeClient
    {
        public Task<ProjectTreeRoot?> GetTreeAsync();
    }
}
