namespace PhilippWebsite.Server.Services.EditorContent
{
    public interface IEditorContentService
    {
        public Task<string[]> GetContent(string filePath);
    }
}