
namespace PhilippWebsite.Services.SolutionExplorer
{
    public class GitHubApiConfig
    {
        public string SolutionName { get; set; } = string.Empty;

        public string BaseUrl { get; set; } = string.Empty;

        public List<string> RepoList { get; set; } = new List<string>();
    }
}