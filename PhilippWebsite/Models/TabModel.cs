
namespace PhilippWebsite.Models
{
    public class TabModel
    {
        public required string Name { get; set; }
        public required string Path { get; set; }

        public required string Repo { get; set; }

        public required FileSource Source { get; set; }
    }
}