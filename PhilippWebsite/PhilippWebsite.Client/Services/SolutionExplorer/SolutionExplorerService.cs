using System.Net.Http.Json;

using PhilippWebsite.Client.Models.SolutionExplorer;

using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Client.Services.SolutionExplorer
{
    public class SolutionExplorerService : ISolutionExplorerService
    {
        private readonly ILogger<SolutionExplorerService> _logger;

        private readonly HttpClient _httpClient;


        public SolutionExplorerService(ILogger<SolutionExplorerService> logger, HttpClient httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
        }


        public async Task<SolutionExplorerRoot?> GetTreeAsync()
        {
            try
            {
                SolutionExplorerRoot explorerRoot = new();

                string url = "https://localhost:7210/ProjectTree/GetTree/";

                ProjectTreeRoot? treeRoot = await this._httpClient.GetFromJsonAsync<ProjectTreeRoot>(url);

                if (treeRoot is null) throw new HttpRequestException("Server return null Project Tree Root");

                foreach (ProjectTreeItem treeItem in treeRoot.Items)
                {
                    SolutionExplorerItem explorerItem = this.BuildTree(treeItem);

                    explorerRoot.Items.Add(explorerItem);
                }

                return explorerRoot;
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "Error get tree");

                return null;
            }
        }


        private SolutionExplorerItem BuildTree(ProjectTreeItem treeItem)
        {
            SolutionExplorerItem explorerItem = new()
            {
                Name = treeItem.Name,
                Path = treeItem.Path,

                Type = (SolutionExplorerItemType)treeItem.Type,
            };

            foreach (ProjectTreeItem itemChild in treeItem.Children)
            {
                SolutionExplorerItem explorerItemChild = this.BuildTree(itemChild);

                explorerItem.Items.Add(explorerItemChild);
            }

            return explorerItem;
        }

        private void SortTree(SolutionExplorerItem treeItem)
        {
            treeItem.Items.Sort((a, b) =>
            {
                if (a.Type != b.Type) return a.Type.CompareTo(b.Type);

                return a.Name.CompareTo(b.Name);
            });

            foreach (var item in treeItem.Items.Where(n => n.Items.Any()))
            {
                this.SortTree(item);
            }
        }
    }
}
