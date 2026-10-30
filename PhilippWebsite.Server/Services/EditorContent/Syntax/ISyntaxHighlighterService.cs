namespace PhilippWebsite.Server.Services.EditorContent.Syntax
{
    public interface ISyntaxHighlighterService
    {
        public string[] GetHighlightedHtml(string fileContent, string filePath);
    }
}