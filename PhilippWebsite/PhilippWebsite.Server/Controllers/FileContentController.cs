using Microsoft.AspNetCore.Mvc;

using PhilippWebsite.Server.Services.FileContent;


namespace PhilippWebsite.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileContentController : ControllerBase
    {
        private readonly ILogger<FileContentController> _logger;

        private readonly IFileContentService _fileContentService;


        public FileContentController(ILogger<FileContentController> logger, IFileContentService fileContentService)
        {
            this._logger = logger;

            this._fileContentService = fileContentService;
        }


        [HttpGet("GetContent", Name = "GetFileContent")]
        public async Task<ActionResult<string>> GetFileContent([FromQuery] string path)
        {
            this._logger.LogInformation("Request to Get File Content by '{FilePath}'.", path);

            try
            {
                string fileContent = await this._fileContentService.GetContentAsync(path);

                return Ok(fileContent);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error to Get File Content by '{FilePath}'.", path);

                return StatusCode(500, "Internal server error occurred while retrieving the file content.");
            }
        }
    }
}
