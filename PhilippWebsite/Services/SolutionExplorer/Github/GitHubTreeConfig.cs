
namespace PhilippWebsite.Services.SolutionExplorer.Github
{
    public class GitHubTreeConfig
    {

        public static readonly string SolutionName = "Philipp's pet projects";
        public static readonly IReadOnlyList<string> RepoList = [ "philipp-website", "docx-markdown-editor" ];

        public static string GetBaseUrl(string repo) => $"https://api.github.com/repos/filiandun/{repo}/git/trees/main?recursive=1";
    }
}