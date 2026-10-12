
namespace PhilippWebsite.Client.Services.FileContent
{
    public interface IFileContentClient
    {
        public Task<string> GetContentAsync(string path);
    }
}
