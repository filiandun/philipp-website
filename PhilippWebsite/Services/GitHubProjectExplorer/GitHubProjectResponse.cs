using System.Text.Json.Serialization;


namespace PhilippWebsite.Services.GitHubProjectExplorer
{
    public class GitHubProjectResponse
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;


        [JsonPropertyName("tree")]
        public List<GitHubTreeItem> Tree { get; set; } = new();

        [JsonPropertyName("truncated")]
        public bool Truncated { get; set; }
    }
}