using PhilippWebsite.Client.Models;

namespace PhilippWebsite.Client.Services.FileContent
{
    public interface IFileContentService
    {
        public Task<string> GetContentAsync(string repo, string path, FileSource source);
    }
}
