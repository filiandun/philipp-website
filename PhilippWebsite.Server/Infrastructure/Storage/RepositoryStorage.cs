using PhilippWebsite.Server.Models;


namespace PhilippWebsite.Server.Infrastructure.Storage
{
    public class RepositoryStorage : IRepositoryReader, IRepositoryWriter
    {
        private readonly ILogger<RepositoryStorage> _logger;

        private readonly RepositoryDbContext _repositories;


        public RepositoryStorage(ILogger<RepositoryStorage> logger, RepositoryDbContext repositories)
        {
            this._logger = logger;

            this._repositories = repositories;
        }


        public IEnumerable<Repository> GetRepositories()
        {
            return this._repositories.Repositories;
        }

        
    }
}
