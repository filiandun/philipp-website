using PhilippWebsite.Server.Services.EditorContent.Content.Source;

namespace PhilippWebsite.Server.Services.EditorContent.Content
{
    public class FileContentService : IFileContentService
    {
        private readonly ILogger<FileContentService> _logger;

        private readonly IEnumerable<IFileSource> _sources;


        public FileContentService(ILogger<FileContentService> logger, IEnumerable<IFileSource> sources)
        {
            this._logger = logger;

            this._sources = sources;
        }
       

        public async Task<string> GetContentAsync(string filePath)
        {
            IFileSource? fileSource = this._sources.FirstOrDefault(s => s.CanHandle(filePath));

            if (fileSource is null)
            {
                this._logger.LogWarning("No file source registered to handle path '{FilePath}'.", filePath);

                throw new InvalidOperationException($"No file source registered to handle path '{filePath}'");
            }

            this._logger.LogDebug("FileSource '{FileSource}' to handling path '{FilePath}'.", fileSource.GetType().Name, filePath);

            return await fileSource.GetRawContentAsync(filePath);
        }
    }
}
