namespace PhilippWebsite.Client.Clients.EditorContent
{
    public interface IEditorContentClient
    {
        public Task<string[]> GetCodeAsync(string path);
    }
}
