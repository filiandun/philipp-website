using PhilippWebsite.Server.Services.FileContent;
using PhilippWebsite.Server.Services.SyntaxHighlighter;


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


        public async Task<string[]> GetHighlightedLines(string filePath)
        {
            string fileContent = await this._fileContentService.GetContentAsync(filePath);

            string[] highlightedLines = this._codeHighlighterService.GetHighlightedHtml(fileContent, filePath);

            return highlightedLines;
        }
    }
}
