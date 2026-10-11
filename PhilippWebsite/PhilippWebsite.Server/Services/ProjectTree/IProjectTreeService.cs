using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Server.Services.ProjectTree
{
    public interface IProjectTreeService
    {
        public Task<ProjectTreeRoot> GetTreeAsync();
    }
}
