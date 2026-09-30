using PhilippWebsite.Models.SolutionExplorer;


namespace PhilippWebsite.Services.SolutionExplorer
{
    public interface ISolutionExplorerService
    {
        public Task<SolutionExplorerRoot?> GetProjectTreeAsync();
    }
}
