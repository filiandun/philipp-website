using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Extensions
{
    public static class TreeExtensions
    {
        public static IEnumerable<SolutionTreeItem> Flatten(this IEnumerable<SolutionTreeItem> treeItem)
        {
            if (treeItem == null) yield break;

            foreach (var item in treeItem)
            {
                yield return item;

                foreach (var child in item.Children.Flatten())
                {
                    yield return child;
                }
            }
        }
    }
}
