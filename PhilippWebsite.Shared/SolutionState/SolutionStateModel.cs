
namespace PhilippWebsite.Shared.SolutionState
{
    public class SolutionStateModel
    {
        public required string Name { get; set; }


        public required SolutionTreeItem Tree { get; set; }

        public required List<SolutionTreeItem> InitialOpenItems { get; set; }

        public SolutionTreeItem? InitialActiveItem { get; set; } = null;
    }
}
