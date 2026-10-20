using System.ComponentModel.DataAnnotations;


namespace PhilippWebsite.Server.Options
{
    public class SolutionStateOptions
    {
        [Required]
        public required string SolutionName { get; init; }

        public List<string> InitialExpandedItems { get; init; } = new();

        public List<string> InitialOpenItems { get; init; } = new();

        public string? InitialActiveItem { get; init; } = null;
    }
}
