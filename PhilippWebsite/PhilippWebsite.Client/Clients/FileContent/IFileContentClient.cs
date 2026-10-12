
namespace PhilippWebsite.Client.Clients.FileContent
{
    public interface IFileContentClient
    {
        public Task<string> GetContentAsync(string path);
    }
}
