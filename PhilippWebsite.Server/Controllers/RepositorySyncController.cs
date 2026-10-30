using Microsoft.AspNetCore.Mvc;

using PhilippWebsite.Server.Models;
using PhilippWebsite.Server.Services.RepositorySync;


namespace PhilippWebsite.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositorySyncController : Controller
    {
        private readonly ILogger<RepositorySyncController> _logger;

        private readonly IRepositorySyncService _repositorySyncService;


        public RepositorySyncController(ILogger<RepositorySyncController> logger, IRepositorySyncService repositorySyncService)
        {
            this._logger = logger;

            this._repositorySyncService = repositorySyncService;
        }


        [HttpPost("PostSync", Name = "PostSync")]
        public async Task<ActionResult<Repository>> PostSync([FromQuery] string repo)
        {
            this._logger.LogInformation("Request to sync repo '{Repo}'.", repo);

            try
            {
                Repository repository = await this._repositorySyncService.SyncRepoAsync(repo);

                return Ok(repository);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to sync repo '{repo}'.", repo);

                return StatusCode(500, "Internal server error occurred while retrieving the repo.");
            }
        }
    }
}
