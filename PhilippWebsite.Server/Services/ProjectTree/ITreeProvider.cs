using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.ProjectTree
{
    public interface ITreeProvider
    {
        public Task<SolutionTreeItem> GetTreeAsync();
    }
}
