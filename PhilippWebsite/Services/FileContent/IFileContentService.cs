using PhilippWebsite.Models;


namespace PhilippWebsite.Services.FileContent
{
    public interface IFileContentService
    {
        public Task<string> GetContentAsync(string repo, string path, FileSource source);
    }
}
