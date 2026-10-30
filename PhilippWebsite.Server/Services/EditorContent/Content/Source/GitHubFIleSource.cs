using Microsoft.Extensions.Options;

using PhilippWebsite.Server.Infrastructure.Storage;
using PhilippWebsite.Server.Options;


namespace PhilippWebsite.Server.Services.EditorContent.Content.Source
{
    public class GitHubFileSource : IFileSource
    {
        private readonly ILogger<GitHubFileSource> _logger;

        private readonly IRepositoryReader _repositoryReader;

        private readonly GitHubContentOptions _options;


        public GitHubFileSource(ILogger<GitHubFileSource> logger, IOptions<GitHubContentOptions> options, IRepositoryReader repositoryReader)
        {
            this._logger = logger;

            this._repositoryReader = repositoryReader;

            this._options = options.Value;
        }


        public bool CanHandle(string url) => url.StartsWith(this._options.SectionName);


        public async Task<string> GetRawContentAsync(string path)
        {
            string subPath = path.Substring(this._options.SectionName.Length + 1);

            this._logger.LogDebug("Path '{Path}' GitHub file content cut to '{SubPath}'.", path, subPath);

            Models.FileContent? fileContent = await this._repositoryReader.GetFileContentAsync(subPath);

            if (fileContent is null)
            {
                this._logger.LogWarning("Database return null for GitHub file content by '{Path}'.", subPath);

                throw new FileNotFoundException($"Database return null for GitHub file content by '{subPath}'.");
            }

            this._logger.LogInformation("Success loading from databse GitHub file content '{Path}'.", path);

            return fileContent.Text;
        }
    }
}
