namespace PhilippWebsite.Server.Services.RepositorySync.Content
{
    public interface IRepositoryContentService
    {
        public Task<Dictionary<string, string>> GetContentAsync(string repo, string sha);
    }
}