
namespace PhilippWebsite.Services.CodeHighlighter
{
    public interface ICodeHighlighterService
    {
        public Task PreloadAsync();

        public Task<string> GetHighlightedHtml(string code, string filePath);
    }
}