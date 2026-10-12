namespace PhilippWebsite.Server.Services.ProjectTree.GitHub
{
    public class GitHubTreeConfig
    {
        public static readonly string SolutionName = "Philipp's pet projects";
        public static readonly IReadOnlyList<string> RepoList = [ "philipp-website", "docx-markdown-editor" ];

        public static string GetApiUrl(string repo) => $"https://api.github.com/repos/filiandun/{repo}/git/trees/main?recursive=1";
        public static string GetGitUrl(string repo, string file) => $"https://github.com/filiandun/{repo}/blob/main/{file}";
    }
}