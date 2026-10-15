using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

using TextMateSharp.Grammars;
using TextMateSharp.Internal.Themes.Reader;
using TextMateSharp.Registry;
using TextMateSharp.Themes;


namespace PhilippWebsite.Server.Services.SyntaxHighlighter
{
    public class SyntaxHighlighterService : ISyntaxHighlighterService
    {
        private readonly ILogger<SyntaxHighlighterService> _logger;

        private readonly RegistryOptions _registryOptions;
        private readonly Registry _registry;

        private readonly Theme _theme;


        public SyntaxHighlighterService(ILogger<SyntaxHighlighterService> logger)
        {
            this._logger = logger;

            this._registryOptions = new RegistryOptions(ThemeName.VisualStudioDark);
            this._registry = new Registry(this._registryOptions);

            string themePath = "LocalData/Resources/dark_vs.json";

            if (File.Exists(themePath))
            {
                using FileStream stream = File.OpenRead(themePath);
                using StreamReader reader = new StreamReader(stream);

                IRawTheme customTheme = ThemeReader.ReadThemeSync(reader);

                this._registry.SetTheme(customTheme);

                this._logger.LogInformation("Set custom theme '{ThemeName}'.", customTheme.GetName());
            } 
            else
            {
                this._logger.LogWarning("Custom theme '{ThemePath}' not found. Set default.", themePath);
            }

            this._theme = this._registry.GetTheme();
        }


        public string[] GetHighlightedHtml(string fileContent, string filePath)
        {
            if (string.IsNullOrEmpty(fileContent))
            {
                this._logger.LogWarning("Content from '{FilePath}' is empty.", filePath);

                return Array.Empty<string>();
            }

            string fileExtension = Path.GetExtension(filePath) ?? string.Empty;

            IGrammar grammar = this._registry.LoadGrammar(this._registryOptions.GetScopeByExtension(Path.GetExtension(fileExtension)));
            if (grammar == null)
            {
                this._logger.LogInformation("File '{FilePath}' has unknown extension '{FileExtension}', return raw lines.", filePath, fileExtension);

                return fileContent.Split('\n');
            }
                

            List<string> htmlLines = new List<string>();

            IStateStack? ruleStack = null;

            using var reader = new StringReader(fileContent);
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                ITokenizeLineResult result = grammar.TokenizeLine(line, ruleStack, TimeSpan.MaxValue);
                ruleStack = result.RuleStack;

                StringBuilder stringBuilder = new StringBuilder();

                foreach (var token in result.Tokens)
                {
                    int foregroundId = -1;

                    foreach (var themeRule in this._theme.Match(token.Scopes))
                    {
                        if (foregroundId == -1 && themeRule.foreground > 0)
                        {
                            foregroundId = themeRule.foreground;
                        }
                    }

                    string colorHex = foregroundId != -1 ? this._theme.GetColor(foregroundId) : "#D4D4D4";

                    int length = Math.Min(token.EndIndex, line.Length) - token.StartIndex;
                    string textSegment = line.Substring(token.StartIndex, length);

                    string escapedText = HttpUtility.HtmlEncode(textSegment);

                    stringBuilder.Append($"<span style=\"color: {colorHex}\">{escapedText}</span>");
                }

                htmlLines.Add(stringBuilder.ToString());
            }

            this._logger.LogInformation("File '{FilePath}' was hightlighted as {Language}.", filePath, grammar.GetScopeName());

            return htmlLines.ToArray();
        }
    }
}
