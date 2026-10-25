using PhilippWebsite.Client.Clients.EditorContent;

using PhilippWebsite.Client.Services.TabsBarState;


namespace PhilippWebsite.Client.Services.EditorContent
{
    public class EditorContentService : IEditorContentService, IDisposable
    {
        public event Action? OnContentChanged;

        private readonly ILogger<EditorContentService> _logger;

        private readonly IEditorContentClient _editorContentClient;

        private readonly TabsBarStateService _tabsBarStateService;


        private Dictionary<string, string[]> _filesContentCache;


        public string[] CurrentContent { get; private set; }

        public bool IsLoading { get; private set; }
        public bool HasError { get; private set; }
        public bool NothingSelected { get; private set; }


        public EditorContentService(ILogger<EditorContentService> logger, IEditorContentClient editorContentClient, TabsBarStateService tabsBarStateService)
        {
            this._logger = logger;

            this._editorContentClient = editorContentClient;

            this._tabsBarStateService = tabsBarStateService;
            this._tabsBarStateService.OnActiveTabChanged += this.OnActiveTabChange;

            this._filesContentCache = new Dictionary<string, string[]>();

            this.CurrentContent = [];

            this.IsLoading = true;
            this.HasError = false;
            this.NothingSelected = false;
        }

        private async void OnActiveTabChange()
        {
            if (this._tabsBarStateService.ActiveTab is null)
            {
                this._logger.LogInformation("Active Tab is null, current content set empty.");

                this.CurrentContent = [];
                this.OnContentChanged?.Invoke();

                this.HasError = false;
                this.NothingSelected = true;

                return;
            }

            if (this._tabsBarStateService.ActiveTab.Path is null)
            {
                this._logger.LogWarning("Active Tab Path is null, current content set empty.");

                this.CurrentContent = [];
                this.OnContentChanged?.Invoke();

                this.HasError = true;
                this.NothingSelected = false;

                return;
            }

            this.IsLoading = true;
            this.OnContentChanged?.Invoke();

            this.CurrentContent = await this.GetContentAsync(this._tabsBarStateService.ActiveTab.Path);

            this.IsLoading = false;
            this.NothingSelected = false;
            this.OnContentChanged?.Invoke();
        }


        private async Task<string[]> GetContentAsync(string filePath)
        {
            if (this._filesContentCache.TryGetValue(filePath, out string[]? cachedFileContent))
            {
                this._logger.LogInformation("File Content for '{FilePath}' get from cache.", filePath);

                return cachedFileContent;
            }

            this._logger.LogInformation("File Content for '{FilePath}' miss in cache. Get from server.", filePath);

            string[]? fileContent = await this._editorContentClient.GetContentAsync(filePath);

            if (fileContent is null)
            {
                this._logger.LogWarning("Server return null file content for '{FilePath}'.", filePath);

                this.HasError = true;

                return [];
            }

            this._filesContentCache[filePath] = fileContent;

            this.HasError = false;

            return fileContent;
        }

        public void Dispose()
        {
            this._tabsBarStateService.OnActiveTabChanged -= this.OnActiveTabChange;
        }
    }
}
