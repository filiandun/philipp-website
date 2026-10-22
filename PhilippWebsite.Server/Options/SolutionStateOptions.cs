using System.ComponentModel.DataAnnotations;


namespace PhilippWebsite.Server.Options
{
    public class SolutionStateOptions
    {
        [Required]
        public required string SolutionName { get; init; }

        public List<string> InitialExpandedPathes { get; init; } = new();

        public List<string> InitialOpenPathes { get; init; } = new();

        public string? InitialActivePath { get; init; } = null;
    }
}
