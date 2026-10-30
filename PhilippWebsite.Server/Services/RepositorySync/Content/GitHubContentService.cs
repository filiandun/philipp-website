using Microsoft.Extensions.Options;

using System.IO.Compression;
using System.Text;

using PhilippWebsite.Server.Options;


namespace PhilippWebsite.Server.Services.RepositorySync.Content
{
    public class GitHubContentService : IRepositoryContentService
    {
        private static readonly HashSet<string> _allowedFileExtensions = new(StringComparer.OrdinalIgnoreCase) // TODO вынести отсюда в статический класс типа options
        {
            // C#, .NET, WPF
            ".cs",
            ".razor",
            ".xaml",
            ".csproj",
            ".sln",
            ".slnx",
            ".props",
            ".targets",
            ".resx",
            ".config",

            // VS Code Extension
            ".ts",
            ".tsx",
            ".js",
            ".mjs",
            ".cjs",
            ".json",
            ".jsonc",
            ".vscodeignore",

            // Web (Blazor)
            ".html",
            ".htm",
            ".css",
            ".scss",
            ".less",
            ".svg",

            // Docs
            ".md",
            ".txt",
            ".yml",
            ".yaml",
            ".xml",
            ".editorconfig",
            ".gitattributes",
            ".gitignore",
            ".dockerignore",
            ".http"
        };

        private static readonly HashSet<string> _allowedFileNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "dockerfile",
            "license",
            "cname"
        };

        private readonly ILogger<GitHubContentService> _logger;

        private readonly HttpClient _httpClient;

        private readonly GitHubContentOptions _options;


        public GitHubContentService(ILogger<GitHubContentService> logger, HttpClient httpClient, IOptions<GitHubContentOptions> options)
        {
            this._logger = logger;

            this._httpClient = httpClient;

            this._options = options.Value;
        }


        public async Task<Dictionary<string, string>> GetContentAsync(string repo, string sha)
        {
            using ZipArchive archiveContent = await this.GetGitHubArchive(repo, sha);

            Dictionary<string, string> fileContents = await this.MapArchiveToDictionary(archiveContent);

            this._logger.LogInformation("Total file contents for repo '{Repo}' sha '{Sha}' count '{ContentsCount}'", repo, sha, fileContents.Count);

            return fileContents;
        }


        private async Task<ZipArchive> GetGitHubArchive(string repo, string sha)
        {
            this._logger.LogInformation("Get Content for repo '{Repo}' sha '{Sha}'.", repo, sha);

            string url = $"https://api.github.com/repos/{this._options.Username}/{repo}/zipball/{sha}"; // TODO вынести в options или в константу

            var response = await this._httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                this._logger.LogInformation("Success loading GitHub content '{Url}'.", url);

                return new ZipArchive(await response.Content.ReadAsStreamAsync());
            }

            this._logger.LogWarning("GitHub returned {StatusCode} for '{Url}'.", response.StatusCode, url);

            throw new HttpRequestException($"GitHub returned error {response.StatusCode}");
        }


        private async Task<Dictionary<string, string>> MapArchiveToDictionary(ZipArchive archiveContent)
        {
            Dictionary<string, string> dictionary = new();

            foreach (var entry in archiveContent.Entries)
            {
                if (entry.FullName.EndsWith('/')) continue;

                this._logger.LogTrace("Handling entry '{EntryName}' has size '{EntryLength}' byte", entry.FullName, entry.Length);

                if (!IsTextFile(entry.Name))
                {
                    this._logger.LogTrace("Entry '{EntryName}' has not allowed extension.", entry.Name);

                    continue;
                }

                using StreamReader fileStream = new StreamReader(await entry.OpenAsync(), Encoding.UTF8);

                string fileContent = await fileStream.ReadToEndAsync();
                string filePath = entry.FullName.Substring(entry.FullName.IndexOf('/') + 1);

                this._logger.LogTrace("Add file content as '{FilePath}' with content '{ContentLength}' byte", filePath, fileContent.Length);

                dictionary.Add(filePath, fileContent);
            }

            return dictionary;
        }

        public static bool IsTextFile(string path)
        {
            string fileName = Path.GetFileName(path);
            if (_allowedFileNames.Contains(fileName)) return true;

            string extension = Path.GetExtension(path);
            return _allowedFileExtensions.Contains(extension);
        }
    }
}
