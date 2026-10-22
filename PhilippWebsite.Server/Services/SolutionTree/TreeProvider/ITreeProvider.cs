using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionTree.TreeProvider
{
    public interface ITreeProvider
    {
        public Task<SolutionTreeItem> GetTreeAsync();
    }
}
