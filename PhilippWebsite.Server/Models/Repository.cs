
namespace PhilippWebsite.Server.Models
{
    public class Repository
    {
        public required Guid Id { get; set; }

        public required string Name { get; set; }
        public required string LastCommitSha { get; set; }

        public ICollection<FileNode> Nodes { get; set; } = new List<FileNode>();
    }
}
