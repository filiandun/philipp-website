using PhilippWebsite.Client.Models.SolutionExplorer;

namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public interface ITreeProvider
    {
        public Task<SolutionExplorerItem?> GetTreeAsync();
    }
}
