
namespace PhilippWebsite.Services.SolutionExplorer.Github
{
    public class GitHubTreeConfig
    {
        public string BaseUrl { get; set; } = string.Empty;

        public string SolutionName { get; set; } = string.Empty;
        public List<string> RepoList { get; set; } = new List<string>();
    }
}