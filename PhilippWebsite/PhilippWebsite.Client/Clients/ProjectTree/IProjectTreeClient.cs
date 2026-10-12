using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Client.Services.FileContent
{
    public interface IProjectTreeClient
    {
        public Task<ProjectTreeRoot?> GetTreeAsync();
    }
}
