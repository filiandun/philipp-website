using PhilippWebsite.Shared.SolutionState;

using System.ComponentModel.DataAnnotations;


namespace PhilippWebsite.Server.Options
{
    public class LocalContentOptions
    {
        [Required]
        public required string SectionName { get; init; }
        [Required]
        public required SolutionTreeItemType SectionType { get; init; }

        [Required]
        public required string StorageFolder { get; init; }

        [Required]
        public required List<string> Files { get; init; }
    }
}
