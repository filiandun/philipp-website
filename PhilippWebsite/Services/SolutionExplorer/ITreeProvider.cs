using PhilippWebsite.Models.SolutionExplorer;


namespace PhilippWebsite.Services.SolutionExplorer
{
    public interface ITreeProvider
    {
        public Task<SolutionExplorerItem?> GetTreeAsync();
    }
}
