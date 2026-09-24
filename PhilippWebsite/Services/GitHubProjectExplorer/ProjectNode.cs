
namespace PhilippWebsite.Services.GitHubProjectExplorer
{
    public class ProjectNode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ProjectNodeType Type { get; set; }

        public List<ProjectNode> Children { get; set; } 


        public ProjectNode()
        {
            this.Name = string.Empty;
            this.Path = string.Empty;
            this.Type = default;

            this.Children = new List<ProjectNode>();
        }
    }
}
