using PhilippWebsite.Models;


namespace PhilippWebsite.Services.TabsBarState
{
    public class TabsBarStateService
    {
        public event Action? OnOpenTabsChange;
        public event Action? OnActiveTabChange;

        private ILogger<TabsBarStateService> _logger;

        private readonly List<TabModel> _openTabs;
        public IReadOnlyList<TabModel> OpenTabs => this._openTabs;

        public TabModel? ActiveTab { get; private set; }


        public TabsBarStateService(ILogger<TabsBarStateService> logger)
        {
            this._logger = logger;

            this._openTabs = new List<TabModel>();

            this.ActiveTab = null;
        }


        public void SetActive(TabModel tabModel)
        {
            if (this._openTabs.Contains(tabModel))
            {
                this.ActiveTab = tabModel;

                this.OnActiveTabChange?.Invoke();

                this._logger.LogInformation("Tab '{TabName}' is now active.", tabModel.Name);
            }
            else
            {
                this._logger.LogWarning("Tab '{TabName}' not found for set active.", tabModel.Name);
            }
        }


        public void Open(TabModel tabModel)
        {
            if (!this._openTabs.Contains(tabModel))
            {
                this._openTabs.Add(tabModel);

                this.OnOpenTabsChange?.Invoke();

                this._logger.LogInformation("Tab '{TabName}' open.", tabModel.Name);

                this.SetActive(tabModel);
            }
            else
            {
                this._logger.LogDebug("Tab '{TabName}' already open.", tabModel.Name);
            }

        }

        public void Close(TabModel tabModel)
        {
            if (this._openTabs.Remove(tabModel))
            {
                this._logger.LogInformation("Tab '{TabName}' close.", tabModel.Name);

                this.OnOpenTabsChange?.Invoke();
            } 
            else
            {
                this._logger.LogWarning("Tab '{TabName}' not found for close.", tabModel.Name);
            }
        }
    }
}
