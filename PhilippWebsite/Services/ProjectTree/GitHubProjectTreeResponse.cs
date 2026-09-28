using System.Text.Json.Serialization;


namespace PhilippWebsite.Services.ProjectTree
{
    public class GitHubProjectTreeResponse
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