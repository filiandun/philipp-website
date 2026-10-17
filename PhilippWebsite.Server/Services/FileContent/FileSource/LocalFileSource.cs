
namespace PhilippWebsite.Server.Services.FileContent.FileSource
{
    public class LocalFileSource : IFileSource
    {
        private readonly ILogger<LocalFileSource> _logger;

        private readonly IWebHostEnvironment _environment;


        public LocalFileSource(ILogger<LocalFileSource> logger, IWebHostEnvironment environment)
        {
            this._logger = logger;

            this._environment = environment;
        }


        public bool CanHandle(string path) => path.StartsWith("Content/");


        public async Task<string> GetRawContentAsync(string path)
        {
            string safePath = Path.GetFullPath(Path.Combine(this._environment.ContentRootPath, "LocalData"));
            if (!safePath.EndsWith(Path.DirectorySeparatorChar)) safePath += Path.DirectorySeparatorChar;

            string targetPath = Path.GetFullPath(Path.Combine(safePath, path));

            if (!targetPath.StartsWith(safePath))
            {
                this._logger.LogWarning("Path traversal attempt detected '{Path}'", targetPath);

                throw new UnauthorizedAccessException("Invalid local path.");
            }

            if (!File.Exists(targetPath))
            {
                this._logger.LogWarning("Local file not found '{Path}'.", targetPath);

                throw new FileNotFoundException($"File not found.", targetPath);
            }

            string fileContent = await File.ReadAllTextAsync(targetPath);

            this._logger.LogInformation("Success loading local file content '{Path}'.", targetPath);

            return fileContent;
        }
    }
}
