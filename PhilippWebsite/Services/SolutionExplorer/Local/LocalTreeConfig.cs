
namespace PhilippWebsite.Services.SolutionExplorer.Local
{
    public static class LocalTreeConfig
    {
        public static readonly string SolutionName = "About Philipp";
        public static readonly IReadOnlyList<string> FileList = [ "aboutme.md", "contacts.md" ];

        public static string GetBaseUrl(string path) => $"content/{path}";
    }
}
