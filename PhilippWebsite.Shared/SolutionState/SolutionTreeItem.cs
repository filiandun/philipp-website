namespace PhilippWebsite.Shared.SolutionState
{
    public class SolutionTreeItem
    {
        public required string Name { get; init; }
        public required string TreePath { get; init; }
        public required SolutionTreeItemType Type { get; init; }

        public string? ContentPath { get; init; }

        public List<SolutionTreeItem> Children { get; init; }

        public bool IsExpanded { get; set; }


        public SolutionTreeItem()
        {
            this.ContentPath = null;

            this.Children = new List<SolutionTreeItem>();

            this.IsExpanded = false;
        }
    }
}
