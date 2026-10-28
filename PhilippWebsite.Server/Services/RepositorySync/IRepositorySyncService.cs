using PhilippWebsite.Server.Models;


namespace PhilippWebsite.Server.Services.RepositorySync
{
    public interface IRepositorySyncService
    {
        public Task<Repository> SyncRepoAsync(string repo);
    }
}