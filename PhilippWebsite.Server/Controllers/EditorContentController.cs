using Microsoft.AspNetCore.Mvc;

using PhilippWebsite.Server.Services.EditorContent;


namespace PhilippWebsite.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EditorContentController : Controller
    {
        private readonly ILogger<EditorContentController> _logger;

        private readonly IEditorContentService _codeViewerService;


        public EditorContentController(ILogger<EditorContentController> logger, IEditorContentService codeViewerService)
        {
            this._logger = logger;

            this._codeViewerService = codeViewerService;
        }


        [HttpGet("GetContent", Name = "GetContent")]
        public async Task<ActionResult<string[]>> GetContent([FromQuery] string path)
        {
            this._logger.LogInformation("Request to Get Content by '{FilePath}'.", path);

            try
            {
                string[] fileCode = await this._codeViewerService.GetContent(path);

                return Ok(fileCode);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to Get Content by '{FilePath}'.", path);

                return StatusCode(500, "Internal server error occurred while retrieving the editor content.");
            }
        }
    }
}
