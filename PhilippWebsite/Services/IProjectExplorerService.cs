using PhilippWebsite.Models;


namespace PhilippWebsite.Services
{
    public interface IProjectExplorerService
    {
        public Task<GitHubTreeResponse?> GetProjectTreeContentAsync();
    }
}
