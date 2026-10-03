using ColorCode;
using ColorCode.Styling;


namespace PhilippWebsite.Services.CodeHighlighter
{
    public class ColorCodeHighlighterService : ICodeHighlighterService
    {
        private readonly ILogger<ColorCodeHighlighterService> _logger;

        private readonly HtmlFormatter _formatter;


        public ColorCodeHighlighterService(ILogger<ColorCodeHighlighterService> logger)
        {
            this._logger = logger;

            this._formatter = new HtmlFormatter(StyleDictionary.DefaultDark);
        }


        public string GetHighlightedHtml(string code, string filePath)
        {
            if (string.IsNullOrEmpty(code))
            {
                this._logger.LogWarning("Code from '{FilePath}' is empty.", filePath);

                return string.Empty;
            }

            string fileExtension = Path.GetExtension(filePath)?.TrimStart('.') ?? string.Empty;

            ILanguage? language = Languages.FindById(fileExtension);
            if (language is null)
            {
                this._logger.LogWarning("File '{FilePath}' has unsupported extension {FileExtension}.", filePath, fileExtension);
                language = Languages.CSharp;
            }

            this._logger.LogInformation("File '{FilePath}' was hightlighted as {Language}.", filePath, language.Name);

            return this._formatter.GetHtmlString(code, language);
        }
    }
}
