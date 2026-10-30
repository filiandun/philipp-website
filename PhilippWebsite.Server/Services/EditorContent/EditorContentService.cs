using PhilippWebsite.Server.Services.EditorContent.Content;
using PhilippWebsite.Server.Services.EditorContent.Syntax;


namespace PhilippWebsite.Server.Services.EditorContent
{
    public class EditorContentService : IEditorContentService
    {
        private readonly ILogger<EditorContentService> _logger;

        private readonly IFileContentService _fileContentService;
        private readonly ISyntaxHighlighterService _codeHighlighterService;


        public EditorContentService(ILogger<EditorContentService> logger, IFileContentService fileContentService, ISyntaxHighlighterService codeHighlighterService)
        {
            this._logger = logger;

            this._fileContentService = fileContentService;
            this._codeHighlighterService = codeHighlighterService;
        }


        public async Task<string[]> GetContent(string filePath)
        {
            string fileContent = await this._fileContentService.GetContentAsync(filePath);

            this._logger.LogInformation("Get file content '{FileContentLength}' length by '{FilePath}'", fileContent.Length, filePath);

            string[] highlightedLines = this._codeHighlighterService.GetHighlightedHtml(fileContent, filePath);

            this._logger.LogInformation("Make highlighted content '{HighlightedLines}' lines by '{FilePath}'", highlightedLines.Length, filePath);

            return highlightedLines;
        }
    }
}
