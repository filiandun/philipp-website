
namespace PhilippWebsite.Server.Services
{
    public static class PathHelper
    {
        public static string Combine(params string[] paths)
        {
            return string.Join("/", paths.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim('/')));
        }
    }
}
