using PhilippWebsite.Services.GitHubProjectExplorer;


namespace PhilippWebsite.Services
{
    public interface IProjectExplorerService
    {
        public Task<List<ProjectNode>?> GetProjectTreeAsync();
    }
}
