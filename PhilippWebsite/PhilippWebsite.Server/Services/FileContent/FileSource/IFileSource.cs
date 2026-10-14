namespace PhilippWebsite.Server.Services.FileContent.FileSource
{
    public interface IFileSource
    {
        public bool CanHandle(string path);
        public Task<string> GetRawContentAsync(string path);
    }
}
