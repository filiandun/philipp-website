namespace PhilippWebsite.Client.Clients.EditorContent
{
    public interface IEditorContentClient
    {
        public Task<string[]?> GetContentAsync(string path);
    }
}
