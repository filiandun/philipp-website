
namespace PhilippWebsite.Services.FileContent
{
    public interface IFileContentService
    {
        public Task<string> GetFileContentAsync(string repo, string filePath);
    }
}
