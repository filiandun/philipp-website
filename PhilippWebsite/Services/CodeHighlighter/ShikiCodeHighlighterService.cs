using Microsoft.JSInterop;


namespace PhilippWebsite.Services.CodeHighlighter
{
    public class ShikiCodeHighlighterService : ICodeHighlighterService, IAsyncDisposable
    {
        private readonly ILogger<ShikiCodeHighlighterService> _logger;

        private readonly IJSRuntime _jsRuntime;

        private Task? _shikiInitTask;
        private IJSObjectReference? _shikiModule;


        public ShikiCodeHighlighterService(ILogger<ShikiCodeHighlighterService> logger, IJSRuntime jsRuntime)
        {
            this._logger = logger;

            this._jsRuntime = jsRuntime;
            
            this._shikiModule = null;
        }


        private Task LazyInitializedAsync()
        {
            if (this._shikiInitTask is null)
            {
                this._shikiInitTask = LazyInitializedTaskAsync();
            }

            return this._shikiInitTask;
        }

        private async Task LazyInitializedTaskAsync()
        {
            try
            {
                this._shikiModule = await this._jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/shiki-highlighter.js");
                this._logger.LogDebug("Success to get shiki script.");

                string theme = ShikiCodeHighlighterConfig.Theme;
                string[] languages = ShikiCodeHighlighterConfig.ExtensionToLanguageMap.Values.Distinct().ToArray();

                await this._shikiModule.InvokeVoidAsync("init", theme, languages);
                this._logger.LogDebug("Success to init shiki script with '{Theme}' theme and '{LanguagesCount}' langs.", theme, languages.Length);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex,"Error to load shiki js.");
            }
        }


        public async Task PreloadAsync()
        {
            this._logger.LogDebug("Preload shiki");

            await this.LazyInitializedAsync();
        }

        public async Task<string> GetHighlightedHtml(string code, string filePath)
        {
            if (string.IsNullOrEmpty(code))
            {
                this._logger.LogWarning("Code from '{FilePath}' is empty.", filePath);

                return string.Empty;
            }

            await this.LazyInitializedAsync();

            if (this._shikiModule is null)
            {
                this._logger.LogWarning("Shiki module failed to initialize. Returning raw code.");
                return code;
            }

            string fileExtension = Path.GetExtension(filePath) ?? string.Empty;

            string language = this.GetShikiLanguage(fileExtension);

            this._logger.LogInformation("File '{FilePath}' was hightlighted as {Language}.", filePath, language);

            return await this._shikiModule.InvokeAsync<string>("highlight", code, language);
        }


        private string GetShikiLanguage(string extension) => ShikiCodeHighlighterConfig.ExtensionToLanguageMap.TryGetValue(extension, out var lang) ? lang : ShikiCodeHighlighterConfig.DefaultLanguage;


        public async ValueTask DisposeAsync()
        {
            if (this._shikiModule is not null)
            {
                await this._shikiModule.DisposeAsync();
            }
        }
    }
}
