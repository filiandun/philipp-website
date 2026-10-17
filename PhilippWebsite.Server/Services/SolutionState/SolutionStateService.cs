using PhilippWebsite.Server.Services.ProjectTree;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionState
{
    public class SolutionStateService : ISolutionStateService
    {
        private readonly ILogger<SolutionStateService> _logger;

        private readonly IProjectTreeService _projectTreeService;


        public SolutionStateService(ILogger<SolutionStateService> logger, IProjectTreeService projectTreeService)
        {
            this._logger = logger;

            this._projectTreeService = projectTreeService;
        }


        public async Task<SolutionStateModel> GetModel()
        {
            SolutionTreeRoot treeRoot = await this._projectTreeService.GetTreeAsync();

            SolutionStateModel solutionModel = new()
            {
                Name = "PhilippPortfolio",
                Tree = treeRoot,
                InitialOpenItems = treeRoot.Items.FirstOrDefault()?.Children ?? [],
                ActiveItem = treeRoot.Items.FirstOrDefault()?.Children.FirstOrDefault(i => i.Name == "AboutMe.md") ?? null
            };

            return solutionModel;
        }

        /* Надо сделать, чтобы не делать поиск как выше через firstordefault
         * 
         * public static class TreeExtensions
        {
            // Метод превращает дерево в плоскую последовательность
            public static IEnumerable<TreeItem> Flatten(this IEnumerable<TreeItem> nodes)
            {
                if (nodes == null) yield break;

                foreach (var node in nodes)
                {
                    yield return node;

                    if (node.Children != null)
                    {
                        foreach (var child in node.Children.Flatten())
                        {
                            yield return child;
                        }
                    }
                }
            }
        }*/

    }
}
