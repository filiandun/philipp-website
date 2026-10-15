
namespace PhilippWebsite.Shared.ProjectTree
{
    public class ProjectTreeRoot
    {
        public List<ProjectTreeItem> Items { get; init; }


        public ProjectTreeRoot()
        {
            this.Items = new List<ProjectTreeItem>();
        }
    }
}