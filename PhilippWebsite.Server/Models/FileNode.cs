
using System.Text.Json.Serialization;

namespace PhilippWebsite.Server.Models
{
    public class FileNode
    {
        public required Guid Id { get; set; }

        public required Guid RepositoryId { get; set; }
        public required Guid? ParentId { get; set; }

        public required string Name { get; set; }
        public required string Path { get; set; }
        public required bool IsDirectory { get; set; }

        [JsonIgnore]
        public Repository? Repository { get; set; }
        [JsonIgnore]
        public FileNode? Parent { get; set; }
        public ICollection<FileNode> Children { get; set; } = new List<FileNode>();
        public FileContent? Content { get; set; }
    }
}
