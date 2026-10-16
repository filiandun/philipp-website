namespace PhilippWebsite.Shared.SolutionState
{
    public class SolutionTreeItem
    {
        public required string Name { get; init; }
        public required string Path { get; init; }

        public required SolutionTreeItemType Type { get; init; }

        public List<SolutionTreeItem> Children { get; init; }


        public SolutionTreeItem()
        {
            this.Children = new List<SolutionTreeItem>();
        }
    }
}
