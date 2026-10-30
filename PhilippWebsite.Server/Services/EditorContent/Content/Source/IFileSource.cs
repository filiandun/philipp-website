namespace PhilippWebsite.Server.Services.EditorContent.Content.Source
{
    public interface IFileSource
    {
        public bool CanHandle(string path);
        public Task<string> GetRawContentAsync(string path);
    }
}
