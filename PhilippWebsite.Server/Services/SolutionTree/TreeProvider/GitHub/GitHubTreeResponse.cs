using System.Text.Json.Serialization;


namespace PhilippWebsite.Server.Services.SolutionTree.TreeProvider.GitHub
{
    public class GitHubTreeResponse
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;


        [JsonPropertyName("tree")]
        public List<GitHubTreeItem> LinearTree { get; set; } = new();

        [JsonPropertyName("truncated")]
        public bool Truncated { get; set; }
    }
}