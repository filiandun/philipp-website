using PhilippWebsite.Models.SolutionTree;


namespace PhilippWebsite.Services.ProjectTree
{
    public interface IProjectTreeService
    {
        public Task<SolutionTreeRoot?> GetProjectTreeAsync();
    }
}
