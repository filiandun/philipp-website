
namespace PhilippWebsite.Models.ProjectTree
{
    public class ProjectTreeItem
    {
        public required string Name { get; init; }
        public required string Path { get; init; }

        public required ProjectTreeItemType Type { get; init; }

        public List<ProjectTreeItem> Children { get; init; }


        public bool HasChildren => this.Children.Any();


        public ProjectTreeItem()
        {
            this.Children = new List<ProjectTreeItem>();
        }
    }
}
