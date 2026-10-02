
namespace PhilippWebsite.Services.SolutionExplorer.Local
{
    public class LocalTreeConfig
    {
        public string BaseUrl { get; set; } = string.Empty;

        public string SolutionName { get; set; } = string.Empty;
        public List<string> FileList { get; set; } = new List<string>();
    }
}
