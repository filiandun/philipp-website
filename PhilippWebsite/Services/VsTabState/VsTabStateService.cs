

using System.Xml.Linq;

namespace PhilippWebsite.Services.VsTabState
{
    public class VsTabStateService
    {
        public event Action? OnChange;

        private ILogger<VsTabStateService> _logger;

        private List<TabModel> _openTabs;
        public IReadOnlyList<TabModel> OpenTabs => this._openTabs;

        public TabModel? ActiveTab { get; private set; }


        public VsTabStateService(ILogger<VsTabStateService> logger)
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

                this._logger.LogInformation("TabModel '{0}' is now active", tabModel.Name);
            }
            else
            {
                this._logger.LogInformation("TabModel '{0}' not found for set active", tabModel.Name);
            }
        }


        public void Open(TabModel tabModel)
        {
            if (!this._openTabs.Contains(tabModel))
            {
                this._openTabs.Add(tabModel);

                this.OnChange?.Invoke();

                this._logger.LogInformation("TabModel '{0}' open", tabModel.Name);
            }
            else
            {
                this._logger.LogInformation("TabModel '{0}' already open", tabModel.Name);
            }

        }

        public void Close(TabModel tabModel)
        {
            if (this._openTabs.Remove(tabModel))
            {
                this._logger.LogInformation("TabModel '{0}' close", tabModel.Name);

                this.OnChange?.Invoke();
            } 
            else
            {
                this._logger.LogInformation("TabModel '{0}' not found for close", tabModel.Name);
            }
        }
    }
}
