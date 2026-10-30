using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Options;


namespace PhilippWebsite.Server.Services.FileContent.FileSource
{
    public class LocalFileSource : IFileSource
    {
        private readonly ILogger<LocalFileSource> _logger;

        private readonly IWebHostEnvironment _environment;

        private readonly LocalContentOptions _options;


        public LocalFileSource(ILogger<LocalFileSource> logger, IWebHostEnvironment environment, IOptions<LocalContentOptions> options)
        {
            this._logger = logger;

            this._environment = environment;

            this._options = options.Value;
        }


        public bool CanHandle(string path) => path.StartsWith(this._options.SectionName);


        public async Task<string> GetRawContentAsync(string path)
        {
            string safePath = Path.GetFullPath(Path.Combine(this._environment.ContentRootPath, "LocalData", "Content"));
            if (!safePath.EndsWith(Path.DirectorySeparatorChar)) safePath += Path.DirectorySeparatorChar;

            string subPath = path.Substring(this._options.SectionName.Length + 1);

            string targetPath = Path.GetFullPath(Path.Combine(safePath, subPath));

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

            this._logger.LogInformation("Success loading local file content '{Path}'.", path);

            return fileContent;
        }
    }
}
