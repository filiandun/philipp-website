namespace PhilippWebsite.Client.Services.EditorContent
{
    public interface IEditorContentService
    {
        public event Action? OnContentChanged;

        public string[] CurrentContent { get; }
        public bool IsLoading { get; }
        public bool HasError { get; }
        public bool NothingSelected { get; }
    }
}