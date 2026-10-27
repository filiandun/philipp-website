
namespace PhilippWebsite.Server.Models
{
    public class FileContent
    {
        public required Guid NodeId { get; set; }

        public required string[] Text { get; set; }
        public required int Size { get; set; }

        public required FileNode Node { get; set; }
    }
}
