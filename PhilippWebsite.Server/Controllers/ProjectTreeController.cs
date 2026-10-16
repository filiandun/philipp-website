using Microsoft.AspNetCore.Mvc;

using PhilippWebsite.Server.Services.ProjectTree;
using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectTreeController : ControllerBase
    {
        private readonly ILogger<ProjectTreeController> _logger;

        private readonly IProjectTreeService _projectTreeService;


        public ProjectTreeController(ILogger<ProjectTreeController> logger, IProjectTreeService projectTreeService)
        {
            this._logger = logger;

            this._projectTreeService = projectTreeService;
        }


        [HttpGet("GetTree", Name = "GetProjectTree")]
        public async Task<ActionResult<SolutionTreeRoot>> GetProjectTree()
        {
            this._logger.LogInformation("Request to Get Project Tree.");

            try
            {
                SolutionTreeRoot projectTreeRoot = await this._projectTreeService.GetTreeAsync();

                return Ok(projectTreeRoot);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to Get Project Tree.");

                return StatusCode(500, "Internal server error occurred while retrieving the tree.");
            }
        }
    }
}
