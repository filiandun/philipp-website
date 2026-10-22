using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public class SolutionExplorerService : ISolutionExplorerService
    {
        public event Action? OnTreeChanged;

        private readonly ILogger<SolutionExplorerService> _logger;

        public SolutionTreeItem? Tree { get; private set; }

        public bool IsLoading { get; private set; }
        public bool HasError { get; private set; }


        public SolutionExplorerService(ILogger<SolutionExplorerService> logger)
        {
            this._logger = logger;

            this.IsLoading = true;
            this.HasError = false;
        }


        public void SetTree(SolutionTreeItem? tree)
        {
            if (tree is null)
            {
                this._logger.LogWarning("Set tree as null");

                this.HasError = true;
            } 
            else
            {
                this._logger.LogInformation("Set tree with root name '{RootName}'", tree.Name);

                this.HasError = false;
            }

            this.Tree = tree;
            this.IsLoading = false;

            this.OnTreeChanged?.Invoke();
        }
    }
}
