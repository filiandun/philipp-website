using Microsoft.AspNetCore.Mvc;

using PhilippWebsite.Server.Services.SolutionState;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolutionStateController : ControllerBase
    {
        private readonly ILogger<SolutionStateController> _logger;

        private readonly ISolutionStateService _solutionStateService;


        public SolutionStateController(ILogger<SolutionStateController> logger, ISolutionStateService solutionStateService)
        {
            this._logger = logger;

            this._solutionStateService = solutionStateService;
        }


        [HttpGet("GetModel", Name = "GetModel")]
        public async Task<ActionResult<SolutionStateModel>> GetModel()
        {
            this._logger.LogInformation("Request to Get Solution State.");

            try
            {
                SolutionStateModel solutionStateModel = await this._solutionStateService.GetModel();

                return Ok(solutionStateModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to Get Solution State.");

                return StatusCode(500, "Internal server error occurred while retrieving the Solution State.");
            }
        }
    }
}
