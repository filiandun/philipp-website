namespace PhilippWebsite.Server.Services.SyntaxHighlighter
{
    public interface ISyntaxHighlighterService
    {
        public string[] GetHighlightedHtml(string fileContent, string filePath);
    }
}