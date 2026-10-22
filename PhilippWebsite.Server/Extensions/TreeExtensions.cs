using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Extensions
{
    public static class TreeExtensions
    {
        public static IEnumerable<SolutionTreeItem> Flatten(this SolutionTreeItem treeItem)
        {
            if (treeItem == null) yield break;

            yield return treeItem;

            foreach (var item in treeItem.Children)
            {
                yield return item;

                foreach (var child in item.Flatten())
                {
                    yield return child;
                }
            }
        }
    }
}
