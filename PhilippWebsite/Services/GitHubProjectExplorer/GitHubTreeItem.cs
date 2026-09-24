using System.Text.Json.Serialization;


namespace PhilippWebsite.Services.GitHubProjectExplorer
{
    public class GitHubTreeItem
    {
        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;


        [JsonPropertyName("mode")]
        public string Mode { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;


        [JsonPropertyName("sha")]
        public string Sha { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public int? Size { get; set; }


        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
