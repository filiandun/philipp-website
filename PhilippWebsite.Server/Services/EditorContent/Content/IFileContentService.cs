namespace PhilippWebsite.Server.Services.EditorContent.Content
{
    public interface IFileContentService
    {
        public Task<string> GetContentAsync(string path);
    }
}
