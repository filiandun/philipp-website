using PhilippWebsite.Models.ProjectTree;


namespace PhilippWebsite.Services.ProjectTree
{
    public interface IProjectTreeService
    {
        public Task<ProjectTreeRoot?> GetProjectTreeAsync();
    }
}
