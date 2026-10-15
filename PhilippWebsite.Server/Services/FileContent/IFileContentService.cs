
namespace PhilippWebsite.Server.Services.FileContent
{
    public interface IFileContentService
    {
        public Task<string> GetContentAsync(string path);
    }
}
