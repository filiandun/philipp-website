using System.Collections;


namespace PhilippWebsite.Models.ProjectTree
{
    public class ProjectTreeRoot : IEnumerable<ProjectTreeItem>
    {
        public string Name { get; private set; }

        public List<ProjectTreeItem> Items { get; private set; }


        public ProjectTreeRoot(string name)
        {
            this.Name = name;

            this.Items = new List<ProjectTreeItem>();
        }

        public ProjectTreeRoot(string name, IEnumerable<ProjectTreeItem> items)
        {
            this.Name = name;

            this.Items = items.ToList();
        }


        public IEnumerator<ProjectTreeItem> GetEnumerator()
        {
            foreach (ProjectTreeItem item in this.Items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
