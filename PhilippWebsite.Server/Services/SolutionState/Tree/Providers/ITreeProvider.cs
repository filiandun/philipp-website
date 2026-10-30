using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionState.Tree.Providers
{
    public interface ITreeProvider
    {
        public Task<SolutionTreeItem> GetTreeAsync();
    }
}
