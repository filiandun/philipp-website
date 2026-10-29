using PhilippWebsite.Server.Models;


namespace PhilippWebsite.Server.Infrastructure.Storage
{
    public interface IRepositoryReader
    {
        public Task<Repository?> GetRepositoryAsync(string name);

        public Task<FileContent?> GetFileContentAsync(string path);
    }
}