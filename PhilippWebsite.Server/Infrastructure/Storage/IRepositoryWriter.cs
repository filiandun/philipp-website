using PhilippWebsite.Server.Models;


namespace PhilippWebsite.Server.Infrastructure.Storage
{
    public interface IRepositoryWriter
    {
        public Task UpdateRepositoryAsync(Repository repository);
    }
}
