using PhilippWebsite.Shared.ProjectTree;


namespace PhilippWebsite.Server.Services.ProjectTree
{
    public interface ITreeProvider
    {
        public Task<ProjectTreeItem?> GetTreeAsync();
    }
}
