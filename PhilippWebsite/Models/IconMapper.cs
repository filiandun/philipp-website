using MudBlazor;


namespace PhilippWebsite.Models
{
    public static class IconMapper
    {
        private static readonly Dictionary<string, string> _iconMapper = new()
        {
            { "GitHub", Icons.Custom.Brands.GitHub },
            { "Telegram", Icons.Custom.Brands.Telegram },
            { "LinkedIn", Icons.Custom.Brands.LinkedIn },
            { "Email", Icons.Material.Filled.Email },

            { "Code", Icons.Material.Filled.Code },
            { "Build", Icons.Material.Filled.Build },
            { "AutoStories", Icons.Material.Filled.AutoStories }
        };

        public static string GetIcon(string iconName)
        {
            if (string.IsNullOrEmpty(iconName)) return Icons.Material.Filled.Link;

            return _iconMapper.TryGetValue(iconName, out var icon) ? icon : Icons.Material.Filled.Link;
        }
    }
}
