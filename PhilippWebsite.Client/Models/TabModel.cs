
namespace PhilippWebsite.Client.Models
{
    public record class TabModel
    {
        public required string Name { get; set; }
        public string? Path { get; set; }
    }
}