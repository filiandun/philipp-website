using PhilippWebsite.Models;


namespace PhilippWebsite.Services.TabsBarState
{
    public class TabsBarStateService
    {
        public event Action? OnChange;

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

                this.OnChange?.Invoke();

                this._logger.LogInformation("Tab '{TabName}' is now active.", tabModel.Name);
            }
            else
            {
                this._logger.LogInformation("Tab '{TabName}' not found for set active.", tabModel.Name);
            }
        }


        public void Open(TabModel tabModel)
        {
            if (!this._openTabs.Contains(tabModel))
            {
                this._openTabs.Add(tabModel);

                this.OnChange?.Invoke();

                this._logger.LogInformation("Tab '{TabName}' open.", tabModel.Name);
            }
            else
            {
                this._logger.LogInformation("Tab '{TabName}' already open.", tabModel.Name);
            }

        }

        public void Close(TabModel tabModel)
        {
            if (this._openTabs.Remove(tabModel))
            {
                this._logger.LogInformation("Tab '{TabName}' close.", tabModel.Name);

                this.OnChange?.Invoke();
            } 
            else
            {
                this._logger.LogInformation("Tab '{TabName}' not found for close.", tabModel.Name);
            }
        }
    }
}
