
namespace PhilippWebsite.Models.SolutionExplorer
{
    public class SolutionExplorerItem
    {
        public required string Name { get; init; }
        public required string Path { get; init; }

        public required string Repo { get; init; }

        public required SolutionExplorerItemType Type { get; init; }

        public required FileSource Source { get; init; }

        public List<SolutionExplorerItem> Items { get; init; }


        public SolutionExplorerItem()
        {
            this.Items = new List<SolutionExplorerItem>();
        }
    }
}
