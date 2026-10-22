using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public interface ISolutionExplorerService
    {
        public event Action? OnTreeChanged;

        public SolutionTreeItem? Tree { get; }

        public bool IsLoading { get; }
        public bool HasError { get; }

        public void SetTree(SolutionTreeItem? tree);
    }
}
