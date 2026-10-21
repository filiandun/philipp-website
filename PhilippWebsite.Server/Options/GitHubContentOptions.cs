using PhilippWebsite.Shared.SolutionState;

using System.ComponentModel.DataAnnotations;


namespace PhilippWebsite.Server.Options
{
    public class GitHubContentOptions
    {
        public const string ApiBaseUrl = "https://api.github.com";
        public const string WebBaseUrl = "https://github.com";
        public const string RawBaseUrl = "https://raw.githubusercontent.com";

        [Required]
        public required string SectionName { get; init; }

        [Required]
        public required string Username { get; init; }
        [Required]
        public required string Branch { get; init; }

        [Required]
        public required Dictionary<string, SolutionTreeItemType> Repositories { get; init; }

        public string GetApiUrl(string repo) => $"{ApiBaseUrl}/repos/{this.Username}/{repo}/git/trees/{this.Branch}?recursive=1";
        public string GetWebUrl(string repo, string file) => $"{WebBaseUrl}/{this.Username}/{repo}/blob/{this.Branch}/{file}";
        public string GetRawUrl(string repo, string file) => $"{RawBaseUrl}/{this.Username}/{repo}/blob/{this.Branch}/{file}";
    }
}