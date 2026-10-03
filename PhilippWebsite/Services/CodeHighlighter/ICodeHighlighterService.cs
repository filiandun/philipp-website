
namespace PhilippWebsite.Services.CodeHighlighter
{
    public interface ICodeHighlighterService
    {
        public string GetHighlightedHtml(string code, string filePath);
    }
}