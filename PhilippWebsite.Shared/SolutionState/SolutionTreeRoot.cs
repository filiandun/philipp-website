namespace PhilippWebsite.Shared.SolutionState
{
    public class SolutionTreeRoot
    {
        public List<SolutionTreeItem> Items { get; init; }


        public SolutionTreeRoot()
        {
            this.Items = new List<SolutionTreeItem>();
        }
    }
}