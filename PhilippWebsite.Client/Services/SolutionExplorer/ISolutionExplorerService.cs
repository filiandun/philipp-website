using PhilippWebsite.Client.Models.SolutionExplorer;


namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public interface ISolutionExplorerService
    {
        public Task<SolutionExplorerRoot?> GetTreeAsync();
    }
}
