using Microsoft.EntityFrameworkCore;

using PhilippWebsite.Server.Models;

using System.Xml.Linq;



namespace PhilippWebsite.Server.Infrastructure.Storage
{
    public class RepositoryStorage : IRepositoryReader, IRepositoryWriter
    {
        private readonly ILogger<RepositoryStorage> _logger;

        private readonly RepositoryDbContext _dbContext;


        public RepositoryStorage(ILogger<RepositoryStorage> logger, RepositoryDbContext dbContext)
        {
            this._logger = logger;

            this._dbContext = dbContext;
        }


        public Task<Repository?> GetRepositoryAsync(string name)
        {
            return this._dbContext.Repositories.Include(r => r.Nodes).AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(r => r.Name == name);
        }

        public Task<FileContent?> GetFileContentAsync(string path)
        {
            return this._dbContext.FileContents.Include(c => c.Node).AsNoTracking().FirstOrDefaultAsync(c => c.Node.Path == path);
        }



        public async Task UpdateRepositoryAsync(Repository repository)
        {
            using var transaction = await this._dbContext.Database.BeginTransactionAsync();

            try
            {
                Repository? existingRepository = this._dbContext.Repositories.FirstOrDefault(r => r.Name == repository.Name);

                if (existingRepository is not null)
                {
                    this._dbContext.Repositories.Remove(existingRepository);

                    this._logger.LogDebug("Repository '{Repository}' remove for update.", existingRepository.Name);

                    await this._dbContext.SaveChangesAsync();
                }

                this._dbContext.Repositories.Add(repository);

                await this._dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                this._logger.LogInformation("Repository '{Repository}' successfully update.", repository.Name);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to update repository '{RepositoryName}'.", repository.Name);

                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
