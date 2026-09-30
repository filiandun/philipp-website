using System.Collections;


namespace PhilippWebsite.Models.SolutionExplorer
{
    public class SolutionExplorerRoot : IEnumerable<SolutionExplorerItem>
    {
        public List<SolutionExplorerItem> Items { get; private set; }


        public SolutionExplorerRoot()
        {
            this.Items = new List<SolutionExplorerItem>();
        }

        public SolutionExplorerRoot(IEnumerable<SolutionExplorerItem> items)
        {
            this.Items = items.ToList();
        }


        public IEnumerator<SolutionExplorerItem> GetEnumerator()
        {
            foreach (SolutionExplorerItem item in this.Items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}