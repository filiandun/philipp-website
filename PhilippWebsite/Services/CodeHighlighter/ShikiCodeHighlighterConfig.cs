
namespace PhilippWebsite.Services.CodeHighlighter
{
    public static class ShikiCodeHighlighterConfig
    {
        public static readonly string Theme = "/content/dark_vs.json";

        public static readonly string DefaultLanguage = "xml";

        public static readonly IReadOnlyDictionary<string, string> ExtensionToLanguageMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".cs", "csharp" },
            { ".js", "javascript" },
            { ".ts", "typescript" },
            { ".md", "markdown" },
            { ".json", "json" },
            { ".html", "html" },
            { ".css", "css" },
            { ".xml", "xml" },
            { ".xaml", "xml" },
            { ".razor", "razor" },
            { ".slnx", "xml" },
            { ".csproj", "xml" }
        };
    }
}
