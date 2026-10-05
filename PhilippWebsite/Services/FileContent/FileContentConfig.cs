
namespace PhilippWebsite.Services.FileContent
{
    public static class FileContentConfig
    {
        public static string GetGitHubUrl(string repo, string path) => $"https://raw.githubusercontent.com/filiandun/{repo}/main/{path}";
        public static string GetLocalUrl(string path) => $"content/{path}";
    }
}
